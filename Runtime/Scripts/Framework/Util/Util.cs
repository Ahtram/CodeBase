using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;
using CodeBaseExtensions;
using System.Security.Cryptography;

//using System.IO;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;

static public class Util {

    public enum OperatorType {
        Equal = 0,
        NotEqual = 1,
        Greater = 2,
        GreaterEqual = 3,
        Less = 4,
        LessEqual = 5,
        And = 6,
        Or = 7
    };

    static public readonly string[] OPERATER_DISPLAY_STRINGS = new string[] {
        "==",
        "!=",
        ">",
        ">=",
        "<",
        "<=",
        "&&",
        "||"
    };

    static public bool LogicConsequence(int lValue, OperatorType operatorType, int rValue) {
        switch (operatorType) {
            case Util.OperatorType.Equal:
                return (lValue == rValue);
            case Util.OperatorType.NotEqual:
                return (lValue != rValue);
            case Util.OperatorType.Greater:
                return (lValue > rValue);
            case Util.OperatorType.GreaterEqual:
                return (lValue >= rValue);
            case Util.OperatorType.Less:
                return (lValue < rValue);
            case Util.OperatorType.LessEqual:
                return (lValue <= rValue);
            case Util.OperatorType.And:
                return (Util.IntToBool(lValue) && Util.IntToBool(rValue));
            case Util.OperatorType.Or:
                return (Util.IntToBool(lValue) || Util.IntToBool(rValue));
            default:
                return false;
        }
    }

    static public int LogicResult(int lValue, OperatorType operatorType, int rValue) {
        switch (operatorType) {
            case Util.OperatorType.Equal:
                return Util.BoolToInt((lValue == rValue));
            case Util.OperatorType.NotEqual:
                return Util.BoolToInt((lValue != rValue));
            case Util.OperatorType.Greater:
                return Util.BoolToInt((lValue > rValue));
            case Util.OperatorType.GreaterEqual:
                return Util.BoolToInt((lValue >= rValue));
            case Util.OperatorType.Less:
                return Util.BoolToInt((lValue < rValue));
            case Util.OperatorType.LessEqual:
                return Util.BoolToInt((lValue <= rValue));
            case Util.OperatorType.And:
                return Util.BoolToInt((Util.IntToBool(lValue) && Util.IntToBool(rValue)));
            case Util.OperatorType.Or:
                return Util.BoolToInt((Util.IntToBool(lValue) || Util.IntToBool(rValue)));
            default:
                return 0;
        }
    }

    static public bool LogicConsequence(float lValue, OperatorType operatorType, float rValue) {
        switch (operatorType) {
            case Util.OperatorType.Equal:
                return (lValue == rValue);
            case Util.OperatorType.NotEqual:
                return (lValue != rValue);
            case Util.OperatorType.Greater:
                return (lValue > rValue);
            case Util.OperatorType.GreaterEqual:
                return (lValue >= rValue);
            case Util.OperatorType.Less:
                return (lValue < rValue);
            case Util.OperatorType.LessEqual:
                return (lValue <= rValue);
            case Util.OperatorType.And:
                return (Util.IntToBool((int)lValue) && Util.IntToBool((int)rValue));
            case Util.OperatorType.Or:
                return (Util.IntToBool((int)lValue) || Util.IntToBool((int)rValue));
            default:
                return false;
        }
    }

    static public int LogicResult(float lValue, OperatorType operatorType, float rValue) {
        switch (operatorType) {
            case Util.OperatorType.Equal:
                return Util.BoolToInt((lValue == rValue));
            case Util.OperatorType.NotEqual:
                return Util.BoolToInt((lValue != rValue));
            case Util.OperatorType.Greater:
                return Util.BoolToInt((lValue > rValue));
            case Util.OperatorType.GreaterEqual:
                return Util.BoolToInt((lValue >= rValue));
            case Util.OperatorType.Less:
                return Util.BoolToInt((lValue < rValue));
            case Util.OperatorType.LessEqual:
                return Util.BoolToInt((lValue <= rValue));
            case Util.OperatorType.And:
                return Util.BoolToInt((Util.IntToBool((int)lValue) && Util.IntToBool((int)rValue)));
            case Util.OperatorType.Or:
                return Util.BoolToInt((Util.IntToBool((int)lValue) || Util.IntToBool((int)rValue)));
            default:
                return 0;
        }
    }

