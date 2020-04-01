#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A popup window for conveniently modify a float.
/// </summary>
public class FloatSliderPopup : PopupWindowContent {

    private string stringTitle = "";
    private float editingFloat = 0;

    private float sliderMin = 50.0f;
    private float sliderMax = 500.0f;

    private Action<float> onEditing = null;
    private Action<float> onCompleteEditing = null;

    public FloatSliderPopup(string titleInput, float editingFloatInput, float sliderMinInput, float sliderMaxInput, Action<float> onEditingInput, Action<float> onCompleteEditingInput = null) {
        stringTitle = titleInput;
        editingFloat = editingFloatInput;
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
            float newEditingFloat = EditorGUILayout.Slider(editingFloat, sliderMin, sliderMax);
            if (newEditingFloat != editingFloat) {
                editingFloat = newEditingFloat;
                if (onEditing != null) {
                    onEditing(editingFloat);
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    public override void OnClose() {
        //Send complete editing.
        if (onCompleteEditing != null) {
            onCompleteEditing(editingFloat);
        }

        editingFloat = 0;
        stringTitle = "";
        onEditing = null;
        onCompleteEditing = null;
    }
}

#endif