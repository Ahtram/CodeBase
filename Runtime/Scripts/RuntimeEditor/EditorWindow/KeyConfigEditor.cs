#if UNITY_EDITOR

namespace Teamuni.Codebase.Editor {

    using UnityEngine;
    using UnityEditor;

    public class KeyConfigEditor : UniEditorWindow {

        private Vector2 m_scrollPos = Vector2.zero;

        private KeyConfig m_editingConfig = null;
        private KeyConfig.DataLocation m_configFileDataLocation = KeyConfig.DataLocation.AssetDataPath;

        private bool m_isListeningKeyboardKey = false;
        private int m_listeningKeyboardKeyIndex = -1;

        private bool m_isListeningKeyboardSubKey = false;
        private int m_listeningKeyboardSubKeyIndex = -1;

        private bool m_isListeningKeyboardExtraKey = false;
        private int m_listeningKeyboardExtraKeyIndex = -1;

        private bool m_isListeningKeyboardExtandKey = false;
        private int m_listeningKeyboardExtandKeyIndex = -1;

        override public void OnGUI() {
            base.OnGUI();
            KeyConfig.DataLocation newDataLocation = (KeyConfig.DataLocation)EditorGUILayout.EnumPopup(m_configFileDataLocation);
            if (m_configFileDataLocation != newDataLocation) {
                //Load the editing config.
                m_configFileDataLocation = newDataLocation;
                m_editingConfig = KeyConfig.GetWithNoCache(m_configFileDataLocation);
            }
            EditorGUILayoutPlus.HorizontalLine();

            if (m_editingConfig != null) {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    EditorGUILayout.BeginVertical("FrameBox");
                    {
                        EditorGUILayout.LabelField("Keyboard Keys", EditorStyles.boldLabel);
                        for (int i = 0; i < m_editingConfig.keyboardSetting.Count; i++) {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField(((KeyConfig.Key)i).ToString(), GUILayout.Width(60.0f));

                                bool isListeningThisKey = (m_isListeningKeyboardKey && m_listeningKeyboardKeyIndex == i) ? (true) : (false);

                                //Detect by key press...(This actually has some problem due to Unity's limitation. Some key's will not be able to set by this way)
                                bool clicked = GUILayout.Toggle(isListeningThisKey, m_editingConfig.keyboardSetting[i].ToString(), EditorStyles.miniButton, GUILayout.MinWidth(10.0f), GUILayout.MaxWidth(500.0f));
                                if (isListeningThisKey != clicked) {
                                    if (clicked) {
                                        StartListenKeyboardKey(i);
                                    } else {
                                        StopListenKeyboardKey();
                                    }
                                }
                                //Dropdown (The list is very long!)
                                m_editingConfig.keyboardSetting[i] = (KeyCode)EditorGUILayout.EnumPopup(m_editingConfig.keyboardSetting[i], GUILayout.MinWidth(10.0f), GUILayout.MaxWidth(500.0f));

                                bool isListeningThisSubKey = (m_isListeningKeyboardSubKey && m_listeningKeyboardSubKeyIndex == i) ? (true) : (false);
                                bool clickedSub = GUILayout.Toggle(isListeningThisSubKey, m_editingConfig.keyboardSettingSub[i].ToString(), EditorStyles.miniButton, GUILayout.MinWidth(10.0f), GUILayout.MaxWidth(500.0f));
                                if (isListeningThisSubKey != clickedSub) {
                                    if (clickedSub) {
                                        StartListenKeyboardSubKey(i);
                                    } else {
                                        StopListenKeyboardSubKey();
                                    }
                                }
                                m_editingConfig.keyboardSettingSub[i] = (KeyCode)EditorGUILayout.EnumPopup(m_editingConfig.keyboardSettingSub[i], GUILayout.MinWidth(10.0f), GUILayout.MaxWidth(500.0f));

                                bool isListeningThisExtraKey = (m_isListeningKeyboardExtraKey && m_listeningKeyboardExtraKeyIndex == i) ? (true) : (false);
                                bool clickedExtra = GUILayout.Toggle(isListeningThisExtraKey, m_editingConfig.keyboardSettingExtra[i].ToString(), EditorStyles.miniButton, GUILayout.MinWidth(10.0f), GUILayout.MaxWidth(500.0f));
                                if (isListeningThisExtraKey != clickedExtra) {
                                    if (clickedExtra) {
                                        StartListenKeyboardExtraKey(i);
                                    } else {
                                        StopListenKeyboardExtraKey();
                                    }
                                }
                                m_editingConfig.keyboardSettingExtra[i] = (KeyCode)EditorGUILayout.EnumPopup(m_editingConfig.keyboardSettingExtra[i], GUILayout.MinWidth(10.0f), GUILayout.MaxWidth(500.0f));

                                bool isListeningThisExtandKey = (m_isListeningKeyboardExtandKey && m_listeningKeyboardExtandKeyIndex == i) ? (true) : (false);
                                bool clickedExtand = GUILayout.Toggle(isListeningThisExtandKey, m_editingConfig.keyboardSettingExtand[i].ToString(), EditorStyles.miniButton, GUILayout.MinWidth(10.0f), GUILayout.MaxWidth(500.0f));
                                if (isListeningThisExtandKey != clickedExtand) {
                                    if (clickedExtand) {
                                        StartListenKeyboardExtandKey(i);
                                    } else {
                                        StopListenKeyboardExtandKey();
                                    }
                                }
                                m_editingConfig.keyboardSettingExtand[i] = (KeyCode)EditorGUILayout.EnumPopup(m_editingConfig.keyboardSettingExtand[i], GUILayout.MinWidth(10.0f), GUILayout.MaxWidth(500.0f));
                            }
                            EditorGUILayout.EndHorizontal();
                        }

                        //===================================================
                        EditorGUILayoutPlus.HorizontalRule();

                        EditorGUILayout.LabelField("Controller Keys", EditorStyles.boldLabel);
                        for (int i = 0; i < m_editingConfig.controllerSetting.Count; i++) {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField(((KeyConfig.Key)i).ToString(), GUILayout.Width(60.0f));
                                Nereus.InputType newInputType = (Nereus.InputType)EditorGUILayout.EnumPopup(m_editingConfig.controllerSetting[i]);
                                if (newInputType != m_editingConfig.controllerSetting[i]) {
                                    m_editingConfig.SetControllerKey((KeyConfig.Key)i, newInputType);
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();

                GUILayout.FlexibleSpace();

                if (m_configFileDataLocation == KeyConfig.DataLocation.PersistentDataPath) {
                    GUI.color = ColorPlus.Orange;
                    if (GUILayout.Button("Copy From AssetDataPath Version", EditorStyles.miniButton)) {
                        GUI.FocusControl("");
                        StopListenKeyboardKey();

                        KeyConfig assetKeyConfig = KeyConfig.GetWithNoCache(KeyConfig.DataLocation.AssetDataPath);
                        m_editingConfig = assetKeyConfig;
                    }
                }

                GUI.color = Color.magenta;
                if (GUILayout.Button("Restore Default", EditorStyles.miniButton)) {
                    GUI.FocusControl("");
                    StopListenKeyboardKey();
                    m_editingConfig.RestoreDefault();
                }

                GUI.color = Color.green;
                if (GUILayout.Button("Save", EditorStyles.miniButton)) {
                    GUI.FocusControl("");
                    StopListenKeyboardKey();
                    m_editingConfig.Serialize(m_configFileDataLocation);
                    AssetDatabase.Refresh();
                }
                GUI.color = Color.white;

                //Listen to Keyboard or Controller key.
                if (m_isListeningKeyboardKey && m_listeningKeyboardKeyIndex != -1) {
                    if (Event.current.isKey && Event.current.type == EventType.KeyDown) {
                        //Change the setting.
                        m_editingConfig.SetKeyboardKey((KeyConfig.Key)m_listeningKeyboardKeyIndex, Event.current.keyCode);
                        StopListenKeyboardKey();
                        Repaint();
                    }
                }

                if (m_isListeningKeyboardSubKey && m_listeningKeyboardSubKeyIndex != -1) {
                    if (Event.current.isKey && Event.current.type == EventType.KeyDown) {
                        //Change the setting.
                        m_editingConfig.SetKeyboardSubKey((KeyConfig.Key)m_listeningKeyboardSubKeyIndex, Event.current.keyCode);
                        StopListenKeyboardSubKey();
                        Repaint();
                    }
                }

                if (m_isListeningKeyboardExtraKey && m_listeningKeyboardExtraKeyIndex != -1) {
                    if (Event.current.isKey && Event.current.type == EventType.KeyDown) {
                        //Change the setting.
                        m_editingConfig.SetKeyboardExtraKey((KeyConfig.Key)m_listeningKeyboardExtraKeyIndex, Event.current.keyCode);
                        StopListenKeyboardExtraKey();
                        Repaint();
                    }
                }

                if (m_isListeningKeyboardExtandKey && m_listeningKeyboardExtandKeyIndex != -1) {
                    if (Event.current.isKey && Event.current.type == EventType.KeyDown) {
                        //Change the setting.
                        m_editingConfig.SetKeyboardExtandKey((KeyConfig.Key)m_listeningKeyboardExtandKeyIndex, Event.current.keyCode);
                        StopListenKeyboardExtandKey();
                        Repaint();
                    }
                }
            }
        }

        private void StartListenKeyboardKey(int index) {
            m_isListeningKeyboardKey = true;
            m_listeningKeyboardKeyIndex = index;
        }

        private void StopListenKeyboardKey() {
            m_isListeningKeyboardKey = false;
            m_listeningKeyboardKeyIndex = -1;
        }

        private void StartListenKeyboardSubKey(int index) {
            m_isListeningKeyboardSubKey = true;
            m_listeningKeyboardSubKeyIndex = index;
        }

        private void StopListenKeyboardSubKey() {
            m_isListeningKeyboardSubKey = false;
            m_listeningKeyboardSubKeyIndex = -1;
        }

        private void StartListenKeyboardExtraKey(int index) {
            m_isListeningKeyboardExtraKey = true;
            m_listeningKeyboardExtraKeyIndex = index;
        }

        private void StopListenKeyboardExtraKey() {
            m_isListeningKeyboardExtraKey = false;
            m_listeningKeyboardExtraKeyIndex = -1;
        }

        private void StartListenKeyboardExtandKey(int index) {
            m_isListeningKeyboardExtandKey = true;
            m_listeningKeyboardExtandKeyIndex = index;
        }

        private void StopListenKeyboardExtandKey() {
            m_isListeningKeyboardExtandKey = false;
            m_listeningKeyboardExtandKeyIndex = -1;
        }

        private void OnEnable() {
            m_editingConfig = KeyConfig.GetWithNoCache(m_configFileDataLocation);
        }

        private void OnDestroy() {
            KeyConfig.ClearCache();
            m_editingConfig = null;
        }

    }

}

#endif