using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ColorTextGizmo : MonoBehaviour {

    public Color gizmoColor = Color.yellow;
    public string displayString = "";

    void OnDrawGizmos() {
        drawString(displayString, transform.position, gizmoColor);
    }

    static void drawString(string text, Vector3 worldPos, Color? color = null) {
#if UNITY_EDITOR
        Handles.BeginGUI();
        if (color.HasValue) GUI.color = color.Value;
        var view = SceneView.currentDrawingSceneView;
        if (view != null) {
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
        }
        Handles.EndGUI();
#endif
    }
}
