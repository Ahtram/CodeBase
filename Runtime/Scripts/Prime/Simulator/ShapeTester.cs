using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShapeTester : MonoBehaviour {

    private FlatParallelogramShape testingflatParallelogramShape = null;

    void Start() {
        //Flat Parallelogram.
        testingflatParallelogramShape = new FlatParallelogramShape {
            sideSlope = 1f,
            size = new Vec2(200.0f, 100.0f),
            center = new Vec2(300.0f, 150.0f)
        };

        GameObject testFlatParallelogramShapeGO = new GameObject("[FlatParallelogramShape]");
        testingflatParallelogramShape.AttachAllComponents(testFlatParallelogramShapeGO);
        testFlatParallelogramShapeGO.transform.localPosition = new Vector2(150.0f, 200.0f);

        //ShapeGraph
        ShapeGraph flatParallelogramShapeGraph = ShapeGraph.Create("[FlatParallelogramShapeGraph]", gameObject, testingflatParallelogramShape);
        flatParallelogramShapeGraph.MeshColor = ColorPlus.Green;
        flatParallelogramShapeGraph.OutLineColor = ColorPlus.LightGreen;
        flatParallelogramShapeGraph.SetLineWidth(10.0f, 10.0f);

        //Slope Ellipse
        SlopeEllipseShape slopeEllipseShape = new SlopeEllipseShape() {
            semiAxisA = 100.0f,
            semiAxisB = 50.0f,
            slope = 1.0f
        };

        GameObject testSlopeEllipseShapeShapeGO = new GameObject("[SlopeEllipseShape]");
        slopeEllipseShape.AttachAllComponents(testSlopeEllipseShapeShapeGO);
        testSlopeEllipseShapeShapeGO.transform.localPosition = new Vector2(-150.0f, -200.0f);

        //ShapeGraph
        ShapeGraph slopeEllipseShapeGraph = ShapeGraph.Create("[SlopeEllipseShapeGraph]", gameObject, slopeEllipseShape);
        slopeEllipseShapeGraph.transform.localPosition = new Vector2(-400.0f, -200.0f);
        slopeEllipseShapeGraph.MeshColor = ColorPlus.Blue;
        slopeEllipseShapeGraph.OutLineColor = ColorPlus.LightBlue;
        slopeEllipseShapeGraph.SetLineWidth(6.0f, 6.0f);

        //Box.
        BoxShape boxShape = new BoxShape() {
            size = new Vec2(100.0f, 100.0f)
        };

        GameObject testBoxShapeShapeGO = new GameObject("[BoxShapeShape]");
        boxShape.AttachAllComponents(testBoxShapeShapeGO);
        testBoxShapeShapeGO.transform.localPosition = new Vector2(150.0f, -200.0f);

        //ShapeGraph
        ShapeGraph boxShapeGraph = ShapeGraph.Create("[BoxShapeGraph]", gameObject, boxShape);
        boxShapeGraph.transform.localPosition = new Vector2(400.0f, -200.0f);
        boxShapeGraph.MeshColor = ColorPlus.Yellow;
        boxShapeGraph.OutLineColor = ColorPlus.LightYellow;
        boxShapeGraph.SetLineWidth(8.0f, 8.0f);

        //Polygon.
        PolygonShape polygonShape = new PolygonShape() {
            points = new List<Vec2>() {
                new Vec2(-50.0f, 50.0f),
                new Vec2(-100.0f, -50.0f),
                new Vec2(100.0f, -50.0f),
                new Vec2(50.0f, 50.0f),
                new Vec2(00.0f, 100.0f),
            }
        };

        GameObject testPolygonShapeShapeGO = new GameObject("[PolygonShape]");
        polygonShape.AttachAllComponents(testPolygonShapeShapeGO);
        testPolygonShapeShapeGO.transform.localPosition = new Vector2(-150.0f, 200.0f);

        //ShapeGraph
        ShapeGraph polygonShapeGraph = ShapeGraph.Create("[PolygonShapeGraph]", gameObject, polygonShape);
        polygonShapeGraph.transform.localPosition = new Vector2(-400.0f, 200.0f);
        polygonShapeGraph.MeshColor = new Color(ColorPlus.LightPink.r, ColorPlus.LightPink.g, ColorPlus.LightPink.b, 0.33f);
        polygonShapeGraph.OutLineColor = ColorPlus.LightPink;
        polygonShapeGraph.SetLineWidth(8.0f, 8.0f);
        polygonShapeGraph.Alpha = 1.0f;

        //=================================================

    }

    public void OnMouseClick(BaseEventData baseEventData) {
        //Test if the click is inside the testing shape.
        if (testingflatParallelogramShape != null) {
            PointerEventData pointerEventData = baseEventData as PointerEventData;
            bool isInsideTestingShape = testingflatParallelogramShape.Contains(pointerEventData.pointerPressRaycast.worldPosition);
            Debug.Log("Click position isInsideTestingShape: " + isInsideTestingShape + " | position: " + pointerEventData.pointerPressRaycast.worldPosition);
        } else {
            Debug.LogWarning("testingflatParallelogramShape is null?");
        }
    }

}
