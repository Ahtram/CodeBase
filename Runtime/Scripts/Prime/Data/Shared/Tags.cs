using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlRoot("TAGS")]
[XmlType("TAGS")]
[Serializable]
public class Tags {

    [XmlArray("IL")]
    [XmlArrayItem("IL", typeof(string))]
    public List<string> items = new List<string>();

    public Tags() {

    }

    public Tags(Tags tags) {
        items = new List<string>(tags.items);
    }

    public bool Contains(string tag) {
        return items.Contains(tag);
    }

    //比較兩個 tag 的內容是否一致
    public bool IsTheSame(Tags tags) {
        return Util.ListIsEqual(items, tags.items);
    }

}
