using UnityEngine;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

[XmlRoot("V3")]
[XmlType("V3")]
[Serializable]
public class Vec3 : IEquatable<Vec3> {

    [XmlElement("X")]
    public float x = 0.0f;

    [XmlElement("Y")]
    public float y = 0.0f;

    [XmlElement("Z")]
    public float z = 0.0f;

    //==================================

    static public Vec3 Back {
        get {
            return new Vec3(0, 0, -1);
        }
    }

    static public Vec3 Down {
        get {
            return new Vec3(0, -1, 0);
        }
    }

    static public Vec3 Forward {
        get {
            return new Vec3(0, 0, 1);
        }
    }

    static public Vec3 Left {
        get {
            return new Vec3(-1, 0, 0);
        }
    }

    static public Vec3 One {
        get {
            return new Vec3(1, 1, 1);
        }
    }

    static public Vec3 Right {
        get {
            return new Vec3(1, 0, 0);
        }
    }

    static public Vec3 Up {
        get {
            return new Vec3(0, 1, 0);
        }
    }

    static public Vec3 Zero {
        get {
            return new Vec3(0, 0, 0);
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

    public Vec3() {

    }

    public Vec3(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vec3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vec3(Vec2i vec) {
        Set(vec);
    }

    public Vec3(Vec2 vec) {
        Set(vec);
    }

    public Vec3(Vec3i vec) {
        Set(vec);
    }

    public Vec3(Vec3 vec) {
        Set(vec);
    }

    public Vec3(Vector2 vec) {
        Set(vec);
    }

    public Vec3(Vector3 vec) {
        Set(vec);
    }

    //==================================

    public void Set(Vec2i vec) {
        x = vec.x;
        y = vec.y;
        z = 0;
    }

    public void Set(Vec2 vec) {
        x = vec.x;
        y = vec.y;
        z = 0;
    }

    public void Set(Vec3i vec) {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }

    public void Set(Vec3 vec) {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }

    public void Set(Vector2 vec) {
        x = vec.x;
        y = vec.y;
        z = 0;
    }

    public void Set(Vector3 vec) {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }

    //==================================

    public Vector2 ToVector2() {
        return new Vector2(x, y);
    }

    public Vector3 ToVector3() {
        return new Vector3(x, y, z);
    }

    //=========================================

    static public Vector3[] ArrayToVector3(Vec3[] vec3Array) {
        Vector3[] newArray = new Vector3[vec3Array.Length];
        for (int i = 0; i < newArray.Length; i++) {
            newArray[i] = vec3Array[i].ToVector3();
        }
        return newArray;
    }

    static public List<Vector3> ListToVector3(List<Vec3> vec3List) {
        List<Vector3> returnList = new List<Vector3>();
        for (int i = 0; i < vec3List.Count; i++) {
            returnList.Add(vec3List[i].ToVector3());
        }
        return returnList;
    }

    //================ Operators ==================

    public static Vec3 operator -(Vec3 vec1, Vec3 vec2) {
        return new Vec3(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
    }

    //[Dep] Dangerous
    // public static bool operator !=(Vec3 vec1, Vec3 vec2) {
    //     return (vec1.x != vec2.x || vec1.y != vec2.y || vec1.z != vec2.z);
    // }

    public static Vec3 operator *(Vec3 vec, float value) {
        return new Vec3(Mathf.RoundToInt((float)vec.x * value), Mathf.RoundToInt((float)vec.y * value), Mathf.RoundToInt((float)vec.z * value));
    }

    public static Vec3 operator *(float value, Vec3 vec) {
        return new Vec3(Mathf.RoundToInt((float)vec.x * value), Mathf.RoundToInt((float)vec.y * value), Mathf.RoundToInt((float)vec.z * value));
    }

    public static Vec3 operator /(Vec3 vec, float value) {
        if (value == 0.0f) {
            return Vec3.Zero;
        }
        return new Vec3(Mathf.RoundToInt(vec.x / value), Mathf.RoundToInt(vec.y / value), Mathf.RoundToInt(vec.z / value));
    }

    public static Vec3 operator +(Vec3 vec1, Vec3 vec2) {
        return new Vec3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
    }

    //[Dep] Dangerous
    // public static bool operator ==(Vec3 vec1, Vec3 vec2) {
    //     return (vec1.x == vec2.x && vec1.y == vec2.y && vec1.z == vec2.z);
    // }

    //================ IEquatable =====================

    public bool Equals(Vec3 vec) {
        if (vec == null)
            return false;

        return (this.x == vec.x && this.y == vec.y && this.z == vec.z);
    }

    public override bool Equals(System.Object obj) {
        if (obj == null)
            return false;

        Vec3 vecObj = obj as Vec3;
        if (vecObj == null)
            return false;
        else
            return (this.x == vecObj.x && this.y == vecObj.y && this.z == vecObj.z);
    }

    public override int GetHashCode() {
        return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }


}
