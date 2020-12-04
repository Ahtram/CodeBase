using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodeBaseExtensions {

    public enum AnchorPresets {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottonCenter,
        BottomRight,
        BottomStretch,

        VertStretchLeft,
        VertStretchRight,
        VertStretchCenter,

        HorStretchTop,
        HorStretchMiddle,
        HorStretchBottom,

        StretchAll
    }

    public enum PivotPresets {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,
    }

    static public class UIExtensions {
        static public Canvas GetCanvas(this RectTransform rectTransform) {
            return rectTransform.GetComponentInParent<Canvas>();
        }

        static public RectTransform GetCanvasRectTransform(this RectTransform rectTransform) {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas != null) {
                return canvas.GetComponent<RectTransform>();
            }
            return null;
        }

        static public Camera GetCanvasRenderCamera(this RectTransform rectTransform) {
            Canvas canvas = rectTransform.GetCanvas();
            if (canvas != null) {
                return canvas.worldCamera;
            } else {
                return null;
            }
        }

        static public Rect GetViewPortRect(this RectTransform rectTransform) {
            //Try get renderCamera first.
            Camera renderCam = rectTransform.GetCanvasRenderCamera();
            if (renderCam != null) {
                //Get world corners of this rect.
                Vector3[] worldCorners = new Vector3[] {
                    Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero
                };
                rectTransform.GetWorldCorners(worldCorners);
                //BottomLeft and TopRight of viewport point.
                Vector3 bl = renderCam.WorldToViewportPoint(worldCorners[0]);
                Vector3 tr = renderCam.WorldToViewportPoint(worldCorners[2]);
                //Calculate the viewport rect.
                return new Rect(bl.x, bl.y, tr.x - bl.x, tr.y - bl.y);
            } else {
                return new Rect(0, 0, 0, 0);
            }
        }

        static public Rect GetViewPortRect(this RectTransform rectTransform, float marginTop, float marginBottom, float marginLeft, float marginRight) {
            //Try get renderCamera first.
            Camera renderCam = rectTransform.GetCanvasRenderCamera();
            if (renderCam != null) {
                //Get world corners of this rect.
                Vector3[] worldCorners = new Vector3[] {
                    Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero
                };
                rectTransform.GetWorldCorners(worldCorners);
                //BottomLeft and TopRight of viewport point.
                Vector3 bl = renderCam.WorldToViewportPoint(worldCorners[0] + new Vector3(marginLeft, marginBottom, 0.0f));
                Vector3 tr = renderCam.WorldToViewportPoint(worldCorners[2] - new Vector3(marginRight, marginTop, 0.0f));
                //Calculate the viewport rect.
                return new Rect(bl.x, bl.y, tr.x - bl.x, tr.y - bl.y);
            } else {
                return new Rect(0, 0, 0, 0);
            }
        }

        static public RectTransform GetRectTransform(this MonoBehaviour monoBehaviour) {
            return monoBehaviour.GetComponent<RectTransform>();
        }

        static public RectTransform GetRectTransform(this UIBehaviour uiBehaviour) {
            return uiBehaviour.GetComponent<RectTransform>();
        }

        static public RectTransform GetRectTransform(this GameObject gameObject) {
            return gameObject.GetComponent<RectTransform>();
        }

        //https://answers.unity.com/questions/1225118/solution-set-ui-recttransform-anchor-presets-from.html
        public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0) {
            source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

            switch (allign) {
                case (AnchorPresets.TopLeft): {
                        source.anchorMin = new Vector2(0, 1);
                        source.anchorMax = new Vector2(0, 1);
                        break;
                    }
                case (AnchorPresets.TopCenter): {
                        source.anchorMin = new Vector2(0.5f, 1);
                        source.anchorMax = new Vector2(0.5f, 1);
                        break;
                    }
                case (AnchorPresets.TopRight): {
                        source.anchorMin = new Vector2(1, 1);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }

                case (AnchorPresets.MiddleLeft): {
                        source.anchorMin = new Vector2(0, 0.5f);
                        source.anchorMax = new Vector2(0, 0.5f);
                        break;
                    }
                case (AnchorPresets.MiddleCenter): {
                        source.anchorMin = new Vector2(0.5f, 0.5f);
                        source.anchorMax = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (AnchorPresets.MiddleRight): {
                        source.anchorMin = new Vector2(1, 0.5f);
                        source.anchorMax = new Vector2(1, 0.5f);
                        break;
                    }

                case (AnchorPresets.BottomLeft): {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(0, 0);
                        break;
                    }
                case (AnchorPresets.BottonCenter): {
                        source.anchorMin = new Vector2(0.5f, 0);
                        source.anchorMax = new Vector2(0.5f, 0);
                        break;
                    }
                case (AnchorPresets.BottomRight): {
                        source.anchorMin = new Vector2(1, 0);
                        source.anchorMax = new Vector2(1, 0);
                        break;
                    }

                case (AnchorPresets.HorStretchTop): {
                        source.anchorMin = new Vector2(0, 1);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }
                case (AnchorPresets.HorStretchMiddle): {
                        source.anchorMin = new Vector2(0, 0.5f);
                        source.anchorMax = new Vector2(1, 0.5f);
                        break;
                    }
                case (AnchorPresets.HorStretchBottom): {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(1, 0);
                        break;
                    }

                case (AnchorPresets.VertStretchLeft): {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(0, 1);
                        break;
                    }
                case (AnchorPresets.VertStretchCenter): {
                        source.anchorMin = new Vector2(0.5f, 0);
                        source.anchorMax = new Vector2(0.5f, 1);
                        break;
                    }
                case (AnchorPresets.VertStretchRight): {
                        source.anchorMin = new Vector2(1, 0);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }

                case (AnchorPresets.StretchAll): {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }
            }
        }

        public static void SetPivot(this RectTransform source, PivotPresets preset) {

            switch (preset) {
                case (PivotPresets.TopLeft): {
                        source.pivot = new Vector2(0, 1);
                        break;
                    }
                case (PivotPresets.TopCenter): {
                        source.pivot = new Vector2(0.5f, 1);
                        break;
                    }
                case (PivotPresets.TopRight): {
                        source.pivot = new Vector2(1, 1);
                        break;
                    }

                case (PivotPresets.MiddleLeft): {
                        source.pivot = new Vector2(0, 0.5f);
                        break;
                    }
                case (PivotPresets.MiddleCenter): {
                        source.pivot = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (PivotPresets.MiddleRight): {
                        source.pivot = new Vector2(1, 0.5f);
                        break;
                    }

                case (PivotPresets.BottomLeft): {
                        source.pivot = new Vector2(0, 0);
                        break;
                    }
                case (PivotPresets.BottomCenter): {
                        source.pivot = new Vector2(0.5f, 0);
                        break;
                    }
                case (PivotPresets.BottomRight): {
                        source.pivot = new Vector2(1, 0);
                        break;
                    }
            }
        }

        //Convient way to get world LTRB positions of a rectTransform. 
        public static float WorldLeft(this RectTransform source) {
            Vector3[] corners = new Vector3[4];
            source.GetWorldCorners(corners);
            return corners[0].x;
        }

        public static float WorldTop(this RectTransform source) {
            Vector3[] corners = new Vector3[4];
            source.GetWorldCorners(corners);
            return corners[2].y;
        }

        public static float WorldRight(this RectTransform source) {
            Vector3[] corners = new Vector3[4];
            source.GetWorldCorners(corners);
            return corners[2].x;
        }

        public static float WorldBottom(this RectTransform source) {
            Vector3[] corners = new Vector3[4];
            source.GetWorldCorners(corners);
            return corners[0].y;
        }

    }

}