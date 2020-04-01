#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A popup window for conveniently modify a float.
/// </summary>
public class FloatPopup : PopupWindowContent {

    private string stringTitle = "";
    private float editingfloat = 0;

    private Action<float> onEditing = null;
    private Action<float> onCompleteEditing = null;

    public FloatPopup(string titleInput, float editingfloatInput, Action<float> onEditingInput, Action<float> onCompleteEditingInput = null) {
        stringTitle = titleInput;
        editingfloat = editingfloatInput;
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
            float newEditingFloat = EditorGUILayoutPlus.FloatField(editingfloat);
            if (newEditingFloat != editingfloat) {
                editingfloat = newEditingFloat;
                if (onEditing != null) {
                    onEditing(editingfloat);
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    public override void OnClose() {
        //Send complete editing.
        if (onCompleteEditing != null) {
            onCompleteEditing(editingfloat);
        }

        editingfloat = 0.0f;
        stringTitle = "";
        onEditing = null;
        onCompleteEditing = null;
    }
}

#endif