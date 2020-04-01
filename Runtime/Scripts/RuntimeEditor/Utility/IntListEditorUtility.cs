#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any string list.
/// </summary>
public class IntListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<int> editingIntList = null;
    private string listTitle = "";
    private Action<List<int>> onClose;

    public static IntListEditorUtility Open(string titleInput, Action<List<int>> onCloseInput, List<int> intListInput = null) {
        // Get existing open window or if none, make a new one:
        IntListEditorUtility instance = (IntListEditorUtility)ShowWindow<IntListEditorUtility>();
        instance.listTitle = titleInput;
        instance.onClose = onCloseInput;
        if (intListInput != null) {
            instance.editingIntList = intListInput;
        } else {
            instance.editingIntList = new List<int>();
        }
        return instance as IntListEditorUtility;
    }

    public override void OnGUI() {
        base.OnGUI();
        if (editingIntList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayoutPlus.EditIntList(listTitle, editingIntList);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void OnDestroy() {
        if (onClose != null) {
            onClose(editingIntList);
        }
    }

}

#endif