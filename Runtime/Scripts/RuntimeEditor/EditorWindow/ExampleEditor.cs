using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class ExampleEditor : IDCollectionDataEditor<ExampleData> {

    public List<ExampleDataEditorUtility> accessingEditorUtilities = new List<ExampleDataEditorUtility>();

    override protected void DrawTitle() {
        GUI.color = ColorPlus.GhostWhite;
        EditorGUILayout.BeginVertical("HelpBox");
        {
            EditorGUILayout.LabelField("Example Editor");
        }
        EditorGUILayout.EndVertical();
        GUI.color = Color.white;
    }

    override protected void DrawDetail(ExampleData data) {
        ExampleDataEditorUtility.DrawExampleData(data);
    }

    //Implement yourown save functions here.
    override protected void Save() {
        SaveIDCollection();

        //Decreted way.
        // SaveEditing();

        //Centrolized way.
        ExampleData.SaveTome(editingDataList);
        // Tome<ExampleData> tome = new Tome<ExampleData>(editingDataList);
        // tome.Save(Application.dataPath + "/Resources/" + SysPath.NonlocalizedDataPath + ExampleData.DataFileName + "Tome.txt");

        AssetDatabase.Refresh();
    }

    override protected void Revert() {
        ExampleData.ClearCache();
        RefreshIDCollection();
        InitialEditingContainer();
        CloseAllEditorUtility();
    }

    override protected void SaveIDCollection() {
        if (editingIDCollection != null) {
            DataUtil.SaveData(Application.dataPath + "/Resources/" + SysPath.TrivialDataPath + ExampleData.DataIDFileName, editingIDCollection);
        }
    }

    //For dynamic load.
    override protected void OnIDSelected(int catIndex, int index, string selectID) {
        //Load the behavior if it's null.
        if (catIndex >= 0 && catIndex < editingDataList.Count) {
            if (index >= 0 && index < editingDataList[catIndex].Count) {
                if (editingDataList[catIndex][index] == null) {
                    editingDataList[catIndex][index] = ExampleData.Get(editingIDCollection.IDIndexes[catIndex].ids[index]);
                }

                if (!useBuiltInDetailView) {
                    //Try to open exist utility window first. If failed, create a new one.
                    if (!RefocusEditorUtility(editingDataList[catIndex][index])) {
                        ExampleDataEditorUtility newUtility = ExampleDataEditorUtility.CreateWindow(editingDataList[catIndex][index]);
                        if (accessingEditorUtilities.Count > 0) {
                            //Slightly shift the new window's position.
                            Rect lastUtilityPos = accessingEditorUtilities[accessingEditorUtilities.Count - 1].position;
                            newUtility.position = new Rect(new Vector2(lastUtilityPos.xMin + 5, lastUtilityPos.yMin + 5), lastUtilityPos.size);
                        }
                        accessingEditorUtilities.Add(newUtility);
                    }
                }
            }
        }
    }

    override protected void OnDataNeeded(int catIndex, int index) {
        //Load the behavior if it's null.
        if (catIndex >= 0 && catIndex < editingDataList.Count) {
            if (index >= 0 && index < editingDataList[catIndex].Count) {
                if (editingDataList[catIndex][index] == null) {
                    editingDataList[catIndex][index] = ExampleData.Get(editingIDCollection.IDIndexes[catIndex].ids[index]);
                }
            }
        }
    }

    override protected void OnDataNeeded(List<string> IDs) {
        if (editingIDCollection != null) {
            for (int cat = 0; cat < editingIDCollection.IDIndexes.Count; cat++) {
                for (int index = 0; index < editingIDCollection.IDIndexes[cat].ids.Count; index++) {
                    if (IDs.Contains(editingIDCollection.IDIndexes[cat].ids[index])) {
                        if (editingDataList[cat][index] == null) {
                            editingDataList[cat][index] = ExampleData.Get(editingIDCollection.IDIndexes[cat].ids[index]);
                        }
                    }
                }
            }
        }
    }

    private bool RefocusEditorUtility(ExampleData data) {
        for (int i = 0; i < accessingEditorUtilities.Count; i++) {
            if (accessingEditorUtilities[i] != null && accessingEditorUtilities[i].DisplayingContent() == data) {
                accessingEditorUtilities[i].Show();
                accessingEditorUtilities[i].Focus();
                return true;
            }
        }
        return false;
    }

    private void CloseAllEditorUtility() {
        for (int i = 0; i < accessingEditorUtilities.Count; i++) {
            if (accessingEditorUtilities[i] != null) {
                accessingEditorUtilities[i].Close();
            }
        }
        accessingEditorUtilities.Clear();
    }

    // -------------------------------------------------------------------------------

    override protected void OnEnable() {
        RefreshIDCollection();
        InitialEditingContainer();

        //For this editor we want it to just be a ID list menu. And open the selected content in an external utility window.
        useBuiltInDetailView = true;
        SetMode(IDCollectionEditor.Mode.ButtonMenu);

        base.OnEnable();
    }

    override public void OnFocus() {
        base.OnFocus();
        if (editingIDCollection == null) {
            //Maybe refresh collection here.
            RefreshIDCollection();
            RefreshEditing();
        }

        //Cleanup accessingEditorUtilities.
        for (int i = accessingEditorUtilities.Count - 1; i >= 0; i--) {
            if (accessingEditorUtilities[i] == null) {
                accessingEditorUtilities.RemoveAt(i);
            }
        }
    }

    override protected void OnDestroy() {
        ExampleData.ClearCache();
        editingIDCollection = null;
        editingDataList.Clear();

        base.OnDestroy();
    }

    private void RefreshIDCollection() {
        //Load ID collection
        editingIDCollection = ExampleData.GetIDCollection();

        if (editingIDCollection == null) {
            //New for it.
            editingIDCollection = new IDCollection();
            Debug.Log("New ExampleData Collection!");
        }

        //Test get the Tome.
        // Tome<ExampleData> tome = ExampleData.GetTome();
        // Debug.Log("Got tome: " + tome.Volumes.Count);
    }

    private void InitialEditingContainer() {
        //Since behavior editor will be too heavy to open to load all behaviors at once. We'll load them one by one when needed.
        editingDataList.Clear();

        if (editingIDCollection != null) {
            for (int i = 0; i < editingIDCollection.IDIndexes.Count; ++i) {
                editingDataList.Add(new List<ExampleData>());
                for (int j = 0; j < editingIDCollection.IDIndexes[i].ids.Count; ++j) {
                    //Skip load the data. To it later.
                    // editingDataList[i].Add(null);
                    editingDataList[i].Add(ExampleData.Get(editingIDCollection.IDIndexes[i].ids[j]));
                }
            }
        }
    }

    private void RefreshEditing() {
        //Since behavior editor will be too heavy to open to load all behaviors at once. We'll load them one by one when needed.
        if (editingIDCollection != null) {
            for (int i = 0; i < editingIDCollection.IDIndexes.Count; ++i) {
                for (int j = 0; j < editingIDCollection.IDIndexes[i].ids.Count; ++j) {
                    //Only reload those already loaded.
                    if (editingDataList[i][j] != null) {
                        editingDataList[i][j] = ExampleData.Get(editingIDCollection.IDIndexes[i].ids[j]);
                    }
                }
            }
        }
    }

    private void SaveEditing() {
        for (int i = 0; i < editingDataList.Count; ++i) {
            for (int j = 0; j < editingDataList[i].Count; ++j) {
                if (editingDataList[i][j] != null) {
                    editingDataList[i][j].Save();
                }
            }
        }
    }
}
