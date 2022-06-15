using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

using static City;

[CustomEditor(typeof(City))]
[CanEditMultipleObjects]
public class CityEditor : Editor
{
    SerializedProperty m_targetInhabitants;
    SerializedProperty m_area;
    SerializedProperty m_elements;
    SerializedProperty m_affinities;
    SerializedProperty m_roadTexture;
    SerializedProperty m_roadWidthMin;
    SerializedProperty m_roadWidthMax;
    SerializedProperty m_crossroads;
    SerializedProperty m_heuristic;

    private void OnEnable()
    {
        m_targetInhabitants = serializedObject.FindProperty("m_targetInhabitants");
        m_area = serializedObject.FindProperty("m_area");
        m_elements = serializedObject.FindProperty("m_elementsArray");
        m_affinities = serializedObject.FindProperty("m_affinitiesArray");
        m_roadTexture = serializedObject.FindProperty("m_roadTexture");
        m_roadWidthMin = serializedObject.FindProperty("m_roadWidthMin");
        m_roadWidthMax = serializedObject.FindProperty("m_roadWidthMax");
        m_crossroads = serializedObject.FindProperty("m_crossroads");
        m_heuristic = serializedObject.FindProperty("m_heuristic");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(m_targetInhabitants);
        EditorGUILayout.PropertyField(m_area);
        EditorGUILayout.PropertyField(m_elements, new GUIContent("City Elements"));
        EditorGUILayout.PropertyField(m_affinities, new GUIContent("Affinities"));
        EditorGUILayout.PropertyField(m_roadTexture);
        EditorGUILayout.PropertyField(m_roadWidthMin, new GUIContent("Min Width of Road"));
        EditorGUILayout.PropertyField(m_roadWidthMax, new GUIContent("Max Width of Road"));
        EditorGUILayout.PropertyField(m_crossroads, new GUIContent("Number of Crossroads"));
        EditorGUILayout.HelpBox("If no Heuristic Calculator is specified the DefaultHeuristic will be used", MessageType.Info);
        EditorGUILayout.PropertyField(m_heuristic, new GUIContent("Heuristic Calculator"));

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate city"))
        {
            ((City)target).Generate();
        }
    }
}
