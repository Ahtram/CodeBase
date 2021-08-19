using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;

//This is a helper class for useing Google Data Protcal API.
public class GDataHelper {

    //A structure for deserialize metadata.
    public class BackgroundColor {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
    }

    public class Padding {
        public int top { get; set; }
        public int right { get; set; }
        public int bottom { get; set; }
        public int left { get; set; }
    }

    public class ForegroundColor {
    }

    public class RgbColor {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
    }

    public class ForegroundColorStyle {
        public RgbColor rgbColor { get; set; }
    }

    public class TextFormat {
        public ForegroundColor foregroundColor { get; set; }
        public string fontFamily { get; set; }
        public int fontSize { get; set; }
        public bool bold { get; set; }
        public bool italic { get; set; }
        public bool strikethrough { get; set; }
        public bool underline { get; set; }
        public ForegroundColorStyle foregroundColorStyle { get; set; }
    }

    public class BackgroundColorStyle {
        public RgbColor rgbColor { get; set; }
    }

    public class DefaultFormat {
        public BackgroundColor backgroundColor { get; set; }
        public Padding padding { get; set; }
        public string verticalAlignment { get; set; }
        public string wrapStrategy { get; set; }
        public TextFormat textFormat { get; set; }
        public BackgroundColorStyle backgroundColorStyle { get; set; }
    }

    public class Properties {
        public string title { get; set; }
        public string locale { get; set; }
        public string autoRecalc { get; set; }
        public string timeZone { get; set; }
        public DefaultFormat defaultFormat { get; set; }
        public int sheetId { get; set; }
        public int index { get; set; }
        public string sheetType { get; set; }
        public GridProperties gridProperties { get; set; }
    }

    public class GridProperties {
        public int rowCount { get; set; }
        public int columnCount { get; set; }
        public int frozenRowCount { get; set; }
    }

    public class Sheet {
        public Properties properties { get; set; }
    }

    //The metadate object class for the main spreadsheets.
    public class SpreadsheetsMetadata {
        public string spreadsheetId { get; set; }
        public Properties properties { get; set; }
        public List<Sheet> sheets { get; set; }
        public string spreadsheetUrl { get; set; }
    }

    //--

    //The metadata object class for just one sheet.
    public class SheetValueRangeMetadata {
        public string range { get; set; }
        public string majorDimension { get; set; }
        public List<List<string>> values { get; set; }
    }

    //--

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

    //--

