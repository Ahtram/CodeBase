#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A popup window for conveniently modify a int.
/// </summary>
public class IntSliderPopup : PopupWindowContent {

    private string stringTitle = "";
    private int editingint = 0;

    private int sliderMin = 50;
    private int sliderMax = 500;

    private Action<int> onEditing = null;
    private Action<int> onCompleteEditing = null;

    public IntSliderPopup(string titleInput, int editingintInput, int sliderMinInput, int sliderMaxInput, Action<int> onEditingInput, Action<int> onCompleteEditingInput = null) {
        stringTitle = titleInput;
        editingint = editingintInput;
        sliderMin = sliderMinInput;
        sliderMax = sliderMaxInput;
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
            int newEditingInt = EditorGUILayout.IntSlider(editingint, sliderMin, sliderMax);
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