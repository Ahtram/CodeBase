#if UNITY_EDITOR

namespace Teamuni.Codebase.Editor {

    using UnityEngine;
    using UnityEditor;

    public class ExampleDataEditorUtility : UniEditorWindow {

        private Vector2 m_scrollPos = Vector2.zero;
        public ExampleData editingData = null;

        static public ExampleDataEditorUtility CreateWindow(ExampleData exampleData) {
            // Get existing open window or if none, make a new one:
            ExampleDataEditorUtility instance = (ExampleDataEditorUtility)CreateWindow<ExampleDataEditorUtility>();
            instance.editingData = exampleData;
            if (exampleData != null) {
                instance.titleContent = new GUIContent(exampleData.ID);
            } else {
                instance.titleContent = new GUIContent("Null Content");
            }
            return instance as ExampleDataEditorUtility;
        }

        //Return the editing stuff.
        public ExampleData DisplayingContent() {
            return editingData;
        }

        //Return the editing stuff.
        public string DisplayingContentID() {
            return editingData.ID;
        }

        public void UpdateData(ExampleData exampleData) {
            editingData = exampleData;
            if (exampleData != null) {
                titleContent = new GUIContent(exampleData.ID);
            } else {
                titleContent = new GUIContent("Null Content");
            }
        }

        override public void OnGUI() {
            base.OnGUI();
            if (editingData != null) {
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                {
                    DrawExampleData(editingData);
                }
                EditorGUILayout.EndScrollView();
            } else {
                EditorGUILayout.LabelField("No data to edit.");
            }
        }

        static public void DrawExampleData(ExampleData exampleData) {

            //This means this data need to be serialize when saved.
            exampleData.MarkDirty();

            EditorGUILayout.BeginVertical("FrameBox");
            {
                EditorGUILayout.LabelField("Implement your editor UI here.");
                exampleData.var = EditorGUILayoutPlus.IntField("Var", exampleData.var);
                EditorGUILayout.BeginVertical();
                {
                    TagsEditor.EditTags("Example Tags", exampleData.exampleTags);
                    EditorGUILayoutPlus.IDSelection("Select a text data", ExampleData.GetIDCollection(), exampleData.IDField, (selectedID) => { exampleData.IDField = selectedID; }, null);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayoutPlus.EditIDList("Select IDs", ExampleData.GetIDCollection(), exampleData.IDList, null);
                EditorGUILayoutPlus.AssetNameSelection("Select a test prefab name", "Assets/CodeBase/Resources/Prefabs", exampleData.PrefabNameField, "prefab", (selectedPrefabName) => exampleData.PrefabNameField = (string)selectedPrefabName);

                EditorGUILayoutPlus.EditToggleLeftList("ToggleList", "Toggle", exampleData.boolList);

                EditorGUILayout.BeginVertical("FrameBox");
                {
                    IDPairEditor.EditIDPairs("Test IDPair", "Label1", "Label2", exampleData.IDPairList, ExampleData.GetIDCollection(), ExampleData.GetIDCollection(), null, null, null, null);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("FrameBox");
            {
                EditorGUILayoutPlus.EditAssetNameList("Select Prefabs", "Assets/CodeBase/Resources/Prefabs", exampleData.PrefabList, "prefab");
                EditorGUILayoutPlus.HorizontalLine();
                EditorGUILayoutPlus.EditAssetNameList("Select Prefabs", "Assets/CodeBase/Resources/Prefabs", exampleData.PrefabList1, "prefab");
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("FrameBox");
            {
                //Test edit shape data.
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayoutPlus.LabelField("Shape", false);
                    Shape.Type newShapeType = (Shape.Type)EditorGUILayout.EnumPopup(exampleData.shapeStuff.type);
                    if (newShapeType != exampleData.shapeStuff.type) {
                        exampleData.shapeStuff = Shape.New(newShapeType);
                    }
                    GUI.color = ColorPlus.White;
                }
                EditorGUILayout.EndHorizontal();
                ShapeEditor.EditShape(exampleData.shapeStuff);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("FrameBox");
            {
                //Test edit vector/quaternion field.
                exampleData.vector3Stuff = EditorGUILayoutPlus.Vec3Field(exampleData.vector3Stuff);
                exampleData.vector2Stuff = EditorGUILayoutPlus.Vec2Field(exampleData.vector2Stuff);
                exampleData.vector4Stuff = EditorGUILayoutPlus.Vec4Field(exampleData.vector4Stuff);
                exampleData.quaternionStuff = EditorGUILayoutPlus.Vec4Field(exampleData.quaternionStuff);
            }
            EditorGUILayout.EndVertical();
        }
    }

}

#endif