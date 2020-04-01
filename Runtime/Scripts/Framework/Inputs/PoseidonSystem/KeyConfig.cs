using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;

//The serializable class for saving keys.
[XmlType("KC")]
public class KeyConfig {

    //Where to save/load the confing data file?
    public enum DataLocation {
        PersistentDataPath,
        AssetDataPath
    };

    //Define your keys here.
    //This should be modified to suit your need.
    public enum Key {
        Confirm,
        Cancel,
        Option,
        Mod,
        Previous,
        Next,
        Select,
        Start,
        Left,
        Right,
        Up,
        Down,
        Backward,
        Forward
    };

    //For performance sake.
    static public int KEY_COUNT = 14;

    //The default setting for keyboard keys.
    [XmlIgnore]
    static readonly private KeyCode[] DEFAULT_KEYBOARD_SETTING = new KeyCode[] {
        KeyCode.O,
        KeyCode.P,
        KeyCode.N,
        KeyCode.M,
        KeyCode.Q,
        KeyCode.E,
        KeyCode.C,
        KeyCode.V,
        KeyCode.A,
        KeyCode.D,
        KeyCode.W,
        KeyCode.S,
        KeyCode.R,
        KeyCode.T
    };

    [XmlIgnore]
    static readonly private KeyCode[] DEFAULT_KEYBOARD_SETTING_SUB = new KeyCode[] {
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None
    };

    [XmlIgnore]
    //The default setting for controller keys.
    static readonly private Nereus.InputType[] DEFAULT_CONTROLLER_SETTING = new Nereus.InputType[] {
        Nereus.InputType.A,
        Nereus.InputType.B,
        Nereus.InputType.X,
        Nereus.InputType.Y,
        Nereus.InputType.LB,
        Nereus.InputType.RB,
        Nereus.InputType.Select,
        Nereus.InputType.Start,
        Nereus.InputType.CBL,
        Nereus.InputType.CBR,
        Nereus.InputType.CBU,
        Nereus.InputType.CBD,
        Nereus.InputType.LT,
        Nereus.InputType.RT
    };

    //The content should ordered by the defines enum above. (Yeah these's are the same stuffs. It's just I have to follow the rule to)
    [XmlArray("KSL")]
    [XmlArrayItem("KS", typeof(KeyCode))]
    public List<KeyCode> keyboardSetting = new List<KeyCode>();

    [XmlArray("KSSL")]
    [XmlArrayItem("KSS", typeof(KeyCode))]
    public List<KeyCode> keyboardSettingSub = new List<KeyCode>();

    [XmlArray("KSEL")]
    [XmlArrayItem("KSE", typeof(KeyCode))]
    public List<KeyCode> keyboardSettingExtra = new List<KeyCode>();

    [XmlArray("KSXL")]
    [XmlArrayItem("KSX", typeof(KeyCode))]
    public List<KeyCode> keyboardSettingExtand = new List<KeyCode>();

    //The content should ordered by the defines enum above.
    [XmlArray("CSL")]
    [XmlArrayItem("CS", typeof(Nereus.InputType))]
    public List<Nereus.InputType> controllerSetting = new List<Nereus.InputType>();

    //Save file name.
    [XmlIgnore]
    static private string KEY_CONFIG_NAME = "KC";

    //Loaded config.
    [XmlIgnore]
    static private KeyConfig keyConfig = null;

    /// <summary>
    /// Get the key config and Cache it. This should be use in runtime to prevent from redundent loading.
    /// </summary>
    /// <param name="dataLocation"></param>
    /// <returns></returns>
    static public KeyConfig Get(DataLocation dataLocation = DataLocation.AssetDataPath) {
        if (keyConfig == null) {
            //Try load from xml first.
            keyConfig = GetWithNoCache(dataLocation);
        }
        return keyConfig;
    }

    /// <summary>
    /// Get the key config file with no cache.
    /// </summary>
    /// <param name="dataLocation"></param>
    /// <returns></returns>
    static public KeyConfig GetWithNoCache(DataLocation dataLocation = DataLocation.AssetDataPath) {
        if (dataLocation == DataLocation.AssetDataPath) {
            //From Assets.
            keyConfig = XMLUtil.GetXMLData64FromResource<KeyConfig>(SysPath.UniqueDataPath + KEY_CONFIG_NAME, false);
        } else {
            //From user dir.
            //Try load from xml first.
            keyConfig = XMLUtil.GetXMLData64<KeyConfig>(Application.persistentDataPath + "/" + KEY_CONFIG_NAME, false);
        }

        //If no data exist, generate a new one.
        if (keyConfig == null) {
            keyConfig = new KeyConfig();
            keyConfig.RestoreDefault();
            Debug.Log("No KeyConfig xml exist. Create a new keyConfig.");
        }

        //Correctify data if there's any problem.
        int KeyCount = Enum.GetNames(typeof(Key)).Length;
        if (keyConfig.keyboardSetting.Count != KeyCount) {
            keyConfig.keyboardSetting.Resize(KeyCount, KeyCode.None);
        }

        if (keyConfig.keyboardSettingSub.Count != KeyCount) {
            keyConfig.keyboardSettingSub.Resize(KeyCount, KeyCode.None);
        }

        if (keyConfig.keyboardSettingExtra.Count != KeyCount) {
            keyConfig.keyboardSettingExtra.Resize(KeyCount, KeyCode.None);
        }

        if (keyConfig.keyboardSettingExtand.Count != KeyCount) {
            keyConfig.keyboardSettingExtand.Resize(KeyCount, KeyCode.None);
        }

        if (keyConfig.controllerSetting.Count != KeyCount) {
            keyConfig.controllerSetting.Resize(KeyCount, Nereus.InputType.None);
        }

        return keyConfig;
    }

