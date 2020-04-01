#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Provide edit methods for editing Shape data.
public class ShapeEditor : EditorWindow {

    /// <summary>
    /// Edit a Shape data.
    /// </summary>
    /// <param name="shape"></param>
    /// <returns></returns>
    static public bool EditShape(Shape shape) {
        if (shape != null) {
            switch (shape.type) {
                case Shape.Type.Box:
                    return EditBoxShape(shape as BoxShape);
                case Shape.Type.Polygon:
                    return EditPolygonShape(shape as PolygonShape);
                case Shape.Type.FlatParallelogram:
                    return EditFlatParallelogramShape(shape as FlatParallelogramShape);
                case Shape.Type.SlopeEllipse:
                    return EditSlopeEllipseShape(shape as SlopeEllipseShape);
                default:
                case Shape.Type.None:
                    return EditNoneShape(shape as NoneShape);
            }
        }
        return false;
    }

    //-------------------------------------------

    /// <summary>
    /// Edit a box shape data.
    /// </summary>
    /// <param name="boxShape"></param>
    /// <returns></returns>
    static public bool EditBoxShape(BoxShape boxShape) {
        if (boxShape != null) {
            bool hasChanged = false;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayoutPlus.LabelField("Size", false);
                Vec2 newVec2 = EditorGUILayoutPlus.Vec2Field(boxShape.size);
                if (!newVec2.Equals(boxShape.size)) {
                    boxShape.size = newVec2;
                    hasChanged = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            return hasChanged;
        }
        return false;
    }

    /// <summary>
    /// Edit a PolygonShape data.
    /// </summary>
    /// <param name="polygonShape"></param>
    /// <returns></returns>
    static public bool EditPolygonShape(PolygonShape polygonShape) {
        if (polygonShape != null) {
            polygonShape.meshThreshold = EditorGUILayoutPlus.FloatField("Mesh Threshold", polygonShape.meshThreshold);
            return EditorGUILayoutPlus.EditVec2List("Polygon Pathes", polygonShape.points);
        }
        return false;
    }

    /// <summary>
    /// Edit a FlatParallelogram data.
    /// </summary>
    /// <param name="polygonShape"></param>
    /// <returns></returns>
    static public bool EditFlatParallelogramShape(FlatParallelogramShape flatParallelogramShape) {
        if (flatParallelogramShape != null) {
            bool hasChanged = false;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayoutPlus.LabelField("Size", false);
                Vec2 newVec2 = EditorGUILayoutPlus.Vec2Field(flatParallelogramShape.size);
                if (!newVec2.Equals(flatParallelogramShape.size)) {
                    flatParallelogramShape.size = newVec2;
                    hasChanged = true;
                }
            }
            EditorGUILayout.EndHorizontal();

            float newSideSlope = EditorGUILayoutPlus.FloatField("Slope", flatParallelogramShape.sideSlope);
            if (flatParallelogramShape.sideSlope != newSideSlope) {
                flatParallelogramShape.sideSlope = newSideSlope;
                hasChanged = true;
            }

            return hasChanged;
        }
        return false;
    }

    /// <summary>
    /// Edit a SlopeEllipse data.
    /// </summary>
    /// <param name="slopeEllipseShape"></param>
    /// <returns></returns>
    static public bool EditSlopeEllipseShape(SlopeEllipseShape slopeEllipseShape) {
        if (slopeEllipseShape != null) {
            bool hasChanged = false;
            EditorGUILayout.BeginHorizontal();
            {
                float newSemiAxisA = EditorGUILayoutPlus.FloatField("SemiAxisA", slopeEllipseShape.semiAxisA);
                if (slopeEllipseShape.semiAxisA != newSemiAxisA) {
                    slopeEllipseShape.semiAxisA = newSemiAxisA;
                    hasChanged = true;
                }
                float newSemiAxisB = EditorGUILayoutPlus.FloatField("SemiAxisB", slopeEllipseShape.semiAxisB);
                if (slopeEllipseShape.semiAxisB != newSemiAxisB) {
                    slopeEllipseShape.semiAxisB = newSemiAxisB;
                    hasChanged = true;
                }
            }
            EditorGUILayout.EndHorizontal();

            float newSlope = EditorGUILayoutPlus.FloatField("Slope", slopeEllipseShape.slope);
            if (slopeEllipseShape.slope != newSlope) {
                slopeEllipseShape.slope = newSlope;
                hasChanged = true;
            }

            int newSliceStep = EditorGUILayoutPlus.IntField("SliceStep", slopeEllipseShape.sliceAxisStep);
            if (slopeEllipseShape.sliceAxisStep != newSliceStep) {
                slopeEllipseShape.sliceAxisStep = newSliceStep;
                hasChanged = true;
            }

            return hasChanged;
        }
        return false;
    }

    /// <summary>
    /// Edit a NoneShape stuff.
    /// </summary>
    /// <param name="noneShape"></param>
    /// <returns></returns>
    static public bool EditNoneShape(NoneShape noneShape) {
        EditorGUILayout.HelpBox("This is a NoneShape. Nothing to edit.", MessageType.None);
        return false;
    }

}

#endif