    /// <summary>
    /// Form a url for a public Google spreadsheet metadata.
    /// </summary>
    /// <param name="spreadsheetsId"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    static public string getSpreadsheetsMetadataUrl(string spreadsheetsId, string apiKey) {
        return "https://sheets.googleapis.com/v4/spreadsheets/" + spreadsheetsId + "?key=" + apiKey;
    }

    /// <summary>
    /// Get the spreadsheets metadata object.
    /// </summary>
    /// <param name="spreadsheetsId"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    static public SpreadsheetsMetadata requestSpreadsheetsMetadata(string spreadsheetsId, string apiKey) {
        string url = getSpreadsheetsMetadataUrl(spreadsheetsId, apiKey);
        UnityWebRequest www = UnityWebRequest.Get(url);
        UnityWebRequestAsyncOperation asyncOperation = www.SendWebRequest();

        //WWW cannot do yield in editor. This is stupid but we can only do this.
        while (!asyncOperation.isDone) ;

        if (www.error != null) {
            Debug.LogError(www.error);
            return null;
        } else {
            return JsonConvert.DeserializeObject<SpreadsheetsMetadata>(www.downloadHandler.text);
        }
    }

    /// <summary>
    /// Get the spreadsheets metadata object.
    /// </summary>
    /// <param name="spreadsheetsId"></param>
    /// <param name="apiKey"></param>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    static public IEnumerator requestSpreadsheetsMetadataAsync(string spreadsheetsId, string apiKey, Action<SpreadsheetsMetadata> onComplete) {
        string url = getSpreadsheetsMetadataUrl(spreadsheetsId, apiKey);
        UnityWebRequest www = UnityWebRequest.Get(url);
        UnityWebRequestAsyncOperation asyncOperation = www.SendWebRequest();

        while (!asyncOperation.isDone) {
            yield return null;
        }

        if (www.error != null) {
            Debug.LogError(www.error);
            onComplete?.Invoke(null);
        } else {
            onComplete?.Invoke(JsonConvert.DeserializeObject<SpreadsheetsMetadata>(www.downloadHandler.text));
        }
    }

    //--

    /// <summary>
    /// Form a url for getting data in range of a spreadsheet. The range could be just a sheet title.
    /// </summary>
    /// <param name="spreadsheetsId"></param>
    /// <param name="apiKey"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    static public string getSheetValueRangeUrl(string spreadsheetsId, string apiKey, string range) {
        return "https://sheets.googleapis.com/v4/spreadsheets/" + spreadsheetsId + "/values/" + UnityWebRequest.EscapeURL(range) + "?key=" + apiKey;
    }

    /// <summary>
    /// Get the sheet metadata object. The range could be just a sheet title.
    /// </summary>
    /// <param name="spreadsheetsId"></param>
    /// <param name="apiKey"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    static public SheetValueRangeMetadata requestSheetValueRangeMetadata(string spreadsheetsId, string apiKey, string range) {
        string url = getSheetValueRangeUrl(spreadsheetsId, apiKey, range);
        UnityWebRequest www = UnityWebRequest.Get(url);
        UnityWebRequestAsyncOperation asyncOperation = www.SendWebRequest();

        //WWW cannot do yield in editor. This is stupid but we can only do this.
        while (!asyncOperation.isDone) ;

        if (www.error != null) {
            Debug.LogError(www.error);
            return null;
        } else {
            return JsonConvert.DeserializeObject<SheetValueRangeMetadata>(www.downloadHandler.text);
        }
    }

    /// <summary>
    /// Get the sheet metadata object. The range could be just a sheet title.
    /// </summary>
    /// <param name="spreadsheetsId"></param>
    /// <param name="apiKey"></param>
    /// <param name="range"></param>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    static public IEnumerator requestSheetValueRangeMetadataAsync(string spreadsheetsId, string apiKey, string range, Action<SheetValueRangeMetadata> onComplete) {
        string url = getSheetValueRangeUrl(spreadsheetsId, apiKey, range);
        UnityWebRequest www = UnityWebRequest.Get(url);
        UnityWebRequestAsyncOperation asyncOperation = www.SendWebRequest();

        while (!asyncOperation.isDone) {
            yield return null;
        }

        if (www.error != null) {
            Debug.LogError(www.error);
            onComplete?.Invoke(null);
        } else {
            onComplete?.Invoke(JsonConvert.DeserializeObject<SheetValueRangeMetadata>(www.downloadHandler.text));
        }
    }

    //--

    //[Dep]: Google stop support for API v3
    //Convert the sheet cells xml feed to a string table.
    // static public WorkSheetData CellFeedToStringTable(string cellFeedXmlContent) {
    //     WorkSheetData returnWorkSheetData = new WorkSheetData();

    //     //Convert content string to XmlDoc.
    //     XmlDocument feedXmlDoc = new XmlDocument();
    //     feedXmlDoc.LoadXml(cellFeedXmlContent);

    //     //Read the "feed" element.
    //     XmlNode feedNode = feedXmlDoc.DocumentElement;

    //     //Add the namespace.
    //     XmlNamespaceManager nsmgr = new XmlNamespaceManager(feedXmlDoc.NameTable);
    //     nsmgr.AddNamespace("ns", "http://www.w3.org/2005/Atom");
    //     nsmgr.AddNamespace("gs", "http://schemas.google.com/spreadsheets/2006");

    //     if (feedNode != null) {
    //         //Get sheet size.
    //         int sheetRowCount = 0;
    //         int sheetColCount = 0;
    //         XmlNode rowCountNode = feedNode.SelectSingleNode("gs:rowCount", nsmgr);
    //         XmlNode colCountNode = feedNode.SelectSingleNode("gs:colCount", nsmgr);

    //         if (rowCountNode != null) {
    //             int.TryParse(rowCountNode.InnerText, out sheetRowCount);
    //         } else {
    //             Debug.LogWarning("Oops! No rowCount element found!");
    //         }

    //         if (colCountNode != null) {
    //             int.TryParse(colCountNode.InnerText, out sheetColCount);
    //         } else {
    //             Debug.LogWarning("Oops! No colCount element found!");
    //         }

    //         //Rreallocate the returnStringTable's space.
    //         for (int i = 0; i < sheetRowCount; ++i) {
    //             returnWorkSheetData.stringTable.Add(new List<string>());
    //             for (int j = 0; j < sheetColCount; ++j) {
    //                 returnWorkSheetData.stringTable[i].Add("");
    //             }
    //         }

    //         XmlNodeList allEntryNodes = feedNode.SelectNodes("ns:entry", nsmgr);

    //         //Read all entry to form the string table.
    //         for (int i = 0; i < allEntryNodes.Count; ++i) {
    //             XmlNode cellNode = allEntryNodes[i].SelectSingleNode("gs:cell", nsmgr);
    //             //Read cell.
    //             if (cellNode != null && cellNode.Attributes["row"] != null && cellNode.Attributes["col"] != null && cellNode.InnerText != null) {
    //                 int cellRowIndex = 0;
    //                 int cellColIndex = 0;
    //                 if (int.TryParse(cellNode.Attributes["row"].Value, out cellRowIndex) && int.TryParse(cellNode.Attributes["col"].Value, out cellColIndex)) {
    //                     //Minus the index 1 since the DGata sheet index is start from 1.
    //                     cellRowIndex -= 1;
    //                     cellColIndex -= 1;
    //                     if (cellRowIndex >= 0 && cellRowIndex < returnWorkSheetData.stringTable.Count && cellColIndex >= 0 && cellColIndex < returnWorkSheetData.stringTable[cellRowIndex].Count) {
    //                         returnWorkSheetData.stringTable[cellRowIndex][cellColIndex] = cellNode.InnerText;
    //                     }
    //                 } else {
    //                     Debug.LogWarning("Parse cell index failed!");
    //                 }
    //             } else {
    //                 Debug.LogWarning("Data corruption!");
    //             }
    //         }

    //         //Read workSheet title.
    //         returnWorkSheetData.title = feedNode.SelectSingleNode("ns:title", nsmgr).InnerText;

    //         Debug.Log("Feed [" + returnWorkSheetData.title + "] Sheet size [" + sheetRowCount.ToString() + "|" + sheetColCount.ToString() + "] Entry count [" + allEntryNodes.Count + "]");
    //     } else {
    //         Debug.LogWarning("Oops! No feed element found!");
    //     }

    //     return returnWorkSheetData;
    // }

    //[Dep]: Google stop support for API v3
    //Parse the worksheets feed xml content to get the cell feed urls for each worksheet (tab).
    // static public List<string> WorkSheetFeedToCellFeedURLs(string worksheetsFeedXmlContent, List<string> ignoreSheetNameKeywords = null) {
    //     List<string> returnUrls = new List<string>();

    //     //Convert content string to XmlDoc.
    //     XmlDocument feedXmlDoc = new XmlDocument();
    //     feedXmlDoc.LoadXml(worksheetsFeedXmlContent);

    //     //Read the "feed" element.
    //     XmlNode feedNode = feedXmlDoc.DocumentElement;

    //     //Add the namespace.
    //     XmlNamespaceManager nsmgr = new XmlNamespaceManager(feedXmlDoc.NameTable);
    //     nsmgr.AddNamespace("ns", "http://www.w3.org/2005/Atom");
    //     nsmgr.AddNamespace("gs", "http://schemas.google.com/spreadsheets/2006");

    //     if (feedNode != null) {

    //         //Get all entry nodes.
    //         XmlNodeList allEntryNodes = feedNode.SelectNodes("ns:entry", nsmgr);

    //         for (int i = 0; i < allEntryNodes.Count; ++i) {

    //             //Read title.
    //             XmlNode titleNode = allEntryNodes[i].SelectSingleNode("ns:title", nsmgr);

    //             //Ignore this sheet?
    //             bool ignore = false;
    //             if (ignoreSheetNameKeywords != null) {
    //                 for (int j = 0; j < ignoreSheetNameKeywords.Count; j++) {
    //                     if (titleNode.InnerText.StartsWith(ignoreSheetNameKeywords[j])) {
    //                         ignore = true;
    //                     }
    //                 }
    //             }

    //             if (!ignore) {
    //                 //Read all link nodes.
    //                 XmlNodeList linkNodes = allEntryNodes[i].SelectNodes("ns:link", nsmgr);
    //                 //Check these nodes to get the cell feed from one of them.
    //                 for (int j = 0; j < linkNodes.Count; ++j) {
    //                     if (linkNodes[j].Attributes["rel"].InnerText.EndsWith("cellsfeed")) {
    //                         //This is the cell feed link, store it.
    //                         returnUrls.Add(linkNodes[j].Attributes["href"].InnerText);
    //                     }
    //                 }
    //             } else {
    //                 Debug.Log("Ignoring sheet: [" + titleNode.InnerText + "]");
    //             }
    //         }

    //     } else {
    //         Debug.LogWarning("Oops! No feed element found!");
    //     }

    //     return returnUrls;
    // }

}