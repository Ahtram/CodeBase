#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class IntPairEditor : EditorWindow {

    static public bool EditIntPair(IntPair intPair) {
        bool hasChanged = false;
        int newIndex = EditorGUILayoutPlus.IntField("Index", intPair.index);
        if (newIndex != intPair.index) {
            hasChanged = true;
            intPair.index = newIndex;
        }
        int newValue = EditorGUILayoutPlus.IntField("Value", intPair.value);
        if (newValue != intPair.value) {
            hasChanged = true;
            intPair.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditIntPair(string indexLabel, string valueLabel, IntPair intPair) {
        bool hasChanged = false;
        int newIndex = EditorGUILayoutPlus.IntField(indexLabel, intPair.index);
        if (newIndex != intPair.index) {
            hasChanged = true;
            intPair.index = newIndex;
        }
        int newValue = EditorGUILayoutPlus.IntField(valueLabel, intPair.value);
        if (newValue != intPair.value) {
            hasChanged = true;
            intPair.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditIntPairs(string title, List<IntPair> intPairs) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    intPairs.Add(new IntPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    intPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                int intValue0 = Util.StringToInt(words[0]);
                                int intValue1 = Util.StringToInt(words[1]);
                                if (!string.IsNullOrEmpty(words[0])) {
                                    intPairs.Add(new IntPair(intValue0, intValue1));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    intPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < intPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool intPairChanged = EditIntPair(intPairs[i]);
                    if (intPairChanged) {
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
                intPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIntPairs(string title, string indexLabel, string valueLabel, List<IntPair> intPairs) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    intPairs.Add(new IntPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    intPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                int intValue0 = Util.StringToInt(words[0]);
                                int intValue1 = Util.StringToInt(words[1]);
                                if (!string.IsNullOrEmpty(words[0])) {
                                    intPairs.Add(new IntPair(intValue0, intValue1));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    intPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < intPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool intPairChanged = EditIntPair(indexLabel, valueLabel, intPairs[i]);
                    if (intPairChanged) {
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
                intPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

}

#endif