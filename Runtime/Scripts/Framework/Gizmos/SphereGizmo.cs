using UnityEngine;

public class SphereGizmo : MonoBehaviour {

    //What color will this gizmos be?
    public Color gizmoColor = Color.yellow;

    //This size of this sphere gizmos.
    public float radius = 1.0f;

    void OnDrawGizmos() {
        Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = Color.white;
    }

}
