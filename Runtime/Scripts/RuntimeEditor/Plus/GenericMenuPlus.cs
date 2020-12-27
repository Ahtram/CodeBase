#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

static public class GenericMenuPlus {

    /// <summary>
    /// Create a GenericMenu for an IDIndex data for conveniently select ID from it.
    /// </summary>
    /// <param name="IDIndexInput"></param>
    /// <param name="menuFunction2"></param>
    /// <param name="selectingID"></param>
    /// <returns></returns>
    static public GenericMenu Create(IDIndex IDIndexInput, GenericMenu.MenuFunction2 menuFunction2, 
    string selectingID = "", Func<string, string> getIDComment = null) {
        GenericMenu menu = new GenericMenu();
		for (int i = 0; i < IDIndexInput.ids.Count; i++) {
            bool isSelection = (!string.IsNullOrEmpty(selectingID) && selectingID == IDIndexInput.ids[i]) ? (true) : (false);
            menu.AddItem(new GUIContent(IDIndexInput.ids[i] + ((getIDComment == null) ? ("") : ("(" + getIDComment(IDIndexInput.ids[i])+ ")"))), 
            isSelection, menuFunction2, IDIndexInput.ids[i]);
		}
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("[Clear]"), false, menuFunction2, "");
        return menu;
    }

    /// <summary>
    /// Create a GenericMenu for an IDIndex data for conveniently select ID from it.
    /// </summary>
    /// <param name="IDIndexInput"></param>
    /// <param name="menuFunction2"></param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    /// <param name="selectingID"></param>
    /// <returns></returns>
    static public GenericMenu Create(IDIndex IDIndexInput, GenericMenu.MenuFunction2 menuFunction2, object pairedKey, 
    string selectingID = "", Func<string, string> getIDComment = null) {
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < IDIndexInput.ids.Count; i++) {
            bool isSelection = (!string.IsNullOrEmpty(selectingID) && selectingID == IDIndexInput.ids[i]) ? (true) : (false);
            menu.AddItem(new GUIContent(IDIndexInput.ids[i] + ((getIDComment == null) ? ("") : ("(" + getIDComment(IDIndexInput.ids[i]) + ")"))), 
            isSelection, menuFunction2, new KeyValuePair<object, string>(pairedKey, IDIndexInput.ids[i]));
        }
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("[Clear]"), false, menuFunction2, new KeyValuePair<object, string>(pairedKey, ""));
        return menu;
    }

    /// <summary>
    /// Create a GenericMenu for an IDCollection data for conveniently select ID from it.
    /// </summary>
    /// <param name="IDCollectionInput"></param>
    /// <param name="menuFunction2"></param>
    /// <returns></returns>
    static public GenericMenu Create(IDCollection IDCollectionInput, GenericMenu.MenuFunction2 menuFunction2, 
    string selectingID = "", Func<string, string> getIDComment = null) {
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < IDCollectionInput.IDIndexes.Count; i++) {
            for (int j = 0; j < IDCollectionInput.IDIndexes[i].ids.Count; j++) {
                bool isSelection = (!string.IsNullOrEmpty(selectingID) && selectingID == IDCollectionInput.IDIndexes[i].ids[j]) ? (true) : (false);
                menu.AddItem(new GUIContent("[" + i + "]" + IDCollectionInput.IDIndexes[i].name + "/" + IDCollectionInput.IDIndexes[i].ids[j] + ((getIDComment == null) ? ("") : ("(" + getIDComment(IDCollectionInput.IDIndexes[i].ids[j]) + ")"))), 
                isSelection, menuFunction2, IDCollectionInput.IDIndexes[i].ids[j]);
			}
        }
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("[Clear]"), false, menuFunction2, "");
        return menu;
    }

    /// <summary>
    /// Create a GenericMenu for an IDCollection data for conveniently select ID from it.
    /// </summary>
    /// <param name="IDCollectionInput"></param>
    /// <param name="menuFunction2"></param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    /// <returns></returns>
    static public GenericMenu Create(IDCollection IDCollectionInput, GenericMenu.MenuFunction2 menuFunction2, object pairedKey, 
    string selectingID = "", Func<string, string> getIDComment = null) {
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < IDCollectionInput.IDIndexes.Count; i++) {
            for (int j = 0; j < IDCollectionInput.IDIndexes[i].ids.Count; j++) {
                bool isSelection = (!string.IsNullOrEmpty(selectingID) && selectingID == IDCollectionInput.IDIndexes[i].ids[j]) ? (true) : (false);
                menu.AddItem(new GUIContent("[" + i + "]" + IDCollectionInput.IDIndexes[i].name + "/" + IDCollectionInput.IDIndexes[i].ids[j] + ((getIDComment == null) ? ("") : ("(" + getIDComment(IDCollectionInput.IDIndexes[i].ids[j]) + ")"))), 
                isSelection, menuFunction2, new KeyValuePair<object, string>(pairedKey, IDCollectionInput.IDIndexes[i].ids[j]));
            }
        }
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("[Clear]"), false, menuFunction2, new KeyValuePair<object, string>(pairedKey, ""));
        return menu;
    }

    /// <summary>
    /// Create a GenericMenu to list all stuffs under an input asset path. (By Using FindAssets)
    /// </summary>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/"</param>
    /// <param name="targetTerm">prefab/texture2D/audioClip/font/sprite/script etc.</param>
    /// <param name="searchTerm">A search term.</param>
    /// <param name="menuFunction2">GenericMenu callback</param>
    /// <returns></returns>
    static public GenericMenu Create(string assetsFolderPath, string targetTerm, string searchTerm, GenericMenu.MenuFunction2 menuFunction2, string selectingAssetName = "") {
        GenericMenu menu = new GenericMenu();
        string[] guids = AssetDatabase.FindAssets(searchTerm + " t:" + targetTerm, new string[] { assetsFolderPath });
        for (int i = 0; i < guids.Length; i++) {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            string assetName = Path.GetFileNameWithoutExtension(assetPath);
            string extensiotn = Path.GetExtension(assetPath);
            string displayGUIContent = assetPath.Remove(assetPath.Length - extensiotn.Length, extensiotn.Length).Remove(0, assetsFolderPath.Length + 1);
            bool isSelection = (!string.IsNullOrEmpty(selectingAssetName) && selectingAssetName == assetName) ? (true) : (false);
            menu.AddItem(new GUIContent(displayGUIContent), isSelection, menuFunction2, assetName);
        }
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("[Clear]"), false, menuFunction2, "");
        return menu;
    }

    /// <summary>
    /// Create a GenericMenu to list all stuffs under an input asset path. (By Using FindAssets)
    /// </summary>
    /// <param name="assetsFolderPath">Start with "Assets/" and "SHOULD NOT" end with "/"</param>
    /// <param name="targetTerm">prefab/texture2D/audioClip/font/sprite/script etc.</param>
    /// <param name="searchTerm">A search term.</param>
    /// <param name="menuFunction2">GenericMenu callback</param>
    /// <param name="pairedKey">This will be callback as a KeyValuePair key from GenericMenu.MenuFunction2</param>
    /// <returns></returns>
    static public GenericMenu Create(string assetsFolderPath, string targetTerm, string searchTerm, GenericMenu.MenuFunction2 menuFunction2, object pairedKey, string selectingAssetName = "") {
        GenericMenu menu = new GenericMenu();
        string[] guids = AssetDatabase.FindAssets(searchTerm + " t:" + targetTerm, new string[] { assetsFolderPath });
        for (int i = 0; i < guids.Length; i++) {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            string assetName = Path.GetFileNameWithoutExtension(assetPath);
            string extensiotn = Path.GetExtension(assetPath);
            string displayGUIContent = assetPath.Remove(assetPath.Length - extensiotn.Length, extensiotn.Length).Remove(0, assetsFolderPath.Length + 1);
            bool isSelection = (!string.IsNullOrEmpty(selectingAssetName) && selectingAssetName == assetName) ? (true) : (false);
            menu.AddItem(new GUIContent(displayGUIContent), isSelection, menuFunction2, new KeyValuePair<object, string>(pairedKey, assetName));
        }
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("[Clear]"), false, menuFunction2, new KeyValuePair<object, string>(pairedKey, ""));
        return menu;
    }

}

#endif