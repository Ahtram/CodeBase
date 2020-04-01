using UnityEngine;
using System.Collections.Generic;

namespace CodeBaseExtensions {
    public static class PrototypeExtensions {
        public static void SetLayer(this GameObject go, int layer) {
            go.layer = layer;
            foreach (Transform trans in go.transform)
                trans.gameObject.SetLayer(layer);
        }

        public static GameObject InstantiateChild(this GameObject go, string path, bool resetScale) {
            Object prefab = Resources.Load(path);
            if (prefab != null) {
                GameObject instantiateGO = GameObject.Instantiate(prefab) as GameObject;
                instantiateGO.transform.SetParent(go.transform);
                instantiateGO.layer = go.layer;
                if (resetScale) {
                    instantiateGO.transform.localScale = Vector3.one;
                }
                return instantiateGO;
            } else {
                Debug.LogError("Error: Prefab not exist: " + path);
                return null;
            }
        }

        public static GameObject InstantiateChild(this GameObject go, string path, string name, Vector3 localPos, bool resetScale) {
            Object prefab = Resources.Load(path);
            if (prefab != null) {
                GameObject instantiateGO = GameObject.Instantiate(prefab) as GameObject;
                instantiateGO.name = name;
                instantiateGO.transform.SetParent(go.transform);
                instantiateGO.layer = go.layer;
                instantiateGO.transform.localPosition = localPos;
                if (resetScale) {
                    instantiateGO.transform.localScale = Vector3.one;
                }
                return instantiateGO;
            } else {
                Debug.LogError("Error: Prefab not exist: " + path);
                return null;
            }
        }

        public static int ToInt(this bool b) {
            return (b) ? (1) : (0);
        }

        //http://wiki.unity3d.com/index.php/Distance_from_a_point_to_a_rectangle
        public static float Distance(this Rect rect, Vector2 point) {
            //  Calculate a distance between a point and a rectangle.
            //  The area around/in the rectangle is defined in terms of
            //  several regions:
            //
            //  O--x
            //  |
            //  y
            //
            //
            //        I   |    II    |  III
            //      ======+==========+======   --yMin
            //       VIII |  IX (in) |  IV
            //      ======+==========+======   --yMax
            //       VII  |    VI    |   V
            //
            //
            //  Note that the +y direction is down because of Unity's GUI coordinates.

            if (point.x < rect.xMin) { // Region I, VIII, or VII
                if (point.y < rect.yMin) { // I
                    Vector2 diff = point - new Vector2(rect.xMin, rect.yMin);
                    return diff.magnitude;
                } else if (point.y > rect.yMax) { // VII
                    Vector2 diff = point - new Vector2(rect.xMin, rect.yMax);
                    return diff.magnitude;
                } else { // VIII
                    return rect.xMin - point.x;
                }
            } else if (point.x > rect.xMax) { // Region III, IV, or V
                if (point.y < rect.yMin) { // III
                    Vector2 diff = point - new Vector2(rect.xMax, rect.yMin);
                    return diff.magnitude;
                } else if (point.y > rect.yMax) { // V
                    Vector2 diff = point - new Vector2(rect.xMax, rect.yMax);
                    return diff.magnitude;
                } else { // IV
                    return point.x - rect.xMax;
                }
            } else { // Region II, IX, or VI
                if (point.y < rect.yMin) { // II
                    return rect.yMin - point.y;
                } else if (point.y > rect.yMax) { // VI
                    return point.y - rect.yMax;
                } else { // IX
                    return 0f;
                }
            }
        }

        /// <summary>
        /// Get a random point in this rect.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 RandomPoint(this Rect rect) {
            return new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
        }

        public static bool Overlaps(this Rect rect, List<Rect> otherRects) {
            for (int i = 0; i < otherRects.Count; i++) {
                if (rect.Overlaps(otherRects[i])) {
                    return true;
                }
            }
            return false;
        }

        //https://stackoverflow.com/questions/18349844/rounded-to-the-nearest-10s-place-in-c-sharp
        public static int RoundOff10s(this int i) {
            return ((int)Mathf.Round(i / 10.0f)) * 10;
        }

        public static float RoundOff10s(this float f) {
            return Mathf.Round(f / 10.0f) * 10;
        }

        //Calculate a 9-grids sprite center width.
        public static float BorderlessWidth(this Sprite sprite) {
            if (sprite != null) {
                return sprite.texture.width - (sprite.border.x + sprite.border.z);
            }
            return 0.0f;
        }

        //Calculate a 9-grids sprite center height.
        public static float BorderlessHeight(this Sprite sprite) {
            if (sprite != null) {
                return sprite.texture.height - (sprite.border.y + sprite.border.w);
            }
            return 0.0f;
        }

    }
}