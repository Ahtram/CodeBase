using UnityEngine;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class LineGizmo : MonoBehaviour {

    public enum TargetType {
        None,
        Vec,
        Trans
    };

    //What color will this gizmos be?
    public Color gizmoColor = Color.yellow;

    //The target transform ("to") of this line.
    public Transform targetTransform = null;

    //The target world position ("to") of this line.
    public Vector3 targetPosition = Vector3.zero;

    //The using target type.
    public TargetType targetType = TargetType.None;

    //Draw function for avoid switch case.
    private List<Action> drawFunctions = new List<Action>();

    void Awake() {
        drawFunctions.Add(DrawNone);
        drawFunctions.Add(DrawVec);
        drawFunctions.Add(DrawTrans);
    }

    void OnDrawGizmos() {
        int targetTypeIndex = (int)targetType;
        if (targetTypeIndex >= 0 && targetTypeIndex < drawFunctions.Count) {
            drawFunctions[targetTypeIndex]();
        }
    }

    public void Setup(Vector3 toPosition, Color color) {
        targetPosition = toPosition;
        gizmoColor = color;
        targetType = TargetType.Vec;
    }

    public void Setup(Transform toTransform, Color color) {
        targetTransform = toTransform;
        gizmoColor = color;
        targetType = TargetType.Trans;
    }

    //========================================

    private void DrawNone() {

    }

    private void DrawVec() {
        Gizmos.color = gizmoColor;
            Gizmos.DrawLine(transform.position, targetPosition);
        Gizmos.color = Color.white;
    }

    private void DrawTrans() {
        if (targetTransform != null) {
            Gizmos.color = gizmoColor;
                Gizmos.DrawLine(transform.position, targetTransform.position);
            Gizmos.color = Color.white;
        }
    }

    //========================================

}
