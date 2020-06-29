#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class ColorPlusViewer : UniEditorWindow {
    private Vector2 scrollPos;

    private static EditorWindow instance;
    private const string REF_URL = "https://www.colorhexa.com/color-names";

    override public void OnGUI() {
        base.OnGUI();

        EditorGUILayout.BeginVertical("FrameBox");
        {
            if (GUILayout.Button("See Reference Site", EditorStyles.miniButton)) {
                Application.OpenURL(REF_URL);
            }
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
                                GUILayout.Label("[" + i.ToString("000") + "]", EditorStyles.whiteLabel, GUILayout.Width(38.0f));
                                if (GUILayout.Button(((ColorPlus.Name)i).ToString())) {
                                    EditorGUIUtility.systemCopyBuffer = ((ColorPlus.Name)i).ToString();
                                }
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