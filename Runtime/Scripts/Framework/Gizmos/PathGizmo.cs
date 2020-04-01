using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PathGizmo : MonoBehaviour {

    public Camera gizmoCamera;

    //What color will this gizmos be?
    public Color gizmoColor = Color.yellow;

    //This size of this sphere gizmos.
    public float pointRadius = 1.0f;

    //路徑節點
    public List<Vector3> pathPoints = new List<Vector3>();

    void OnDrawGizmos() {
        Gizmos.color = gizmoColor;
        {
            if (pathPoints.Count >= 2) {
#if UNITY_EDITOR
                if (gizmoCamera == null || Camera.current == gizmoCamera || Camera.current == SceneView.lastActiveSceneView.camera) {
                    for (int i = 0; i < pathPoints.Count; i++) {
                        if (i + 1 < pathPoints.Count) {
                            Gizmos.DrawSphere(pathPoints[i], pointRadius);
                            Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]);
                            Gizmos.DrawSphere(pathPoints[i + 1], pointRadius);
                        }
                    }
                }
#endif

            }
        }
        Gizmos.color = Color.white;
    }

}
