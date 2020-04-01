using UnityEngine;

public class WireCubeGizmo : MonoBehaviour {

    //What color will this gizmos be?
    public Color gizmoColor = Color.yellow;

    //The shape of the WireCube.
    public Vector3 vec3 = Vector3.one;

    public Vector3 shift = Vector3.zero;

    void OnDrawGizmos() {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position + shift, vec3);
        Gizmos.color = Color.white;
    }

}
