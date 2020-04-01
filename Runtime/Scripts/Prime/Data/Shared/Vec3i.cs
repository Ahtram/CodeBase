using UnityEngine;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

[XmlRoot("V3I")]
[XmlType("V3I")]
[Serializable]
public class Vec3i : IEquatable<Vec3i> {
    
    [XmlElement("X")]
    public int x = 0;

    [XmlElement("Y")]
    public int y = 0;

    [XmlElement("Z")]
    public int z = 0;

    //==================================

    static public Vec3i Back {
        get {
            return new Vec3i(0, 0, -1);
        }
    }

    static public Vec3i Down {
        get {
            return new Vec3i(0, -1, 0);
        }
    }

    static public Vec3i Forward {
        get {
            return new Vec3i(0, 0, 1);
        }
    }

    static public Vec3i Left {
        get {
            return new Vec3i(-1, 0, 0);
        }
    }

    static public Vec3i One {
        get {
            return new Vec3i(1, 1, 0);
        }
    }

    static public Vec3i Right {
        get {
            return new Vec3i(1, 0, 0);
        }
    }

    static public Vec3i Up {
        get {
            return new Vec3i(0, 1, 0);
        }
    }

    static public Vec3i Zero {
        get {
            return new Vec3i(0, 0, 0);
        }
    }

    //==================================

    public float sqrMagnitude {
        get {
            return (x * x + y * y + z * z);
        }
    }

    public float magnitude {
        get {
            return Mathf.Sqrt(x * x + y * y + z * z);
        }
    }

    //==================================

    public Vec3i() {

    }

    public Vec3i(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vec3i(Vec2i vec) {
        Set(vec);
    }

    public Vec3i(Vec2 vec) {
        Set(vec);
    }

    public Vec3i(Vec3i vec) {
        Set(vec);
    }

    public Vec3i(Vec3 vec) {
        Set(vec);
    }

    public Vec3i(Vector2 vec) {
        Set(vec);
    }

    public Vec3i(Vector3 vec) {
        Set(vec);
    }

    //==================================

    public void Set(Vec2i vec) {
        x = vec.x;
        y = vec.y;
        z = 0;
    }

    public void Set(Vec2 vec) {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
        z = 0;
    }

    public void Set(Vec3i vec) {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }

    public void Set(Vec3 vec) {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
        z = Mathf.RoundToInt(vec.z);
    }

    public void Set(Vector2 vec) {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
        z = 0;
    }

    public void Set(Vector3 vec) {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
        z = Mathf.RoundToInt(vec.z);
    }

    //==================================

    public Vector2 ToVector2() {
        return new Vector2(x, y);
    }

    public Vector3 ToVector3() {
        return new Vector3(x, y, z);
    }

    //=========================================

    static public Vector3[] ArrayToVector3(Vec3i[] vec3Array) {
        Vector3[] newArray = new Vector3[vec3Array.Length];
        for (int i = 0; i < newArray.Length; i++) {
            newArray[i] = vec3Array[i].ToVector3();
        }
        return newArray;
    }

    static public List<Vector3> ListToVector3(List<Vec3i> vec3List) {
        List<Vector3> returnList = new List<Vector3>();
        for (int i = 0; i < vec3List.Count; i++) {
            returnList.Add(vec3List[i].ToVector3());
        }
        return returnList;
    }

    //================ Operators ==================

    public static Vec3i operator -(Vec3i vec1, Vec3i vec2) {
        return new Vec3i(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
    }

    //[Dep] Dangerous
    // public static bool operator !=(Vec3i vec1, Vec3i vec2) {
    //     return (vec1.x != vec2.x || vec1.y != vec2.y|| vec1.z != vec2.z);
    // }

    public static Vec3i operator *(Vec3i vec, float value) {
        return new Vec3i(Mathf.RoundToInt((float)vec.x * value), Mathf.RoundToInt((float)vec.y * value), Mathf.RoundToInt((float)vec.z * value));
    }

    public static Vec3i operator *(float value, Vec3i vec) {
        return new Vec3i(Mathf.RoundToInt((float)vec.x * value), Mathf.RoundToInt((float)vec.y * value), Mathf.RoundToInt((float)vec.z * value));
    }

    public static Vec3i operator /(Vec3i vec, float value) {
        if (value == 0.0f) {
            return Vec3i.Zero;
        }
        return new Vec3i(Mathf.RoundToInt(vec.x / value), Mathf.RoundToInt(vec.y / value), Mathf.RoundToInt(vec.z / value));
    }

    public static Vec3i operator +(Vec3i vec1, Vec3i vec2) {
        return new Vec3i(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
    }

    //[Dep] Dangerous
    // public static bool operator ==(Vec3i vec1, Vec3i vec2) {
    //     return (vec1.x == vec2.x && vec1.y == vec2.y && vec1.z == vec2.z);
    // }

    //================ IEquatable =====================

    public bool Equals(Vec3i vec) {
        if (vec == null)
            return false;

        return (this.x == vec.x && this.y == vec.y && this.z == vec.z);
    }

    public override bool Equals(System.Object obj) {
        if (obj == null)
            return false;

        Vec3i vecObj = obj as Vec3i;
        if (vecObj == null)
            return false;
        else
            return (this.x == vecObj.x && this.y == vecObj.y && this.z == vecObj.z);
    }

    public override int GetHashCode() {
        return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }

}
