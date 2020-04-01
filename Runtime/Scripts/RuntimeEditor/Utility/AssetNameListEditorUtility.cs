#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A generic utility editor for conveniently edit any AssetName list.
/// </summary>
public class AssetNameListEditorUtility : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;

    private List<string> editingAssetNameList = null;
    private string listTitle = "";
    private string assetFolderPath = "";
    private string targetTerm = "";
    private Action<List<string>> onClose;

    public static AssetNameListEditorUtility Open(string titleInput, string assetsFolderPath, string targetTerm, Action<List<string>> onCloseInput, List<string> assetNameListInput = null) {
        // Get existing open window or if none, make a new one:
        AssetNameListEditorUtility instance = (AssetNameListEditorUtility)ShowWindow<AssetNameListEditorUtility>();
        instance.listTitle = titleInput;
        instance.assetFolderPath = assetsFolderPath;
        instance.targetTerm = targetTerm;
        instance.onClose = onCloseInput;
        if (assetNameListInput != null) {
            instance.editingAssetNameList = assetNameListInput;
        } else {
            instance.editingAssetNameList = new List<string>();
        }
        return instance as AssetNameListEditorUtility;
    }

    public override void OnGUI() {
        base.OnGUI();
        if (editingAssetNameList != null) {
            EditorGUILayout.BeginVertical("FrameBox");
            {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayoutPlus.EditAssetNameList(listTitle, assetFolderPath, editingAssetNameList, targetTerm);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void OnDestroy() {
        if (onClose != null) {
            onClose(editingAssetNameList);
        }
    }

}

#endif