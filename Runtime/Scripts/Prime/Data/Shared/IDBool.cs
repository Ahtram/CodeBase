using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlRoot("IDB")]
[XmlType("IDB")]
[Serializable]
public class IDBool {

    [XmlElement("ID")]
    public string ID;

    [XmlElement("O")]
    public bool toggle = false;

    public IDBool() {

    }

    public IDBool(IDBool idBool) {
        ID = idBool.ID;
        toggle = idBool.toggle;
    }

    public IDBool(string IDInput, bool toggleInput) {
        ID = IDInput;
        toggle = toggleInput;
    }

    //Return a value from the input IDBool list of the first appear given ID.
    //Possible return an empty string.
    static public bool GetValue(List<IDBool> idBools, string key) {
        for (int i = 0; i < idBools.Count; i++) {
            if (idBools[i].ID == key) {
                return idBools[i].toggle;
            }
        }

        return false;
    }

}
