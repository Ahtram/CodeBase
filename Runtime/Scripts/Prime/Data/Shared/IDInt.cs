using System.Xml.Serialization;
using System.Collections.Generic;
using System;

/// <summary>
/// 由一個任意 ID 配上一個 int 的簡單結構，可由 IDIntEditor 編輯。
/// </summary>
[XmlRoot("IDIN")]
[XmlType("IDIN")]
[Serializable]
public class IDInt {

    [XmlElement("ID")]
    public string ID = "";

    [XmlElement("V")]
    public int value = 1;

    public IDInt() {

    }

    public IDInt(IDInt intPair) {
        ID = intPair.ID;
        value = intPair.value;
    }

    public IDInt(string IDInput, int valueInput) {
        ID = IDInput;
        value = valueInput;
    }

    //Return a value from the input IDInt list of the first appear given ID.
    //Possible return an zero value.
    static public int GetValue(List<IDInt> IDInts, string ID) {
        for (int i = 0; i < IDInts.Count; i++) {
            if (IDInts[i].ID == ID) {
                return IDInts[i].value;
            }
        }

        return 0;
    }
}
