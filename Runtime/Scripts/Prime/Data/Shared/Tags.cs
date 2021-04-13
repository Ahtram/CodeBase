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

    //是否含有任何一個傳入的 Tags 中的一個
    public bool ContainsAnyOf(Tags tags) {
        for (int i = 0; i < tags.items.Count; i++) {
            if (items.Contains(tags.items[i])) {
                return true;
            }
        }
        return false;
    }

    //是否含有任何一個傳入的 Tags 中的一個
    public bool ContainsAnyOf(List<string> tags) {
        for (int i = 0; i < tags.Count; i++) {
            if (items.Contains(tags[i])) {
                return true;
            }
        }
        return false;
    }

    //是否含有所有的傳入的 Tags
    public bool ContainsAllOf(Tags tags) {
        for (int i = 0; i < tags.items.Count; i++) {
            if (!items.Contains(tags.items[i])) {
                return false;
            }
        }

        return true;
    }

    //是否含有所有的傳入的 Tags
    public bool ContainsAllOf(List<string> tags) {
        for (int i = 0; i < tags.Count; i++) {
            if (!items.Contains(tags[i])) {
                return false;
            }
        }

        return true;
    }

    //往前移動一個Tag
    public bool MoveUpIndex(int index) {
        if (index > 0 && index < items.Count) {
            string moving = items[index];
            items.RemoveAt(index);
            items.Insert(index - 1, moving);
            return true;
        }
        return false;
    }

    //往後移動一個Tag
    public bool MoveDownIndex(int index) {
        if (index >= 0 && index < items.Count - 1) {
            string moving = items[index];
            items.RemoveAt(index);
            items.Insert(index + 1, moving);
            return true;
        }
        return false;
    }

}
