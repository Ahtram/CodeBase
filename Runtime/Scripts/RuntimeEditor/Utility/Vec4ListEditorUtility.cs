#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any Vec4 list.
/// </summary>
public class Vec4ListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<Vec4> editingVecList = null;
    private string listTitle = "";
    private Action<List<Vec4>> onClose;

    public static Vec4ListEditorUtility Open(string titleInput, Action<List<Vec4>> onCloseInput, List<Vec4> vecListInput = null) {
        // Get existing open window or if none, make a new one:
        Vec4ListEditorUtility instance = (Vec4ListEditorUtility)ShowWindow<Vec4ListEditorUtility>();
        instance.listTitle = titleInput;
        instance.onClose = onCloseInput;
        if (vecListInput != null) {
            instance.editingVecList = vecListInput;
        } else {
            instance.editingVecList = new List<Vec4>();
        }
        return instance as Vec4ListEditorUtility;
    }

    public override void OnGUI() {
        if (editingVecList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayoutPlus.EditVec4List(listTitle, editingVecList);
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