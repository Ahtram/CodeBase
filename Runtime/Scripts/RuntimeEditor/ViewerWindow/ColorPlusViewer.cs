#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class ColorPlusViewer : UniEditorWindow {
    private Vector2 scrollPos;

    private static EditorWindow instance;

    override public void OnGUI() {
        base.OnGUI();

        EditorGUILayout.BeginVertical("FrameBox");
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        for (int i = 0; i < ColorPlus.Colors.Length; ++i) {
                            EditorGUILayout.BeginHorizontal();
                            {
                                GUI.color = ColorPlus.Colors[i];
                                if (GUILayout.Button(((ColorPlus.Name)i).ToString(), GUILayout.MinWidth(20.0f), GUILayout.MaxWidth(300.0f))) {
                                    EditorGUIUtility.systemCopyBuffer = ((ColorPlus.Name)i).ToString();
                                }

                                GUILayout.Button(((ColorPlus.Name)i).ToString(), EditorStyles.helpBox, GUILayout.MinWidth(20.0f), GUILayout.MaxWidth(300.0f));

                                EditorGUILayout.BeginVertical("FrameBox", GUILayout.Height(17.0f), GUILayout.MinWidth(20.0f), GUILayout.MaxWidth(300.0f));
                                {
                                    GUILayout.FlexibleSpace();
                                }
                                EditorGUILayout.EndVertical();

                                GUILayout.Button(((ColorPlus.Name)i).ToString(), EditorStyles.centeredGreyMiniLabel, GUILayout.MinWidth(20.0f), GUILayout.MaxWidth(300.0f));

                                GUILayout.Label(((ColorPlus.Name)i).ToString(), EditorStyles.whiteLabel, GUILayout.MinWidth(20.0f), GUILayout.MaxWidth(300.0f));
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
                GUI.color = Color.white;
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

}

#endif