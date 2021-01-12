#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any Vec3 list.
/// </summary>
public class Vec3ListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<Vec3> editingVecList = null;
    private string listTitle = "";
    private Action<List<Vec3>> onClose;

    public static Vec3ListEditorUtility Open(string titleInput, Action<List<Vec3>> onCloseInput, List<Vec3> vecListInput = null) {
        // Get existing open window or if none, make a new one:
        Vec3ListEditorUtility instance = (Vec3ListEditorUtility)ShowWindow<Vec3ListEditorUtility>();
        instance.listTitle = titleInput;
        instance.onClose = onCloseInput;
        if (vecListInput != null) {
            instance.editingVecList = vecListInput;
        } else {
            instance.editingVecList = new List<Vec3>();
        }
        return instance as Vec3ListEditorUtility;
    }

    public override void OnGUI() {
        if (editingVecList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayoutPlus.EditVec3List(listTitle, editingVecList);
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