#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// Attach a set of collider on selected GameObject.
/// </summary>
public class ShapeHelperUtility : UniEditorWindow {

    private Shape shapeToAttach = Shape.New(Shape.Type.None);

    public static ShapeHelperUtility Open() {
        // Get existing open window or if none, make a new one:
        ShapeHelperUtility instance = (ShapeHelperUtility)ShowWindow<ShapeHelperUtility>();
        return instance as ShapeHelperUtility;
    }

    public override void OnGUI() {
        base.OnGUI();
        EditorGUILayout.BeginVertical("FrameBox");
        {
            //Edit the shape data.
            if (shapeToAttach != null) {
                Shape.Type newShapeType = (Shape.Type)EditorGUILayout.EnumPopup(shapeToAttach.type);
                if (newShapeType != shapeToAttach.type) {
                    shapeToAttach = Shape.New(newShapeType);
                }
                ShapeEditor.EditShape(shapeToAttach);
                GUI.color = ColorPlus.LightYellow;
                if (GUILayout.Button("Attach Collider2D To Selected", EditorStyles.miniButton)) {
                    if (Selection.activeGameObject != null) {
                        shapeToAttach.AttachCollider2D(Selection.activeGameObject, true);
                    }
                }

                if (GUILayout.Button("Attach MeshRenderer To Selected", EditorStyles.miniButton)) {
                    if (Selection.activeGameObject != null) {
                        shapeToAttach.AttachMeshRenderer(Selection.activeGameObject);
                    }
                }

                if (GUILayout.Button("Attach LineRenderer To Selected", EditorStyles.miniButton)) {
                    if (Selection.activeGameObject != null) {
                        shapeToAttach.AttachLineRenderer(Selection.activeGameObject);
                    }
                }
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndVertical();
    }

}

#endif