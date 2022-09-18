#if UNITY_EDITOR

namespace Teamuni.Codebase.Editor {

    using UnityEngine;
    using UnityEditor;

    public class VersionEditor : UniEditorWindow {

        private Version m_editingVersion = null;

        override public void OnGUI() {
            base.OnGUI();
            if (m_editingVersion != null) {
                //We'll use a very stupid but effective way here for now.
                //This can be done better when we upgraded to 5.6 with
                //https://docs.unity3d.com/560/Documentation/ScriptReference/Build.IPreprocessBuild.html
                bool autoIncrement = Util.IntToBool(PlayerPrefs.GetInt("AutoIncrementBuild", 0));

                GUI.color = ColorPlus.AliceBlue;
                EditorGUILayout.BeginVertical("FrameBox");
                {
                    EditorGUILayout.LabelField("Version");
                }
                EditorGUILayout.EndVertical();
                GUI.color = Color.white;

                EditorGUILayout.BeginVertical("FrameBox");
                {
                    //Read the auto increment config on this PC. This will make this option seperated from different user.
                    bool newAutoIncrement = EditorGUILayout.ToggleLeft("Auto Increment", autoIncrement);
                    if (autoIncrement != newAutoIncrement) {
                        PlayerPrefs.SetInt("AutoIncrementBuild", Util.BoolToInt(newAutoIncrement));
                        PlayerPrefs.Save();
                    }
                    EditorGUILayoutPlus.HorizontalRule();
                    m_editingVersion.major = EditorGUILayoutPlus.IntField("Major", m_editingVersion.major);
                    m_editingVersion.minor = EditorGUILayoutPlus.IntField("Minor", m_editingVersion.minor);
                    m_editingVersion.bugFix = EditorGUILayoutPlus.IntField("BugFix", m_editingVersion.bugFix);
                    m_editingVersion.build = EditorGUILayoutPlus.IntField("Build", m_editingVersion.build);
                    EditorGUILayoutPlus.HorizontalRule();
                    EditorGUILayoutPlus.TextField("Bundle Version", Version.BundleVersion());
                    EditorGUILayoutPlus.TextField("Full Version", Version.FullVersion());
                    GUILayout.FlexibleSpace();

                    GUI.color = Color.green;
                    if (GUILayout.Button("Save", EditorStyles.miniButton)) {
                        GUI.FocusControl("");
                        m_editingVersion.Serialize();
                        UnityEditor.PlayerSettings.bundleVersion = Version.BundleVersion();
                        AssetDatabase.Refresh();
                    }
                    GUI.color = Color.white;
                }
                EditorGUILayout.EndVertical();
            }
        }

        void OnEnable() {
            m_editingVersion = Version.Get();
        }

        private void OnDestroy() {
            Version.ClearCache();
            m_editingVersion = null;
        }
    }
}

#endif