using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeBaseExtensions;

//Draw a colored cube gizmo to visualize the bound of the attached RectTransform.
[ExecuteInEditMode]
public class RectTransformBoundGizmo : MonoBehaviour {

    //What color will this gizmos be?
    public Color gizmoColor = Color.yellow;

    //The RectTransform we are viewing.
    private RectTransform rectTransform;

    void Awake() {
        rectTransform = this.GetRectTransform();
    }

    void OnDrawGizmos() {
        //Calculate the bound of this RectTransform.
        if (rectTransform != null) {
            Gizmos.color = gizmoColor;
                Gizmos.DrawWireCube(transform.position, rectTransform.sizeDelta);
            Gizmos.color = Color.white;
        }
    }

}
