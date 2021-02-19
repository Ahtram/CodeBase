using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("IDCO")]
[XmlType("IDCO")]
[Serializable]
public class IDCollection {

    [XmlArray("IDXL")]
    [XmlArrayItem("IDX", typeof(IDIndex))]
    public List<IDIndex> IDIndexes = new List<IDIndex>();

    public IDCollection() {

    }

    //For editor
    public void NewIDIndex() {
        IDIndexes.Add(new IDIndex());
    }

    //For editor
    public void NewIDIndex(string inputName) {
        IDIndexes.Add(new IDIndex(inputName));
    }

    public bool IDIndexExist(int catIndex) {
        if (catIndex >= 0 && catIndex < IDIndexes.Count) {
            return true;
        }
        return false;
    }

    public bool RemoveIDIndex(int catIndex) {
        if (catIndex >= 0 && catIndex < IDIndexes.Count) {
            IDIndexes.RemoveAt(catIndex);
            return true;
        }
        return false;
    }

    //For editor
    public bool MoveUpIDIndex(int catIndex) {
        if (catIndex > 0 && catIndex < IDIndexes.Count) {
            IDIndex moving = IDIndexes[catIndex];
            IDIndexes.RemoveAt(catIndex);
            IDIndexes.Insert(catIndex - 1, moving);
            return true;
        }
        return false;
    }

    //For editor
    public bool MoveDownIDIndex(int catIndex) {
        if (catIndex >= 0 && catIndex < IDIndexes.Count - 1) {
            IDIndex moving = IDIndexes[catIndex];
            IDIndexes.RemoveAt(catIndex);
            IDIndexes.Insert(catIndex + 1, moving);
            return true;
        }
        return false;
    }

    public bool AppendNewID(int catIndex, string ID) {
        if (catIndex >= 0 && catIndex < IDIndexes.Count && !IDExist(ID) && Util.IsLegalIDString(ID)) {
            return IDIndexes[catIndex].AppendNewID(ID);
        }
        return false;
    }

    public bool InsertNewID(int catIndex, int index, string ID) {
        if (catIndex >= 0 && catIndex < IDIndexes.Count && !IDExist(ID) && Util.IsLegalIDString(ID)) {
            return IDIndexes[catIndex].InsertNewID(index, ID);
        }
        return false;
    }

    public bool RemoveID(int catIndex, int index) {
        if (catIndex >= 0 && catIndex < IDIndexes.Count) {
            return IDIndexes[catIndex].RemoveID(index);
        }
        return false;
    }

    //This will just remove one that matches.
    public bool RemoveID(string ID) {
        foreach (IDIndex index in IDIndexes) {
            for (int i = index.ids.Count - 1; i >= 0; i--) {
                if (ID == index.ids[i]) {
                    index.ids.RemoveAt(i);
                    return true;
                }
            }
        }
        return false;
    }

    public bool RenameID(int catIndex, int index, string newID) {
        if (catIndex >= 0 && catIndex < IDIndexes.Count) {
            if (index >= 0 && index < IDIndexes[catIndex].ids.Count) {
                if (!IDExist(newID)) {
                    IDIndexes[catIndex].ids[index] = newID;
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }

    //Convenient function for editor
    public string[] GetDisplayOptions() {
        List<string> returnOptions = new List<string>();
        for (int i = 0; i < IDIndexes.Count; ++i) {
            returnOptions.Add(i.ToString() + "(" + IDIndexes[i].name + ")");
        }
        return returnOptions.ToArray();
    }

    //Get the category names of this IDCollection.
    public List<string> GetCatNames() {
        List<string> returnOptions = new List<string>();
        for (int i = 0; i < IDIndexes.Count; ++i) {
            returnOptions.Add(IDIndexes[i].name);
        }
        return returnOptions;
    }

    public List<string> GetAllIDs() {
        List<string> returnIDs = new List<string>();
        foreach (IDIndex index in IDIndexes) {
            foreach (string id in index.ids) {
                returnIDs.Add(id);
            }
        }
        return returnIDs;
    }

    public List<string> GetAllIDs(int catIndex) {
        List<string> returnIDs = new List<string>();
        if (catIndex < IDIndexes.Count) {
            foreach (string id in IDIndexes[catIndex].ids) {
                returnIDs.Add(id);
            }
        }
        return returnIDs;
    }

    public bool MoveIDToCat(string ID, int originalCatIndex, int targetCatIndex) {
        if (originalCatIndex < IDIndexes.Count && targetCatIndex < IDIndexes.Count) {
            foreach (string id in IDIndexes[originalCatIndex].ids) {
                if (id == ID) {
                    IDIndexes[originalCatIndex].ids.Remove(ID);
                    break;
                }
            }

            IDIndexes[targetCatIndex].ids.Add(ID);
            return true;
        }
        return false;
    }

    public bool MoveIDToCat(int originalCatIndex, int originalIDIndex, int targetCatIndex) {
        if (originalCatIndex < IDIndexes.Count && originalIDIndex < IDIndexes[originalCatIndex].ids.Count && targetCatIndex < IDIndexes.Count) {
            string ID = IDIndexes[originalCatIndex].ids[originalIDIndex];
            IDIndexes[originalCatIndex].ids.RemoveAt(originalIDIndex);
            IDIndexes[targetCatIndex].ids.Add(ID);
            return true;
        }
        return false;
    }

    public bool MoveIDToCatIndex(int originalCatIndex, int originalIDIndex, int targetCatIndex, int targetIndex) {
        if (originalCatIndex < IDIndexes.Count && originalIDIndex < IDIndexes[originalCatIndex].ids.Count && targetCatIndex < IDIndexes.Count && targetIndex < IDIndexes[targetCatIndex].ids.Count) {
            string ID = IDIndexes[originalCatIndex].ids[originalIDIndex];
            IDIndexes[originalCatIndex].ids.RemoveAt(originalIDIndex);
            IDIndexes[targetCatIndex].ids.Insert(targetIndex, ID);
            return true;
        }
        return false;
    }

    public bool IDExist(int catIndex, int index) {
        if (catIndex >= 0 && catIndex < IDIndexes.Count) {
            if (index >= 0 && index < IDIndexes[catIndex].ids.Count) {
                return true;
            }
            return false;
        }
        return false;
    }

    public bool IDExist(string ID) {
        foreach (IDIndex index in IDIndexes) {
            foreach (string id in index.ids) {
                if (ID == id) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsLegalToAddID(string ID) {
        if (!IDExist(ID) && Util.IsLegalIDString(ID)) {
            return true;
        }
        return false;
    }

    //Try get this ID's index.
    public bool GetIDIndex(string ID, ref int catIndex, ref int IDIndex) {
        for (int i = 0; i < IDIndexes.Count; ++i) {
            for (int j = 0; j < IDIndexes[i].ids.Count; ++j) {
                if (ID == IDIndexes[i].ids[j]) {
                    catIndex = i;
                    IDIndex = j;
                    return true;
                }
            }
        }
        return false;
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
                    if (!IDExist(tryID)) {
                        return tryID;
                    }
                }
                break;
            }
        }

        //No digit at tail...
        for (int i = 1; i < 10000; ++i) {
            string thisID = originalID + i.ToString();
            if (!IDExist(thisID)) {
                return thisID;
            }//Else try next.
        }

        //What? This is not possible.
        return originalID;
    }

    public void SortCats() {
        IDIndexes.Sort();
    }

    public void SortIDsInCat(int catIndex) {
        if (catIndex >= 0 && catIndex < IDIndexes.Count) {
            IDIndexes[catIndex].SortIDs();
        }
    }

}
