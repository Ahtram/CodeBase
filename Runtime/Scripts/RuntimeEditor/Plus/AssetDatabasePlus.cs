#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// A static class for provide customize AssetDatabase APIs.
/// </summary>
static public class AssetDatabasePlus {
    
    /// <summary>
    /// A record for one-time FindAssets.
    /// </summary>
    private class FindAssetsRecord {
        public string filter = "";
        public string assetsFolderPath = "";
        //Just for fast reference.
        public List<string> guids = new List<string>();
        public List<string> assetPathes = new List<string>();
        public List<string> assetNames = new List<string>();

        //Refer as live time of this record.
        public int passedCallCount = 0;
    }

    //Cache body.
    static private List<FindAssetsRecord> findAssetsRecordCache = new List<FindAssetsRecord>();

    //Cache life time setting.
    private const int RECORD_MAINTAIN_PASSCOUNT = 720;

    /// <summary>
    /// Check if an asset exist in the current AssetDatabase. This method use a cache trick to limit the performance impact.
    /// </summary>
    /// <param name="assetsFolderPath"></param>
    /// <param name="targetTerm"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    static public bool IsAssetExistCache(string assetsFolderPath, string targetTerm, string assetName) {
        string filter = assetName + " t:" + targetTerm;

        //Try find in cache.
        bool foundMatchedRecord = false;
        bool assetExistInFoundRecord = false;
        for (int i = 0; i < findAssetsRecordCache.Count; i++) {
            findAssetsRecordCache[i].passedCallCount++;
            if (findAssetsRecordCache[i].filter == filter && findAssetsRecordCache[i].assetsFolderPath == assetsFolderPath) {
                foundMatchedRecord = true;
                if (findAssetsRecordCache[i].assetNames.Contains(assetName)) {
                    assetExistInFoundRecord = true;
                }
            }
        }

        //Try clear old cache
        for (int i = findAssetsRecordCache.Count - 1; i >= 0 ; i--) {
            if(findAssetsRecordCache[i].passedCallCount >= RECORD_MAINTAIN_PASSCOUNT) {
                findAssetsRecordCache.RemoveAt(i);
            }
        }

        if (foundMatchedRecord) {
            return assetExistInFoundRecord;
        } else {
            //Not found any matched record. We need to do an actual FindAssets.
            string[] guids = AssetDatabase.FindAssets(filter, new string[] { assetsFolderPath });
            //Add a new record.
            FindAssetsRecord newRecord = new FindAssetsRecord();
            newRecord.filter = filter;
            newRecord.assetsFolderPath = assetsFolderPath;
            newRecord.guids = new List<string>(guids);
            for (int i = 0; i < guids.Length; i++) {
                string recordAssetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                newRecord.assetPathes.Add(recordAssetPath);
                string recordAssetName = Path.GetFileNameWithoutExtension(recordAssetPath);
                newRecord.assetNames.Add(recordAssetName);
            }
            findAssetsRecordCache.Add(newRecord);
            return newRecord.assetNames.Contains(assetName);
        }
    }

    /// <summary>
    /// Return the first asset path from the matched record. This is for conveniently select that asset in the Editor.
    /// </summary>
    /// <param name="assetsFolderPath"></param>
    /// <param name="targetTerm"></param>
    /// <param name="assetName"></param>
    /// <returns>First asset path </returns>
    static public string GetAssetPathFromRecordCache(string assetsFolderPath, string targetTerm, string assetName) {
        string filter = assetName + " t:" + targetTerm;
        //Try find in cache.
        for (int i = 0; i < findAssetsRecordCache.Count; i++) {
            findAssetsRecordCache[i].passedCallCount++;
            if (findAssetsRecordCache[i].filter == filter && findAssetsRecordCache[i].assetsFolderPath == assetsFolderPath) {
                //Just return the first found asset path.
                if (findAssetsRecordCache[i].assetPathes.Count > 0) {
                    return findAssetsRecordCache[i].assetPathes[0];
                }
            }
        }
        return "";
    }

}

#endif