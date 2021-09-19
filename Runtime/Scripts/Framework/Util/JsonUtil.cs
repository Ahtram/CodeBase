using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// A convenient JSON serizlise/deserialize API provider.
/// </summary>
public class JsonUtil {

    /// <summary>
    /// Get plain text JSON data from project resource.
    /// Note the resPath must end with a .txt or .bytes extension name!
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetJSONDataFromResource<T>(string resPath, bool logErrorIfNullData = true) {
        //Load text asset.
        TextAsset dataJSON = Resources.Load(resPath, typeof(TextAsset)) as TextAsset;
        if (dataJSON != null) {
            T returnData = JsonConvert.DeserializeObject<T>(dataJSON.text);
            return returnData;
        } else {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! json is null? Return a default constructor data. [Path]: " + resPath);
            } else {
                Debug.Log("Oops! json is null? Return a default constructor data. [Path]: " + resPath);
            }
            return default(T);
        }
    }

    /// <summary>
    /// Get plain text JSON data from project resource. (Async with callback)
    /// Note the resPath must end with a .txt or .bytes extension name!
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="onFinish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetJSONDataFromResourceAsync<T>(string resPath, System.Action<T> onFinish, bool logErrorIfNullData = true) {
        ResourceRequest resourceRequest = Resources.LoadAsync(resPath, typeof(TextAsset));
        while (!resourceRequest.isDone) {
            yield return null;
        }

        //Get the xml text asset.
        TextAsset dataJSON = (TextAsset)resourceRequest.asset;
        if (dataJSON != null) {
            T returnData = JsonConvert.DeserializeObject<T>(dataJSON.text);
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
    /// Get plain text JSON data from a system full path.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="logErrorIfFailed"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetJSONData<T>(string fullPath, bool logErrorIfFailed = true) {
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
    /// Get plain text JSON data from a system full path. (Async with callback)
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="onFinish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetJSONDataAsync<T>(string fullPath, System.Action<T, bool> onFinish, bool logErrorIfFailed = true) {
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

}
