#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any int.
/// </summary>
public class IntSliderEditorUtility : UniEditorWindow {

    private string stringTitle = "";
    private int editingInt = 0;

    private int sliderMin = 50;
    private int sliderMax = 500;

    private Action<int> onEditing = null;
    private Action<int> onCompleteEditing = null;

    public static IntSliderEditorUtility Open(string titleInput, int editingIntInput, int sliderMin, int sliderMax, Action<int> onEditingInput , Action<int> onCompleteEditingInput = null) {
        // Get existing open window or if none, make a new one:
        IntSliderEditorUtility instance = (IntSliderEditorUtility)ShowWindow<IntSliderEditorUtility>();
        instance.stringTitle = titleInput;
        instance.editingInt = editingIntInput;
        instance.sliderMin = sliderMin;
        instance.sliderMax = sliderMax;
        instance.onEditing = onEditingInput;
        instance.onCompleteEditing = onCompleteEditingInput;
        return instance as IntSliderEditorUtility;
    }

    public override void OnGUI() {
        base.OnGUI();
        EditorGUILayout.BeginVertical("FrameBox");
        {
            EditorGUILayout.LabelField(stringTitle);
            int newEditingInt = EditorGUILayout.IntSlider(editingInt, sliderMin, sliderMax);
            if (newEditingInt != editingInt) {
                editingInt = newEditingInt;
                if (onEditing != null) {
                    onEditing(editingInt);
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
                        onCompleteEditing(editingInt);
                    }
                    break;
            }
        }
    }

    private void OnDestroy() {
        editingInt = 0;
        stringTitle = "";
        onEditing = null;
        onCompleteEditing = null;
    }

}

#endif