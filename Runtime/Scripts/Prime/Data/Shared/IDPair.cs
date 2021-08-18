using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlRoot("STP")]
[XmlType("STP")]
public class IDPair {

    [XmlElement("ID1")]
    public string ID1;

    [XmlElement("ID2")]
    public string ID2;

    public IDPair() {

    }

    public IDPair(IDPair idPair) {
        ID1 = idPair.ID1;
        ID2 = idPair.ID2;
    }

    public IDPair(string ID1Input, string ID2Input) {
        ID1 = ID1Input;
        ID2 = ID2Input;
    }

    //Return a value from the input IDPair list of the first appear given ID.
    //Possible return an empty string.
    static public string GetValue(List<IDPair> idPairs, string key) {
        for (int i = 0; i < idPairs.Count; i++) {
            if (idPairs[i].ID1 == key) {
                return idPairs[i].ID2;
            }
        }

        return "";
    }

}
