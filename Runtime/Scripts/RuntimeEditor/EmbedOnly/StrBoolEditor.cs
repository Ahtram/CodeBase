#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StrBoolEditor : EditorWindow {

    static public bool EditStrBool(StrBool strBool) {
        bool hasChanged = false;
        string newIndex = EditorGUILayoutPlus.TextField("Index", strBool.index);
        if (newIndex != strBool.index) {
            hasChanged = true;
            strBool.index = newIndex;
        }
        bool newValue = EditorGUILayoutPlus.ToggleLeft("Value", strBool.toggle);
        if (newValue != strBool.toggle) {
            hasChanged = true;
            strBool.toggle = newValue;
        }
        return hasChanged;
    }

    static public bool EditStrBool(string indexLabel, string valueLabel, StrBool strBool) {
        bool hasChanged = false;
        string newIndex = EditorGUILayoutPlus.TextField(indexLabel, strBool.index);
        if (newIndex != strBool.index) {
            hasChanged = true;
            strBool.index = newIndex;
        }
        bool newValue = EditorGUILayoutPlus.ToggleLeft(valueLabel, strBool.toggle);
        if (newValue != strBool.toggle) {
            hasChanged = true;
            strBool.toggle = newValue;
        }
        return hasChanged;
    }

    static public bool EditStrBools(string title, List<StrBool> strBools) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    strBools.Add(new StrBool());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    strBools.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string str = words[0];
                                bool b = Util.StringToBool(words[1]);
                                if (!string.IsNullOrEmpty(str)) {
                                    strBools.Add(new StrBool(str, b));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    strBools.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < strBools.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool strBoolChanged = EditStrBool(strBools[i]);
                    if (strBoolChanged) {
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
                strBools.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditStrBools(string title, string indexLabel, string valueLabel, List<StrBool> strBools) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    hasChanged = true;
                    strBools.Add(new StrBool());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    strBools.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                string str = words[0];
                                bool b = Util.StringToBool(words[1]);
                                if (!string.IsNullOrEmpty(str)) {
                                    strBools.Add(new StrBool(str, b));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    strBools.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < strBools.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool strBoolChanged = EditStrBool(indexLabel, valueLabel, strBools[i]);
                    if (strBoolChanged) {
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
                strBools.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

}

#endif