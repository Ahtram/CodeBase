using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.Linq;

[XmlRoot("PS")]
[XmlType("PS")]
[Serializable]
public class PolygonShape : Shape {

    [XmlArray("PL")]
    [XmlArrayItem("P", typeof(Vec2))]
    public List<Vec2> points = new List<Vec2>();

    [XmlElement("MT")]
    public float meshThreshold = 0.1f;

    public PolygonShape() {
        type = Type.Polygon;
    }

    public PolygonShape(List<Vec2> points) {
        type = Type.Polygon;
        this.points.Clear();
        points.ForEach((p) => this.points.Add(new Vec2(p)));
    }

    //Set points.
    public void SetPoints(Vector2[] points) {
        this.points.Clear();
        for (int i = 0; i < points.Length; i++) {
            this.points.Add(new Vec2(points[i]));
        }
    }

    /// <summary>
    /// Attach this polygon as a PolygonCollider2D to a GameObject.
    /// </summary>
    /// <param name="go">Target GameObject.</param>
    /// <returns></returns>
    override public Collider2D AttachCollider2D(GameObject go, bool isTrigger = false, bool attachRigidbody2D = false) {
        base.AttachCollider2D(go);

        PolygonCollider2D newPolygonCollider2D = go.AddComponent<PolygonCollider2D>();
        if (points.Count > 3) {
            newPolygonCollider2D.SetPath(0, Vec2.ArrayToVector2(points.ToArray()));
            newPolygonCollider2D.isTrigger = isTrigger;

            if (attachRigidbody2D) {
                Rigidbody2D rigidbody2D = go.AddComponent<Rigidbody2D>();
                rigidbody2D.gravityScale = 0.0f;
                rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
            }
        }
        return newPolygonCollider2D;
    }

    //Attach a MeshRenderer and MeshFilter for this shape.
    override public MeshRenderer AttachMeshRenderer(GameObject go) {
        base.AttachMeshRenderer(go);

        if (points.Count > 3) {
            MeshFilter newMeshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = go.AddComponent<MeshRenderer>();
            Mesh newMesh = new Mesh();

            newMesh.vertices = points.Select((vec2) => vec2.ToVector3()).ToArray();
            Triangulator triangulator = new Triangulator(points.Select((vec2) => vec2.ToVector2()).ToArray());
            newMesh.triangles = triangulator.Triangulate();

            newMeshFilter.mesh = newMesh;

            return newMeshRenderer;
        } else {
            return null;
        }
    }

    //Attach a LineRenderer for this shape.
    override public LineRenderer AttachLineRenderer(GameObject go) {
        base.AttachLineRenderer(go);

        LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
        Vector3[] vec3Path = points.Select((vec2) => new Vector3(vec2.x, vec2.y, -0.0001f)).ToArray();
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = vec3Path.Length;
        lineRenderer.SetPositions(vec3Path);
        lineRenderer.loop = true;
        return lineRenderer;
    }

    //Attach the stuffs above.
    override public void AttachMeshAndCollider2D(GameObject go, bool colliderIsTrigger = false) {
        base.AttachCollider2D(go, colliderIsTrigger);
        base.AttachMeshRenderer(go);

        if (points.Count > 3) {
            //Collider stuffs.
            PolygonCollider2D newPolygonCollider2D = go.AddComponent<PolygonCollider2D>();
            newPolygonCollider2D.SetPath(0, Vec2.ArrayToVector2(points.ToArray()));
            newPolygonCollider2D.isTrigger = colliderIsTrigger;

            Rigidbody2D rigidbody2D = go.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0.0f;
            rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

            //Mesh stuffs
            MeshFilter newMeshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = go.AddComponent<MeshRenderer>();
            Mesh newMesh = new Mesh();

            newMesh.vertices = points.Select((vec2) => vec2.ToVector3()).ToArray();
            Triangulator triangulator = new Triangulator(points.Select((vec2) => vec2.ToVector2()).ToArray());
            newMesh.triangles = triangulator.Triangulate();

            newMeshFilter.mesh = newMesh;
        }
    }

    //Attach all stuffs.
    override public void AttachAllComponents(GameObject go, bool colliderIsTrigger = false) {
        base.AttachCollider2D(go, colliderIsTrigger);
        base.AttachMeshRenderer(go);
        base.AttachLineRenderer(go);

        if (points.Count > 3) {
            //Collider stuffs.
            PolygonCollider2D newPolygonCollider2D = go.AddComponent<PolygonCollider2D>();
            newPolygonCollider2D.SetPath(0, Vec2.ArrayToVector2(points.ToArray()));
            newPolygonCollider2D.isTrigger = colliderIsTrigger;

            Rigidbody2D rigidbody2D = go.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0.0f;
            rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

            //Mesh stuffs
            MeshFilter newMeshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = go.AddComponent<MeshRenderer>();
            Mesh newMesh = new Mesh();

            newMesh.vertices = points.Select((vec2) => vec2.ToVector3()).ToArray();
            Triangulator triangulator = new Triangulator(points.Select((vec2) => vec2.ToVector2()).ToArray());
            newMesh.triangles = triangulator.Triangulate();

            newMeshFilter.mesh = newMesh;

            //LineRenderer
            LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
            Vector3[] vec3Path = points.Select((vec2) => new Vector3(vec2.x, vec2.y, -0.0001f)).ToArray();
            lineRenderer.useWorldSpace = false;
            lineRenderer.positionCount = vec3Path.Length;
            lineRenderer.SetPositions(vec3Path);
            lineRenderer.loop = true;
        }
    }

}
