using UnityEngine;
using System.Xml.Serialization;
using System;
using System.Linq;

[XmlRoot("BS")]
[XmlType("BS")]
[Serializable]
public class BoxShape : Shape {

    [XmlElement("S")]
    public Vec2 size = new Vec2(100.0f, 100.0f);

    [XmlElement("O")]
    public Vec2 offset = new Vec2(0.0f, 0.0f);

    public BoxShape() {
        type = Type.Box;
    }

    public BoxShape(Vec2 size) {
        type = Type.Box;
        this.size.Set(size);
    }

    public BoxShape(Vec2 size, Vec2 offset) {
        type = Type.Box;
        this.size.Set(size);
        this.offset.Set(offset);
    }

    /// <summary>
    /// Calculate the path of this thing.
    /// </summary>
    /// <returns></returns>
    private Vector2[] CalculatePath() {
        float halfWidth = size.x * 0.5f;
        float halfHeight = size.y * 0.5f;

        Vector2 lt = new Vector2(-(halfWidth), halfHeight) + offset.ToVector2();
        Vector2 lb = new Vector2(-(halfWidth), -halfHeight) + offset.ToVector2();
        Vector2 rb = new Vector2(halfWidth, -halfHeight) + offset.ToVector2();
        Vector2 rt = new Vector2(halfWidth, halfHeight) + offset.ToVector2();

        return new Vector2[] {
            lt,
            lb,
            rb,
            rt
        };
    }

    /// <summary>
    /// Attach this box as a BoxCollider2D to a GameObject.
    /// </summary>
    /// <param name="go">Target GameObject.</param>
    /// <returns></returns>
    override public Collider2D AttachCollider2D(GameObject go, bool isTrigger = false, bool attachRigidbody2D = false) {
        base.AttachCollider2D(go);

        BoxCollider2D newBoxCollider2D = go.AddComponent<BoxCollider2D>();
        newBoxCollider2D.offset = offset.ToVector2();
        newBoxCollider2D.size = size.ToVector2();
        newBoxCollider2D.isTrigger = isTrigger;

        if (attachRigidbody2D) {
            Rigidbody2D rigidbody2D = go.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0.0f;
            rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }

        return newBoxCollider2D;
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

        //Collider stuffs.
        BoxCollider2D newBoxCollider2D = go.AddComponent<BoxCollider2D>();
        newBoxCollider2D.offset = offset.ToVector2();
        newBoxCollider2D.size = size.ToVector2();
        newBoxCollider2D.isTrigger = colliderIsTrigger;

        Rigidbody2D rigidbody2D = go.AddComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0.0f;
        rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

        //Calculate the path.
        //The path should be counter-clock wise.
        Vector2[] path = CalculatePath();
        if (path != null) {
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

        //Collider stuffs.
        BoxCollider2D newBoxCollider2D = go.AddComponent<BoxCollider2D>();
        newBoxCollider2D.offset = offset.ToVector2();
        newBoxCollider2D.size = size.ToVector2();
        newBoxCollider2D.isTrigger = colliderIsTrigger;

        Rigidbody2D rigidbody2D = go.AddComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0.0f;
        rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

        //Calculate the path.
        //The path should be counter-clock wise.
        Vector2[] path = CalculatePath();
        if (path != null) {
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
