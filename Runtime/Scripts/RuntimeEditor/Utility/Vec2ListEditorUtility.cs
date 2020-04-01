#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any Vec2i list.
/// </summary>
public class Vec2ListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<Vec2> editingVecList = null;
    private string listTitle = "";
    private Action<List<Vec2>> onClose;

    public static Vec2ListEditorUtility Open(string titleInput, Action<List<Vec2>> onCloseInput, List<Vec2> vecListInput = null) {
        // Get existing open window or if none, make a new one:
        Vec2ListEditorUtility instance = (Vec2ListEditorUtility)ShowWindow<Vec2ListEditorUtility>();
        instance.listTitle = titleInput;
        instance.onClose = onCloseInput;
        if (vecListInput != null) {
            instance.editingVecList = vecListInput;
        } else {
            instance.editingVecList = new List<Vec2>();
        }
        return instance as Vec2ListEditorUtility;
    }

    public override void OnGUI() {
        base.OnGUI();
        if (editingVecList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayoutPlus.EditVec2List(listTitle, editingVecList);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void OnDestroy() {
        if (onClose != null) {
            onClose(editingVecList);
        }
    }

}

#endif