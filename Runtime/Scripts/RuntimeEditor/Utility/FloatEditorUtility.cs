#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any float.
/// </summary>
public class FloatEditorUtility : UniEditorWindow {

    private string stringTitle = "";
    private float editingFloat = 0;
    private Action<float> onEditing = null;
    private Action<float> onCompleteEditing = null;

    public static FloatEditorUtility Open(string titleInput, float editingFloatInput, Action<float> onEditingInput, Action<float> onCompleteEditingInput = null) {
        // Get existing open window or if none, make a new one:
        FloatEditorUtility instance = (FloatEditorUtility)ShowWindow<FloatEditorUtility>();
        instance.stringTitle = titleInput;
        instance.editingFloat = editingFloatInput;
        instance.onEditing = onEditingInput;
        instance.onCompleteEditing = onCompleteEditingInput;
        return instance as FloatEditorUtility;
    }

    public override void OnGUI() {
        base.OnGUI();
        EditorGUILayout.BeginVertical("FrameBox");
        {
            EditorGUILayout.LabelField(stringTitle);
            float newEditingFloat = EditorGUILayoutPlus.FloatField(editingFloat);
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