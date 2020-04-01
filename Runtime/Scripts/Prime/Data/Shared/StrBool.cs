using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlRoot("STRB")]
[XmlType("STRB")]
[Serializable]
public class StrBool {

    [XmlElement("I")]
    public string index;

    [XmlElement("O")]
    public bool toggle = false;

    public StrBool() {

    }

    public StrBool(StrBool strBool) {
        index = strBool.index;
        toggle = strBool.toggle;
    }

    public StrBool(string indexInput, bool toggleInput) {
        index = indexInput;
        toggle = toggleInput;
    }

    //Return a value from the input StrBool list of the first appear given index.
    //Possible return an empty string.
    static public bool GetValue(List<StrBool> strBools, string key) {
        for (int i = 0; i < strBools.Count; i++) {
            if (strBools[i].index == key) {
                return strBools[i].toggle;
            }
        }

        return false;
    }

}
