using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

[Serializable]
[XmlType("ED")]
public class ExampleData : BaseData<ExampleData> {

    [XmlIgnore]
    static public string DataFileName = "ExampleData/ExampleData_";

    [XmlIgnore]
    static public string DataIDFileName = "ExampleDataIDs";

    [XmlElement("V")]
    public int var = 0;

    [XmlElement("ET")]
    public Tags exampleTags = new Tags();

    [XmlElement("IF")]
    public string IDField = "";

    [XmlArray("IL")]
    [XmlArrayItem("I", typeof(string))]
    public List<string> IDList = new List<string>();

    [XmlElement("PF")]
    public string PrefabNameField = "";

    [XmlArray("PL")]
    [XmlArrayItem("P", typeof(string))]
    public List<string> PrefabList = new List<string>();

    [XmlArray("P1L")]
    [XmlArrayItem("P1", typeof(string))]
    public List<string> PrefabList1 = new List<string>();

    [XmlArray("BL")]
    [XmlArrayItem("B", typeof(bool))]
    public List<bool> boolList = new List<bool>();

    [XmlArray("IDPL")]
    [XmlArrayItem("IDP", typeof(IDPair))]
    public List<IDPair> IDPairList = new List<IDPair>();

    [XmlElement("SS")]
    public Shape shapeStuff = new NoneShape();

    [XmlElement("V3S")]
    public Vector3 vector3Stuff;

    [XmlElement("V2S")]
    public Vector2 vector2Stuff;

    [XmlElement("V4S")]
    public Vector4 vector4Stuff;

    [XmlElement("QS")]
    public Quaternion quaternionStuff;

    [XmlIgnore]
    static private IDCollection m_IDCollection = null;

    [XmlIgnore]
    static private Dictionary<string, ExampleData> m_exampleCache = new Dictionary<string, ExampleData>();

    [XmlIgnore]
    static private Tome<ExampleData> m_tome = null;

    //Constructor.
    public ExampleData() {

    }

    public ExampleData(string inputID) {
        ID = inputID;
    }

    override public void Save() {
        //If you gonna do SaveTome() please leave this to blank. We don't want the be work with SaveTome().
        // if (isDirty) {
        //     DataUtil.SaveData<ExampleData>(Application.dataPath + "/Resources/" + SysPath.NonlocalizedDataPath + DataFileName + ID, this);
        // }
    }

    override public void CleanUpFiles() {
        //If you gonna do SaveTome() please leave this to blank. We don't want the be work with SaveTome().
        //Check and remove all saved files.
        // DataUtil.CleanUpData(Application.dataPath + "/Resources/" + SysPath.NonlocalizedDataPath + DataFileName + ID);
    }

    //=====================================================

    public static IDCollection GetIDCollection() {
        if (m_IDCollection == null) {
            m_IDCollection = DataUtil.GetDataFromResource<IDCollection>(SysPath.TrivialDataPath + DataIDFileName, false);
            if (m_IDCollection == null) {
                m_IDCollection = new IDCollection();
            }
        }
        return m_IDCollection;
    }

    //[Will Try To Return a Copy From Cache First.]
    public static ExampleData Get(string ID) {
        //This is the centrolized way.
        if (!m_exampleCache.ContainsKey(ID)) {
            Tome<ExampleData> tome = GetTome();
            if (tome != null) {
                m_exampleCache.Add(ID, tome.GetData(ID));
            }
        }
        return m_exampleCache[ID];

        //[Preserve for ref]
        //This is the discreted way.
        // if (!m_exampleCache.ContainsKey(ID)) {
        //     ExampleData exampleData = DataUtil.GetDataFromResource<ExampleData>(SysPath.NonlocalizedDataPath + DataFileName + ID);
        //     if (exampleData != null) {
        //         exampleData.MarkClean();
        //         m_exampleCache.Add(ID, exampleData);
        //     }
        // }
        // return m_exampleCache[ID];

    }

    //Load the existing Tome.
    public static Tome<ExampleData> GetTome() {
        if (m_tome == null) {
            m_tome = Tome<ExampleData>.Get<ExampleData>(SysPath.NonlocalizedDataPath + DataFileName + "Tome");
            if (m_tome == null) {
                m_tome = new Tome<ExampleData>();
            }
        }
        return m_tome;
    }

    public static void ClearCache() {
        m_IDCollection = null;
        m_exampleCache.Clear();
        m_tome = null;
    }

    //Save the input editing data as a Tome.
    public static void SaveTome(List<List<ExampleData>> data) {
        Tome<ExampleData> tome = new Tome<ExampleData>(data);
        tome.Save(Application.dataPath + "/Resources/" + SysPath.NonlocalizedDataPath + DataFileName + "Tome");
    }

}
