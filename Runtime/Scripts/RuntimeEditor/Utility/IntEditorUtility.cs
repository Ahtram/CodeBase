#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any int.
/// </summary>
public class IntEditorUtility : UniEditorWindow {

    private string stringTitle = "";
    private int editingInt = 0;
    private Action<int> onEditing = null;
    private Action<int> onCompleteEditing = null;

    public static IntEditorUtility Open(string titleInput, int editingIntInput, Action<int> onEditingInput, Action<int> onCompleteEditingInput = null) {
        // Get existing open window or if none, make a new one:
        IntEditorUtility instance = (IntEditorUtility)ShowWindow<IntEditorUtility>();
        instance.stringTitle = titleInput;
        instance.editingInt = editingIntInput;
        instance.onEditing = onEditingInput;
        instance.onCompleteEditing = onCompleteEditingInput;
        return instance as IntEditorUtility;
    }

    public override void OnGUI() {
        EditorGUILayout.BeginVertical("FrameBox");
        {
            EditorGUILayout.LabelField(stringTitle);
            int newEditingInt = EditorGUILayoutPlus.IntField(editingInt);
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
                    Close();
                    break;
            }
        }
        base.OnGUI();
    }

    private void OnDestroy() {
        //Send complete editing.
        if (onCompleteEditing != null) {
            onCompleteEditing(editingInt);
        }

        editingInt = 0;
        stringTitle = "";
        onEditing = null;
        onCompleteEditing = null;
    }

}

#endif