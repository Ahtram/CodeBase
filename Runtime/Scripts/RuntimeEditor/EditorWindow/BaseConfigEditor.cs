using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseConfigEditor : UniEditorWindow {

    private Vector2 m_scrollPos = Vector2.zero;
    private BaseConfig m_editingConfig = null;

    override public void OnGUI() {
        base.OnGUI();
        if (m_editingConfig != null) {

            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
            {
                EditorGUILayout.BeginVertical("FrameBox");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayoutPlus.LabelField("SerializeType For DataUtil", false);
                        m_editingConfig.dataSerializeType = (BaseConfig.DataType)EditorGUILayout.EnumPopup(m_editingConfig.dataSerializeType);
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayoutPlus.LabelField("DeserializeType For DataUtil", false);
                        m_editingConfig.dataDeserializeType = (BaseConfig.DataType)EditorGUILayout.EnumPopup(m_editingConfig.dataDeserializeType);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            GUI.color = Color.green;
            if (GUILayout.Button("Save", EditorStyles.miniButton)) {
                GUI.FocusControl("");
                m_editingConfig.Serialize();
                AssetDatabase.Refresh();
            }
            GUI.color = Color.white;
        }
    }

    private void OnEnable() {
        m_editingConfig = BaseConfig.GetWithNoCache();
    }

    private void OnDestroy() {
        BaseConfig.ClearCache();
        m_editingConfig = null;
    }

}
