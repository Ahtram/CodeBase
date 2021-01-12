#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any string list.
/// </summary>
public class StringListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<string> editingStringList = null;
    private string listTitle = "";
    private Action<List<string>> onClose;

    public static StringListEditorUtility Open(string titleInput, Action<List<string>> onCloseInput, List<string> stringListInput = null) {
        // Get existing open window or if none, make a new one:
        StringListEditorUtility instance = (StringListEditorUtility)ShowWindow<StringListEditorUtility>();
        instance.listTitle = titleInput;
        instance.onClose = onCloseInput;
        if (stringListInput != null) {
            instance.editingStringList = stringListInput;
        } else {
            instance.editingStringList = new List<string>();
        }
        return instance as StringListEditorUtility;
    }

    public override void OnGUI() {
        if (editingStringList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayoutPlus.EditStringList(listTitle, editingStringList);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        base.OnGUI();
    }

    private void OnDestroy() {
        if (onClose != null) {
            onClose(editingStringList);
        }
    }

}

#endif