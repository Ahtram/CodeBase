using System.Collections;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

/// <summary>
/// A convenient binary serialize/deserialize API provider.
/// </summary>
static public class BinaryUtil {

    /// <summary>
    /// Get plain text Bin data from project resource.
    /// Note the resPath must end with a .txt or .bytes extension name!
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetBinDataFromResource<T>(string resPath, bool logErrorIfNullData = true) {
        //Load text asset.
        TextAsset dataBin = Resources.Load(resPath, typeof(TextAsset)) as TextAsset;
        if (dataBin != null) {
            Stream stream = new MemoryStream(dataBin.bytes);
            BinaryFormatter formatter = new BinaryFormatter();
            T returnData = (T)formatter.Deserialize(stream);
            return returnData;
        } else {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! dataBin is null? Return a default constructor data. [Path]: " + resPath);
            } else {
                Debug.Log("Oops! dataBin is null? Return a default constructor data. [Path]: " + resPath);
            }
            return default(T);
        }
    }

    /// <summary>
    /// Get plain text Bin data from project resource. (Async with callback)
    /// Note the resPath must end with a .txt or .bytes extension name!
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="onFinish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetBinDataFromResourceAsync<T>(string resPath, System.Action<T> onFinish, bool logErrorIfNullData = true) {
        ResourceRequest resourceRequest = Resources.LoadAsync(resPath, typeof(TextAsset));
        while (!resourceRequest.isDone) {
            yield return null;
        }

        //Get the binary text asset.
        TextAsset dataBin = (TextAsset)resourceRequest.asset;
        if (dataBin != null) {
            Stream stream = new MemoryStream(dataBin.bytes);
            BinaryFormatter formatter = new BinaryFormatter();
            T returnData = (T)formatter.Deserialize(stream);
            onFinish?.Invoke(returnData);
        } else {
            if (logErrorIfNullData) {
                Debug.LogError("Oops! dataBin is null? Return a default constructor data. [Path]: " + resPath);
            } else {
                Debug.Log("Oops! dataBin is null? Return a default constructor data. [Path]: " + resPath);
            }
            onFinish?.Invoke(default(T));
        }
    }

    /// <summary>
    /// Get Bin data from a system full path.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="logErrorIfFailed"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetBinData<T>(string fullPath, bool logErrorIfFailed = true) {
        // Open the file containing the data that you want to deserialize.
        FileStream fs = new FileStream(fullPath, FileMode.Open);
        try {
            BinaryFormatter formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(fs);
        } catch (SerializationException e) {
            if (logErrorIfFailed) {
                Debug.LogError("Failed to deserialize. Reason: " + e.Message);
            } else {
                Debug.Log("Failed to deserialize. Reason: " + e.Message);
            }
            return default(T);
        } finally {
            fs.Close();
        }
    }

    /// <summary>
    /// Get Bin data from a system full path. (Async with callback)
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="onFihish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetBinDataAsync<T>(string fullPath, System.Action<T, bool> onFinish, bool logErrorIfFailed = true) {
        yield return null;
        // Open the file containing the data that you want to deserialize.
        FileStream fs = new FileStream(fullPath, FileMode.Open);
        try {
            BinaryFormatter formatter = new BinaryFormatter();
            onFinish?.Invoke((T)formatter.Deserialize(fs), true);
        } catch (SerializationException e) {
            if (logErrorIfFailed) {
                Debug.LogError("Failed to deserialize. Reason: " + e.Message);
            } else {
                Debug.Log("Failed to deserialize. Reason: " + e.Message);
            }
            onFinish?.Invoke(default(T), false);
        } finally {
            fs.Close();
        }
    }

    /// <summary>
    /// Save a plain text Bin file to a system path.
    /// Note that you need to save them as ".txt" or ".bytes" in order to load them as TextAsset in Unity.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public static void SaveBinData<T>(string fullPath, T obj) {
        Util.CreateDirectoryIfNotExist(fullPath);
        FileStream fs = new FileStream(fullPath, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        try {
            formatter.Serialize(fs, obj);
        } catch (SerializationException e) {
            Debug.LogError("SaveBinData Failed to serialize! Reason: " + e.Message);
            throw;
        } finally {
            fs.Close();
        }
    }

}
