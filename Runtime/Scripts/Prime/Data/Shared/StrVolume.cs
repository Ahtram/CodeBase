using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlRoot("STRV")]
[XmlType("STRV")]
[Serializable]
public class StrVolume {

    [XmlElement("I")]
    public string index;

	[XmlArray("VL")]
	[XmlArrayItem("V", typeof(string))]
	public List<string> values = new List<string>();

    public StrVolume() {

    }

    public StrVolume(StrVolume strVolume) {
        index = strVolume.index;
        values = new List<string>(strVolume.values.ToArray());
    }

    public StrVolume(string indexInput, List<string> valuesInput) {
        index = indexInput;
        values = new List<string>(valuesInput.ToArray());
    }

}
