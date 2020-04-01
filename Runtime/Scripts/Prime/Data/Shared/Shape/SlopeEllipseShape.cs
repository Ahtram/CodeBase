using UnityEngine;
using System.Xml.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;
using mattatz.Triangulation2DSystem;

[XmlRoot("SELS")]
[XmlType("SELS")]
[Serializable]
public class SlopeEllipseShape : Shape {

    // Reference:
    // http://math.stackexchange.com/questions/432902/how-to-get-the-radius-of-an-ellipse-at-a-specific-angle-by-knowing-its-semi-majo

    [XmlElement("SAXA")]
    public float semiAxisA = 108.0f;

    [XmlElement("SAXB")]
    public float semiAxisB = 42.0f;

    [XmlElement("S")]
    public float slope = 1.525f;

    [XmlElement("SAS")]
    public int sliceAxisStep = 50;

    public SlopeEllipseShape() {
        type = Type.SlopeEllipse;
    }

    public SlopeEllipseShape(float semiAxisA, float semiAxisB) {
        type = Type.SlopeEllipse;
        this.semiAxisA = semiAxisA;
        this.semiAxisB = semiAxisB;
        this.slope = float.PositiveInfinity;
    }

    public SlopeEllipseShape(float semiAxisA, float semiAxisB, float slope) {
        type = Type.SlopeEllipse;
        this.semiAxisA = semiAxisA;
        this.semiAxisB = semiAxisB;
        this.slope = slope;
    }

    public SlopeEllipseShape(float semiAxisA, float semiAxisB, float slope, int sliceAxisStep) {
        type = Type.SlopeEllipse;
        this.semiAxisA = semiAxisA;
        this.semiAxisB = semiAxisB;
        this.slope = slope;
        this.sliceAxisStep = sliceAxisStep;
    }

    /// <summary>
    /// Calculate the path of this thing.
    /// </summary>
    /// <returns></returns>
    private Vector2[] CalculatePath() {
        //Calculate the path.
        //The path should be counter-clock wise.
        if (slope != 0.0f) {
            if (sliceAxisStep <= 0) {
                Debug.LogWarning("Cannot use a negative value for step.");
                return null;
            }
            float step = semiAxisA / (float)sliceAxisStep;

            //Ref: 
            //Ellipse y = Mathf.Sqrt((Mathf.Pow(semiAxisA, 2) * Mathf.Pow(semiAxisB, 2) - Mathf.Pow(x, 2) * Mathf.Pow(semiAxisB, 2)) / Mathf.Pow(semiAxisA, 2));

            //lt path. x = 0.0f ~ -semiAxisA
            List<Vector2> ltPath = new List<Vector2>();
            for (float x = 0.0f; x >= -semiAxisA; x += -step) {
                ltPath.Add(new Vector2(x, Mathf.Sqrt((Mathf.Pow(semiAxisA, 2) * Mathf.Pow(semiAxisB, 2) - Mathf.Pow(x, 2) * Mathf.Pow(semiAxisB, 2)) / Mathf.Pow(semiAxisA, 2))));
            }

            //lb path. x = -semiAxisA ~ 0.0f (actually mirror lt path)
            List<Vector2> lbPath = new List<Vector2>();
            for (int i = ltPath.Count - 1; i >= 0; i--) {
                Vector2 thisPoint = ltPath[i];
                lbPath.Add(new Vector2(thisPoint.x, -thisPoint.y));
            }

            //rb path. x = 0.0f ~ semiAxisA
            List<Vector2> rbPath = new List<Vector2>();
            for (int i = 0; i < ltPath.Count; i++) {
                Vector2 thisPoint = ltPath[i];
                rbPath.Add(new Vector2(-thisPoint.x, -thisPoint.y));
            }

            //rt path. x = semiAxisA ~ 0.0f
            List<Vector2> rtPath = new List<Vector2>();
            for (int i = ltPath.Count - 1; i >= 0; i--) {
                Vector2 thisPoint = ltPath[i];
                rtPath.Add(new Vector2(-thisPoint.x, thisPoint.y));
            }

            List<Vector2> ellipsePath = new List<Vector2>();
            ellipsePath.AddRange(ltPath);
            ellipsePath.AddRange(lbPath);
            ellipsePath.AddRange(rbPath);
            ellipsePath.AddRange(rtPath);

            List<Vector2> slopeEllipsePath = new List<Vector2>();
            for (int i = 0; i < ellipsePath.Count; i++) {
                float shiftXValue = ellipsePath[i].y / slope;
                slopeEllipsePath.Add(new Vector2(ellipsePath[i].x + shiftXValue, ellipsePath[i].y));
            }

            return slopeEllipsePath.ToArray();
        } else {
            Debug.LogWarning("Oops! The slope cannot be 0!");
            return null;
        }
    }

