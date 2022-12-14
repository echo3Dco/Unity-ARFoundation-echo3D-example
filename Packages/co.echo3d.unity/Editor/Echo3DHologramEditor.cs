using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Echo3DHologram))]
public class Echo3DHologramEditor : Editor
{
    SerializedProperty apiKey;
    //SerializedProperty secKey;
    SerializedProperty entries;

    SerializedProperty tags;
    SerializedProperty editorPreview;
    SerializedProperty queryURL;
    SerializedProperty queryOnly;
    SerializedProperty disableRemoteTransformations;

    SerializedProperty ignoreModelTransforms;

    bool showAdvanced = false;
    void OnEnable()
    {
        apiKey = serializedObject.FindProperty("apiKey");
        //secKey = serializedObject.FindProperty("secKey");
        entries = serializedObject.FindProperty("entries");
        tags = serializedObject.FindProperty("tags");
        disableRemoteTransformations = serializedObject.FindProperty("disableRemoteTransformations");
        editorPreview = serializedObject.FindProperty("editorPreview");
        queryURL = serializedObject.FindProperty("queryURL");
        queryOnly = serializedObject.FindProperty("queryOnly");
        ignoreModelTransforms = serializedObject.FindProperty("ignoreModelTransforms");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(apiKey);
        //secKey.stringValue = EditorGUILayout.PasswordField("Security Key", secKey.stringValue);
        //EditorGUILayout.PropertyField(secKey);
        EditorGUILayout.PropertyField(entries);
        EditorGUILayout.PropertyField(tags);
        EditorGUILayout.PropertyField(editorPreview);
        EditorGUILayout.PropertyField(ignoreModelTransforms);
        EditorGUILayout.PropertyField(disableRemoteTransformations);

        showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Advanced");

        if (showAdvanced)
        {
            EditorGUILayout.PropertyField(queryOnly);
            EditorGUILayout.PropertyField(queryURL);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
