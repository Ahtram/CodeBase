using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlRoot("STRP")]
[XmlType("STRP")]
[Serializable]
public class StrPair {

    [XmlElement("I")]
    public string index;

    [XmlElement("V")]
    public string value;

    public StrPair() {

    }

    public StrPair(StrPair strPair) {
        index = strPair.index;
        value = strPair.value;
    }

    public StrPair(string indexInput, string valueInput) {
        index = indexInput;
        value = valueInput;
    }

    //Return a value from the input StrPair list of the first appear given index.
    //Possible return an empty string.
    static public string GetValue(List<StrPair> strPairs, string key) {
        for (int i = 0; i < strPairs.Count; i++) {
            if (strPairs[i].index == key) {
                return strPairs[i].value;
            }
        }

        return "";
    }

}
