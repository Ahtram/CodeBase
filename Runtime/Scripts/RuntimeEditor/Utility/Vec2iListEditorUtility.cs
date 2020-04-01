#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any Vec2i list.
/// </summary>
public class Vec2iListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<Vec2i> editingVecList = null;
    private string listTitle = "";
    private Action<List<Vec2i>> onClose;

    public static Vec2iListEditorUtility Open(string titleInput, Action<List<Vec2i>> onCloseInput, List<Vec2i> vecListInput = null) {
        // Get existing open window or if none, make a new one:
        Vec2iListEditorUtility instance = (Vec2iListEditorUtility)ShowWindow<Vec2iListEditorUtility>();
        instance.listTitle = titleInput;
        instance.onClose = onCloseInput;
        if (vecListInput != null) {
            instance.editingVecList = vecListInput;
        } else {
            instance.editingVecList = new List<Vec2i>();
        }
        return instance as Vec2iListEditorUtility;
    }

    public override void OnGUI() {
        base.OnGUI();
        if (editingVecList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayoutPlus.EditVec2iList(listTitle, editingVecList);
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