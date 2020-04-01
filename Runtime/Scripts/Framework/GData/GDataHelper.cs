using System.Collections.Generic;
using UnityEngine;
using System.Xml;

//This is a helper class for useing Google Data Protcal API.
public class GDataHelper {

    public static readonly string[] DEFAULT_SHEET_IGNORE_KEYWORDS = {
        "Ignore"
    };

    //A structure for output data.
    public class WorkSheetData {
        public string title = "";
        public List<List<string>> stringTable = new List<List<string>>();

        //Just for debug.
        public string ToFormatString(string columSeperator, string rowSeperator) {
            if (string.IsNullOrEmpty(columSeperator)) {
                return "Error: No columSeperator assigned.";
            }

            if (string.IsNullOrEmpty(rowSeperator)) {
                return "Error: No rowSeperator assigned.";
            }

            string returnString = "";
            for (int i = 0; i < stringTable.Count; i++) {
                for (int j = 0; j < stringTable[i].Count; j++) {
                    returnString += stringTable[i][j];
                    if (j < stringTable[i].Count - 1) {
                        returnString += columSeperator;
                    }
                }

                if (i < stringTable.Count - 1) {
                    returnString += rowSeperator;
                }
            }
            return returnString;
        }
    }

    //Convert the sheet cells xml feed to a string table.
    static public WorkSheetData CellFeedToStringTable(string cellFeedXmlContent) {
        WorkSheetData returnWorkSheetData = new WorkSheetData();

        //Convert content string to XmlDoc.
        XmlDocument feedXmlDoc = new XmlDocument();
        feedXmlDoc.LoadXml(cellFeedXmlContent);

        //Read the "feed" element.
        XmlNode feedNode = feedXmlDoc.DocumentElement;

        //Add the namespace.
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(feedXmlDoc.NameTable);
        nsmgr.AddNamespace("ns", "http://www.w3.org/2005/Atom");
        nsmgr.AddNamespace("gs", "http://schemas.google.com/spreadsheets/2006");

        if (feedNode != null) {
            //Get sheet size.
            int sheetRowCount = 0;
            int sheetColCount = 0;
            XmlNode rowCountNode = feedNode.SelectSingleNode("gs:rowCount", nsmgr);
            XmlNode colCountNode = feedNode.SelectSingleNode("gs:colCount", nsmgr);

            if (rowCountNode != null) {
                int.TryParse(rowCountNode.InnerText, out sheetRowCount);
            } else {
                Debug.LogWarning("Oops! No rowCount element found!");
            }

            if (colCountNode != null) {
                int.TryParse(colCountNode.InnerText, out sheetColCount);
            } else {
                Debug.LogWarning("Oops! No colCount element found!");
            }

            //Rreallocate the returnStringTable's space.
            for (int i = 0; i < sheetRowCount; ++i) {
                returnWorkSheetData.stringTable.Add(new List<string>());
                for (int j = 0; j < sheetColCount; ++j) {
                    returnWorkSheetData.stringTable[i].Add("");
                }
            }

            XmlNodeList allEntryNodes = feedNode.SelectNodes("ns:entry", nsmgr);

            //Read all entry to form the string table.
            for (int i = 0; i < allEntryNodes.Count; ++i) {
                XmlNode cellNode = allEntryNodes[i].SelectSingleNode("gs:cell", nsmgr);
                //Read cell.
                if (cellNode != null && cellNode.Attributes["row"] != null && cellNode.Attributes["col"] != null && cellNode.InnerText != null) {
                    int cellRowIndex = 0;
                    int cellColIndex = 0;
                    if (int.TryParse(cellNode.Attributes["row"].Value, out cellRowIndex) && int.TryParse(cellNode.Attributes["col"].Value, out cellColIndex)) {
                        //Minus the index 1 since the DGata sheet index is start from 1.
                        cellRowIndex -= 1;
                        cellColIndex -= 1;
                        if (cellRowIndex >= 0 && cellRowIndex < returnWorkSheetData.stringTable.Count && cellColIndex >= 0 && cellColIndex < returnWorkSheetData.stringTable[cellRowIndex].Count) {
                            returnWorkSheetData.stringTable[cellRowIndex][cellColIndex] = cellNode.InnerText;
                        }
                    } else {
                        Debug.LogWarning("Parse cell index failed!");
                    }
                } else {
                    Debug.LogWarning("Data corruption!");
                }
            }

            //Read workSheet title.
            returnWorkSheetData.title = feedNode.SelectSingleNode("ns:title", nsmgr).InnerText;

            Debug.Log("Feed [" + returnWorkSheetData.title + "] Sheet size [" + sheetRowCount.ToString() + "|" + sheetColCount.ToString() + "] Entry count [" + allEntryNodes.Count + "]");
        } else {
            Debug.LogWarning("Oops! No feed element found!");
        }

        return returnWorkSheetData;
    }

    //Parse the worksheets feed xml content to get the cell feed urls for each worksheet (tab).
    static public List<string> WorkSheetFeedToCellFeedURLs(string worksheetsFeedXmlContent, List<string> ignoreSheetNameKeywords = null) {
        List<string> returnUrls = new List<string>();

        //Convert content string to XmlDoc.
        XmlDocument feedXmlDoc = new XmlDocument();
        feedXmlDoc.LoadXml(worksheetsFeedXmlContent);

        //Read the "feed" element.
        XmlNode feedNode = feedXmlDoc.DocumentElement;

        //Add the namespace.
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(feedXmlDoc.NameTable);
        nsmgr.AddNamespace("ns", "http://www.w3.org/2005/Atom");
        nsmgr.AddNamespace("gs", "http://schemas.google.com/spreadsheets/2006");

        if (feedNode != null) {

            //Get all entry nodes.
            XmlNodeList allEntryNodes = feedNode.SelectNodes("ns:entry", nsmgr);

            for (int i = 0; i < allEntryNodes.Count; ++i) {

                //Read title.
                XmlNode titleNode = allEntryNodes[i].SelectSingleNode("ns:title", nsmgr);

                //Ignore this sheet?
                bool ignore = false;
                if (ignoreSheetNameKeywords != null) {
                    for (int j = 0; j < ignoreSheetNameKeywords.Count; j++) {
                        if (titleNode.InnerText.StartsWith(ignoreSheetNameKeywords[j])) {
                            ignore = true;
                        }
                    }
                }

                if (!ignore) {
                    //Read all link nodes.
                    XmlNodeList linkNodes = allEntryNodes[i].SelectNodes("ns:link", nsmgr);
                    //Check these nodes to get the cell feed from one of them.
                    for (int j = 0; j < linkNodes.Count; ++j) {
                        if (linkNodes[j].Attributes["rel"].InnerText.EndsWith("cellsfeed")) {
                            //This is the cell feed link, store it.
                            returnUrls.Add(linkNodes[j].Attributes["href"].InnerText);
                        }
                    }
                } else {
                    Debug.Log("Ignoring sheet: [" + titleNode.InnerText + "]");
                }
            }

        } else {
            Debug.LogWarning("Oops! No feed element found!");
        }

        return returnUrls;
    }

}