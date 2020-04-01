#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A popup window for conveniently modify a bool.
/// </summary>
public class BoolPopup : PopupWindowContent {

    private string stringTitle = "";
    private bool editingbool = false;

    private Action<bool> onEditing = null;
    private Action<bool> onCompleteEditing = null;

    public BoolPopup(string titleInput, bool editingboolInput, Action<bool> onEditingInput, Action<bool> onCompleteEditingInput = null) {
        stringTitle = titleInput;
        editingbool = editingboolInput;
        onEditing = onEditingInput;
        onCompleteEditing = onCompleteEditingInput;
    }

    public override Vector2 GetWindowSize() {
        return new Vector2(250.0f, 40.0f);
    }

    public override void OnGUI(Rect rect) {
        EditorGUILayout.BeginVertical();
        {
            bool newEditingFloat = EditorGUILayoutPlus.Toggle(stringTitle, editingbool);
            if (newEditingFloat != editingbool) {
                editingbool = newEditingFloat;
                if (onEditing != null) {
                    onEditing(editingbool);
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    public override void OnClose() {
        //Send complete editing.
        if (onCompleteEditing != null) {
            onCompleteEditing(editingbool);
        }

        editingbool = false;
        stringTitle = "";
        onEditing = null;
        onCompleteEditing = null;
    }
}

#endif