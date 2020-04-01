#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any float.
/// </summary>
public class FloatSliderEditorUtility : UniEditorWindow {

    private string stringTitle = "";
    private float editingFloat = 0;

    private float sliderMin = 50.0f;
    private float sliderMax = 500.0f;

    private Action<float> onEditing = null;
    private Action<float> onCompleteEditing = null;

    public static FloatSliderEditorUtility Open(string titleInput, float editingFloatInput, float sliderMin, float sliderMax, Action<float> onEditingInput, Action<float> onCompleteEditingInput = null) {
        // Get existing open window or if none, make a new one:
        FloatSliderEditorUtility instance = (FloatSliderEditorUtility)ShowWindow<FloatSliderEditorUtility>();
        instance.stringTitle = titleInput;
        instance.editingFloat = editingFloatInput;
        instance.sliderMin = sliderMin;
        instance.sliderMax = sliderMax;
        instance.onEditing = onEditingInput;
        instance.onCompleteEditing = onCompleteEditingInput;
        return instance as FloatSliderEditorUtility;
    }

    public override void OnGUI() {
        base.OnGUI();
        EditorGUILayout.BeginVertical("FrameBox");
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

        if (Event.current.isKey) {
            switch (Event.current.keyCode) {
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    //Send complete editing.
                    if (onCompleteEditing != null) {
                        onCompleteEditing(editingFloat);
                    }
                    break;
            }
        }
    }

    private void OnDestroy() {
        editingFloat = 0;
        stringTitle = "";
        onEditing = null;
        onCompleteEditing = null;
    }

}

#endif