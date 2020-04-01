using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlRoot("INTP")]
[XmlType("INTP")]
[Serializable]
public class IntPair {

    [XmlElement("I")]
    public int index;

    [XmlElement("V")]
    public int value;

    public IntPair() {

    }

    public IntPair(IntPair intPair) {
        index = intPair.index;
        value = intPair.value;
    }

    public IntPair(int indexInput, int valueInput) {
        index = indexInput;
        value = valueInput;
    }

    //Return a value from the input IntPair list of the first appear given index.
    //Possible return an zero value.
    static public int GetValue(List<IntPair> intPairs, int key) {
        for (int i = 0; i < intPairs.Count; i++) {
            if (intPairs[i].index == key) {
                return intPairs[i].value;
            }
        }

        return 0;
    }

}
