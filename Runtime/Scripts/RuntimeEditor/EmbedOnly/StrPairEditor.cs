#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StrPairEditor : EditorWindow {

    static public bool EditStrPair(StrPair strPair) {
        bool hasChanged = false;
        string newIndex = EditorGUILayoutPlus.TextField("Index", strPair.index);
        if (newIndex != strPair.index) {
            hasChanged = true;
            strPair.index = newIndex;
        }
        string newValue = EditorGUILayoutPlus.TextField("Value", strPair.value);
        if (newValue != strPair.value) {
            hasChanged = true;
            strPair.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditStrPair(string indexLabel, string valueLabel, StrPair strPair) {
        bool hasChanged = false;
        string newIndex = EditorGUILayoutPlus.TextField(indexLabel, strPair.index);
        if (newIndex != strPair.index) {
            hasChanged = true;
            strPair.index = newIndex;
        }
        string newValue = EditorGUILayoutPlus.TextField(valueLabel, strPair.value);
        if (newValue != strPair.value) {
            hasChanged = true;
            strPair.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditStrPairs(string title, List<StrPair> strPairs) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    strPairs.Add(new StrPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    strPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string str0 = words[0];
                                string str1 = words[1];
                                if (!string.IsNullOrEmpty(str0)) {
                                    strPairs.Add(new StrPair(str0, str1));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    strPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < strPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool strPairChanged = EditStrPair(strPairs[i]);
                    if (strPairChanged) {
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
                strPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditStrPairs(string title, string indexLabel, string valueLabel, List<StrPair> strPairs) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    strPairs.Add(new StrPair());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    strPairs.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string str0 = words[0];
                                string str1 = words[1];
                                if (!string.IsNullOrEmpty(str0)) {
                                    strPairs.Add(new StrPair(str0, str1));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    strPairs.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < strPairs.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool strPairChanged = EditStrPair(indexLabel, valueLabel, strPairs[i]);
                    if (strPairChanged) {
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
                strPairs.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

}

#endif