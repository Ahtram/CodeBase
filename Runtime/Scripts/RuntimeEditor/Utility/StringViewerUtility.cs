#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any string.
/// </summary>
public class StringViewerUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private string viewingString = "";

    public static StringViewerUtility Open(string titleInput, string viewingStringInput) {
        // Get existing open window or if none, make a new one:
        StringViewerUtility instance = (StringViewerUtility)ShowWindow<StringViewerUtility>();
        instance.titleContent = new GUIContent(titleInput);
        instance.viewingString = viewingStringInput;
        return instance as StringViewerUtility;
    }

    public override void OnGUI() {

        EditorGUILayout.BeginVertical("FrameBox");
        {
            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
            {
                GUIStyle customTextAreaStyle = new GUIStyle(EditorStyles.textArea) { wordWrap = true };
                EditorGUILayout.LabelField(viewingString, customTextAreaStyle, GUILayout.ExpandHeight(true));
            }
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("Copy", EditorStyles.miniButton)) {
                EditorGUIUtility.systemCopyBuffer = viewingString;
            }
        }
        EditorGUILayout.EndVertical();

        base.OnGUI();
    }

}

#endif