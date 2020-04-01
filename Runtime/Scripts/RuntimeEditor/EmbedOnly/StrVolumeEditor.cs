#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StrVolumeEditor : EditorWindow {

    static public bool EditStrVolume(StrVolume strVolume) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical("FrameBox");
        {
            string newIndex = EditorGUILayoutPlus.TextField("Index", strVolume.index);
            if (newIndex != strVolume.index) {
                hasChanged = true;
                strVolume.index = newIndex;
            }
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayoutPlus.Separator();
                EditorGUILayout.BeginVertical();
                {
                    bool listHasChanged = EditorGUILayoutPlus.EditStringList("Values", strVolume.values);
                    if (listHasChanged) {
                        hasChanged = true;
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditStrVolume(string indexLabel, string valuesLabel, StrVolume strVolume) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical("FrameBox");
        {
            string newIndex = EditorGUILayoutPlus.TextField(indexLabel, strVolume.index);
            if (newIndex != strVolume.index) {
                hasChanged = true;
                strVolume.index = newIndex;
            }
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayoutPlus.Separator();
                EditorGUILayout.BeginVertical();
                {
                    bool listHasChanged = EditorGUILayoutPlus.EditStringList(valuesLabel, strVolume.values);
                    if (listHasChanged) {
                        hasChanged = true;
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditStrVolumes(string title, List<StrVolume> strVolumes) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;

                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    strVolumes.Add(new StrVolume());
                    hasChanged = true;
                }
                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < strVolumes.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool volumeHasChanged = EditStrVolume(strVolumes[i]);
                    if (volumeHasChanged) {
                        hasChanged = true;
                    }
                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                strVolumes.RemoveAt(deleteIndex);
                hasChanged = true;
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

    static public bool EditStrVolumes(string title, string indexLabel, string valuesLabel, List<StrVolume> strVolumes) {
        bool hasChanged = false;
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;

                if (GUILayout.Button("Add New", EditorStyles.miniButton, GUILayout.Width(62.0f))) {
                    strVolumes.Add(new StrVolume());
                    hasChanged = true;
                }
                GUI.color = tempColor1;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            int deleteIndex = -1;
            for (int i = 0; i < strVolumes.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                {
                    bool volumeHasChanged = EditStrVolume(indexLabel, valuesLabel, strVolumes[i]);
                    if (volumeHasChanged) {
                        hasChanged = true;
                    }
                    Color tempColor2 = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                        deleteIndex = i;
                    }
                    GUI.color = tempColor2;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (deleteIndex != -1) {
                strVolumes.RemoveAt(deleteIndex);
                hasChanged = true;
            }
        }
        EditorGUILayout.EndVertical();
        return hasChanged;
    }

}

#endif