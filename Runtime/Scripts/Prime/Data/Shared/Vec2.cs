using UnityEngine;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

[XmlRoot("V2")]
[XmlType("V2")]
[Serializable]
public class Vec2 : IEquatable<Vec2> {

    [XmlElement("X")]
    public float x = 0.0f;

    [XmlElement("Y")]
    public float y = 0.0f;

    //==================================

    static public Vec2 Down {
        get {
            return new Vec2(0, -1);
        }
    }

    static public Vec2 Left {
        get {
            return new Vec2(-1, 0);
        }
    }

    static public Vec2 One {
        get {
            return new Vec2(1, 1);
        }
    }

    static public Vec2 Right {
        get {
            return new Vec2(1, 0);
        }
    }

    static public Vec2 Up {
        get {
            return new Vec2(0, 1);
        }
    }

    static public Vec2 Zero {
        get {
            return new Vec2(0, 0);
        }
    }

    //==================================

    public float sqrMagnitude {
        get {
            return (x * x + y * y);
        }
    }

    public float magnitude {
        get {
            return Mathf.Sqrt(x * x + y * y);
        }
    }

    //==================================

    public Vec2() {

    }

    public Vec2(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Vec2(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public Vec2(Vec2i vec) {
        Set(vec);
    }

    public Vec2(Vec2 vec) {
        Set(vec);
    }

    public Vec2(Vec3i vec) {
        Set(vec);
    }

    public Vec2(Vec3 vec) {
        Set(vec);
    }

    public Vec2(Vector2 vec) {
        Set(vec);
    }

    public Vec2(Vector3 vec) {
        Set(vec);
    }

    //==================================

    public void Set(Vec2i vec) {
        x = vec.x;
        y = vec.y;
    }

    public void Set(Vec2 vec) {
        x = vec.x;
        y = vec.y;
    }

    public void Set(Vec3i vec) {
        x = vec.x;
        y = vec.y;
    }

    public void Set(Vec3 vec) {
        x = vec.x;
        y = vec.y;
    }

    public void Set(Vector2 vec) {
        x = vec.x;
        y = vec.y;
    }

    public void Set(Vector3 vec) {
        x = vec.x;
        y = vec.y;
    }

    //==================================

    public Vector2 ToVector2() {
        return new Vector2(x, y);
    }

    public Vector3 ToVector3() {
        return new Vector3(x, y, 1.0f);
    }

    //=========================================

    static public Vector2[] ArrayToVector2(Vec2[] vec2Array) {
        Vector2[] newArray = new Vector2[vec2Array.Length];
        for (int i = 0; i < newArray.Length; i++) {
            newArray[i] = vec2Array[i].ToVector2();
        }
        return newArray;
    }

    static public List<Vector2> ListToVector2(List<Vec2> vec2List) {
        List<Vector2> returnList = new List<Vector2>();
        for (int i = 0; i < vec2List.Count; i++) {
            returnList.Add(vec2List[i].ToVector2());
        }
        return returnList;
    }

    //================ Operators ==================

    public static Vec2 operator -(Vec2 vec1, Vec2 vec2) {
        return new Vec2(vec1.x - vec2.x, vec1.y - vec2.y);
    }

    //[Dep] Dangerous
    // public static bool operator !=(Vec2 vec1, Vec2 vec2) {
    //     return (vec1.x != vec2.x || vec1.y != vec2.y);
    // }

    public static Vec2 operator *(Vec2 vec, float value) {
        return new Vec2(vec.x * value, vec.y * value);
    }

    public static Vec2 operator *(float value, Vec2 vec) {
        return new Vec2(vec.x * value, vec.y * value);
    }

    public static Vec2 operator /(Vec2 vec, float value) {
        if (value == 0.0f) {
            return Vec2.Zero;
        }
        return new Vec2(vec.x / value, vec.y / value);
    }

    public static Vec2 operator +(Vec2 vec1, Vec2 vec2) {
        return new Vec2(vec1.x + vec2.x, vec1.y + vec2.y);
    }

    //[Dep] Dangerous
    // public static bool operator ==(Vec2 vec1, Vec2 vec2) {
    //     return (vec1.x == vec2.x && vec1.y == vec2.y);
    // }

    //================ IEquatable =====================

    public bool Equals(Vec2 vec) {
        if (vec == null)
            return false;

        return (this.x == vec.x && this.y == vec.y);
    }

    public override bool Equals(System.Object obj) {
        if (obj == null)
            return false;

        Vec2 vecObj = obj as Vec2;
        if (vecObj == null)
            return false;
        else
            return (this.x == vecObj.x && this.y == vecObj.y);
    }

    public override int GetHashCode() {
        return x.GetHashCode() ^ y.GetHashCode();
    }

}
