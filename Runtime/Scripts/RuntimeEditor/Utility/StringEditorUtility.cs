#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any string.
/// </summary>
public class StringEditorUtility : UniEditorWindow {

    private string stringTitle = "";
    private string editingString = "";
    private Action<string> onEditing = null;
    private Action<string> onCompleteEditing = null;

    public static StringEditorUtility Open(string titleInput, string editingStringInput, Action<string> onEditingInput, Action<string> onCompleteEditingInput = null) {
        // Get existing open window or if none, make a new one:
        StringEditorUtility instance = (StringEditorUtility)ShowWindow<StringEditorUtility>();
        instance.stringTitle = titleInput;
        instance.editingString = editingStringInput;
        instance.onEditing = onEditingInput;
        instance.onCompleteEditing = onCompleteEditingInput;
        return instance as StringEditorUtility;
    }

    public override void OnGUI() {
        EditorGUILayout.BeginVertical("FrameBox");
        {
            EditorGUILayout.LabelField(stringTitle);
            string newEditingString = EditorGUILayoutPlus.TextField(editingString);
            if (newEditingString != editingString) {
                editingString = newEditingString;
                if (onEditing != null) {
                    onEditing(editingString);
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