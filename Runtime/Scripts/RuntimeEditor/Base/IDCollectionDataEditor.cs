#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A generic IDCollection serious data editor base.
/// </summary>
/// <typeparam name="T">Data type.</typeparam>
abstract public class IDCollectionDataEditor<T> : UniEditorWindow where T : BaseData<T>, new() {

    private Vector2 scrollPos;
    public const string EDITMODE_SCROLLPOS_POSTFIX = "_scrollPos";

    private float builtInDetailViewIDViewWidth = 300.0f;
    public const string BUILTINDETAIL_IDVIEW_WIDTH_POSTFIX = "_builtInDetailViewIDViewWidth";
    private Rect builtInDetailViewIDViewPopupRect;

    private Vector2 detailScrollPos;

    protected IDCollection editingIDCollection = null;

    //[Cat][List]: Note this may store with a mixed type.
    protected List<List<T>> editingDataList = new List<List<T>>();

    protected int selectingCatIndex = -1;
    protected int selectingIDIndex = -1;

    //Use this built-in detail view?
    //Disable this to implement yourown detail view.
    //This allow you to implement an external detail view even multi-editing utility window.
    protected bool useBuiltInDetailView = true;
    protected IDCollectionEditor.Mode mode = IDCollectionEditor.Mode.RadioSelector;

    //Hide the ID View?
    protected bool minimizeIDView = false;

    //The string user input for filter.
    private string filterString = "";
    private int selectingFilteredIDIndex = -1;
    private List<string> filteredIDs = new List<string>();

    private string newIDStr = "";
    private int renameingCatIndex = -1;
    private int renameingIDIndex = -1;
    private int movingCat = -1;
    private int movingIDIndex = -1;

    private bool editMode = false;

    public const string EDITMODE_PLAYERPREFS_POSTFIX = "_editMode";

    //For foldOut all cats.
    public List<bool> foldOutCats = new List<bool>();
    public const string EDITMODE_FOLDOUTCATS_POSTFIX = "_foldOutCats";

    virtual protected void OnEnable() {
        if (editingIDCollection != null) {
            //Select the first cat if possible.
            if (selectingCatIndex == -1 && editingIDCollection.IDIndexes.Count > 0) {
                selectingCatIndex = 0;
            }
        }
        //Load state.
        LoadPref();
        //Listen to play mode state changed;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void LoadPref() {
        //Try restore the edit mode option.
        editMode = Util.IntToBool(PlayerPrefs.GetInt(this.GetType().Name + EDITMODE_PLAYERPREFS_POSTFIX));
        //Try restore the foldOutCats.
        foldOutCats = new List<bool>(PlayerPrefsX.GetBoolArray(this.GetType().Name + EDITMODE_FOLDOUTCATS_POSTFIX));
        //Try restore the scrollPos.
        scrollPos = PlayerPrefsX.GetVector2(this.GetType().Name + EDITMODE_SCROLLPOS_POSTFIX, scrollPos);
        //Try restore ID view width.
        builtInDetailViewIDViewWidth = PlayerPrefs.GetFloat(this.GetType().Name + BUILTINDETAIL_IDVIEW_WIDTH_POSTFIX, builtInDetailViewIDViewWidth);
    }

    virtual protected void OnDestroy() {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        //Save state.
        SavePref();
    }

