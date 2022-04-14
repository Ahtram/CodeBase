#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class IntFloatEditor : EditorWindow {

    static public bool EditIntFloat(IntFloat intFloat) {
        bool hasChanged = false;
        int newIndex = EditorGUILayoutPlus.IntField("Index", intFloat.index);
        if (newIndex != intFloat.index) {
            hasChanged = true;
            intFloat.index = newIndex;
        }
        float newValue = EditorGUILayoutPlus.FloatField("Value", intFloat.value);
        if (newValue != intFloat.value) {
            hasChanged = true;
            intFloat.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditIntFloat(string indexLabel, string valueLabel, IntFloat intFloat) {
        bool hasChanged = false;
        int newIndex = EditorGUILayoutPlus.IntField(indexLabel, intFloat.index);
        if (newIndex != intFloat.index) {
            hasChanged = true;
            intFloat.index = newIndex;
        }
        float newValue = EditorGUILayoutPlus.FloatField(valueLabel, intFloat.value);
        if (newValue != intFloat.value) {
            hasChanged = true;
            intFloat.value = newValue;
        }
        return hasChanged;
    }

    static public bool EditIntFloats(string title, List<IntFloat> intFloats) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                    hasChanged = true;
                    intFloats.Add(new IntFloat());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    intFloats.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                int intValue = Util.StringToInt(words[0]);
                                float floatValue = Util.StringToFloat(words[1]);
                                if (!string.IsNullOrEmpty(words[0])) {
                                    intFloats.Add(new IntFloat(intValue, floatValue));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    intFloats.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < intFloats.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool intFloatChanged = EditIntFloat(intFloats[i]);
                    if (intFloatChanged) {
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
                intFloats.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditIntFloats(string title, string indexLabel, string valueLabel, List<IntFloat> intFloats) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(66.0f))) {
                    hasChanged = true;
                    intFloats.Add(new IntFloat());
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    intFloats.Clear();
                    if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer)) {
                        string[] lines = EditorGUIUtility.systemCopyBuffer.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Length; i++) {
                            string[] words = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (words.Length >= 2) {
                                int intValue = Util.StringToInt(words[0]);
                                float floatValue = Util.StringToFloat(words[1]);
                                if (!string.IsNullOrEmpty(words[0])) {
                                    intFloats.Add(new IntFloat(intValue, floatValue));
                                }
                            }
                        }
                    }
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    intFloats.Clear();
                    hasChanged = true;
                }

                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < intFloats.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool intFloatChanged = EditIntFloat(indexLabel, valueLabel, intFloats[i]);
                    if (intFloatChanged) {
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
                intFloats.RemoveAt(deleteIndex);
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

}

#endif