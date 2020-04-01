using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一個 Singlton 類別來幫助顯示想要顯示的路徑. 例如: CreatureActivity SetPath.
[RequireComponent(typeof(PathGizmo))]
public class PathGizmodo : MonoBehaviour {

    //我們依賴PathGizmo來畫Path.
    private PathGizmo pathGizmo;

    static private PathGizmodo instance;

    void Awake() {
        instance = this;
        pathGizmo = GetComponent<PathGizmo>();
    }

    static public bool ExistInTheWorld() {
        return instance != null;
    }

    static public bool SetColor(Color color) {
        if (instance != null) {
            instance.pathGizmo.gizmoColor = color;
            return true;
        }
        return false;
    }

    static public bool SetPointRadius(float pointRadius) {
        if (instance != null) {
            instance.pathGizmo.pointRadius = pointRadius;
            return true;
        }
        return false;
    }

    static public bool SetPath(List<Vector3> pathPoints) {
        if (instance != null) {
            instance.pathGizmo.pathPoints = pathPoints;
            return true;
        }
        return false;
    }

    static public bool ClearPath() {
        if (instance != null) {
            instance.pathGizmo.pathPoints.Clear();
            return true;
        }
        return false;
    }

}
