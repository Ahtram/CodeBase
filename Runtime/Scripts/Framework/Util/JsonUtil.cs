using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

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
    public static T GetJsonDataFromResource<T>(string resPath, bool logErrorIfNullData = true) {
        //Load text asset.
        TextAsset dataJSON = Resources.Load(resPath, typeof(TextAsset)) as TextAsset;
        if (dataJSON != null) {
            T returnData = JsonConvert.DeserializeObject<T>(dataJSON.text, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
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
    public static IEnumerator GetJsonDataFromResourceAsync<T>(string resPath, System.Action<T> onFinish, bool logErrorIfNullData = true) {
        ResourceRequest resourceRequest = Resources.LoadAsync(resPath, typeof(TextAsset));
        while (!resourceRequest.isDone) {
            yield return null;
        }

        //Get the json text asset.
        TextAsset dataJSON = (TextAsset)resourceRequest.asset;
        if (dataJSON != null) {
            T returnData = JsonConvert.DeserializeObject<T>(dataJSON.text, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            onFinish?.Invoke(returnData);
        } else {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! json is null? Return a default constructor data. [Path]: " + resPath);
            } else {
                Debug.Log("Oops! json is null? Return a default constructor data. [Path]: " + resPath);
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
    public static T GetJsonData<T>(string fullPath, bool logErrorIfFailed = true) {
        try {
            //This is faster.
            StreamReader streamReader = new StreamReader(fullPath, Encoding.UTF8);
            T returnData = JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd(), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            return returnData;
        } catch {
            if (logErrorIfFailed) {
                Debug.LogError("Oops! json is null? Return a default constructor data. [Path]: " + fullPath);
            } else {
                Debug.Log("Oops! json is null? Return a default constructor data. [Path]: " + fullPath);
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
    public static IEnumerator GetJsonDataAsync<T>(string fullPath, System.Action<T, bool> onFinish, bool logErrorIfFailed = true) {
        yield return null;
        try {
            //This is faster.
            StreamReader streamReader = new StreamReader(fullPath, Encoding.UTF8);
            T returnData = JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd(), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            onFinish?.Invoke(returnData, true);
        } catch {
            if (logErrorIfFailed) {
                Debug.LogError("Oops! json is null? Return a default constructor data. [Path]: " + fullPath);
            } else {
                Debug.Log("Oops! json is null? Return a default constructor data. [Path]: " + fullPath);
            }
            onFinish?.Invoke(default(T), false);
        }
    }

    /// <summary>
    /// Save a plain text JSON file to a system path.
    /// Note the you need to save them as ".txt" or ".bytes" in order to load them as TextAsset in Unity.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public static void SaveJsonData<T>(string fullPath, T obj) {
        Util.CreateDirectoryIfNotExist(fullPath);
        //[Convert the json content to a 64 bit string]
        string content = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        FileStream fileStream = new FileStream(fullPath, FileMode.Create);
        //WSA require us to use this stream version.
        StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
        streamWriter.Write(content);
        streamWriter.Dispose();
        fileStream.Dispose();
    }

    //-- 64 bit versions --

    /// <summary>
    /// Get JSON data of base64 string from resource path.
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetJsonData64FromResource<T>(string resPath, bool logErrorIfNullData = true) {
        //Load text asset.
        TextAsset dataJSON64 = Resources.Load(resPath, typeof(TextAsset)) as TextAsset;
        if (dataJSON64 != null) {
            string content = Util.BytesToString(Convert.FromBase64String(dataJSON64.text));
            T returnData = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
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
    /// Get JSON data of base64 string from resource path.
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="onFinish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetJsonData64FromResourceAsync<T>(string resPath, System.Action<T> onFinish, bool logErrorIfNullData = true) {
        ResourceRequest resourceRequest = Resources.LoadAsync(resPath, typeof(TextAsset));
        while (!resourceRequest.isDone) {
            yield return null;
        }

        //Get the json text asset.
        TextAsset dataJSON64 = (TextAsset)resourceRequest.asset;
        if (dataJSON64 != null) {
            string content = Util.BytesToString(Convert.FromBase64String(dataJSON64.text));
            T returnData = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            onFinish?.Invoke(returnData);
        } else {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! json is null? Return a default constructor data. [Path]: " + resPath);
            } else {
                Debug.Log("Oops! json is null? Return a default constructor data. [Path]: " + resPath);
            }
            onFinish?.Invoke(default(T));
        }
    }

    /// <summary>
    /// Get JSON data of base64 string from system path.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetJsonData64<T>(string fullPath, bool logErrorIfNullData = true) {
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

            T returnData = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

            return returnData;
        } catch {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! json is null? Return a default constructor data. [Path]: " + fullPath);
            } else {
                Debug.Log("Oops! json is null? Return a default constructor data. [Path]: " + fullPath);
            }
            return default(T);
        }
    }

    /// <summary>
    /// Get JSON data of base64 string from system path.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="onFinish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetJsonData64Async<T>(string fullPath, System.Action<T, bool> onFinish, bool logErrorIfNullData = true) {
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
                Debug.LogError("Oops! json is null? Return a default constructor data. [Path]: " + fullPath);
            } else {
                Debug.Log("Oops! json is null? Return a default constructor data. [Path]: " + fullPath);
            }
            readSucces = false;
        }

        if (readSucces) {
            yield return null;
            //[Convert back to normal string]
            string content = Util.BytesToString(Convert.FromBase64String(content64));
            yield return null;

            T returnData = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            yield return null;

            onFinish?.Invoke(returnData, true);
        } else {
            yield return default(T);
            onFinish?.Invoke(default(T), true);
        }
    }

    /// <summary>
    /// Save JSON data as base64 string to system path.
    /// Note that you need to save them as ".txt" or ".bytes" in order to load them as TextAsset in Unity.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public static void SaveJsonData64<T>(string fullPath, T obj) {
        Util.CreateDirectoryIfNotExist(fullPath);
        //[Convert the json content to a 64 bit string]
        string str64 = Convert.ToBase64String(Util.StringToBytes(JsonConvert.SerializeObject(obj, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto })));
        FileStream fileStream = new FileStream(fullPath, FileMode.Create);
        //WSA require us to use this stream version.
        StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
        streamWriter.Write(str64);
        streamWriter.Dispose();
        fileStream.Dispose();
    }

}
