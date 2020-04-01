using UnityEngine;

public class RayGizmo : MonoBehaviour {

    //What color will this gizmos be?
    public Color gizmoColor = Color.yellow;

    //Target direction of the ray.
    public Vector3 direction = Vector3.zero;

    void OnDrawGizmos() {
        Gizmos.color = gizmoColor;
            Gizmos.DrawRay(transform.position, direction);
        Gizmos.color = Color.white;
    }
}
