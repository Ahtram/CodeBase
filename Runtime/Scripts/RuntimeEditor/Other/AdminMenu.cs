#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

namespace Teamuni.Codebase.Editor {
    /// <summary>
    /// The main menu for our home made editors. And of course the one is an UniEditorWindow too.
    /// </summary>
    public class AdminMenu : UniEditorWindow {

        private Vector2 scrollPos;

        //Structure of an editor menu item.
        public class EditorMenuItem {
            public string name;
            public KeyCode key = KeyCode.Clear;
            public Func<UniEditorWindow> openFunc = null;
            public Action openAction = null;

            public EditorMenuItem(string nameInput, KeyCode keyCodeInput, Func<UniEditorWindow> func) {
                name = nameInput;
                key = keyCodeInput;
                openFunc = func;
            }

            public EditorMenuItem(string nameInput, KeyCode keyCodeInput, Action action) {
                name = nameInput;
                key = keyCodeInput;
                openAction = action;
            }

            public void OpenEditor() {
                if (openFunc != null) {
                    openFunc();
                }
                if (openAction != null) {
                    openAction();
                }
            }
        }

        //Define the menu instance: Add new items here in the future.
        private EditorMenuItem[] editorMenu = new EditorMenuItem[] {
        new EditorMenuItem("BaseConfig", KeyCode.B, (Func<UniEditorWindow>)ShowWindow<BaseConfigEditor>),
        new EditorMenuItem("KeyConfig", KeyCode.K, (Func<UniEditorWindow>)ShowWindow<KeyConfigEditor>),
        new EditorMenuItem("Version", KeyCode.V, (Func<UniEditorWindow>)ShowWindow<VersionEditor>),
        new EditorMenuItem("ShapeHelperUtility", KeyCode.H, (Func<UniEditorWindow>)ShowWindow<ShapeHelperUtility>)
    };

        //Define the menu instance: Add new items here in the future.
        private EditorMenuItem[] viewerMenu = new EditorMenuItem[] {
        new EditorMenuItem("ColorPlus", KeyCode.C, (Func<UniEditorWindow>)ShowWindow<ColorPlusViewer>)
    };

        //Define the menu instance: Add new items here in the future.
        private EditorMenuItem[] helperMenu = new EditorMenuItem[] {
        new EditorMenuItem("Collect Game Text", KeyCode.G, CollectGameText)
    };

        //Define the menu instance: Add new items here in the future.
        private EditorMenuItem[] exampleMenu = new EditorMenuItem[] {
        new EditorMenuItem("Example", KeyCode.X, (Func<UniEditorWindow>)ShowWindow<ExampleEditor>)
    };

        [MenuItem("{uni}/AdminMenu %g", false, 0)]
        public static void OpenEditorMenu() {
            // Get existing open window or if none, make a new one:
            ShowWindow<AdminMenu>();
        }

