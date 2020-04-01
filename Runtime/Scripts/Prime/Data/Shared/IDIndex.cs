using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("IDX")]
[XmlType("IDX")]
[Serializable]
public class IDIndex {

    [XmlArray("IDL")]
    [XmlArrayItem("ID", typeof(string))]
    public List<string> ids = new List<string>();

    [XmlElement("N")]
    //The name is just for identified by human. Will not use this in any place outside editors.
    public string name = "";

    public IDIndex() {

    }

    public IDIndex(string inputName) {
        name = inputName;
    }

    public IDIndex(List<string> inputIDs) {
        ids = new List<string>(inputIDs);
    }

    public bool IDExist(string ID) {
        return ids.Contains(ID);
    }

    public bool IsLegalToAddID(string ID) {
        if (!IDExist(ID) &&  Util.IsLegalIDString(ID)) {
            return true;
        }
        return false;
    }

    public bool AppendNewID(string ID) {
        if (!ids.Contains(ID) && Util.IsLegalIDString(ID)) {
            ids.Add(ID);
            return true;
        } else {
            return false;
        }
    }

    public bool InsertNewID(int index, string ID) {
        if (!ids.Contains(ID) && Util.IsLegalIDString(ID)) {
            if (index >= 0 && index <= ids.Count) {
                ids.Insert(index, ID);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    public bool RemoveID(string ID) {
        return ids.Remove(ID);
    }

    public bool RemoveID(int index) {
        if (index >= 0 && index < ids.Count) {
            ids.RemoveAt(index);
            return true;
        }else {
            return false;
        }
    }

    public bool RenameID(int index, string newID) {
        if (index >= 0 && index < ids.Count) {
            if(!IDExist(newID)) {
                ids[index] = newID;
                return true;
            }else {
                return false;
            }
        }else {
            return false;
        }
    }

    //A convenient function for editor.
    public string GenerateDuplicateID(string originalID) {
        //Check 4 to 1 tail digit characters.
        for (int i = 0; i < originalID.Length; i++) {
            string tailStr = originalID.Substring(i);
            int number = 0;
            if (!int.TryParse(tailStr, out number)) {
                continue;
            } else {
                //Got a tail number. Let's try use it.
                int maxSum = (int)(Math.Pow(10, tailStr.Length)) - number - 1;

                string format = "";
                for (int j = 0; j < tailStr.Length; j++) {
                    format += "0";
                }

                for (int j = 1; j <= maxSum; j++) {
                    int tryNum = number + j;

                    string tryID = originalID.Substring(0, i) + tryNum.ToString(format);
                    if (!ids.Contains(tryID)) {
                        return tryID;
                    }
                }
                break;
            }
        }

        //No digit at tail...
        for (int i = 1; i < 10000; ++i) {
            string thisID = originalID + i.ToString();
            if (!ids.Contains(thisID)) {
                return thisID;
            }//Else try next.
        }

        //What? This is not possible.
        return originalID;
    }

    public bool MoveUp(int index) {
        if (index > 0 && index < ids.Count) {
            string moving = ids[index];
            ids.RemoveAt(index);
            ids.Insert(index - 1, moving);
            return true;
        } else {
            return false;
        }
    }

    public bool MoveDown(int index) {
        if (index >= 0 && index < ids.Count - 1) {
            string moving = ids[index];
            ids.RemoveAt(index);
            ids.Insert(index + 1, moving);
            return true;
        } else {
            return false;
        }
    }
}
