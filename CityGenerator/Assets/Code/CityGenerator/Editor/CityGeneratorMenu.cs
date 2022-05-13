using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

using static CityGeneratorParameters;

public class CityGeneratorMenu : EditorWindow
{
    private Vector2 scrollPos;
    private string m_menuName = "CityGeneratorMenu";

    [SerializeField] int m_targetInhabitants;
    [SerializeField] Bounds m_area;
    [SerializeField] public CityElement[] m_cityElements;
    [SerializeField] public Type m_heuristic /*tmp*/ = typeof(DefaultHeuristic);
    [SerializeField] public CityElementAffinity[] m_affinities;
    [SerializeField] Texture m_roadTexture;
    [SerializeField] float m_roadWidthMin;
    [SerializeField] float m_roadWidthMax;
    [SerializeField] int m_crossroads;

    [MenuItem("Window/City Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CityGeneratorMenu));
    }

    private void OnEnable()
    {
        string data = EditorPrefs.GetString(m_menuName, JsonUtility.ToJson(this, false));
        JsonUtility.FromJsonOverwrite(data, this);
    }

    private void OnDisable()
    {
        string data = JsonUtility.ToJson(this, false);
        EditorPrefs.SetString(m_menuName, data);
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);

        m_targetInhabitants = EditorGUILayout.IntField("Target Inhabitants", m_targetInhabitants);
        m_area = EditorGUILayout.BoundsField("City Area", m_area);

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        
        SerializedProperty serializedElements = so.FindProperty("m_cityElements");
        EditorGUILayout.PropertyField(serializedElements, true);

        SerializedProperty serializedAffinities = so.FindProperty("m_affinities");
        EditorGUILayout.PropertyField(serializedAffinities, true);
        
        so.ApplyModifiedProperties();

        m_roadTexture = (Texture)EditorGUILayout.ObjectField("Road Texture", m_roadTexture, typeof(Texture2D), false);
        m_roadWidthMin = EditorGUILayout.FloatField("Min Road Width", m_roadWidthMin);
        m_roadWidthMax = EditorGUILayout.FloatField("Max Road Width", m_roadWidthMax);
        m_crossroads = EditorGUILayout.IntField("Number of crossroads", m_crossroads);

        SanitizeInputs();

        EditorGUILayout.EndScrollView();

        //GUILayout.FlexibleSpace();
        if (GUILayout.Button("Generate city"))
        {
            CityGeneratorParameters cityParams = new CityGeneratorParameters((uint)m_targetInhabitants,
                                                                             m_area,
                                                                             m_cityElements,
                                                                             m_affinities,
                                                                             m_roadTexture,
                                                                             m_roadWidthMin,
                                                                             m_roadWidthMax,
                                                                             (uint)m_crossroads,
                                                                             m_heuristic);
            AlgorithmMain algorithmMain = new AlgorithmMain(cityParams);
            algorithmMain.Run();
        }
    }

    private void SanitizeInputs()
    {
        m_targetInhabitants = Math.Max(0, m_targetInhabitants);
        m_roadWidthMin = Mathf.Max(0, m_roadWidthMin);
        m_roadWidthMax = Mathf.Max(0, m_roadWidthMax);
        m_crossroads = Math.Max(0, m_crossroads);
    }
}