        override public void OnGUI() {
            base.OnGUI();

            int openEditorIndex = -1;
            int openViewerIndex = -1;
            int useHelperIndex = -1;
            int openExampleIndex = -1;
            EditorGUILayout.BeginVertical("FrameBox");
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    //Draw Editor Buttons.
                    GUI.color = ColorPlus.Aqua;
                    EditorGUILayout.HelpBox("Editors", MessageType.None);
                    GUI.color = Color.white;
                    for (int i = 0; i < editorMenu.Length; i++) {
                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button(editorMenu[i].name, EditorStyles.miniButton)) {
                                openEditorIndex = i;
                            }

                            GUILayout.Label(editorMenu[i].key.ToString(), EditorStyles.miniLabel, GUILayout.Width(25.0f));
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    //Draw Viewer Buttons.
                    GUI.color = ColorPlus.Chartreuse;
                    EditorGUILayout.HelpBox("Viewers", MessageType.None);
                    GUI.color = Color.white;
                    for (int i = 0; i < viewerMenu.Length; i++) {
                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button(viewerMenu[i].name, EditorStyles.miniButton)) {
                                openViewerIndex = i;
                            }

                            GUILayout.Label(viewerMenu[i].key.ToString(), EditorStyles.miniLabel, GUILayout.Width(25.0f));
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    //Draw Helper Buttons.
                    GUI.color = ColorPlus.MediumPurple;
                    EditorGUILayout.HelpBox("Helpers", MessageType.None);
                    GUI.color = Color.white;
                    for (int i = 0; i < helperMenu.Length; i++) {
                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button(helperMenu[i].name, EditorStyles.miniButton)) {
                                useHelperIndex = i;
                            }

                            GUILayout.Label(helperMenu[i].key.ToString(), EditorStyles.miniLabel, GUILayout.Width(25.0f));
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    //Draw Example Buttons.
                    GUI.color = ColorPlus.Orange;
                    EditorGUILayout.HelpBox("Examples", MessageType.None);
                    GUI.color = Color.white;
                    for (int i = 0; i < exampleMenu.Length; i++) {
                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button(exampleMenu[i].name, EditorStyles.miniButton)) {
                                openExampleIndex = i;
                            }

                            GUILayout.Label(exampleMenu[i].key.ToString(), EditorStyles.miniLabel, GUILayout.Width(25.0f));
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    GUI.color = Color.white;

                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            if (openEditorIndex != -1) {
                editorMenu[openEditorIndex].OpenEditor();
                Close();
            }

            if (openViewerIndex != -1) {
                viewerMenu[openViewerIndex].OpenEditor();
                Close();
            }

            if (useHelperIndex != -1) {
                helperMenu[useHelperIndex].OpenEditor();
                Close();
            }

            if (openExampleIndex != -1) {
                exampleMenu[openExampleIndex].OpenEditor();
                Close();
            }

            //Hotkeys: read the correspond hotkey from EditorMenuItem data and open the editor.
            if (!ctrlDown && !cmdDown) {
                switch (Event.current.type) {
                    case EventType.KeyDown:
                        if (Event.current.keyCode == KeyCode.Clear)
                            break;

                        for (int i = 0; i < editorMenu.Length; i++) {
                            if (editorMenu[i].key == Event.current.keyCode) {
                                editorMenu[i].OpenEditor();
                                Close();
                                break;
                            }
                        }

                        for (int i = 0; i < viewerMenu.Length; i++) {
                            if (viewerMenu[i].key == Event.current.keyCode) {
                                viewerMenu[i].OpenEditor();
                                Close();
                                break;
                            }
                        }

                        for (int i = 0; i < helperMenu.Length; i++) {
                            if (helperMenu[i].key == Event.current.keyCode) {
                                helperMenu[i].OpenEditor();
                                Close();
                                break;
                            }
                        }

                        for (int i = 0; i < exampleMenu.Length; i++) {
                            if (exampleMenu[i].key == Event.current.keyCode) {
                                exampleMenu[i].OpenEditor();
                                Close();
                                break;
                            }
                        }

                        break;
                }
            }
        }

        //================ Helper functions ================

        /// <summary>
        /// 收集所有在語系資料夾底下的所有Text文字檔字元並且整理後存成一個 Text file (提供給 TextMesh Pro 類似的工具使用)
        /// </summary>
        static private void CollectGameText() {
            string localizedAssetsFolderPath = "Assets/Resources/Data/Localized";
            string[] guids = AssetDatabase.FindAssets(" t:" + "TextAsset", new string[] { localizedAssetsFolderPath });

            List<char> collectedCharacters = new List<char>();

            EditorUtility.DisplayProgressBar("Collecting Game Text", "Reading assets...", 0.0f);

            for (int i = 0; i < guids.Length; i++) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath) as TextAsset;
                if (textAsset != null) {
                    for (int j = 0; j < textAsset.text.Length; j++) {
                        EditorUtility.DisplayProgressBar("Collecting Game Text", "Reading assets...[" + i + "/" + guids.Length + "] [" + j + "/" + textAsset.text.Length + "]", (float)(i + 2) / (float)(guids.Length + 1));
                        if (!collectedCharacters.Contains(textAsset.text[j])) {
                            collectedCharacters.Add(textAsset.text[j]);
                        }
                    }
                }
            }

            string collectedCharacterString = "";
            for (int i = 0; i < collectedCharacters.Count; i++) {
                collectedCharacterString += collectedCharacters[i];
            }

            StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/" + SysPath.TrivialDataPath + "CollectedText.txt", false, Encoding.UTF8);
            sw.Write(collectedCharacterString);
            sw.Close();

            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resources/" + SysPath.TrivialDataPath + "CollectedText.txt");

            Debug.Log("Collect Text: [" + collectedCharacters.Count + "]");
        }

    }
}

#endif