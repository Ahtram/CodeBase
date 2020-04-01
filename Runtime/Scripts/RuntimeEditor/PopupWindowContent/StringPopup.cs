#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A popup window for conveniently modify a string.
/// </summary>
public class StringPopup : PopupWindowContent {

    private string stringTitle = "";
    private string editingString = "";

    private Action<string> onEditing = null;
    private Action<string> onCompleteEditing = null;

    public StringPopup(string titleInput, string editingStringInput, Action<string> onEditingInput, Action<string> onCompleteEditingInput = null) {
        stringTitle = titleInput;
        editingString = editingStringInput;
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
            String newEditingInt = EditorGUILayoutPlus.TextField(editingString);
            if (newEditingInt != editingString) {
                editingString = newEditingInt;
                if (onEditing != null) {
                    onEditing(editingString);
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    public override void OnClose() {
        //Send complete editing.
        if (onCompleteEditing != null) {
            onCompleteEditing(editingString);
        }

        editingString = "";
        stringTitle = "";
        onEditing = null;
        onCompleteEditing = null;
    }
}

#endif