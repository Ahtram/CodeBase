using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlRoot("INF")]
[XmlType("INF")]
[Serializable]
public class IntFloat {

    [XmlElement("I")]
    public int index;

    [XmlElement("V")]
    public float value;

    public IntFloat() {

    }

    public IntFloat(IntFloat intFloat) {
        index = intFloat.index;
        value = intFloat.value;
    }

    public IntFloat(int indexInput, float valueInput) {
        index = indexInput;
        value = valueInput;
    }

    //Return a value from the input IntFloat list of the first appear given index.
    //Possible return an zero value.
    static public float GetValue(List<IntFloat> intFloats, int key) {
        for (int i = 0; i < intFloats.Count; i++) {
            if (intFloats[i].index == key) {
                return intFloats[i].value;
            }
        }

        return 0;
    }

}