    static public void ClearCache() {
        keyConfig = null;
    }

    static public bool Save() {
        Get();
        if (keyConfig != null) {
            keyConfig.Serialize();
            return true;
        } else {
            Debug.LogWarning("OOps! KeyConfig not loaded yet! Save failed!");
            return false;
        }
    }

    public void SetKeyboardKey(Key key, KeyCode keyCode) {
        //Replace any exist with none.
        for (int i = 0; i < keyboardSetting.Count; i++) {
            if (keyboardSetting[i] == keyCode) {
                keyboardSetting[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingSub.Count; i++) {
            if (keyboardSettingSub[i] == keyCode) {
                keyboardSettingSub[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingExtra.Count; i++) {
            if (keyboardSettingExtra[i] == keyCode) {
                keyboardSettingExtra[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingExtand.Count; i++) {
            if (keyboardSettingExtand[i] == keyCode) {
                keyboardSettingExtand[i] = KeyCode.None;
            }
        }
        keyboardSetting[(int)key] = keyCode;
    }

    public void SetKeyboardSubKey(Key key, KeyCode keyCode) {
        //Replace any exist with none.
        for (int i = 0; i < keyboardSetting.Count; i++) {
            if (keyboardSetting[i] == keyCode) {
                keyboardSetting[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingSub.Count; i++) {
            if (keyboardSettingSub[i] == keyCode) {
                keyboardSettingSub[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingExtra.Count; i++) {
            if (keyboardSettingExtra[i] == keyCode) {
                keyboardSettingExtra[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingExtand.Count; i++) {
            if (keyboardSettingExtand[i] == keyCode) {
                keyboardSettingExtand[i] = KeyCode.None;
            }
        }
        keyboardSettingSub[(int)key] = keyCode;
    }

    public void SetKeyboardExtraKey(Key key, KeyCode keyCode) {
        //Replace any exist with none.
        for (int i = 0; i < keyboardSetting.Count; i++) {
            if (keyboardSetting[i] == keyCode) {
                keyboardSetting[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingSub.Count; i++) {
            if (keyboardSettingSub[i] == keyCode) {
                keyboardSettingSub[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingExtra.Count; i++) {
            if (keyboardSettingExtra[i] == keyCode) {
                keyboardSettingExtra[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingExtand.Count; i++) {
            if (keyboardSettingExtand[i] == keyCode) {
                keyboardSettingExtand[i] = KeyCode.None;
            }
        }
        keyboardSettingExtra[(int)key] = keyCode;
    }

    public void SetKeyboardExtandKey(Key key, KeyCode keyCode) {
        //Replace any exist with none.
        for (int i = 0; i < keyboardSetting.Count; i++) {
            if (keyboardSetting[i] == keyCode) {
                keyboardSetting[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingSub.Count; i++) {
            if (keyboardSettingSub[i] == keyCode) {
                keyboardSettingSub[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingExtra.Count; i++) {
            if (keyboardSettingExtra[i] == keyCode) {
                keyboardSettingExtra[i] = KeyCode.None;
            }
        }
        for (int i = 0; i < keyboardSettingExtand.Count; i++) {
            if (keyboardSettingExtand[i] == keyCode) {
                keyboardSettingExtand[i] = KeyCode.None;
            }
        }
        keyboardSettingExtand[(int)key] = keyCode;
    }

    public void SetControllerKey(Key key, Nereus.InputType inputType) {
        //Replace any exist with none.
        for (int i = 0; i < controllerSetting.Count; i++) {
            if (controllerSetting[i] == inputType) {
                controllerSetting[i] = Nereus.InputType.None;
            }
        }
        controllerSetting[(int)key] = inputType;
    }

    public void RestoreDefault() {
        keyboardSetting = new List<KeyCode>(DEFAULT_KEYBOARD_SETTING);
        keyboardSettingSub = new List<KeyCode>(DEFAULT_KEYBOARD_SETTING_SUB);
        controllerSetting = new List<Nereus.InputType>(DEFAULT_CONTROLLER_SETTING);
    }

    public void Serialize(DataLocation dataLocation = DataLocation.AssetDataPath) {
        string dataPath = "";
        if (dataLocation == DataLocation.AssetDataPath) {
            //From Assets.
            dataPath = Application.dataPath + "/Resources/" + SysPath.UniqueDataPath;
            XMLUtil.SaveXMLData64<KeyConfig>(dataPath + KEY_CONFIG_NAME + ".txt", this);
        } else {
            //From user dir.
            dataPath = Application.persistentDataPath;
            XMLUtil.SaveXMLData64<KeyConfig>(dataPath + "/" + KEY_CONFIG_NAME, this);
        }
    }

};