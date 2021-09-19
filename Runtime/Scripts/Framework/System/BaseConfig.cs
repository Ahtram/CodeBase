using UnityEngine;
using System.Xml.Serialization;
using System;

[Serializable]
[XmlType("BC")]
/// <summary>
/// Base system config data.
/// </summary>
public class BaseConfig {

    //Save file name.
    [XmlIgnore]
    static private string BASE_CONFIG_NAME = "BC";

    //Loaded config.
    [XmlIgnore]
    static private BaseConfig baseConfig = null;

    //Which data type should we serialize to?
    public enum DataType {
        XML,
        Json,
        Binary
    };

    [XmlElement("ST")]
    public DataType dataSerializeType = DataType.Json;

    [XmlElement("DT")]
    public DataType dataDeserializeType = DataType.Json;

    /// <summary>
    /// Get the base config and Cache it. This should be use in runtime to prevent from redundent loading.
    /// </summary>
    static public BaseConfig Get() {
        if (baseConfig == null) {
            baseConfig = GetWithNoCache();
        }
        return baseConfig;
    }

    /// <summary>
    /// Get the base config file with no cache.
    /// </summary>
    /// <returns></returns>
    static public BaseConfig GetWithNoCache() {
        //From Assets.
        baseConfig = BinaryUtil.GetBinDataFromResource<BaseConfig>(SysPath.UniqueDataPath + BASE_CONFIG_NAME, false);

        //If no data exist, generate a new one.
        if (baseConfig == null) {
            baseConfig = new BaseConfig();
            Debug.Log("No BaseConfig xml exist. Create a new baseConfig.");
        }

        return baseConfig;
    }

    static public void ClearCache() {
        baseConfig = null;
    }

    static public bool Save() {
        Get();
        if (baseConfig != null) {
            baseConfig.Serialize();
            return true;
        } else {
            Debug.LogWarning("OOps! BaseConfig not loaded yet! Save failed!");
            return false;
        }
    }

    public void Serialize() {
        //From Assets.
        string dataPath = Application.dataPath + "/Resources/" + SysPath.UniqueDataPath;
        BinaryUtil.SaveBinData<BaseConfig>(dataPath + BASE_CONFIG_NAME + ".bytes", this);
    }

    //--------

    /// <summary>
    /// Get the data serialize type setting in config file.
    /// </summary>
    /// <returns></returns>
    static public DataType DataSerializeType() {
        Get();
        if (baseConfig != null) {
            return baseConfig.dataSerializeType;
        } else {
            Debug.LogWarning("OOps! BaseConfig not loaded yet! Return a default value!");
            return DataType.XML;
        }
    }

    /// <summary>
    /// Get the data deserialize type setting in config file.
    /// </summary>
    /// <returns></returns>
    static public DataType DataDeserializeType() {
        Get();
        if (baseConfig != null) {
            return baseConfig.dataDeserializeType;
        } else {
            Debug.LogWarning("OOps! BaseConfig not loaded yet! Return a default value!");
            return DataType.XML;
        }
    }

}
