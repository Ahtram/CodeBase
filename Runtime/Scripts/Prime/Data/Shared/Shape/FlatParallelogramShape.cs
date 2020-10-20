using UnityEngine;
using System.Xml.Serialization;
using System;
using System.Linq;
using mattatz.Triangulation2DSystem;

[XmlRoot("FPLGS")]
[XmlType("FPLGS")]
[Serializable]
public class FlatParallelogramShape : Shape {

    [XmlElement("S")]
    //Width/Height of this Parallelogram.
    public Vec2 size = new Vec2(134.0f, 64.0f);

    [XmlElement("SS")]
    public float sideSlope = 1.525f;

    [XmlElement("C")]
    //The center of this shape (optional if you just need the is shape the create collider or path)
    public Vec2 center = Vec2.Zero;

    public FlatParallelogramShape() {
        type = Type.FlatParallelogram;
    }

    public FlatParallelogramShape(Vec2 size) {
        type = Type.FlatParallelogram;
        this.size.Set(size);
    }

    public FlatParallelogramShape(Vec2 size, float sideSlope) {
        type = Type.FlatParallelogram;
        this.size.Set(size);
        this.sideSlope = sideSlope;
    }

    public FlatParallelogramShape(Vec2 center, Vec2 size, float sideSlope) {
        type = Type.FlatParallelogram;
        this.center.Set(center);
        this.size.Set(size);
        this.sideSlope = sideSlope;
    }

    /// <summary>
    /// Get the vertical height of the flat parallelogram.
    /// </summary>
    /// <returns></returns>
    public float Height() {
        return size.y;
    }

    /// <summary>
    /// The base (bottom or top) width of the flat parallelogram.
    /// </summary>
    /// <returns></returns>
    public float BaseWidth() {
        return size.x;
    }

    /// <summary>
    /// The top coord y of this shape.
    /// </summary>
    /// <returns></returns>
    public float Top() {
        return center.y + size.y * 0.5f;
    }

    /// <summary>
    /// The bottom coord y of this shape.
    /// </summary>
    /// <returns></returns>
    public float Bottom() {
        return center.y - size.y * 0.5f;
    }

    /// <summary>
    /// Top-Left coord.
    /// </summary>
    /// <returns></returns>
    public Vector2 TopLeft() {
        if (sideSlope != 0.0f) {
            float sideDiff = size.y / sideSlope;
            float halfWidth = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;
            float halfSideDiff = sideDiff * 0.5f;

            return new Vector2(-(halfWidth - halfSideDiff), halfHeight);
        } else {
            Debug.LogWarning("Oops! The sideSlope cannot be 0!");
            return Vector2.zero;
        }
    }

    /// <summary>
    /// Bottom-Left coord.
    /// </summary>
    /// <returns></returns>
    public Vector2 BottomLeft() {
        if (sideSlope != 0.0f) {
            float sideDiff = size.y / sideSlope;
            float halfWidth = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;
            float halfSideDiff = sideDiff * 0.5f;

            return new Vector2(-(halfWidth + halfSideDiff), -halfHeight);
        } else {
            Debug.LogWarning("Oops! The sideSlope cannot be 0!");
            return Vector2.zero;
        }
    }

    /// <summary>
    /// Bottom-Right coord.
    /// </summary>
    /// <returns></returns>
    public Vector2 BottomRight() {
        if (sideSlope != 0.0f) {
            float sideDiff = size.y / sideSlope;
            float halfWidth = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;
            float halfSideDiff = sideDiff * 0.5f;

            return new Vector2(halfWidth - halfSideDiff, -halfHeight);
        } else {
            Debug.LogWarning("Oops! The sideSlope cannot be 0!");
            return Vector2.zero;
        }
    }

