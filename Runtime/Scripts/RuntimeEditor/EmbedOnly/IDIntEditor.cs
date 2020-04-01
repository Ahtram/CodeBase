#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class IDIntEditor : EditorWindow {

    static public bool EditIDInt(IDInt idInt, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection("ID", IDCollectionInput, idInt.ID, (ID) => {
            idInt.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        int newValue = EditorGUILayoutPlus.IntField("Value", idInt.value);
        if (newValue != idInt.value) {
            hasChanged = true;
            idInt.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditIDInt(string IDLabel, string valueLabel, IDInt idInt, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection(IDLabel, IDCollectionInput, idInt.ID, (ID) => {
            idInt.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        int newValue = EditorGUILayoutPlus.IntField(valueLabel, idInt.value);
        if (newValue != idInt.value) {
            hasChanged = true;
            idInt.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditIDInt(IDInt idInt, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection("ID", IDIndexInput, idInt.ID, (ID) => {
            idInt.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        int newValue = EditorGUILayoutPlus.IntField("Value", idInt.value);
        if (newValue != idInt.value) {
            hasChanged = true;
            idInt.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditIDInt(string IDLabel, string valueLabel, IDInt idInt, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection(IDLabel, IDIndexInput, idInt.ID, (ID) => {
            idInt.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        int newValue = EditorGUILayoutPlus.IntField(valueLabel, idInt.value);
        if (newValue != idInt.value) {
            hasChanged = true;
            idInt.value = newValue;
        }
        return hasChanged;
    }

    //== Edit List ==

    static public bool EditIDInts(string title, List<IDInt> idInts, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idInts.Add(new IDInt());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idInts.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID = words[0];
                                int value = Util.StringToInt(words[1]);
                                if (!string.IsNullOrEmpty(ID)) {
                                    idInts.Add(new IDInt(ID, value));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idInts.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idInts.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idIntChanged = EditIDInt(idInts[i], IDCollectionInput, onOpenEditorClick, onGreenLitClick);
                    if (idIntChanged) {
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
                idInts.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDInts(string title, string indexLabel, string valueLabel, List<IDInt> idInts, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idInts.Add(new IDInt());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idInts.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID = words[0];
                                int value = Util.StringToInt(words[1]);
                                if (!string.IsNullOrEmpty(ID)) {
                                    idInts.Add(new IDInt(ID, value));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idInts.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idInts.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idIntChanged = EditIDInt(indexLabel, valueLabel, idInts[i], IDCollectionInput, onOpenEditorClick, onGreenLitClick);
                    if (idIntChanged) {
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
                idInts.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDInts(string title, List<IDInt> idInts, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idInts.Add(new IDInt());
                }
                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idInts.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idIntChanged = EditIDInt(idInts[i], IDIndexInput, onOpenEditorClick, onGreenLitClick);
                    if (idIntChanged) {
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
                idInts.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDInts(string title, string indexLabel, string valueLabel, List<IDInt> idInts, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idInts.Add(new IDInt());
                }
                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idInts.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idIntChanged = EditIDInt(indexLabel, valueLabel, idInts[i], IDIndexInput, onOpenEditorClick, onGreenLitClick);
                    if (idIntChanged) {
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
                idInts.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

}

#endif