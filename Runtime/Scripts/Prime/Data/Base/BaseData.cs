using System.Xml.Serialization;
using System.Reflection;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

/// <summary>
/// The data interface class for integrate to data editor class.
/// </summary>
/// <typeparam name="T">This should be the derived class.</typeparam>
[Serializable]
public class BaseData<T> : Cloneable<T> {

    [XmlElement("ID")]
    public string ID = "";

    [XmlIgnore]
    [JsonIgnore]
    //Is this data is changed and currently in an unsaved state?
    //The actual save logic can take advantage of this flag and skip the actual serialize bahvior when save.
    //This flag should be set to false after a deserialize and set to ture when editing.
    public bool isDirty = true;

    //Implement how you save this data. There're two ways doing this.
    //1. Save this object to a single file like XML or Json.
    //2. Store this object in to their belonging IDCOllection or IDIndex and serialize the container to a single file.
    virtual public void Save() {

    }

    virtual public void CleanUpFiles() {

    }

    //Mark this data is dirty and need to be serialize when saving.
    public void MarkDirty() {
        isDirty = true;
    }

    //Mark as clean.
    public void MarkClean() {
        isDirty = false;
    }

}

[Serializable]
abstract public class Cloneable<T> {
    //Use reflection to make a default constructor directly for child class.
    virtual public object GetDefault() {
        return this.GetType().GetMethod("GetDefaultGeneric", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(this.GetType()).Invoke(this, null);
    }

    virtual public T1 GetDefaultGeneric<T1>() {
        return default(T1);
    }

    virtual public T Clone() {
        if (System.Object.ReferenceEquals(this, null)) {
            return (T)GetDefault();
        }

        XmlSerializer serializer = new XmlSerializer(typeof(T));
        Stream stream = new MemoryStream();
        serializer.Serialize(stream, this);
        stream.Seek(0, SeekOrigin.Begin);
        T returnData = (T)serializer.Deserialize(stream);
        return returnData;
    }
}

//This data is for 
[XmlType("TM")]
[XmlRoot("TM")]
[Serializable]
public class Tome<T> : Cloneable<T> where T : BaseData<T> {

    [XmlArray]
    public List<Volume<T>> Volumes = new List<Volume<T>>();

    public Tome() {

    }

    public Tome(List<List<T>> data) {
        Format(data);
    }

    public void Format(List<List<T>> data) {
        Volumes = data.Select((item) => new Volume<T>(item)).ToList();
    }

    public void Save(string applicationFullPath) {
        DataUtil.SaveData<Tome<T>>(applicationFullPath, this);
    }

    //Try find a data from the tome and return it.
    public T GetData(string ID) {
        for (int i = 0; i < Volumes.Count; i++) {
            T t = Volumes[i].GetData(ID);
            if (t != null) {
                return t;
            }
        }
        return null;
    }

    //--

    static public Tome<M> Get<M>(string resPathNoExt) where M : BaseData<M> {
        Tome<M> tome = DataUtil.GetDataFromResource<Tome<M>>(resPathNoExt);
        if (tome == null) {
            //In case we didn't found the old data just create a new one.
            tome = new Tome<M>();
        }
        return tome;
    }

}

[XmlType("VOL")]
[XmlRoot("VOL")]
[Serializable]
public class Volume<T> : Cloneable<T> where T : BaseData<T> {

    [XmlArray]
    public List<T> List = new List<T>();

    public Volume() {

    }

    public Volume(List<T> data) {
        Format(data);
    }

    public void Format(List<T> data) {
        List = data.Select((item) => item.Clone()).ToList();
    }

    //Save this volume to a path.
    public void Save(string applicationFullPath) {
        DataUtil.SaveData<Volume<T>>(applicationFullPath, this);
    }

    //Try find a data from the volume and return it.
    public T GetData(string ID) {
        for (int i = 0; i < List.Count; i++) {
            if (List[i].ID == ID) {
                return List[i];
            }
        }
        return null;
    }

    //--

    //Get a Volume from resource path (path without ext name)
    static public Volume<M> Get<M>(string resPathNoExt) where M : BaseData<M> {
        Volume<M> volume = DataUtil.GetDataFromResource<Volume<M>>(resPathNoExt);
        if (volume == null) {
            //In case we didn't found the old data just create a new one.
            volume = new Volume<M>();
        }
        return volume;
    }
}