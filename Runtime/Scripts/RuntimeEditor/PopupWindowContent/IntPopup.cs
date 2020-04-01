#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A popup window for conveniently modify an int.
/// </summary>
public class IntPopup : PopupWindowContent {

    private string stringTitle = "";
    private int editingint = 0;

    private Action<int> onEditing = null;
    private Action<int> onCompleteEditing = null;

    public IntPopup(string titleInput, int editingintInput, Action<int> onEditingInput, Action<int> onCompleteEditingInput = null) {
        stringTitle = titleInput;
        editingint = editingintInput;
        onEditing = onEditingInput;
        onCompleteEditing = onCompleteEditingInput;
    }

    public override Vector2 GetWindowSize() {
        return new Vector2(250.0f, 40.0f);
    }

    public override void OnGUI(Rect rect) {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField(stringTitle);
            int newEditingInt = EditorGUILayoutPlus.IntField(editingint);
            if (newEditingInt != editingint) {
                editingint = newEditingInt;
                if (onEditing != null) {
                    onEditing(editingint);
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    public override void OnClose() {
        //Send complete editing.
        if (onCompleteEditing != null) {
            onCompleteEditing(editingint);
        }

        editingint = 0;
        stringTitle = "";
        onEditing = null;
        onCompleteEditing = null;
    }
}

#endif