    private void SavePref() {
        //Save the edit mode option.
        PlayerPrefs.SetInt(this.GetType().Name + EDITMODE_PLAYERPREFS_POSTFIX, Util.BoolToInt(editMode));
        //Save the foldOutCats.
        PlayerPrefsX.SetBoolArray(this.GetType().Name + EDITMODE_FOLDOUTCATS_POSTFIX, foldOutCats.ToArray());
        //Save the scrollPos.
        PlayerPrefsX.SetVector2(this.GetType().Name + EDITMODE_SCROLLPOS_POSTFIX, scrollPos);
        //Save ID view width.
        PlayerPrefs.SetFloat(this.GetType().Name + BUILTINDETAIL_IDVIEW_WIDTH_POSTFIX, builtInDetailViewIDViewWidth);
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingEditMode) {
            SavePref();
        }
    }

    override public void OnGUI() {
        base.OnGUI();

        if (editingIDCollection != null) {
            EditorGUILayout.BeginHorizontal();
            {
                //Left view
                if (useBuiltInDetailView) {
                    if (!minimizeIDView) {
                        EditorGUILayout.BeginVertical("FrameBox", GUILayout.Width(builtInDetailViewIDViewWidth));
                    } else {
                        EditorGUILayout.BeginVertical();
                    }
                } else {
                    EditorGUILayout.BeginVertical("FrameBox", GUILayout.ExpandWidth(true));
                }

                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        //Draw an editor title block. Customized by derived classes.
                        if (!minimizeIDView) {
                            DrawTitle();
                        }

                        if (useBuiltInDetailView) {
                            //IDView setting buttons.
                            if (!minimizeIDView) {
                                EditorGUILayout.BeginHorizontal("HelpBox", GUILayout.Width(24.0f), GUILayout.Height(24.0f));
                                {
                                    GUILayout.FlexibleSpace();
                                    EditorGUILayout.BeginVertical();
                                    {
                                        GUILayout.FlexibleSpace();
                                        if (GUILayout.Button(EditorGUIUtility.IconContent("d_RectTransformBlueprint"), EditorStyles.label)) {
                                            PopupWindow.Show(builtInDetailViewIDViewPopupRect, new FloatSliderPopup("ID View Width", builtInDetailViewIDViewWidth, 270.0f, 800.0f, (width) => {
                                                builtInDetailViewIDViewWidth = width;
                                                Repaint();
                                            }, (width) => {
                                                builtInDetailViewIDViewWidth = width;
                                                Repaint();
                                            }));
                                        }
                                        GUILayout.FlexibleSpace();
                                    }
                                    EditorGUILayout.EndVertical();
                                    GUILayout.FlexibleSpace();
                                }
                                EditorGUILayout.EndHorizontal();
                                if (Event.current.type == EventType.Repaint) builtInDetailViewIDViewPopupRect = GUILayoutUtility.GetLastRect();

                            }

                            //Toggle
                            if (!minimizeIDView) {
                                EditorGUILayout.BeginHorizontal("HelpBox", GUILayout.Width(24.0f), GUILayout.Height(24.0f));
                                {
                                    GUILayout.FlexibleSpace();
                                    EditorGUILayout.BeginVertical();
                                    {
                                        GUILayout.FlexibleSpace();
                                        if (GUILayout.Button(EditorGUIUtility.IconContent("scrollleft"), EditorStyles.label)) {
                                            minimizeIDView = !minimizeIDView;
                                        }
                                        GUILayout.FlexibleSpace();
                                    }
                                    EditorGUILayout.EndVertical();
                                    GUILayout.FlexibleSpace();
                                }
                                EditorGUILayout.EndHorizontal();
                            } else {
                                if (GUILayout.Button(EditorGUIUtility.IconContent("scrollright"), "HelpBox", GUILayout.Width(24.0f), GUILayout.ExpandHeight(true))) {
                                    minimizeIDView = !minimizeIDView;
                                }
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (!minimizeIDView) {
                        //IDCollection editing interface.
                        IDCollectionEditor.Draw(mode, editingIDCollection, ref selectingCatIndex, ref selectingIDIndex, ref newIDStr
                        , ref renameingCatIndex, ref renameingIDIndex, ref movingCat, ref movingIDIndex, ref editMode, foldOutCats
                        , OnNewCat, OnPreDeleteCat, OnPostDeleteCat
                        , OnInsertNewID, OnInsertNewIDs
                        , OnPreDeleteID, OnPreDeleteIDs
                        , OnPostDeleteID, OnPostDeleteIDs
                        , OnMoveIDUp, OnMoveIDDown, OnMoveCatUp, OnMoveCatDown
                        , OnMoveToCatIndex, OnDuplicateID, OnSelectIndex, OnRenameID, OnVerifyIDs, OnSave, OnRevert, ref scrollPos, OnGetIDComment
                        , ref filterString, ref selectingFilteredIDIndex, filteredIDs);

                        //Draw additional buttons after the IDCollection editor interface. Customized by derived classes.
                        DrawAdditionUtilityButtons();
                    }
                }
                EditorGUILayout.EndVertical();

                //Detail view
                if (useBuiltInDetailView) {
                    if (selectingCatIndex != -1 && selectingIDIndex != -1 && selectingCatIndex < editingDataList.Count && selectingIDIndex < editingDataList[selectingCatIndex].Count && editingDataList[selectingCatIndex][selectingIDIndex] != null) {
                        EditorGUILayout.BeginVertical();
                        {
                            detailScrollPos = EditorGUILayout.BeginScrollView(detailScrollPos, "FrameBox", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            {
                                //The editing detail will be drawen here...
                                //Customized by derived classes.
                                DrawDetail(editingDataList[selectingCatIndex][selectingIDIndex]);
                            }
                            EditorGUILayout.EndScrollView();

                            //Draw status bar.
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayoutPlus.LabelField("ID: [", false);
                                EditorGUILayoutPlus.LabelField(editingDataList[selectingCatIndex][selectingIDIndex].ID);
                                EditorGUILayoutPlus.LabelField("]", false);

                                DrawExtraStatusBarContent(editingDataList[selectingCatIndex][selectingIDIndex]);
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayoutPlus.HorizontalLine();
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            //Hot key for quick switching cat/IDIndex.
            switch (Event.current.type) {
                case EventType.KeyDown:

                    if (Event.current.keyCode == KeyCode.M) {
                        if (ctrlDown) {
                            //Toggle EditMode.
                            editMode = !editMode;
                            Repaint();
                        }
                    }

                    if (Event.current.keyCode == KeyCode.L) {
                        if (ctrlDown) {
                            ToggleFoldState();
                            Repaint();
                        }
                    }

                    if (Event.current.keyCode == KeyCode.T) {
                        if (ctrlDown && editMode) {
                            //Add a new category to collection.
                            editingIDCollection.NewIDIndex();
                            OnNewCat();
                            Repaint();
                        }
                    }

                    if (Event.current.keyCode == KeyCode.F12) {
                        if (ctrlDown) {
                            //Save data.
                            OnSave();
                        }
                    }

                    if (Event.current.keyCode == KeyCode.F8) {
                        if (ctrlDown) {
                            if (EditorUtility.DisplayDialog("Beep!", "You are about to revert all your works! Are you sure?", "Yes!", "Hell No!")) {
                                //Revert data.
                                OnRevert();
                            }
                        }
                    }

                    break;
            }
        }
    }

    public T GetSelectingData() {
        if (selectingCatIndex != -1 && selectingIDIndex != -1 && selectingCatIndex < editingDataList.Count && selectingIDIndex < editingDataList[selectingCatIndex].Count) {
            return editingDataList[selectingCatIndex][selectingIDIndex];
        }
        return null;
    }

    public void ToggleFoldState() {
        if (foldOutCats.Count > 0) {
            bool currentFoldState = foldOutCats[0];
            for (int i = 0; i < foldOutCats.Count; i++) {
                foldOutCats[i] = !currentFoldState;
            }
        }
    }

    public void PreviousCat() {
        if (selectingCatIndex > 0) {
            selectingCatIndex--;
            //Fix selecting IDIndex.
            FixSelectingIDIndex();
            OnSelectIndex(selectingCatIndex, selectingIDIndex);
        }
    }

    public void NextCat() {
        if (editingIDCollection != null) {
            if (selectingCatIndex < editingIDCollection.IDIndexes.Count - 1) {
                selectingCatIndex++;
                FixSelectingIDIndex();
                OnSelectIndex(selectingCatIndex, selectingIDIndex);
            }
        }
    }

    private void FixSelectingIDIndex() {
        if (editingIDCollection != null) {
            if (selectingCatIndex >= 0 && selectingCatIndex < editingIDCollection.IDIndexes.Count) {
                if (selectingIDIndex >= editingIDCollection.IDIndexes[selectingCatIndex].ids.Count) {
                    selectingIDIndex = editingIDCollection.IDIndexes[selectingCatIndex].ids.Count - 1;
                }
            }
        } else {
            selectingIDIndex = -1;
            OnEmptySelected();
        }
    }

    public void PreviousIDIndex() {
        if (editingIDCollection != null) {
            if (selectingCatIndex >= 0 && selectingCatIndex < editingIDCollection.IDIndexes.Count) {
                if (selectingIDIndex > 0) {
                    selectingIDIndex--;
                    OnSelectIndex(selectingCatIndex, selectingIDIndex);
                }
            }
        }
    }

    public void NextIDIndex() {
        if (editingIDCollection != null) {
            if (selectingCatIndex >= 0 && selectingCatIndex < editingIDCollection.IDIndexes.Count) {
                if (selectingIDIndex < editingIDCollection.IDIndexes[selectingCatIndex].ids.Count - 1) {
                    selectingIDIndex++;
                    OnSelectIndex(selectingCatIndex, selectingIDIndex);
                }
            }
        }
    }

    //Set the menu function mode.
    protected void SetMode(IDCollectionEditor.Mode modeInput) {
        mode = modeInput;
        if (mode == IDCollectionEditor.Mode.ButtonMenu) {
            selectingIDIndex = -1;
            OnEmptySelected();
        }
    }

    private void OnNewCat() {
        editingDataList.Add(new List<T>());
    }

    private void OnPreDeleteCat(int catIndex) {
        if (catIndex >= 0 && catIndex < editingIDCollection.IDIndexes.Count) {
            OnDataNeeded(editingIDCollection.IDIndexes[catIndex].ids);
        }
    }

    private void OnPostDeleteCat(int catIndex) {
        if (catIndex >= 0 && catIndex < editingDataList.Count) {
            for (int i = 0; i < editingDataList[catIndex].Count; i++) {
                if (editingDataList[catIndex][i] != null) {
                    editingDataList[catIndex][i].CleanUpFiles();
                } else {
                    Debug.LogWarning("Oops! Null Data! Cannot clean up files!");
                }
            }
            editingDataList.RemoveAt(catIndex);
        }
        Save();
        AssetDatabase.Refresh();
        Repaint();
    }

    private void OnInsertNewID(int catIndex, int index, string newID) {
        T newT = new T();
        newT.ID = newID;
        editingDataList[catIndex].Insert(index, newT);
        Repaint();
        Focus();
        //Switch to new data right away.
        selectingCatIndex = catIndex;
        selectingIDIndex = index;
        //Select the index again ley the detail view be refresh.
        OnSelectIndex(catIndex, index);
    }

    private void OnInsertNewIDs(int catIndex, int index, List<string> newIDs) {
        List<T> newDatas = new List<T>();
        for (int i = 0; i < newIDs.Count; i++) {
            T newT = new T();
            newT.ID = newIDs[i];
            newDatas.Add(newT);
        }
        editingDataList[catIndex].InsertRange(index, newDatas);
        Repaint();
        Focus();
        //Switch to new data right away.
        selectingCatIndex = catIndex;
        selectingIDIndex = index;
        //Select the index again ley the detail view be refresh.
        OnSelectIndex(catIndex, index);
    }

    private void OnPreDeleteID(int catIndex, int index) {
        OnDataNeeded(catIndex, index);
    }

    private void OnPreDeleteIDs(List<string> IDs) {
        OnDataNeeded(IDs);
        Focus();
    }

    private void OnPostDeleteID(int catIndex, int index) {
        if (editingDataList[catIndex][index] != null) {
            editingDataList[catIndex][index].CleanUpFiles();
            editingDataList[catIndex].RemoveAt(index);
            Save();
            AssetDatabase.Refresh();

            //Note that delete a data may cause selecting index out of continer range. So we need to check it.
            if (selectingCatIndex >= 0 && selectingCatIndex < editingDataList.Count) {
                //Legal Cat
                if (selectingIDIndex >= editingDataList[selectingCatIndex].Count) {
                    //No longer available...(This will clear the detail view)
                    selectingIDIndex = -1;
                    OnEmptySelected();
                }
            }

            Repaint();
            Focus();
        } else {
            Debug.LogWarning("Cannot delete ID. Data not prepared for index: [" + catIndex + "] | [" + index + "]");
        }
    }

    private void OnPostDeleteIDs(List<string> IDs, int selectingCat, int selectnigIndex) {
        //Scan and remove all IDs.
        for (int cat = 0; cat < editingDataList.Count; cat++) {
            for (int index = editingDataList[cat].Count - 1; index >= 0; index--) {
                if (editingDataList[cat][index] != null && IDs.Contains(editingDataList[cat][index].ID)) {
                    editingDataList[cat][index].CleanUpFiles();
                    editingDataList[cat].RemoveAt(index);
                }
            }
        }
        Save();
        AssetDatabase.Refresh();

        //Note that delete a data may cause selecting index out of continer range. So we need to check it.
        if (selectingCatIndex >= 0 && selectingCatIndex < editingDataList.Count) {
            //Legal Cat
            if (selectingIDIndex >= editingDataList[selectingCatIndex].Count) {
                //No longer available...(This will clear the detail view)
                selectingIDIndex = -1;
                OnEmptySelected();
            }
        }

        Repaint();
        Focus();
    }

    private void OnMoveIDUp(int catIndex, int index) {
        T moving = editingDataList[catIndex][index];
        editingDataList[catIndex].RemoveAt(index);
        editingDataList[catIndex].Insert(index - 1, moving);
    }

    private void OnMoveIDDown(int catIndex, int index) {
        T moving = editingDataList[catIndex][index];
        editingDataList[catIndex].RemoveAt(index);
        editingDataList[catIndex].Insert(index + 1, moving);
    }

    private void OnMoveCatUp(int catIndex) {
        List<T> movingList = editingDataList[catIndex];
        editingDataList.RemoveAt(catIndex);
        editingDataList.Insert(catIndex - 1, movingList);
    }

    private void OnMoveCatDown(int catIndex) {
        List<T> movingList = editingDataList[catIndex];
        editingDataList.RemoveAt(catIndex);
        editingDataList.Insert(catIndex + 1, movingList);
    }

    private void OnMoveToCat(int targetCatIndex) {
        T moving = editingDataList[movingCat][movingIDIndex];
        editingDataList[movingCat].Remove(moving);
        editingDataList[targetCatIndex].Add(moving);
        Repaint();
    }

    private void OnMoveToCatIndex(int targetCat, int index) {
        T moving = editingDataList[movingCat][movingIDIndex];
        editingDataList[movingCat].Remove(moving);
        editingDataList[targetCat].Insert(index, moving);
        Repaint();
    }

    private void OnDuplicateID(int catIndex, int index, string newID) {
        OnDataNeeded(catIndex, index);
        if (editingDataList[catIndex][index] != null) {
            T duplicate = editingDataList[catIndex][index].Clone();
            duplicate.ID = newID;

            //Add this new duplicated data.
            editingDataList[catIndex].Insert(index + 1, duplicate);
        } else {
            Debug.LogWarning("Cannot duplicate ID. Data not prepared for index: [" + catIndex + "] | [" + index + "]");
        }
    }

    //This will refresh editing ID.
    private void OnSelectIndex(int catIndex, int index) {
        if (catIndex >= 0 && catIndex < editingIDCollection.IDIndexes.Count) {
            if (index >= 0 && index < editingIDCollection.IDIndexes[catIndex].ids.Count) {
                OnDataNeeded(catIndex, index);
                OnIDSelected(catIndex, index, editingIDCollection.IDIndexes[catIndex].ids[index]);
            }
        }
    }

    private void OnVerifyIDs() {
        //Check copy buffer.
        List<string> pastedMatrix = Util.StringSplitByNewLineTab(EditorGUIUtility.systemCopyBuffer);
        int foundNotExist = 0;
        //Check all IDs and print the result.
        for (int i = 0; i < pastedMatrix.Count; i++) {
            if (!editingIDCollection.IDExist(pastedMatrix[i])) {
                foundNotExist++;
                Debug.LogWarning("[" + pastedMatrix[i] + "] does not exist.");
            }
        }
        Debug.Log("Verify finished. [" + foundNotExist + "] IDs not exist in the current data.");
    }

    private void OnSave() {
        Save();
        ShowNotification(new GUIContent("Saved!"));
        Repaint();
    }

    private void OnRevert() {
        Revert();
        ShowNotification(new GUIContent("Reverted!"));
        Repaint();
        Focus();
    }

    private void OnRenameID(string newID) {
        OnDataNeeded(renameingCatIndex, renameingIDIndex);
        if (editingDataList[renameingCatIndex][renameingIDIndex] != null) {
            if (editingIDCollection.IsLegalToAddID(newID)) {
                OnIDRenamed(renameingCatIndex, renameingIDIndex, newID);

                //Rename the ID.
                //Cleanup file with old id.
                editingDataList[renameingCatIndex][renameingIDIndex].CleanUpFiles();

                //Save file with new id.
                editingDataList[renameingCatIndex][renameingIDIndex].ID = newID;
                editingDataList[renameingCatIndex][renameingIDIndex].Save();

                //Change id.
                editingIDCollection.IDIndexes[renameingCatIndex].ids[renameingIDIndex] = newID;

                //Save
                Save();

                AssetDatabase.Refresh();

                Focus();
            } else {
                Debug.LogWarning("Illegal ID! Cannot contain space character or ID already exist. ");
            }
        } else {
            Debug.LogWarning("Cannot rename ID. Data not prepared for index: [" + renameingCatIndex + "] | [" + renameingIDIndex + "]");
        }
    }

    // -------------------------------------------------------------------------------

    /// <summary>
    /// Implement this to draw a title bar at top left of the window.
    /// </summary>
    abstract protected void DrawTitle();

    /// <summary>
    /// Implement this to draw the detail editing interface.
    /// </summary>
    virtual protected void DrawDetail(T t) {

    }

    /// <summary>
    /// Implement this to draw extra interface on the status bar.
    /// </summary>
    virtual protected void DrawExtraStatusBarContent(T t) {

    }

    /// <summary>
    /// Implement this to draw additional utility buttons at the bottom left.
    /// </summary>
    virtual protected void DrawAdditionUtilityButtons() {

    }

    /// <summary>
    /// Implement your save functions for IDCollection here.
    /// </summary>
    abstract protected void SaveIDCollection();

    /// <summary>
    /// Implement how you save all data here.
    /// </summary>
    abstract protected void Save();

    /// <summary>
    /// Implement how you revert all data here.
    /// </summary>
    abstract protected void Revert();

    /// <summary>
    /// Called when an ID has been selected.
    /// </summary>
    /// <param name="selectID"></param>
    virtual protected void OnIDSelected(int catIndex, int index, string selectID) {

    }

    /// <summary>
    /// Called when nothing is selected.virtual (Deselect on editor interface)
    /// This could be useful when you don't use the built-in detail view.
    /// </summary>
    virtual protected void OnEmptySelected() {

    }

    /// <summary>
    /// Called when an ID has been renamed.
    /// </summary>
    /// <param name="selectID"></param>
    virtual protected void OnIDRenamed(int catIndex, int index, string newID) {

    }

    /// <summary>
    /// Called when a data enery is been touched (clone or CleanUp).
    /// In case you need dynamic prepare the data for these operations.
    /// </summary>
    /// <param name="selectID"></param>
    virtual protected void OnDataNeeded(int catIndex, int index) {

    }

    /// <summary>
    /// Called when a data enery is been touched (clone or CleanUp).
    /// In case you need dynamic prepare the data for these operations.
    /// </summary>
    /// <param name="IDs"></param>
    virtual protected void OnDataNeeded(List<string> IDs) {

    }

    /// <summary>
    /// Implement this if you want to leave comment after each ID in the ID list.
    /// </summary>
    /// <param name="ID"></param>
    virtual protected string OnGetIDComment(string ID) {
        return "";
    }

}

#endif