    static public Vector2 VecAverage(List<Vector2> inputVectors) {
        if (inputVectors.Count > 0) {
            Vector2 sumVector = Vector2.zero;
            for (int i = 0; i < inputVectors.Count; i++) {
                sumVector += inputVectors[i];
            }
            return sumVector / inputVectors.Count;
        } else {
            return Vector2.zero;
        }
    }

    static public Vector3 VecAverage(List<Vector3> inputVectors) {
        if (inputVectors.Count > 0) {
            Vector3 sumVector = Vector3.zero;
            for (int i = 0; i < inputVectors.Count; i++) {
                sumVector += inputVectors[i];
            }
            return sumVector / inputVectors.Count;
        } else {
            return Vector3.zero;
        }
    }

    static public bool VecEqual(Vector3 vec1, Vector3 vec2, float tolerateValue = 0.0001f) {
        //return (Mathf.Approximately(vec1.x, vec2.x) && Mathf.Approximately(vec1.y, vec2.y) && Mathf.Approximately(vec1.z, vec2.z));
        return Vector3.SqrMagnitude(vec2 - vec1) < tolerateValue;
    }

    static public bool VecEqual(Vector2 vec1, Vector2 vec2, float tolerateValue = 0.0001f) {
        //return (Mathf.Approximately(vec1.x, vec2.x) && Mathf.Approximately(vec1.y, vec2.y));
        return Vector2.SqrMagnitude(vec2 - vec1) < tolerateValue;
    }

    static public bool VecEqual(Vec3 vec1, Vec3 vec2, float tolerateValue = 0.0001f) {
        //return (Mathf.Approximately(vec1.x, vec2.x) && Mathf.Approximately(vec1.y, vec2.y) && Mathf.Approximately(vec1.z, vec2.z));
        return VecEqual(vec1.ToVector3(), vec2.ToVector3(), tolerateValue);
    }

    static public bool VecEqual(Vec2 vec1, Vec2 vec2, float tolerateValue = 0.0001f) {
        //return (Mathf.Approximately(vec1.x, vec2.x) && Mathf.Approximately(vec1.y, vec2.y));
        return VecEqual(vec1.ToVector2(), vec2.ToVector2(), tolerateValue);
    }

    static public Color RandomColor() {
        return new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
    }

    static public Color Color255Convert(int r, int g, int b, int a) {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
    }

