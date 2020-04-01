#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TagsEditor : EditorWindow {

    //A hack for string editing util to edit a tag content.
    static private int editingTagIndex = 0;
    static private Tags editingTags = null;

    //A hack for properly get the bound with of rendering these tag items.
    // static private float boundWidthCache = 0.0f;

    static public bool EditTags(string title, Tags tags, int itemsPerRow = 8) {
        int removeIndex = -1;
        bool hasChanged = false;

        EditorGUILayout.BeginVertical("FrameBox");
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel, GUILayout.Width(EditorGUILayoutPlus.CalcBoldLabelWidth(title)));
                GUILayout.FlexibleSpace();
                Color tempColor1 = GUI.color;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, GUILayout.Width(62.0f))) {
                    tags.items.Add("Tag" + tags.items.Count);
                    hasChanged = true;
                }
                GUI.color = ColorPlus.LightSalmon;
                if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                    EditorGUILayoutPlus.stringListBuffer = new List<string>(tags.items);
                }

                GUI.color = ColorPlus.LightBlue;
                if (GUILayout.Button("p", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                    if (EditorGUILayoutPlus.stringListBuffer != null && EditorGUILayoutPlus.stringListBuffer.Count > 0) {
                        tags.items.Clear();
                        tags.items.AddRange(EditorGUILayoutPlus.stringListBuffer);
                        hasChanged = true;
                    }
                }

                GUI.color = ColorPlus.SpringGreen;
                if (GUILayout.Button("PX", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    List<string> pastedMatrix = Util.StringSplitByNewLineTab(EditorGUIUtility.systemCopyBuffer);
                    tags.items.Clear();
                    tags.items.AddRange(pastedMatrix);
                    hasChanged = true;
                }
                GUI.color = Color.magenta;
                if (GUILayout.Button("CLR", EditorStyles.miniButton, GUILayout.Width(35.0f))) {
                    tags.items.Clear();
                    hasChanged = true;
                }
                GUI.color = tempColor1;
            }
            EditorGUILayout.EndHorizontal();

            if (itemsPerRow > 0) {
                EditorGUILayout.BeginVertical();
                {
                    int rowCount = (tags.items.Count / itemsPerRow) + ((tags.items.Count % itemsPerRow != 0) ? (1) : (0));
                    for (int j = 0; j < rowCount; j++) {
                        EditorGUILayout.BeginHorizontal();
                        {
                            for (int i = 0; i < itemsPerRow; ++i) {
                                int usingItemIndex = j * itemsPerRow + i;
                                if (usingItemIndex < tags.items.Count) {
                                    EditorGUILayout.BeginHorizontal();
                                    {

                                        Color tempColor = GUI.color;
                                        GUI.color = Util.GetMagicColorLighten(tags.items[usingItemIndex]);
                                        if (GUILayout.Button(tags.items[usingItemIndex], EditorStyles.miniButtonLeft, GUILayout.Width(EditorGUILayoutPlus.CalcLabelWidth(tags.items[usingItemIndex]) + 6.0f))) {
                                            editingTags = tags;
                                            editingTagIndex = i;
                                            StringEditorUtility.Open("Edit tag item", tags.items[usingItemIndex], OnStringEditUtilEditing);
                                        }

                                        GUI.color = Color.red;
                                        if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(15.0f))) {
                                            removeIndex = i;
                                        }
                                        GUI.color = tempColor;
                                    }
                                    EditorGUILayout.EndHorizontal();
                                    // Rect itemRect = GUILayoutUtility.GetLastRect();	
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndVertical();

        // Rect boundRect = GUILayoutUtility.GetLastRect();
        // boundWidthCache = boundRect.width;

        if (removeIndex != -1) {
            tags.items.RemoveAt(removeIndex);
            hasChanged = true;
        }

        return hasChanged;
    }

    static private void OnStringEditUtilEditing(string str) {
        if (editingTags != null) {
            if (editingTagIndex < editingTags.items.Count) {
                editingTags.items[editingTagIndex] = str;
            }
        }
    }
}

#endif