#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class IDFloatEditor : EditorWindow {

    static public bool EditIDFloat(IDFloat idFloat, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection("ID", IDCollectionInput, idFloat.ID, (ID) => {
            idFloat.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        float newValue = EditorGUILayoutPlus.FloatField("Value", idFloat.value);
        if (newValue != idFloat.value) {
            hasChanged = true;
            idFloat.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditIDFloat(string IDLabel, string valueLabel, IDFloat idFloat, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection(IDLabel, IDCollectionInput, idFloat.ID, (ID) => {
            idFloat.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        float newValue = EditorGUILayoutPlus.FloatField(valueLabel, idFloat.value);
        if (newValue != idFloat.value) {
            hasChanged = true;
            idFloat.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditIDFloat(IDFloat idFloat, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection("ID", IDIndexInput, idFloat.ID, (ID) => {
            idFloat.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        float newValue = EditorGUILayoutPlus.FloatField("Value", idFloat.value);
        if (newValue != idFloat.value) {
            hasChanged = true;
            idFloat.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditIDFloat(string IDLabel, string valueLabel, IDFloat idFloat, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;

        EditorGUILayoutPlus.IDSelection(IDLabel, IDIndexInput, idFloat.ID, (ID) => {
            idFloat.ID = ID;
            hasChanged = true;
        }, onOpenEditorClick, onGreenLitClick);

        float newValue = EditorGUILayoutPlus.FloatField(valueLabel, idFloat.value);
        if (newValue != idFloat.value) {
            hasChanged = true;
            idFloat.value = newValue;
        }
        return hasChanged;
    }

    //== Edit List ==

    static public bool EditIDFloats(string title, List<IDFloat> idFloats, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idFloats.Add(new IDFloat());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idFloats.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID = words[0];
                                float f = Util.StringToFloat(words[1]);
                                if (!string.IsNullOrEmpty(ID)) {
                                    idFloats.Add(new IDFloat(ID, f));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idFloats.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idFloats.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idFloatChanged = EditIDFloat(idFloats[i], IDCollectionInput, onOpenEditorClick, onGreenLitClick);
                    if (idFloatChanged) {
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
                idFloats.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDFloats(string title, string indexLabel, string valueLabel, List<IDFloat> idFloats, IDCollection IDCollectionInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idFloats.Add(new IDFloat());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idFloats.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string ID = words[0];
                                float f = Util.StringToFloat(words[1]);
                                if (!string.IsNullOrEmpty(ID)) {
                                    idFloats.Add(new IDFloat(ID, f));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    idFloats.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idFloats.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idFloatChanged = EditIDFloat(indexLabel, valueLabel, idFloats[i], IDCollectionInput, onOpenEditorClick, onGreenLitClick);
                    if (idFloatChanged) {
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
                idFloats.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDFloats(string title, List<IDFloat> idFloats, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idFloats.Add(new IDFloat());
                }
                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idFloats.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idFloatChanged = EditIDFloat(idFloats[i], IDIndexInput, onOpenEditorClick, onGreenLitClick);
                    if (idFloatChanged) {
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
                idFloats.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIDFloats(string title, string indexLabel, string valueLabel, List<IDFloat> idFloats, IDIndex IDIndexInput, Action onOpenEditorClick, Action<string> onGreenLitClick = null) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    idFloats.Add(new IDFloat());
                }
                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < idFloats.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool idFloatChanged = EditIDFloat(indexLabel, valueLabel, idFloats[i], IDIndexInput, onOpenEditorClick, onGreenLitClick);
                    if (idFloatChanged) {
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
                idFloats.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

}

#endif