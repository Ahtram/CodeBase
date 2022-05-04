using UnityEngine;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

[XmlRoot("V4I")]
[XmlType("V4I")]
[Serializable]
public class Vec4i : IEquatable<Vec4i> {

    [XmlElement("X")]
    public int x = 0;

    [XmlElement("Y")]
    public int y = 0;

    [XmlElement("Z")]
    public int z = 0;

    [XmlElement("W")]
    public int w = 0;

    //==================================

    static public Vec4i One {
        get {
            return new Vec4i(1, 1, 1, 1);
        }
    }

    static public Vec4i Zero {
        get {
            return new Vec4i(0, 0, 0, 0);
        }
    }

    //==================================

    public Vec4i() {

    }

    public Vec4i(int x, int y, int z, int w) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Vec4i(Vec4 vec) {
        Set(vec);
    }

    public Vec4i(Vec4i vec) {
        Set(vec);
    }

    public Vec4i(Quaternion quaternion) {
        Set(quaternion);
    }

    //==================================

    public void Set(Vec4 vec) {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
        z = Mathf.RoundToInt(vec.z);
        w = Mathf.RoundToInt(vec.w);
    }

    public void Set(Vec4i vec) {
        x = vec.x;
        y = vec.y;
        z = vec.z;
        w = vec.w;
    }

    public void Set(Vector4 vec) {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
        z = Mathf.RoundToInt(vec.z);
        w = Mathf.RoundToInt(vec.w);
    }

    public void Set(Quaternion quaternion) {
        x = Mathf.RoundToInt(quaternion.x);
        y = Mathf.RoundToInt(quaternion.y);
        z = Mathf.RoundToInt(quaternion.z);
        w = Mathf.RoundToInt(quaternion.w);
    }

    //==================================

    public Vector4 ToVector4() {
        return new Vector4(x, y, z, w);
    }

    public Quaternion ToQuaternion() {
        return new Quaternion(x, y, z, w);
    }

    //=========================================

    static public Vector4[] ArrayToVector4(Vec4i[] vec3Array) {
        Vector4[] newArray = new Vector4[vec3Array.Length];
        for (int i = 0; i < newArray.Length; i++) {
            newArray[i] = vec3Array[i].ToVector4();
        }
        return newArray;
    }

    static public List<Vector4> ListToVector4(List<Vec4i> vec3List) {
        List<Vector4> returnList = new List<Vector4>();
        for (int i = 0; i < vec3List.Count; i++) {
            returnList.Add(vec3List[i].ToVector4());
        }
        return returnList;
    }

    static public Quaternion[] ArrayToQuaternion(Vec4i[] vec3Array) {
        Quaternion[] newArray = new Quaternion[vec3Array.Length];
        for (int i = 0; i < newArray.Length; i++) {
            newArray[i] = vec3Array[i].ToQuaternion();
        }
        return newArray;
    }

    static public List<Quaternion> ListToQuaternion(List<Vec4i> vec3List) {
        List<Quaternion> returnList = new List<Quaternion>();
        for (int i = 0; i < vec3List.Count; i++) {
            returnList.Add(vec3List[i].ToQuaternion());
        }
        return returnList;
    }

    //================ Operators ==================

    public static Vec4i operator -(Vec4i vec1, Vec4i vec2) {
        return new Vec4i(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z, vec1.w - vec2.w);
    }

    public static Vec4i operator *(Vec4i vec, float value) {
        return new Vec4i(Mathf.RoundToInt((float)vec.x * value), Mathf.RoundToInt((float)vec.y * value), Mathf.RoundToInt((float)vec.z * value), Mathf.RoundToInt((float)vec.w * value));
    }

    public static Vec4i operator *(float value, Vec4i vec) {
        return new Vec4i(Mathf.RoundToInt((float)vec.x * value), Mathf.RoundToInt((float)vec.y * value), Mathf.RoundToInt((float)vec.z * value), Mathf.RoundToInt((float)vec.w * value));
    }

    public static Vec4i operator /(Vec4i vec, float value) {
        if (value == 0.0f) {
            return Vec4i.Zero;
        }
        return new Vec4i(Mathf.RoundToInt(vec.x / value), Mathf.RoundToInt(vec.y / value), Mathf.RoundToInt(vec.z / value), Mathf.RoundToInt(vec.w / value));
    }

    public static Vec4i operator +(Vec4i vec1, Vec4i vec2) {
        return new Vec4i(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z, vec1.w + vec2.w);
    }

    //================ IEquatable =====================

    public bool Equals(Vec4i vec) {
        if (vec == null)
            return false;

        return (this.x == vec.x && this.y == vec.y && this.z == vec.z && this.w == vec.w);
    }

    public override bool Equals(System.Object obj) {
        if (obj == null)
            return false;

        Vec4i vecObj = obj as Vec4i;
        if (vecObj == null)
            return false;
        else
            return (this.x == vecObj.x && this.y == vecObj.y && this.z == vecObj.z && this.w == vecObj.w);
    }

    public override int GetHashCode() {
        return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
    }

}
