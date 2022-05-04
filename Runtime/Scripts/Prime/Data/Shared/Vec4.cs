using UnityEngine;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

[XmlRoot("V4")]
[XmlType("V4")]
[Serializable]
public class Vec4 : IEquatable<Vec4> {

    [XmlElement("X")]
    public float x = 0.0f;

    [XmlElement("Y")]
    public float y = 0.0f;

    [XmlElement("Z")]
    public float z = 0.0f;

    [XmlElement("W")]
    public float w = 0.0f;

    //==================================

    static public Vec4 One {
        get {
            return new Vec4(1, 1, 1, 1);
        }
    }

    static public Vec4 Zero {
        get {
            return new Vec4(0, 0, 0, 0);
        }
    }

    //==================================

    public Vec4() {

    }

    public Vec4(int x, int y, int z, int w) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Vec4(float x, float y, float z, float w) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Vec4(Vec4i vec) {
        Set(vec);
    }

    public Vec4(Vec4 vec) {
        Set(vec);
    }

    public Vec4(Quaternion quaternion) {
        Set(quaternion);
    }

    //==================================
    public void Set(Vec4i vec) {
        x = vec.x;
        y = vec.y;
        z = vec.z;
        w = vec.w;
    }

    public void Set(Vec4 vec) {
        x = vec.x;
        y = vec.y;
        z = vec.z;
        w = vec.w;
    }

    public void Set(Vector4 vec) {
        x = vec.x;
        y = vec.y;
        z = vec.z;
        w = vec.w;
    }

    public void Set(Quaternion quaternion) {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }

    //==================================

    public Vector4 ToVector4() {
        return new Vector4(x, y, z, w);
    }

    public Quaternion ToQuaternion() {
        return new Quaternion(x, y, z, w);
    }

    //=========================================

    static public Vector4[] ArrayToVector4(Vec4[] vec3Array) {
        Vector4[] newArray = new Vector4[vec3Array.Length];
        for (int i = 0; i < newArray.Length; i++) {
            newArray[i] = vec3Array[i].ToVector4();
        }
        return newArray;
    }

    static public List<Vector4> ListToVector4(List<Vec4> vec3List) {
        List<Vector4> returnList = new List<Vector4>();
        for (int i = 0; i < vec3List.Count; i++) {
            returnList.Add(vec3List[i].ToVector4());
        }
        return returnList;
    }

    static public Quaternion[] ArrayToQuaternion(Vec4[] vec3Array) {
        Quaternion[] newArray = new Quaternion[vec3Array.Length];
        for (int i = 0; i < newArray.Length; i++) {
            newArray[i] = vec3Array[i].ToQuaternion();
        }
        return newArray;
    }

    static public List<Quaternion> ListToQuaternion(List<Vec4> vec3List) {
        List<Quaternion> returnList = new List<Quaternion>();
        for (int i = 0; i < vec3List.Count; i++) {
            returnList.Add(vec3List[i].ToQuaternion());
        }
        return returnList;
    }

    //================ Operators ==================

    public static Vec4 operator -(Vec4 vec1, Vec4 vec2) {
        return new Vec4(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z, vec1.w - vec2.w);
    }

    public static Vec4 operator *(Vec4 vec, float value) {
        return new Vec4(Mathf.RoundToInt((float)vec.x * value), Mathf.RoundToInt((float)vec.y * value), Mathf.RoundToInt((float)vec.z * value), Mathf.RoundToInt((float)vec.w * value));
    }

    public static Vec4 operator *(float value, Vec4 vec) {
        return new Vec4(Mathf.RoundToInt((float)vec.x * value), Mathf.RoundToInt((float)vec.y * value), Mathf.RoundToInt((float)vec.z * value), Mathf.RoundToInt((float)vec.w * value));
    }

    public static Vec4 operator /(Vec4 vec, float value) {
        if (value == 0.0f) {
            return Vec4.Zero;
        }
        return new Vec4(Mathf.RoundToInt(vec.x / value), Mathf.RoundToInt(vec.y / value), Mathf.RoundToInt(vec.z / value), Mathf.RoundToInt(vec.w / value));
    }

    public static Vec4 operator +(Vec4 vec1, Vec4 vec2) {
        return new Vec4(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z, vec1.w + vec2.w);
    }

    //================ IEquatable =====================

    public bool Equals(Vec4 vec) {
        if (vec == null)
            return false;

        return (this.x == vec.x && this.y == vec.y && this.z == vec.z && this.w == vec.w);
    }

    public override bool Equals(System.Object obj) {
        if (obj == null)
            return false;

        Vec4 vecObj = obj as Vec4;
        if (vecObj == null)
            return false;
        else
            return (this.x == vecObj.x && this.y == vecObj.y && this.z == vecObj.z && this.w == vecObj.w);
    }

    public override int GetHashCode() {
        return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
    }


}
