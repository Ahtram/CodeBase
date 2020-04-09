#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GameObjectPoolEditor : UniEditorWindow {

    private Vector2 m_scrollPos;

    //Editing objectPool.
    private GameObjectPool m_objectPool = null;

    //[MenuItem("{uni}/A/ObjectPool Editor")]
    static public void OpenWindow() {
        GameObjectPoolEditor instance = (GameObjectPoolEditor)EditorWindow.GetWindow(typeof(GameObjectPoolEditor), false, "GameObjectPool", true);
        instance.minSize = new Vector2(640, 480);
        instance.Show();
    }

    override public void OnGUI() {
        if (m_objectPool != null) {
            GUI.color = Color.green;
            EditorGUILayout.HelpBox("There are [" + m_objectPool.loadOnAwake.Count + "] type of prefabs in this object pool. Current scene: " + EditorSceneManager.GetActiveScene().path, MessageType.Info);
            GUI.color = Color.white;
            m_objectPool.asyncPreload = EditorGUILayoutPlus.ToggleLeft("Async Preload", m_objectPool.asyncPreload);
            int removeAtIndex = -1;
            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
            {
                for (int i = 0; i < m_objectPool.loadOnAwake.Count; i++) {
                    EditorGUILayout.BeginVertical("FrameBox");
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUI.color = Color.yellow;
                            EditorGUILayout.LabelField("Path", GUILayout.Width(50.0f));
                            m_objectPool.loadOnAwake[i].path = EditorGUILayoutPlus.TextField(m_objectPool.loadOnAwake[i].path);

                            GUI.color = ColorPlus.MediumVioletRed;
                            if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(15.0f))) {
                                removeAtIndex = i;
                            }
                            GUI.color = Color.white;
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        {
                            GUI.color = Color.yellow;
                            EditorGUILayout.LabelField("Count", GUILayout.Width(50.0f));
                            m_objectPool.loadOnAwake[i].count = EditorGUILayoutPlus.IntField(m_objectPool.loadOnAwake[i].count);
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        {
                            GUI.color = Color.magenta;
                            EditorGUILayout.LabelField("Drag prefab here", GUILayout.Width(110.0f));
                            Object dragObject = EditorGUILayout.ObjectField(null, typeof(Object), false);
                            if (dragObject != null) {
                                //Get this object's path in asset database.
                                string pathOfObject = AssetDatabase.GetAssetOrScenePath(dragObject);
                                if (pathOfObject.EndsWith(".prefab")) {
                                    if (pathOfObject.StartsWith("Assets/Resources/")) {
                                        //Legal prefab.
                                        string trimedPath = pathOfObject.Remove(pathOfObject.LastIndexOf(".")).Remove(0, 17);
                                        m_objectPool.loadOnAwake[i].path = trimedPath;
                                    } else {
                                        Debug.LogWarning("This object doesnt location in [Assets/Resources/]!");
                                    }
                                } else {
                                    Debug.LogWarning("This is not a prefab Object! Try make it a prefab!");
                                }
                            }
                            GUI.color = Color.white;
                        }
                        EditorGUILayout.EndHorizontal();

                        if (Resources.Load(m_objectPool.loadOnAwake[i].path) == null) {
                            EditorGUILayout.HelpBox("Oops! Cannot load this resource. Better check it out!", MessageType.Warning);
                        }

                        if (m_objectPool.loadOnAwake[i].count <= 0) {
                            EditorGUILayout.HelpBox("Oops! There is a problem of this count, you sure you wanna do this?", MessageType.Warning);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndScrollView();

            if (removeAtIndex != -1) {
                m_objectPool.loadOnAwake.RemoveAt(removeAtIndex);
            }

            GUI.color = Color.cyan;
            if (GUILayout.Button("New", EditorStyles.miniButton)) {
                m_objectPool.loadOnAwake.Add(new GameObjectPool.PathCount());
            }
            GUI.color = Color.white;

        } else {
            EditorGUILayout.HelpBox("You don't have a GameObject called [ObjectPool] with ObjectPool component on it.", MessageType.Info);
        }

        base.OnGUI();
    }

    void OnEnable() {
        //Search for a gameobject call "ObjectPool".
        GameObject objectPoolGO = GameObject.Find("ObjectPool");
        if (objectPoolGO != null) {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            m_objectPool = objectPoolGO.GetComponent<GameObjectPool>();
        }
    }

    void OnDestroy() {
        m_objectPool = null;
    }

}

#endif