    /// <summary>
    /// Top-Right coord.
    /// </summary>
    /// <returns></returns>
    public Vector2 TopRight() {
        if (sideSlope != 0.0f) {
            float sideDiff = size.y / sideSlope;
            float halfWidth = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;
            float halfSideDiff = sideDiff * 0.5f;

            return new Vector2(halfWidth + halfSideDiff, halfHeight);
        } else {
            Debug.LogWarning("Oops! The sideSlope cannot be 0!");
            return Vector2.zero;
        }
    }

    /// <summary>
    /// Check if a point is inside the shape.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool Contains(Vector2 point) {
        if (point.y > Top()) {
            return false;
        }

        if (point.y < Bottom()) {
            return false;
        }

        float halfWidth = size.x * 0.5f;
        float xMaxOfY = center.x + halfWidth + (1.0f / sideSlope) * (point.y - center.y);
        if (point.x > xMaxOfY) {
            return false;
        }

        float xMinOfY = center.x - halfWidth + (1.0f / sideSlope) * (point.y - center.y);
        if (point.x < xMinOfY) {
            return false;
        }

        return true;
    }

    //Do we have a center position?
    override public bool HasACenterPosition() {
        return true;
    }

    //Implement this for your center position.
    override public Vector2 CenterPosition() {
        return center.ToVector2();
    }

    //==

    /// <summary>
    /// Calculate the path of this thing.
    /// </summary>
    /// <returns></returns>
    private Vector2[] CalculatePath() {
        if (sideSlope != 0.0f) {
            float sideDiff = size.y / sideSlope;
            float halfWidth = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;
            float halfSideDiff = sideDiff * 0.5f;

            Vector2 lt = new Vector2(-(halfWidth - halfSideDiff), halfHeight);
            Vector2 lb = new Vector2(-(halfWidth + halfSideDiff), -halfHeight);
            Vector2 rb = new Vector2(halfWidth - halfSideDiff, -halfHeight);
            Vector2 rt = new Vector2(halfWidth + halfSideDiff, halfHeight);

            return new Vector2[] {
                lt,
                lb,
                rb,
                rt
            };
        } else {
            Debug.LogWarning("Oops! The sideSlope cannot be 0!");
            return null;
        }
    }

    /// <summary>
    /// Attach this FlatParallelogramShape as a PolygonCollider2D to a GameObject.
    /// </summary>
    /// <param name="go">Target GameObject.</param>
    /// <returns></returns>
    override public Collider2D AttachCollider2D(GameObject go, bool isTrigger = false, bool attachRigidbody2D = false) {
        base.AttachCollider2D(go);

        //Calculate the path.
        //The path should be counter-clock wise.
        Vector2[] path = CalculatePath();
        if (path != null) {
            PolygonCollider2D newPolygonCollider2D = go.AddComponent<PolygonCollider2D>();
            newPolygonCollider2D.SetPath(0, path);
            newPolygonCollider2D.isTrigger = isTrigger;

            if (attachRigidbody2D) {
                Rigidbody2D rigidbody2D = go.AddComponent<Rigidbody2D>();
                rigidbody2D.gravityScale = 0.0f;
                rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
            }

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
            Mesh newMesh = new Mesh();

            newMesh.vertices = path.Select((vec2) => (Vector3)vec2).ToArray();
            newMesh.uv = path;
            Triangulator triangulator = new Triangulator(path);
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

            //Mesh stuffs.
            MeshFilter newMeshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = go.AddComponent<MeshRenderer>();
            Mesh newMesh = new Mesh();

            newMesh.vertices = path.Select((vec2) => (Vector3)vec2).ToArray();
            newMesh.uv = path;
            Triangulator triangulator = new Triangulator(path);
            newMesh.triangles = triangulator.Triangulate();

            newMeshFilter.mesh = newMesh;
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
            Mesh newMesh = new Mesh();

            newMesh.vertices = path.Select((vec2) => (Vector3)vec2).ToArray();
            newMesh.uv = path;
            Triangulator triangulator = new Triangulator(path);
            newMesh.triangles = triangulator.Triangulate();

            newMeshFilter.mesh = newMesh;

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
