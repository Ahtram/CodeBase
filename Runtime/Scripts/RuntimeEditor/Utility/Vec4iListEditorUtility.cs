#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any Vec4i list.
/// </summary>
public class Vec4iListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<Vec4i> editingVecList = null;
    private string listTitle = "";
    private Action<List<Vec4i>> onClose;

    public static Vec4iListEditorUtility Open(string titleInput, Action<List<Vec4i>> onCloseInput, List<Vec4i> vecListInput = null) {
        // Get existing open window or if none, make a new one:
        Vec4iListEditorUtility instance = (Vec4iListEditorUtility)ShowWindow<Vec4iListEditorUtility>();
        instance.listTitle = titleInput;
        instance.onClose = onCloseInput;
        if (vecListInput != null) {
            instance.editingVecList = vecListInput;
        } else {
            instance.editingVecList = new List<Vec4i>();
        }
        return instance as Vec4iListEditorUtility;
    }

    public override void OnGUI() {
        if (editingVecList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayoutPlus.EditVec4iList(listTitle, editingVecList);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        base.OnGUI();
    }

    private void OnDestroy() {
        if (onClose != null) {
            onClose(editingVecList);
        }
    }

}

#endif