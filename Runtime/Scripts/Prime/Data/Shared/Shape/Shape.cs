using System;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using UnityEngine;

#if NETFX_CORE && !UNITY_WSA_10_0
using WinRTLegacy;
#endif

[XmlRoot("S")]
[XmlType("S")]
[XmlInclude(typeof(NoneShape))]
[XmlInclude(typeof(BoxShape))]
[XmlInclude(typeof(PolygonShape))]
[XmlInclude(typeof(FlatParallelogramShape))]
[XmlInclude(typeof(SlopeEllipseShape))]
[Serializable]
abstract public class Shape {

    static public string SHAPE_DEFAULT_MAT = "Materials/ShapeDefault";

    public enum Type {
        None,
        Box,
        Polygon,
        FlatParallelogram,
        SlopeEllipse
    }

    [XmlElement("T")]
    public Type type = Type.None;

    //Implement this to make the shape be able to apply a collider2D to a gameobject. (like BoxCollider2D or PolyCollider2D)
    //Possible return null.
    virtual public Collider2D AttachCollider2D(GameObject go, bool isTrigger = true, bool attachRigidbody2D = false) {
        //Destroy exist stuff.
        PolygonCollider2D existPolygonCollider2D = go.GetComponent<PolygonCollider2D>();
        if (existPolygonCollider2D != null) {
            Debug.Log("existPolygonCollider2D detected!");
        }

        //Destroy exist stuff.
        BoxCollider2D existBoxCollider2D = go.GetComponent<BoxCollider2D>();
        if (existBoxCollider2D != null) {
            Debug.Log("existBoxCollider2D detected!");
        }

        //Destroy exist stuff.
        Rigidbody2D rigidbody2D = go.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null) {
            Debug.Log("rigidbody2D detected!");
        }

        return null;
    }

    //Attach a MeshRenderer and MeshFilter for this shape.
    virtual public MeshRenderer AttachMeshRenderer(GameObject go) {
        //Destroy exist stuff.
        MeshRenderer existMeshRenderer = go.GetComponent<MeshRenderer>();
        if (existMeshRenderer != null) {
            Debug.Log("existMeshRenderer detected!");
        }

        //Destroy exist stuff.
        MeshFilter existMeshFilter = go.GetComponent<MeshFilter>();
        if (existMeshFilter != null) {
            Debug.Log("existMeshFilter detected!");
        }

        return null;
    }

    //Attach a LineRenderer for this shape.
    virtual public LineRenderer AttachLineRenderer(GameObject go) {
        //Destroy exist stuff.
        LineRenderer existLineRenderer = go.GetComponent<LineRenderer>();
        if (existLineRenderer != null) {
            Debug.Log("existLineRenderer detected!");
        }

        return null;
    }

    //Attach the stuffs above.
    abstract public void AttachMeshAndCollider2D(GameObject go, bool colliderIsTrigger = false);

    //Attach all stuffs.
    abstract public void AttachAllComponents(GameObject go, bool colliderIsTrigger = false);

    //Do we have a center position?
    virtual public bool HasACenterPosition() {
        return false;
    }

    //Implement this for your center position.
    virtual public Vector2 CenterPosition() {
        return Vector2.zero;
    }

    //================= Tools For Editor ==================
    static public Shape New(Shape.Type type) {
        //A bit tricky: use this to replace the swithc case.
        Shape returnObject = (Shape)Activator.CreateInstance(System.Type.GetType(type.ToString() + "Shape"));
        returnObject.type = type;
        return returnObject;
    }

    //Use reflection to make a default constructor directly for child class.
    public object GetDefault() {
        return this.GetType().GetMethod("GetDefaultGeneric", BindingFlags.Instance | BindingFlags.Public).MakeGenericMethod(this.GetType()).Invoke(this, null);
    }

    public T GetDefaultGeneric<T>() {
        return default(T);
    }

    virtual public Shape Clone() {
        if (System.Object.ReferenceEquals(this, null)) {
            return (Shape)GetDefault();
        }

        XmlSerializer serializer = new XmlSerializer(typeof(Shape));
        Stream stream = new MemoryStream();
        serializer.Serialize(stream, this);
        stream.Seek(0, SeekOrigin.Begin);
        Shape returnObject = (Shape)serializer.Deserialize(stream);
        return returnObject;
    }

}
