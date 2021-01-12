#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit IntPair.
/// </summary>
public class IntPairEditorUtility : UniEditorWindow {

    private string indexLabel = "";
    private string valueLabel = "";
    private IntPair editingIntPair = null;
    private Action<IntPair> onEditing = null;
    private Action<IntPair> onCompleteEditing = null;

    public static IntPairEditorUtility Open(string indexLabel, string valueLabel, IntPair editingIntPair, Action<IntPair> onEditingInput, Action<IntPair> onCompleteEditingInput = null) {
        // Get existing open window or if none, make a new one:
        IntPairEditorUtility instance = (IntPairEditorUtility)ShowWindow<IntPairEditorUtility>();
        instance.indexLabel = indexLabel;
        instance.valueLabel = valueLabel;
        instance.editingIntPair = new IntPair(editingIntPair);
        instance.onEditing = onEditingInput;
        instance.onCompleteEditing = onCompleteEditingInput;
        return instance as IntPairEditorUtility;
    }

    public static IntPairEditorUtility Open(string indexLabel, string valueLabel, int initIndex, int initValue, Action<IntPair> onEditingInput, Action<IntPair> onCompleteEditingInput = null) {
        // Get existing open window or if none, make a new one:
        IntPairEditorUtility instance = (IntPairEditorUtility)ShowWindow<IntPairEditorUtility>();
        instance.indexLabel = indexLabel;
        instance.valueLabel = valueLabel;
        instance.editingIntPair = new IntPair(initIndex, initValue);
        instance.onEditing = onEditingInput;
        instance.onCompleteEditing = onCompleteEditingInput;
        return instance as IntPairEditorUtility;
    }

    public override void OnGUI() {
        EditorGUILayout.BeginVertical("FrameBox");
        {
            if (IntPairEditor.EditIntPair(indexLabel, valueLabel, editingIntPair)) {
                if (onEditing != null) {
                    onEditing(editingIntPair);
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
            onCompleteEditing(editingIntPair);
        }

        editingIntPair = null;
        indexLabel = "";
        valueLabel = "";
        onEditing = null;
        onCompleteEditing = null;
    }


}

#endif