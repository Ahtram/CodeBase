#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any Vec3i list.
/// </summary>
public class Vec3iListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<Vec3i> editingVecList = null;
    private string listTitle = "";
    private Action<List<Vec3i>> onClose;

    public static Vec3iListEditorUtility Open(string titleInput, Action<List<Vec3i>> onCloseInput, List<Vec3i> vecListInput = null) {
        // Get existing open window or if none, make a new one:
        Vec3iListEditorUtility instance = (Vec3iListEditorUtility)ShowWindow<Vec3iListEditorUtility>();
        instance.listTitle = titleInput;
        instance.onClose = onCloseInput;
        if (vecListInput != null) {
            instance.editingVecList = vecListInput;
        } else {
            instance.editingVecList = new List<Vec3i>();
        }
        return instance as Vec3iListEditorUtility;
    }

    public override void OnGUI() {
        base.OnGUI();
        if (editingVecList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayoutPlus.EditVec3iList(listTitle, editingVecList);
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