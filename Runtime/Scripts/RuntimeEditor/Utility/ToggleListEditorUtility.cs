#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ToggleListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<bool> editingBoolList = null;
    private List<string> contentDislayList = null;
    private string listTitle = "";
    private Action<List<bool>> onClose;

    public static ToggleListEditorUtility Open(string titleInput, Action<List<bool>> onCloseInput, List<bool> boolListInput, List<string> contentDislayListInput = null) {
        // Get existing open window or if none, make a new one:
        ToggleListEditorUtility instance = (ToggleListEditorUtility)ShowWindow<ToggleListEditorUtility>();
        instance.listTitle = titleInput;
        instance.onClose = onCloseInput;

        if (boolListInput != null) {
            instance.editingBoolList = boolListInput;
        } else {
            instance.editingBoolList = new List<bool>();
        }

        if (contentDislayListInput != null) {
            instance.contentDislayList = contentDislayListInput;
        } else {
            instance.contentDislayList = new List<string>();
        }
        return instance as ToggleListEditorUtility;
    }

    public override void OnGUI() {
        base.OnGUI();
        if (editingBoolList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayoutPlus.EditToggleList(listTitle, editingBoolList, contentDislayList);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void OnDestroy() {
        if (onClose != null) {
            onClose(editingBoolList);
        }
    }

}

#endif