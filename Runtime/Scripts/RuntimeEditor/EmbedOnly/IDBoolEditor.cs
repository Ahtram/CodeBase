#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class IDBoolEditor : EditorWindow {

    static public bool EditIDBool(IDBool idBool, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection("ID", IDCollectionInput, idBool.ID, (ID) => {
            idBool.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        bool newToggle = EditorGUILayoutPlus.ToggleLeft("Toggle", idBool.toggle);
        if (newToggle != idBool.toggle) {
            hasChanged = true;
            idBool.toggle = newToggle;
        }
        return hasChanged;
    }

    static public bool EditIDBool(string IDLabel, string valueLabel, IDBool idBool, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection(IDLabel, IDCollectionInput, idBool.ID, (ID) => {
            idBool.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        bool newToggle = EditorGUILayoutPlus.ToggleLeft(valueLabel, idBool.toggle);
        if (newToggle != idBool.toggle) {
            hasChanged = true;
            idBool.toggle = newToggle;
        }
        return hasChanged;
    }

    static public bool EditIDBool(IDBool idBool, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection("ID", IDIndexInput, idBool.ID, (ID) => {
            idBool.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        bool newToggle = EditorGUILayoutPlus.ToggleLeft("Toggle", idBool.toggle);
        if (newToggle != idBool.toggle) {
            hasChanged = true;
            idBool.toggle = newToggle;
        }
        return hasChanged;
    }

    static public bool EditIDBool(string IDLabel, string toggleLabel, IDBool idBool, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection(IDLabel, IDIndexInput, idBool.ID, (ID) => {
            idBool.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        bool newToggle = EditorGUILayoutPlus.ToggleLeft(toggleLabel, idBool.toggle);
        if (newToggle != idBool.toggle) {
            hasChanged = true;
            idBool.toggle = newToggle;
        }
        return hasChanged;
    }

    //== Edit List ==

    static public bool EditIDBools(string title, List<IDBool> idBools, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idBools.Add(new IDBool());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idBools.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID = words[0];
                                bool b = Util.StringToBool(words[1]);
                                if (!string.IsNullOrEmpty(ID)) {
                                    idBools.Add(new IDBool(ID, b));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idBools.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idBools.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDBool(idBools[i], IDCollectionInput, onOpenEditorClick, onGreenLitClick);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idBools.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDBools(string title, string indexLabel, string valueLabel, List<IDBool> idBools, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idBools.Add(new IDBool());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idBools.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID = words[0];
                                bool b = Util.StringToBool(words[1]);
                                if (!string.IsNullOrEmpty(ID)) {
                                    idBools.Add(new IDBool(ID, b));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idBools.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idBools.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDBool(indexLabel, valueLabel, idBools[i], IDCollectionInput, onOpenEditorClick, onGreenLitClick);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idBools.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDBools(string title, List<IDBool> idBools, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idBools.Add(new IDBool());
                }
                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idBools.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDBool(idBools[i], IDIndexInput, onOpenEditorClick, onGreenLitClick);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idBools.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDBools(string title, string indexLabel, string valueLabel, List<IDBool> idBools, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idBools.Add(new IDBool());
                }
                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idBools.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idBoolChanged = EditIDBool(indexLabel, valueLabel, idBools[i], IDIndexInput, onOpenEditorClick, onGreenLitClick);
                    if (idBoolChanged) {
                        hasChanged = true;
                    }

                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        hasChanged = true;
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                idBools.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

}

#endif