using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// Serialize or deserialize by BaseConfig setting.
/// </summary>
static public class DataUtil {

    /// <summary>
    /// Get data from project resource.
    /// Note the resPath must end with a .txt or .bytes extension name!
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetDataFromResource<T>(string resPath, bool logErrorIfNullData = true) {
        BaseConfig.DataType dataType = BaseConfig.DataDeserializeType();
        switch (dataType) {
            default:
            case BaseConfig.DataType.XML:
                return XMLUtil.GetXMLDataFromResource<T>(resPath, logErrorIfNullData);
            case BaseConfig.DataType.Binary:
                return BinaryUtil.GetBinDataFromResource<T>(resPath, logErrorIfNullData);
        }
    }

    /// <summary>
    /// Get data from project resource. (Async with callback)
    /// Note the resPath must end with a .txt or .bytes extension name!
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="onFinish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetDataFromResourceAsync<T>(string resPath, System.Action<T> onFinish, bool logErrorIfNullData = true) {
        BaseConfig.DataType dataType = BaseConfig.DataDeserializeType();
        switch (dataType) {
            default:
            case BaseConfig.DataType.XML:
                yield return XMLUtil.GetXMLDataFromResourceAsync(resPath, onFinish, logErrorIfNullData);
                break;
            case BaseConfig.DataType.Binary:
                yield return BinaryUtil.GetBinDataFromResourceAsync(resPath, onFinish, logErrorIfNullData);
                break;
        }
    }

    /// <summary>
    /// Get data from a system full path.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="logErrorIfFailed"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetData<T>(string fullPath, bool logErrorIfFailed = true) {
        BaseConfig.DataType dataType = BaseConfig.DataDeserializeType();
        switch (dataType) {
            default:
            case BaseConfig.DataType.XML:
                return XMLUtil.GetXMLData<T>(fullPath, logErrorIfFailed);
            case BaseConfig.DataType.Binary:
                return BinaryUtil.GetBinData<T>(fullPath, logErrorIfFailed);
        }
    }

    /// <summary>
    /// Get data from a system full path. (Async with callback)
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="onFihish"></param>
    /// <param name="logErrorIfNullData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerator GetDataAsync<T>(string fullPath, System.Action<T, bool> onFinish, bool logErrorIfFailed = true) {
        BaseConfig.DataType dataType = BaseConfig.DataDeserializeType();
        switch (dataType) {
            default:
            case BaseConfig.DataType.XML:
                yield return XMLUtil.GetXMLDataAsync(fullPath, onFinish, logErrorIfFailed);
                break;
            case BaseConfig.DataType.Binary:
                yield return BinaryUtil.GetBinDataAsync(fullPath, onFinish, logErrorIfFailed);
                break;
        }
    }

    /// <summary>
    /// Save a data file to a system path.
    /// Note that you need to save them as ".txt" or ".bytes" in order to load them as TextAsset in Unity.
    /// </summary>
    /// <param name="fullPathWithoutExt"></param>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public static void SaveData<T>(string fullPathWithoutExt, T obj, bool autoGenerateFileExt = true) {
        BaseConfig.DataType dataType = BaseConfig.DataSerializeType();
        switch (dataType) {
            default:
            case BaseConfig.DataType.XML:
                CleanUpBinData(fullPathWithoutExt);
                //This is for Unity TextAsset's sake.
                if (autoGenerateFileExt) {
                    fullPathWithoutExt += ".txt";
                }
                XMLUtil.SaveXMLData(fullPathWithoutExt, obj);
                break;
            case BaseConfig.DataType.Binary:
                CleanUpXMLData(fullPathWithoutExt);
                //This is for Unity TextAsset's sake.
                if (autoGenerateFileExt) {
                    fullPathWithoutExt += ".bytes";
                }
                BinaryUtil.SaveBinData(fullPathWithoutExt, obj);
                break;
        }
    }

    /// <summary>
    /// Rmove both XML and binary data file of this path.
    /// </summary>
    public static void CleanUpData(string fullPathWithoutExt) {
        //XML
        CleanUpXMLData(fullPathWithoutExt);
        //Binary
        CleanUpBinData(fullPathWithoutExt);
    }

    /// <summary>
    /// Remove XML data file.
    /// </summary>
    /// <param name="fullPathWithoutExt"></param>
    public static void CleanUpXMLData(string fullPathWithoutExt) {
        if (File.Exists(fullPathWithoutExt + ".txt")) {
            File.Delete(fullPathWithoutExt + ".txt");
        }
        if (File.Exists(fullPathWithoutExt + ".txt.meta")) {
            File.Delete(fullPathWithoutExt + ".txt.meta");
        }
    }

    /// <summary>
    /// Remove binary data file.
    /// </summary>
    /// <param name="fullPathWithoutExt"></param>
    public static void CleanUpBinData(string fullPathWithoutExt) {
        if (File.Exists(fullPathWithoutExt + ".bytes")) {
            File.Delete(fullPathWithoutExt + ".bytes");
        }
        if (File.Exists(fullPathWithoutExt + ".bytes.meta")) {
            File.Delete(fullPathWithoutExt + ".bytes.meta");
        }
    }

}
