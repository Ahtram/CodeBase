using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Text;

/// <summary>
/// A convenient XML serizlise/deserialize API provider.
/// </summary>
static public class XMLUtil {

    /// <summary>
    /// Get plain text XML data from project resource.
    /// Note the resPath must end with a .txt or .bytes extension name!
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetXMLDataFromResource<T>(string resPath, bool logErrorIfNullData = true) {
        //Load text asset.
        TextAsset dataXML = Resources.Load(resPath, typeof(TextAsset)) as TextAsset;
        if (dataXML != null) {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(dataXML.text);
            T returnData = (T)deserializer.Deserialize(stringReader);
            return returnData;
        } else {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! xml is null? Return a default constructor data. [Path]: " + resPath);
            } else {
                Debug.Log("Oops! xml is null? Return a default constructor data. [Path]: " + resPath);
            }
            return default(T);
        }
    }

    /// <summary>
    /// Get plain text XML data from project resource. (Async with callback)
    /// Note the resPath must end with a .txt or .bytes extension name!
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="onFinish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetXMLDataFromResourceAsync<T>(string resPath, System.Action<T> onFinish, bool logErrorIfNullData = true) {
        ResourceRequest resourceRequest = Resources.LoadAsync(resPath, typeof(TextAsset));
        while (!resourceRequest.isDone) {
            yield return null;
        }

        //Get the xml text asset.
        TextAsset dataXML = (TextAsset)resourceRequest.asset;
        if (dataXML != null) {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(dataXML.text);
            T returnData = (T)deserializer.Deserialize(stringReader);
            onFinish?.Invoke(returnData);
        } else {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! xml is null? Return a default constructor data. [Path]: " + resPath);
            } else {
                Debug.Log("Oops! xml is null? Return a default constructor data. [Path]: " + resPath);
            }
            onFinish?.Invoke(default(T));
        }
    }

    /// <summary>
    /// Get plain text XML data from a system full path.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="logErrorIfFailed"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetXMLData<T>(string fullPath, bool logErrorIfFailed = true) {
        try {
            //This is faster.
            StreamReader streamReader = new StreamReader(fullPath, Encoding.UTF8);
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            T returnData = (T)deserializer.Deserialize(streamReader.BaseStream);

            return returnData;
        } catch {
            if (logErrorIfFailed) {
                Debug.LogError("Oops! xml is null? Return a default constructor data. [Path]: " + fullPath);
            } else {
                Debug.Log("Oops! xml is null? Return a default constructor data. [Path]: " + fullPath);
            }
            return default(T);
        }
    }

    /// <summary>
    /// Get plain text XML data from a system full path. (Async with callback)
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="onFinish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetXMLDataAsync<T>(string fullPath, System.Action<T, bool> onFinish, bool logErrorIfFailed = true) {
        yield return null;
        try {
            //This is faster.
            StreamReader streamReader = new StreamReader(fullPath, Encoding.UTF8);
            XmlSerializer deserializer = new XmlSerializer(typeof(T));

            T returnData = (T)deserializer.Deserialize(streamReader.BaseStream);
            onFinish?.Invoke(returnData, true);
        } catch {
            if (logErrorIfFailed) {
                Debug.LogError("Oops! xml is null? Return a default constructor data. [Path]: " + fullPath);
            } else {
                Debug.Log("Oops! xml is null? Return a default constructor data. [Path]: " + fullPath);
            }
            onFinish?.Invoke(default(T), false);
        }
    }

    /// <summary>
    /// Save a plain text XML file to a system path.
    /// Note the you need to save them as ".txt" or ".bytes" in order to load them as TextAsset in Unity.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public static void SaveXMLData<T>(string fullPath, T obj) {
        Util.CreateDirectoryIfNotExist(fullPath);
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        FileStream fileStream = new FileStream(fullPath, FileMode.Create);
        //WSA require us to use this stream version.
        TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8);
        serializer.Serialize(textWriter, obj);
        textWriter.Dispose();
        fileStream.Dispose();
    }

    //-- 64 bit versions --

    /// <summary>
    /// Get XML data of base64 string from resource path.
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetXMLData64FromResource<T>(string resPath, bool logErrorIfNullData = true) {
        //Load text asset.
        TextAsset dataXML64 = Resources.Load(resPath, typeof(TextAsset)) as TextAsset;
        if (dataXML64 != null) {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            string content = Util.BytesToString(Convert.FromBase64String(dataXML64.text));
            StringReader stringReader = new StringReader(content);
            T returnData = (T)deserializer.Deserialize(stringReader);
            return returnData;
        } else {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! xml is null? Return a default constructor data. [Path]: " + resPath);
            } else {
                Debug.Log("Oops! xml is null? Return a default constructor data. [Path]: " + resPath);
            }
            return default(T);
        }
    }

    /// <summary>
    /// Get XML data of base64 string from resource path.
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="onFinish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetXMLData64FromResourceAsync<T>(string resPath, System.Action<T> onFinish, bool logErrorIfNullData = true) {
        ResourceRequest resourceRequest = Resources.LoadAsync(resPath, typeof(TextAsset));
        while (!resourceRequest.isDone) {
            yield return null;
        }

        //Get the xml text asset.
        TextAsset dataXML64 = (TextAsset)resourceRequest.asset;
        if (dataXML64 != null) {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            string content = Util.BytesToString(Convert.FromBase64String(dataXML64.text));
            StringReader stringReader = new StringReader(content);
            T returnData = (T)deserializer.Deserialize(stringReader);
            onFinish?.Invoke(returnData);
        } else {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! xml is null? Return a default constructor data. [Path]: " + resPath);
            } else {
                Debug.Log("Oops! xml is null? Return a default constructor data. [Path]: " + resPath);
            }
            onFinish?.Invoke(default(T));
        }
    }

    /// <summary>
    /// Get XML data of base64 string from system path.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetXMLData64<T>(string fullPath, bool logErrorIfNullData = true) {
        try {
            //Fuck it. This is slower.
            //string content64 = File.ReadAllText(path, Encoding.UTF8);
            string content64 = "";

            //This is faster.
            FileStream fileStream = new FileStream(fullPath, FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
            content64 = streamReader.ReadToEnd();
            fileStream.Dispose();

            //[Convert back to normal string]
            string content = Util.BytesToString(Convert.FromBase64String(content64));
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(content);

            T returnData = (T)deserializer.Deserialize(stringReader);

            return returnData;
        } catch {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! xml is null? Return a default constructor data. [Path]: " + fullPath);
            } else {
                Debug.Log("Oops! xml is null? Return a default constructor data. [Path]: " + fullPath);
            }
            return default(T);
        }
    }

    /// <summary>
    /// Get XML data of base64 string from system path.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="onFinish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetXMLData64Async<T>(string fullPath, System.Action<T, bool> onFinish, bool logErrorIfNullData = true) {
        string content64 = "";
        bool readSucces = false;

        try {
            //Fuck it. This is slower.
            //content64 = File.ReadAllText(path, Encoding.UTF8);
            //This is faster.
            FileStream fileStream = new FileStream(fullPath, FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
            content64 = streamReader.ReadToEnd();
            fileStream.Dispose();

            readSucces = true;
        } catch {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! xml is null? Return a default constructor data. [Path]: " + fullPath);
            } else {
                Debug.Log("Oops! xml is null? Return a default constructor data. [Path]: " + fullPath);
            }
            readSucces = false;
        }

        if (readSucces) {
            yield return null;
            //[Convert back to normal string]
            string content = Util.BytesToString(Convert.FromBase64String(content64));
            yield return null;
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(content);
            yield return null;

            T returnData = (T)deserializer.Deserialize(stringReader);
            yield return null;

            onFinish?.Invoke(returnData, true);
        } else {
            yield return default(T);
            onFinish?.Invoke(default(T), true);
        }
    }

    /// <summary>
    /// Save XML data as base64 string to system path.
    /// Note that you need to save them as ".txt" or ".bytes" in order to load them as TextAsset in Unity.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public static void SaveXMLData64<T>(string fullPath, T obj) {
        Util.CreateDirectoryIfNotExist(fullPath);
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StringWriter stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, obj);
        //[Convert the xml content to a 64 bit string]
        string str64 = Convert.ToBase64String(Util.StringToBytes(stringWriter.ToString()));
        FileStream fileStream = new FileStream(fullPath, FileMode.Create);
        //WSA require us to use this stream version.
        StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
        streamWriter.Write(str64);
        streamWriter.Dispose();
        fileStream.Dispose();
    }
}
