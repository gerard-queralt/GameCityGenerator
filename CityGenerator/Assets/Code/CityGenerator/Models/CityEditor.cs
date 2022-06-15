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
    SerializedProperty m_districts;
    SerializedProperty m_affinities;
    SerializedProperty m_heuristic;

    private void OnEnable()
    {
        m_districts = serializedObject.FindProperty("m_districtsArray");
        m_affinities = serializedObject.FindProperty("m_affinitiesArray");
        m_heuristic = serializedObject.FindProperty("m_heuristic");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(m_districts, new GUIContent("City Districts"));
        EditorGUILayout.PropertyField(m_affinities, new GUIContent("Affinities"));
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
