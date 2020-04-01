using UnityEngine;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

[XmlRoot("V2I")]
[XmlType("V2I")]
[Serializable]
public class Vec2i : IEquatable<Vec2i> {

    [XmlElement("X")]
    public int x = 0;

    [XmlElement("Y")]
    public int y = 0;

    //==================================

    static public Vec2i Down {
        get {
            return new Vec2i(0, -1);
        }
    }

    static public Vec2i Left {
        get {
            return new Vec2i(-1, 0);
        }
    }

    static public Vec2i One {
        get {
            return new Vec2i(1, 1);
        }
    }

    static public Vec2i Right {
        get {
            return new Vec2i(1, 0);
        }
    }

    static public Vec2i Up {
        get {
            return new Vec2i(0, 1);
        }
    }

    static public Vec2i Zero {
        get {
            return new Vec2i(0, 0);
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

    public Vec2i() {

    }

    public Vec2i(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Vec2i(Vec2i vec) {
        Set(vec);
    }

    public Vec2i(Vec2 vec) {
        Set(vec);
    }

    public Vec2i(Vec3i vec) {
        Set(vec);
    }

    public Vec2i(Vec3 vec) {
        Set(vec);
    }

    public Vec2i(Vector2 vec) {
        Set(vec);
    }

    public Vec2i(Vector3 vec) {
        Set(vec);
    }

    //==================================

    public void Set(Vec2i vec) {
        x = vec.x;
        y = vec.y;
    }

    public void Set(Vec2 vec) {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
    }

    public void Set(Vec3i vec) {
        x = vec.x;
        y = vec.y;
    }

    public void Set(Vec3 vec) {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
    }

    public void Set(Vector2 vec) {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
    }

    public void Set(Vector3 vec) {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
    }

    //==================================

    public Vector2 ToVector2() {
        return new Vector2(x, y);
    }

    public Vector3 ToVector3() {
        return new Vector3(x, y, 1.0f);
    }

    //=========================================

    static public Vector2[] ArrayToVector2(Vec2i[] vec2Array) {
        Vector2[] newArray = new Vector2[vec2Array.Length];
        for (int i = 0; i < newArray.Length; i++) {
            newArray[i] = vec2Array[i].ToVector2();
        }
        return newArray;
    }

    static public List<Vector2> ListToVector2(List<Vec2i> vec2List) {
        List<Vector2> returnList = new List<Vector2>();
        for (int i = 0; i < vec2List.Count; i++) {
            returnList.Add(vec2List[i].ToVector2());
        }
        return returnList;
    }

    //================ Operators ==================

    public static Vec2i operator -(Vec2i vec1, Vec2i vec2) {
        return new Vec2i(vec1.x - vec2.x, vec1.y - vec2.y);
    }

    //[Dep] Dangerous
    // public static bool operator !=(Vec2i vec1, Vec2i vec2) {
    //     return (vec1.x != vec2.x || vec1.y != vec2.y);
    // }

    public static Vec2i operator *(Vec2i vec, float value) {
        return new Vec2i(Mathf.RoundToInt((float)vec.x * value), Mathf.RoundToInt((float)vec.y * value));
    }

    public static Vec2i operator *(float value, Vec2i vec) {
        return new Vec2i(Mathf.RoundToInt((float)vec.x * value), Mathf.RoundToInt((float)vec.y * value));
    }

    public static Vec2i operator /(Vec2i vec, float value) {
        if (value == 0.0f) {
            return Vec2i.Zero;
        }
        return new Vec2i(Mathf.RoundToInt(vec.x / value), Mathf.RoundToInt(vec.y / value));
    }

    public static Vec2i operator +(Vec2i vec1, Vec2i vec2) {
        return new Vec2i(vec1.x + vec2.x, vec1.y + vec2.y);
    }

    //[Dep] Dangerous
    // public static bool operator ==(Vec2i vec1, Vec2i vec2) {
    //     return (vec1.x == vec2.x && vec1.y == vec2.y);
    // }

    //================ IEquatable =====================

    public bool Equals(Vec2i vec) {
        if (vec == null)
            return false;

        return (this.x == vec.x && this.y == vec.y);
    }

    public override bool Equals(System.Object obj) {
        if (obj == null)
            return false;

        Vec2i vecObj = obj as Vec2i;
        if (vecObj == null)
            return false;
        else
            return (this.x == vecObj.x && this.y == vecObj.y);
    }

    public override int GetHashCode() {
        return x.GetHashCode() ^ y.GetHashCode();
    }

}
