using System.Xml.Serialization;
using System.Collections.Generic;
using System;

/// <summary>
/// 由一個任意 ID 配上一個 float 的簡單結構，可由 IDFloatEditor 編輯。
/// </summary>
[XmlRoot("IDFL")]
[XmlType("IDFL")]
[Serializable]
public class IDFloat {

    [XmlElement("ID")]
    public string ID = "";

    [XmlElement("V")]
    public float value = 1.0f;

    public IDFloat() {

    }

    public IDFloat(IDFloat idFloat) {
        ID = idFloat.ID;
        value = idFloat.value;
    }

    public IDFloat(string IDInput, float valueInput) {
        ID = IDInput;
        value = valueInput;
    }

    //Return a value from the input IDFloat list of the first appear given ID.
    //Possible return an zero value.
    static public float GetValue(List<IDFloat> IDFloats, string ID) {
        for (int i = 0; i < IDFloats.Count; i++) {
            if (IDFloats[i].ID == ID) {
                return IDFloats[i].value;
            }
        }

        return 0;
    }
}