    static public Color Color255Convert(int r, int g, int b) {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    static public string ColorToHex(Color32 color) {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
        return hex;
    }

    static public Color HexToColor(string hex) {
        if (hex.Length == 6 || hex.Length == 8) {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            if (hex.Length == 8) {
                //Hex with alpha.
                byte a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                return new Color32(r, g, b, a);
            } else {
                //No alpha.
                return new Color32(r, g, b, 255);
            }
        } else {
            return Color.white;
        }
    }

    //Alpha = 0.0f ~ 1.0f
    static public Color HexToColor(string hex, float alpha) {
        if (hex.Length == 6 || hex.Length == 8) {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = (byte)(alpha * 255.0f);
            return new Color32(r, g, b, a);
        } else {
            Console.OutWarning("Oops! This is not a 6 characters hex color: " + hex);
            return new Color(1.0f, 1.0f, 1.0f, alpha); ;
        }
    }

    //A hack way to generate a color for a specific string. (very stupid but it work!)
    static public Color GetMagicColor(string inputStr) {
        int hashCode = inputStr.GetHashCode();
        //Just use the first 3 digit for generate color.
        float r = 1.0f - ((hashCode / Mathf.Pow(10, 0)) % 10) / 9.0f;
        float g = 1.0f - ((hashCode / Mathf.Pow(10, 1)) % 10) / 9.0f;
        float b = 1.0f - ((hashCode / Mathf.Pow(10, 2)) % 10) / 9.0f;
        return new Color(r, g, b);
    }

    static public Color GetMagicColorLighten(string inputStr) {
        int hashCode = inputStr.GetHashCode();
        //Just use the first 3 digit for generate color.
        float r = 1.0f - (((hashCode / Mathf.Pow(10, 0)) % 10) / 18.0f);
        float g = 1.0f - (((hashCode / Mathf.Pow(10, 1)) % 10) / 18.0f);
        float b = 1.0f - (((hashCode / Mathf.Pow(10, 2)) % 10) / 18.0f);
        return new Color(r, g, b);
    }

    static public string MarkupWithColorTag(string inputStr, string hex) {
        return ("<color=#" + hex + ">" + inputStr + "</color>");
    }

    static public string MarkupWithColorTag(string inputStr, Color color) {
        return ("<color=#" + ColorToHex(color) + ">" + inputStr + "</color>");
    }

    static public bool RandomBool() {
        if (UnityEngine.Random.value >= 0.5f) {
            return true;
        } else {
            return false;
        }
    }

    //Rate = the rate return true (0.0f ~ 1.0f)
    static public bool RandomBool(float rate) {
        if (UnityEngine.Random.value < rate) {
            return true;
        } else {
            return false;
        }
    }

    static public int RandomPositiveAndNegative() {
        return (RandomBool()) ? (1) : (-1);
    }

    static public int RandomPositiveAndNegative(float positiveRate) {
        return (RandomBool(positiveRate)) ? (1) : (-1);
    }

    static public int BoolToInt(bool b) {
        return (b) ? (1) : (0);
    }

    static public bool IntToBool(int i) {
        return (i == 0) ? (false) : (true);
    }

    static public int StringToInt(string str) {
        int value = 0;
        int.TryParse(str, out value);
        return value;
    }

    static public bool StringToBool(string str) {
        if (string.Equals(str, "TRUE", System.StringComparison.CurrentCultureIgnoreCase)) {
            return true;
        } else {
            return false;
        }
    }

    public static float StringToFloat(string inputStr) {
        float returnValue = 0.0f;
        float.TryParse(inputStr, out returnValue);
        return returnValue;
    }

    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    /// <summary>
    /// Perform a deep Copy of the object.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>The copied object.</returns>
    //public static T Clone<T>(T source) {
    //    // Don't serialize a null object, simply return the default for that object
    //    if (Object.ReferenceEquals(source, null)) {
    //        return default(T);
    //    }

    //    IFormatter formatter = new BinaryFormatter();
    //    Stream stream = new MemoryStream();
    //    using (stream) {
    //        formatter.Serialize(stream, source);
    //        stream.Seek(0, SeekOrigin.Begin);
    //        return (T)formatter.Deserialize(stream);
    //    }
    //}

    //https://stackoverflow.com/questions/273313/randomize-a-listt
    public static void Shuffle<T>(this List<T> list) {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1) {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    //T MUST be a non-container built-in type.
    public static void Resize<T>(this List<T> list, int size, T t) {
        int count = list.Count;
        if (size < count)
            list.RemoveRange(size, count - size);
        else if (size > count) {
            if (size > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                list.Capacity = size;
            list.AddRange(Enumerable.Repeat(t, size - count));
        }
    }

    public static void Resize<T>(this List<T> list, int size) where T : new() {
        Resize(list, size, new T());
    }

    public static byte[] StringToBytes(string str) {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static string BytesToString(byte[] bytes) {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    public static List<string> StringSplitByComma(string inputStr, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries) {
        return new List<string>(inputStr.Split(new string[] { "," }, stringSplitOptions));
    }

    public static List<string> StringSplitByNewLineTab(string inputStr, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries) {
        string[] lines = inputStr.Split(new string[] { "\r\n", "\r", "\n", "\t" }, stringSplitOptions);
        return new List<string>(lines);
    }

    public static List<int> StringSplitByNewLineTabAsInt(string inputStr, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries) {
        List<string> strings = StringSplitByNewLineTab(inputStr, stringSplitOptions);
        List<int> returnInts = new List<int>();
        for (int i = 0; i < strings.Count; i++) {
            int result = 0;
            if (int.TryParse(strings[i], out result)) {
                returnInts.Add(result);
            }
        }
        return returnInts;
    }

    public static List<float> StringSplitByNewLineTabAsFloat(string inputStr, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries) {
        List<string> strings = StringSplitByNewLineTab(inputStr, stringSplitOptions);
        List<float> returnFloats = new List<float>();
        for (int i = 0; i < strings.Count; i++) {
            float result = 0.0f;
            if (float.TryParse(strings[i], out result)) {
                returnFloats.Add(result);
            }
        }
        return returnFloats;
    }

    public static T InstantiateModule<T>(string modulePath, RectTransform parentRectTransform) {
        UnityEngine.Object prefab = Resources.Load(modulePath);
        if (prefab != null) {
            GameObject uiGO = GameObject.Instantiate(prefab) as GameObject;
            if (uiGO != null) {
                RectTransform rt = uiGO.GetRectTransform();
                rt.SetParent(parentRectTransform);
                rt.anchoredPosition3D = Vector3.zero;
                rt.localRotation = Quaternion.identity;
                rt.localScale = Vector3.one;
                rt.sizeDelta = parentRectTransform.sizeDelta;
                uiGO.layer = parentRectTransform.gameObject.layer;

                T returnComponent = uiGO.GetComponent<T>();

                if (returnComponent != null) {
                    return returnComponent;
                } else {
                    Debug.LogError("GetComponent failed!");
                    return default(T);
                }
            } else {
                Debug.LogError("GameObject.Instantiate failed!");
                return default(T);
            }
        } else {
            Debug.LogError("Resources.Load failed!");
            return default(T);
        }
    }

    public static IEnumerator InstantiateModuleAsync<T>(string modulePath, RectTransform parentRectTransform, System.Action<T> onFinish) {
        ResourceRequest resourceRequest = Resources.LoadAsync(modulePath);
        while (!resourceRequest.isDone) {
            yield return null;
        }
        if (resourceRequest.asset != null) {
            GameObject uiGO = GameObject.Instantiate(resourceRequest.asset) as GameObject;
            if (uiGO != null) {
                RectTransform rt = uiGO.GetRectTransform();
                rt.SetParent(parentRectTransform);
                rt.anchoredPosition3D = Vector3.zero;
                rt.localRotation = Quaternion.identity;
                rt.localScale = Vector3.one;
                rt.sizeDelta = parentRectTransform.sizeDelta;
                uiGO.layer = parentRectTransform.gameObject.layer;

                T returnComponent = uiGO.GetComponent<T>();
                yield return null;

                if (returnComponent != null) {
                    onFinish(returnComponent);
                } else {
                    Debug.LogError("GetComponent failed!");
                    onFinish(default(T));
                }
            } else {
                Debug.LogError("GameObject.Instantiate failed!");
                onFinish(default(T));
            }
        } else {
            Debug.LogError("Resources.LoadAsync failed!");
            onFinish(default(T));
        }
    }

    //http://answers.unity3d.com/questions/960413/editor-window-how-to-center-a-window.html
    public static System.Type[] GetAllDerivedTypes(this System.AppDomain aAppDomain, System.Type aType) {
        var result = new List<System.Type>();
        var assemblies = aAppDomain.GetAssemblies();
        foreach (var assembly in assemblies) {
            var types = assembly.GetTypes();
            foreach (var type in types) {
                if (type.IsSubclassOf(aType))
                    result.Add(type);
            }
        }
        return result.ToArray();
    }

    //http://answers.unity3d.com/questions/960413/editor-window-how-to-center-a-window.html
    public static Rect GetEditorMainWindowPos() {
        var containerWinType = System.AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ScriptableObject)).Where(t => t.Name == "ContainerWindow").FirstOrDefault();
        if (containerWinType == null)
            throw new System.MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");
        var showModeField = containerWinType.GetField("m_ShowMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var positionProperty = containerWinType.GetProperty("position", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        if (showModeField == null || positionProperty == null)
            throw new System.MissingFieldException("Can't find internal fields 'm_ShowMode' or 'position'. Maybe something has changed inside Unity");
        var windows = Resources.FindObjectsOfTypeAll(containerWinType);
        foreach (var win in windows) {
            var showmode = (int)showModeField.GetValue(win);
            if (showmode == 4) // main window
             {
                var pos = (Rect)positionProperty.GetValue(win, null);
                return pos;
            }
        }
        throw new System.NotSupportedException("Can't find internal main window. Maybe something has changed inside Unity");
    }

    //https://stackoverflow.com/questions/2394725/enum-parse-surely-a-neater-way
    public static bool TryParseFromIntStr<T>(this Enum theEnum, string intValueToParse, out T returnValue) {
        returnValue = default(T);
        int intEnumValue;
        if (Int32.TryParse(intValueToParse, out intEnumValue)) {
            if (Enum.IsDefined(typeof(T), intEnumValue)) {
                returnValue = (T)(object)intEnumValue;
                return true;
            }
        }
        return false;
    }

    //http://regexr.com/
    //https://www.dotnetperls.com/regex
    static public bool IsLegalIDString(string str) {
        Regex regex = new Regex(@"^[a-zA-Z0-9-_]+$");
        Match match = regex.Match(str);
        if (match.Success) {
            return true;
        } else {
            return false;
        }
    }

    //A Utility function for pick an index of the input list by their weight.
    //Return -1 if the inpuut list was not setup correctly(logically).
    static public int PickIndexByWeight(List<float> weights) {
        float totalWeight = 0.0f;
        foreach (float f in weights) {
            totalWeight += f;
        }

        //Defensive.
        if (totalWeight > 0.0f) {
            float randPlacement = UnityEngine.Random.Range(0.0f, totalWeight);
            float tempWeight = 0.0f;
            for (int i = 0; i < weights.Count; ++i) {
                tempWeight += weights[i];
                if (randPlacement < tempWeight) {
                    return i;
                }
            }

            //This is logically not possible.
            return -1;
        } else {
            return -1;
        }
    }

    /// <summary>
    /// Randomly pick a index from a general ordered int list.
    /// </summary>
    /// <param name="indexCount"></param>
    /// <returns></returns>
    static public int PickOneIndex(int indexCount) {
        List<int> list = new List<int>();
        for (int i = 0; i < indexCount; i++) {
            list.Add(i);
        }

        if (list.Count > 0) {
            return list[UnityEngine.Random.Range(0, list.Count)];
        } else {
            return -1;
        }
    }

    /// <summary>
    /// Randomly pick indexes form a general ordered int list.
    /// </summary>
    /// <param name="indexCount"></param>
    /// <param name="returnCount"></param>
    /// <returns></returns>
    static public List<int> PickIndexes(int indexCount, int returnCount) {
        List<int> sourceList = new List<int>();
        for (int i = 0; i < indexCount; i++) {
            sourceList.Add(i);
        }

        sourceList.Shuffle();

        List<int> returnList = new List<int>();
        for (int i = 0; i < returnCount; i++) {
            if (i < sourceList.Count) {
                returnList.Add(sourceList[i]);
            }
        }

        return returnList;
    }

    /// <summary>
    /// Try pick multiple indexes from a weight list.
    /// </summary>
    /// <param name="weights"></param>
    /// <param name="wantCount"></param>
    /// <returns></returns>
    static public List<int> PickIndexesByWeight(List<float> weights, int wantCount) {
        List<int> returnIndexes = new List<int>();

        //暫存還沒被抽到的 Weight.
        List<float> existWeight = new List<float>(weights);
        //這是暫存用的 Index, 用來存放尚未被抽到的 Index, 以提供的 Weight 數量作為初始化
        List<int> existIndexes = new List<int>();
        for (int i = 0; i < weights.Count; i++) {
            existIndexes.Add(i);
        }

        //Loop需求數量
        for (int i = 0; i < wantCount; i++) {
            if (existWeight.Count > 0) {
                //還有東西可以抽
                int thisIndex = PickIndexByWeight(existWeight);
                //此次抽出的 Index
                if (thisIndex != -1) {
                    //加入此Index於結果
                    returnIndexes.Add(existIndexes[thisIndex]);
                    //移除此次Index的東西
                    existWeight.RemoveAt(thisIndex);
                    existIndexes.RemoveAt(thisIndex);
                }
            }
        }

        return returnIndexes;
    }

    //A Utility function for pick an index of the input list by their weight.
    //Return -1 if the inpuut list was not setup correctly(logically).
    static public int PickIndexByWeight(List<int> weights) {
        int totalWeight = 0;
        foreach (int i in weights) {
            totalWeight += i;
        }

        //Defensive.
        if (totalWeight > 0) {
            int randPlacement = UnityEngine.Random.Range(0, totalWeight);
            int tempWeight = 0;
            for (int i = 0; i < weights.Count; ++i) {
                tempWeight += weights[i];
                if (randPlacement < tempWeight) {
                    return i;
                }
            }

            //This is logically not possible.
            return -1;
        } else {
            return -1;
        }
    }

    /// <summary>
    /// Try pick multiple indexes from a weight list.
    /// </summary>
    /// <param name="weights"></param>
    /// <param name="wantCount"></param>
    /// <returns></returns>
    static public List<int> PickIndexesByWeight(List<int> weights, int wantCount) {
        List<int> returnIndexes = new List<int>();

        //暫存還沒被抽到的 Weight.
        List<int> existWeight = new List<int>(weights);
        //這是暫存用的 Index, 用來存放尚未被抽到的 Index, 以提供的 Weight 數量作為初始化
        List<int> existIndexes = new List<int>();
        for (int i = 0; i < weights.Count; i++) {
            existIndexes.Add(i);
        }

        //Loop需求數量
        for (int i = 0; i < wantCount; i++) {
            if (existWeight.Count > 0) {
                //還有東西可以抽
                int thisIndex = PickIndexByWeight(existWeight);
                //此次抽出的 Index
                if (thisIndex != -1) {
                    //加入此Index於結果
                    returnIndexes.Add(existIndexes[thisIndex]);
                    //移除此次Index的東西
                    existWeight.RemoveAt(thisIndex);
                    existIndexes.RemoveAt(thisIndex);
                }
            }
        }

        return returnIndexes;
    }

    //A convenient utility that create shorter GUID then the default way.
    //https://stackoverflow.com/questions/1458468/youtube-like-guid
    static public string NewBase64Guid() {
        string base64Guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        // Replace URL unfriendly characters with better ones
        base64Guid = base64Guid.Replace('+', '-').Replace('/', '_');

        // Remove the trailing ==
        return base64Guid.Substring(0, base64Guid.Length - 2);
    }

    //Return a randomized fix for depth. (This is used in the situation you want to give 2D object a random minor shift for their z value)
    static public float RandomMinorShift() {
        return UnityEngine.Random.Range(-0.000001f, 0.000001f);
    }

    /// <summary>
    /// Create the directory from ths full path of it's not exist.
    /// </summary>
    /// <returns></returns>
    static public bool CreateDirectoryIfNotExist(string fullPath) {
        string directoryPath = Path.GetDirectoryName(fullPath);
        bool pathExist = Directory.Exists(directoryPath);
        if (!pathExist) {
            Directory.CreateDirectory(directoryPath);
            return true;
        }
        return false;
    }

}