    /// <summary>
    /// Attach this box as a PolygonCollider2D to a GameObject.
    /// </summary>
    /// <param name="go">Target GameObject.</param>
    /// <returns></returns>
    override public Collider2D AttachCollider2D(GameObject go, bool isTrigger = false) {
        base.AttachCollider2D(go);

        //Calculate the path.
        //The path should be counter-clock wise.
        Vector2[] path = CalculatePath();
        if (path != null) {
            PolygonCollider2D newPolygonCollider2D = go.AddComponent<PolygonCollider2D>();
            newPolygonCollider2D.SetPath(0, path);
            newPolygonCollider2D.isTrigger = isTrigger;

            Rigidbody2D rigidbody2D = go.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0.0f;
            rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

            return newPolygonCollider2D;
        } else {
            return null;
        }
    }

    //Attach a MeshRenderer and MeshFilter for this shape.
    override public MeshRenderer AttachMeshRenderer(GameObject go) {
        base.AttachMeshRenderer(go);

        //Calculate the path.
        //The path should be counter-clock wise.
        Vector2[] path = CalculatePath();
        if (path != null) {
            MeshFilter newMeshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = go.AddComponent<MeshRenderer>();

            //I dont know what's this. But thanks to mattatz!
            List<Vector2> points = Utils2D.Constrain(new List<Vector2>(path), 1.5f);
            Polygon2D polygon = Polygon2D.Contour(points.ToArray());
            Vertex2D[] vertices = polygon.Vertices;
            if (vertices.Length < 3) return null; // error
            Triangulation2D triangulation = new Triangulation2D(polygon, 20.0f, semiAxisA + semiAxisB);
            newMeshFilter.mesh = triangulation.Build();

            return newMeshRenderer;
        } else {
            return null;
        }
    }

    //Attach a LineRenderer for this shape.
    override public LineRenderer AttachLineRenderer(GameObject go) {
        base.AttachLineRenderer(go);

        //Calculate the path.
        //The path should be counter-clock wise.
        Vector2[] path = CalculatePath();
        if (path != null) {
            LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
            Vector3[] vec3Path = path.Select((vector2) => new Vector3(vector2.x, vector2.y, -0.0001f)).ToArray();
            lineRenderer.useWorldSpace = false;
            lineRenderer.positionCount = vec3Path.Length;
            lineRenderer.SetPositions(vec3Path);
            lineRenderer.loop = true;
            return lineRenderer;
        } else {
            return null;
        }
    }

    //Attach the stuffs above.
    override public void AttachMeshAndCollider2D(GameObject go, bool colliderIsTrigger = false) {
        base.AttachCollider2D(go, colliderIsTrigger);
        base.AttachMeshRenderer(go);

        //Calculate the path.
        //The path should be counter-clock wise.
        Vector2[] path = CalculatePath();
        if (path != null) {
            //Collider stuffs.
            PolygonCollider2D newPolygonCollider2D = go.AddComponent<PolygonCollider2D>();
            newPolygonCollider2D.SetPath(0, path);
            newPolygonCollider2D.isTrigger = colliderIsTrigger;

            Rigidbody2D rigidbody2D = go.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0.0f;
            rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

            //Mesh stuffs
            MeshFilter newMeshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = go.AddComponent<MeshRenderer>();

            //I dont know what's this. But thanks to mattatz!
            List<Vector2> points = Utils2D.Constrain(new List<Vector2>(path), 1.5f);
            Polygon2D polygon = Polygon2D.Contour(points.ToArray());
            Vertex2D[] vertices = polygon.Vertices;
            if (vertices.Length < 3) return; // error
            Triangulation2D triangulation = new Triangulation2D(polygon, 20.0f, semiAxisA + semiAxisB);
            newMeshFilter.mesh = triangulation.Build();
        }
    }

    //Attach all stuffs.
    override public void AttachAllComponents(GameObject go, bool colliderIsTrigger = false) {
        base.AttachCollider2D(go, colliderIsTrigger);
        base.AttachMeshRenderer(go);
        base.AttachLineRenderer(go);

        //Calculate the path.
        //The path should be counter-clock wise.
        Vector2[] path = CalculatePath();
        if (path != null) {

            //Collider stuffs.
            PolygonCollider2D newPolygonCollider2D = go.AddComponent<PolygonCollider2D>();
            newPolygonCollider2D.SetPath(0, path);
            newPolygonCollider2D.isTrigger = colliderIsTrigger;

            Rigidbody2D rigidbody2D = go.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0.0f;
            rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

            //Mesh stuffs.
            MeshFilter newMeshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = go.AddComponent<MeshRenderer>();

            //I dont know what's this. But thanks to mattatz!
            List<Vector2> points = Utils2D.Constrain(new List<Vector2>(path), 1.5f);
            Polygon2D polygon = Polygon2D.Contour(points.ToArray());
            Vertex2D[] vertices = polygon.Vertices;
            if (vertices.Length < 3) return; // error
            Triangulation2D triangulation = new Triangulation2D(polygon, 20.0f, semiAxisA + semiAxisB);
            newMeshFilter.mesh = triangulation.Build();

            //LineRenderer.
            LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
            Vector3[] vec3Path = path.Select((vector2) => new Vector3(vector2.x, vector2.y, -0.0001f)).ToArray();
            lineRenderer.useWorldSpace = false;
            lineRenderer.positionCount = vec3Path.Length;
            lineRenderer.SetPositions(vec3Path);
            lineRenderer.loop = true;
        }
    }

}
