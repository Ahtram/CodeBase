#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any ID list.
/// </summary>
public class IDListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<string> editingIDList = null;
    private string listTitle = "";
    private Action<List<string>> onClose = null;
    private Action onOpenEditorClick = null;
    private Action<string> onGreenLitClick = null;

    private IDCollection usingIDCollection = null;
    private IDIndex usingIDIndex = null;

    public static IDListEditorUtility Open(string titleInput, IDCollection IDCollection, Action<List<string>> onCloseInput, List<string> IDListInput = null, Action onOpenEditorClick = null, Action<string> onGreenLitClick = null) {
        // Get existing open window or if none, make a new one:
        IDListEditorUtility instance = (IDListEditorUtility)ShowWindow<IDListEditorUtility>();
        instance.listTitle = titleInput;
        instance.usingIDCollection = IDCollection;
        instance.usingIDIndex = null;
        instance.onClose = onCloseInput;
        if (IDListInput != null) {
            instance.editingIDList = IDListInput;
        } else {
            instance.editingIDList = new List<string>();
        }
        instance.onOpenEditorClick = onOpenEditorClick;
        instance.onGreenLitClick = onGreenLitClick;
        return instance as IDListEditorUtility;
    }

    public static IDListEditorUtility Open(string titleInput, IDIndex IDIndex, Action<List<string>> onCloseInput, List<string> IDListInput = null, Action onOpenEditorClick = null, Action<string> onGreenLitClick = null) {
        // Get existing open window or if none, make a new one:
        IDListEditorUtility instance = (IDListEditorUtility)ShowWindow<IDListEditorUtility>();
        instance.listTitle = titleInput;
        instance.usingIDCollection = null;
        instance.usingIDIndex = IDIndex;
        instance.onClose = onCloseInput;
        if (IDListInput != null) {
            instance.editingIDList = IDListInput;
        } else {
            instance.editingIDList = new List<string>();
        }
        instance.onOpenEditorClick = onOpenEditorClick;
        instance.onGreenLitClick = onGreenLitClick;
        return instance as IDListEditorUtility;
    }

    public override void OnGUI() {
        if (editingIDList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    if (usingIDCollection != null) {
                        EditorGUILayoutPlus.EditIDList(listTitle, usingIDCollection, editingIDList, onOpenEditorClick, onGreenLitClick);
                    } else if (usingIDIndex != null) {
                        EditorGUILayoutPlus.EditIDList(listTitle, usingIDIndex, editingIDList, onOpenEditorClick, onGreenLitClick);
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        base.OnGUI();
    }

    private void OnDestroy() {
        if (onClose != null) {
            onClose(editingIDList);
        }
    }

}

#endif