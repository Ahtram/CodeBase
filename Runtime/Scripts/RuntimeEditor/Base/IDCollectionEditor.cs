#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A base class for IDCollection serious editors.
/// To use this class: Derive from it for making individual window of different IDIndex object. (There's a tricky reason for use the deriving here:
///  we need each editor instance contains a copy of IDCollection object.)
/// Or: Use the Draw function directly for embed the editor drawing UI. In this case you need to store all necessary state outside the function.
/// </summary>
abstract public class IDCollectionEditor : UniEditorWindow {

    public enum Mode {
        RadioSelector,  //Select with radio button
        ButtonMenu      //Display in a button list. No current selecting stuff logicly. Users shouls implement the external Utility window for editing data.
    };

    public Action onNewCat = null;
    //<catIndex>
    public Action<int> onPreDeleteCat = null;
    //<catIndex>
    public Action<int> onPostDeleteCat = null;
    //<catIndex, IDIndexInCat, ID>
    public Action<int, int, string> onInsertNewID = null;
    //<catIndex, IDIndexInCat, IDs>
    public Action<int, int, List<string>> onInsertNewIDs = null;
    //<catIndex, IDIndexInCat>
    public Action<int, int> onPreDeleteID = null;
    //<IDs>
    public Action<List<string>> onPreDeleteIDs = null;
    //<catIndex, IDIndexInCat>
    public Action<int, int> onPostDeleteID = null;
    //<IDs>
    public Action<List<string>, int, int> onPostDeleteIDs = null;
    //<catIndex, IDIndexInCat>
    public Action<int, int> onMoveIDUp = null;
    //<catIndex, IDIndexInCat>
    public Action<int, int> onMoveIDDown = null;
    //<catIndex>
    public Action<int> onMoveCatUp = null;
    //<catIndex>
    public Action<int> onMoveCatDown = null;
    //<targetCat>
    public Action<int> onMoveToCat = null;
    //<selectingCat, selectingIndex, newID>
    public Action<int, int, string> onDuplicateID = null;
    //<catIndex, IDIndexInCat>
    public Action<int, int> onSelectIndex = null;
    //<catIndex, IDIndexInCat, new ID>
    public Action<string> onRenameID = null;
    public Action onVerifyIDs = null;
    public Action onSave = null;
    public Action onRevert = null;

    public Func<string, string> getIDComment = null;

    public IDCollection editingIDCollection = null;
    private Mode mode = Mode.RadioSelector;

    private Vector2 scrollPos = Vector2.zero;
    public const string EDITMODE_SCROLLPOS_POSTFIX = "_scrollPos";

    //The string user input for filter.
    private string filterString = "";
    private int selectingFilteredIDIndex = -1;
    private List<string> filteredIDs = new List<string>();

    private int selectingCatIndex = -1;
    private int selectingIDIndex = -1;
    private string newIDStr = "";

    private int renameingCatIndex = -1;
    private int renameingIDIndex = -1;
    private int movingCatIndex = -1;
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
        //Try restore the edit mode option.
        editMode = Util.IntToBool(PlayerPrefs.GetInt(this.GetType().Name + EDITMODE_PLAYERPREFS_POSTFIX));
        //Try restore the foldOutCats.
        foldOutCats = new List<bool>(PlayerPrefsX.GetBoolArray(this.GetType().Name + EDITMODE_FOLDOUTCATS_POSTFIX));
        //Try restore the scrollPos.
        scrollPos = PlayerPrefsX.GetVector2(this.GetType().Name + EDITMODE_SCROLLPOS_POSTFIX, scrollPos);
    }

    virtual protected void OnDestroy() {
        //Save the edit mode option.
        PlayerPrefs.SetInt(this.GetType().Name + EDITMODE_PLAYERPREFS_POSTFIX, Util.BoolToInt(editMode));
        //Save the foldOutCats.
        PlayerPrefsX.SetBoolArray(this.GetType().Name + EDITMODE_FOLDOUTCATS_POSTFIX, foldOutCats.ToArray());
        //Save the scrollPos.
        PlayerPrefsX.SetVector2(this.GetType().Name + EDITMODE_SCROLLPOS_POSTFIX, scrollPos);
    }

    override public void OnGUI() {
        base.OnGUI();
        if (editingIDCollection != null) {
            Draw(mode, editingIDCollection, ref selectingCatIndex, ref selectingIDIndex, ref newIDStr
            , ref renameingCatIndex, ref renameingIDIndex, ref movingCatIndex, ref movingIDIndex, ref editMode, foldOutCats
            , onNewCat, onPreDeleteCat, onPostDeleteCat
            , onInsertNewID, onInsertNewIDs
            , onPreDeleteID, onPreDeleteIDs
            , onPostDeleteID, onPostDeleteIDs
            , onMoveIDUp, onMoveIDDown
            , onMoveCatUp, onMoveCatDown, onMoveToCat, onDuplicateID, onSelectIndex, onRenameID, onVerifyIDs, onSave, onRevert, ref scrollPos, getIDComment
            , ref filterString, ref selectingFilteredIDIndex, filteredIDs);

            //Hot key for quick switching cat/IDIndex.
            switch (Event.current.type) {
                case EventType.KeyDown:
                    // if (Event.current.keyCode == KeyCode.LeftArrow) {
                    //     if (ctrlDown) {
                    //         PreviousCat();
                    //         Repaint();
                    //     }
                    // }
                    // if (Event.current.keyCode == KeyCode.RightArrow) {
                    //     if (ctrlDown) {
                    //         NextCat();
                    //         Repaint();
                    //     }
                    // }

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
                            onNewCat?.Invoke();
                            Repaint();
                        }
                    }

                    if (Event.current.keyCode == KeyCode.F12) {
                        if (ctrlDown) {
                            //Save data.
                            onSave?.Invoke();
                        }
                    }

                    if (Event.current.keyCode == KeyCode.F11) {
                        if (ctrlDown) {
                            onVerifyIDs?.Invoke();
                        }
                    }

                    if (Event.current.keyCode == KeyCode.F8) {
                        if (ctrlDown) {
                            if (EditorUtility.DisplayDialog("Beep!", "You are about to revert all your works! Are you sure?", "Yes!", "Hell No!")) {
                                //Revert data.
                                onRevert?.Invoke();
                            }
                        }
                    }

                    // if (mode == Mode.RadioSelector) {
                    //     if (Event.current.keyCode == KeyCode.UpArrow) {
                    //         if (ctrlDown) {
                    //             PreviousIDIndex();
                    //             Repaint();
                    //         }
                    //     }
                    //     if (Event.current.keyCode == KeyCode.DownArrow) {
                    //         if (ctrlDown) {
                    //             NextIDIndex();
                    //             Repaint();
                    //         }
                    //     }
                    // }

                    break;
            }
        }
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
            onSelectIndex?.Invoke(selectingCatIndex, selectingIDIndex);
        }
    }

    public void NextCat() {
        if (editingIDCollection != null) {
            if (selectingCatIndex < editingIDCollection.IDIndexes.Count - 1) {
                selectingCatIndex++;
                FixSelectingIDIndex();
                onSelectIndex?.Invoke(selectingCatIndex, selectingIDIndex);
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
        }
    }

    public void PreviousIDIndex() {
        if (editingIDCollection != null) {
            if (selectingCatIndex >= 0 && selectingCatIndex < editingIDCollection.IDIndexes.Count) {
                if (selectingIDIndex > 0) {
                    selectingIDIndex--;
                    onSelectIndex?.Invoke(selectingCatIndex, selectingIDIndex);
                }
            }
        }
    }

    public void NextIDIndex() {
        if (editingIDCollection != null) {
            if (selectingCatIndex >= 0 && selectingCatIndex < editingIDCollection.IDIndexes.Count) {
                if (selectingIDIndex < editingIDCollection.IDIndexes[selectingCatIndex].ids.Count - 1) {
                    selectingIDIndex++;
                    onSelectIndex?.Invoke(selectingCatIndex, selectingIDIndex);
                }
            }
        }
    }

    //Set the menu function mode.
    protected void SetMode(Mode modeInput) {
        mode = modeInput;
        if (mode == Mode.ButtonMenu) {
            selectingIDIndex = -1;
        }
    }

    public int SelectingCatIndex() {
        return selectingCatIndex;
    }

    public int SelectingIDIndex() {
        return selectingIDIndex;
    }

    public int RenamingCatIndex() {
        return renameingCatIndex;
    }

    public int RenamingIDIndex() {
        return renameingIDIndex;
    }

    public int MovingCatIndex() {
        return movingCatIndex;
    }

    public int MovingIDIndex() {
        return movingIDIndex;
    }

    public void UnSelectID() {
        selectingIDIndex = -1;
    }

    //Static UI Drawer. Can be used in any editor class if needed.
    static public void Draw(Mode mode, IDCollection idCollection, ref int selectingCatIndex, ref int selectingIndex, ref string newIDStr
    , ref int renameingCatIndex, ref int renameingIDIndex, ref int movingCatIndex, ref int movingIDIndex, ref bool editMode, List<bool> foldOutCats
    , Action onNewCat, Action<int> onPreDeleteCat, Action<int> onPostDeleteCat
    , Action<int, int, string> onInsertNewID, Action<int, int, List<string>> onInsertNewIDs
    , Action<int, int> onPreDeleteID, Action<List<string>> onPreDeleteIDs
    , Action<int, int> onPostDeleteID, Action<List<string>, int, int> onPostDeleteIDs
    , Action<int, int> onMoveIDUp, Action<int, int> onMoveIDDown
    , Action<int> onMoveCatUp, Action<int> onMoveCatDown, Action<int> onMoveToCat, Action<int, int, string> onDuplicateID, Action<int, int> onSelectIndex
    , Action<string> onRenameID, Action onVerifyIDs, Action onSave, Action onRevert, ref Vector2 vScollPos, Func<string, string> getIDComment
    , ref string filterString, ref int selectingFilteredIDIndex, List<string> filteredIDs) {

        //For solve a minor color consistancy issue...
        GUI.color = Color.white;

        EditorGUILayout.BeginVertical();
        {
            //Filter
            EditorGUILayout.BeginHorizontal("HelpBox");
            {
                EditorGUILayout.LabelField("Filter", GUILayout.Width(33.0f));
                GUI.color = ColorPlus.LightYellow;
                string newFilterString = EditorGUILayout.TextField(filterString);
                GUI.color = Color.white;
                if (!string.Equals(newFilterString, filterString)) {
                    filterString = newFilterString;
                    //Got a new filter string. Refresh the listed IDs.
                    RefreshFilteredIDs(idCollection, filterString, filteredIDs);
                    selectingFilteredIDIndex = -1;
                }

                GUI.color = ColorPlus.FloralWhite;
                if (GUILayout.Button("Clear", EditorStyles.miniButton, GUILayout.Width(43.0f))) {
                    filterString = "";
                    RefreshFilteredIDs(idCollection, filterString, filteredIDs);
                    selectingFilteredIDIndex = -1;
                    GUI.FocusControl("");
                }
                GUI.color = Color.white;
            }
            EditorGUILayout.EndHorizontal();

            //Draw filtered IDs or normal IDCollection?
            if (!string.IsNullOrEmpty(filterString)) {
                //Filtered IDs.
                EditorGUILayout.BeginVertical("HelpBox");
                {
                    vScollPos = EditorGUILayout.BeginScrollView(vScollPos);
                    {
                        int newSelectingFilteredIDIndex = -1;
                        //Display all ids in selecting cat.
                        newSelectingFilteredIDIndex = EditorGUILayoutPlus.Tabs(filteredIDs.ToArray(), selectingFilteredIDIndex, EditorStyles.radioButton);

                        if (newSelectingFilteredIDIndex != -1 && newSelectingFilteredIDIndex != selectingFilteredIDIndex) {
                            selectingFilteredIDIndex = newSelectingFilteredIDIndex;
                            //Query the cat and index of this ID.
                            int catIndex = -1;
                            int idIndex = -1;
                            if (idCollection.GetIDIndex(filteredIDs[selectingFilteredIDIndex], ref catIndex, ref idIndex)) {
                                selectingCatIndex = catIndex;
                                selectingIndex = idIndex;
                                onSelectIndex?.Invoke(catIndex, idIndex);
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

            } else {
                //Normal IDCollection edit interface.
                switch (mode) {
                    case Mode.RadioSelector:
                        DrawRadioSelector(idCollection, ref selectingCatIndex, ref selectingIndex, ref newIDStr
                        , ref renameingCatIndex, ref renameingIDIndex, ref movingCatIndex, ref movingIDIndex, ref editMode, foldOutCats
                        , onNewCat, onPreDeleteCat, onPostDeleteCat
                        , onInsertNewID, onInsertNewIDs
                        , onPreDeleteID, onPreDeleteIDs
                        , onPostDeleteID, onPostDeleteIDs
                        , onMoveIDUp, onMoveIDDown
                        , onMoveCatUp, onMoveCatDown, onMoveToCat, onDuplicateID, onSelectIndex
                        , onRenameID, onVerifyIDs, onSave, onRevert, ref vScollPos, getIDComment);
                        break;
                    case Mode.ButtonMenu:
                        DrawButtonMenu(idCollection, ref selectingCatIndex, ref selectingIndex, ref newIDStr
                        , ref renameingCatIndex, ref renameingIDIndex, ref movingCatIndex, ref movingIDIndex, ref editMode, foldOutCats
                        , onNewCat, onPreDeleteCat, onPostDeleteCat
                        , onInsertNewID, onInsertNewIDs
                        , onPreDeleteID, onPreDeleteIDs
                        , onPostDeleteID, onPostDeleteIDs
                        , onMoveIDUp, onMoveIDDown
                        , onMoveCatUp, onMoveCatDown, onMoveToCat, onDuplicateID, onSelectIndex
                        , onRenameID, onVerifyIDs, onSave, onRevert, ref vScollPos, getIDComment);
                        break;
                }

                EditorGUILayout.BeginHorizontal();
                {
                    GUI.color = ColorPlus.CornflowerBlue;
                    editMode = GUILayout.Toggle(editMode, "Edit Mode (M)", EditorStyles.miniButtonLeft);
                    GUI.color = ColorPlus.Golden;
                    if (GUILayout.Button("Expand/Collapse (L)", EditorStyles.miniButtonRight)) {
                        if (foldOutCats.Count > 0) {
                            bool currentFoldState = foldOutCats[0];
                            for (int i = 0; i < foldOutCats.Count; i++) {
                                foldOutCats[i] = !currentFoldState;
                            }
                        }
                    }
                    GUI.color = Color.white;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayoutPlus.HorizontalLine();

                EditorGUILayout.BeginHorizontal();
                {
                    GUI.color = ColorPlus.PaleTurquoise;
                    if (GUILayout.Button("Verify Pasted IDs (F11)", EditorStyles.miniButton)) {
                        onVerifyIDs?.Invoke();
                    }
                    GUI.color = Color.white;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    GUI.color = ColorPlus.Fuchsia;
                    if (GUILayout.Button("Revert (F8)", EditorStyles.miniButtonLeft)) {
                        if (EditorUtility.DisplayDialog("Beep!", "You are about to revert all your works! Are you sure?", "Yes!", "Hell No!")) {
                            onRevert?.Invoke();
                        }
                    }
                    GUI.color = Color.green;
                    if (GUILayout.Button("Save (F12)", EditorStyles.miniButtonRight)) {
                        onSave?.Invoke();
                    }
                    GUI.color = Color.white;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
    }

    static private void RefreshFilteredIDs(IDCollection idCollection, string filterString, List<string> filteredIDs) {
        filteredIDs.Clear();

        if (string.IsNullOrEmpty(filterString)) {
            //Do nothing.
        } else {
            foreach (IDIndex idIndex in idCollection.IDIndexes) {
                foreach (string ID in idIndex.ids) {
                    if (ID.StartsWith(filterString, true, System.Globalization.CultureInfo.CurrentCulture)) {
                        filteredIDs.Add(ID);
                    }
                }
            }
        }
    }

    static private void DrawRadioSelector(IDCollection idCollection, ref int selectingCatIndex, ref int selectingIndex, ref string newIDStr
    , ref int renameingCatIndex, ref int renameingIDIndex, ref int movingCatIndex, ref int movingIDIndex, ref bool editMode, List<bool> foldOutCats
    , Action onNewCat, Action<int> onPreDeleteCat, Action<int> onPostDeleteCat
    , Action<int, int, string> onInsertNewID, Action<int, int, List<string>> onInsertNewIDs
    , Action<int, int> onPreDeleteID, Action<List<string>> onPreDeleteIDs
    , Action<int, int> onPostDeleteID, Action<List<string>, int, int> onPostDeleteIDs
    , Action<int, int> onMoveIDUp, Action<int, int> onMoveIDDown
    , Action<int> onMoveCatUp, Action<int> onMoveCatDown, Action<int> onMoveToCat, Action<int, int, string> onDuplicateID, Action<int, int> onSelectIndex
    , Action<string> onRenameID, Action onVerifyIDs, Action onSave, Action onRevert, ref Vector2 vScollPos, Func<string, string> getIDComment) {

        EditorGUILayout.BeginVertical("HelpBox", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        {
            //Check and resize the fold vars container.
            if (foldOutCats.Count != idCollection.IDIndexes.Count) {
                Util.Resize(foldOutCats, idCollection.IDIndexes.Count);
            }

            //All ids by cats.
            EditorGUILayout.BeginVertical();
            {
                vScollPos = EditorGUILayout.BeginScrollView(vScollPos);
                {
                    //Display all flags in selecting cat.
                    int newSelectingCatIndex = -1;
                    int newSelectingIndex = -1;

                    for (int c = 0; c < idCollection.IDIndexes.Count; c++) {
                        EditorGUILayout.BeginVertical();
                        {
                            //Foldout
                            EditorGUILayout.BeginHorizontal("SelectionRect");
                            {
                                foldOutCats[c] = EditorGUILayout.Foldout(foldOutCats[c], "[" + c + "](" + idCollection.IDIndexes[c].ids.Count + ") " + idCollection.IDIndexes[c].name, true);

                                if (editMode) {
                                    GUI.color = Color.yellow;
                                    if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(17.0f))) {
                                        int catIndex = c;
                                        int addTargetIndex = idCollection.IDIndexes[catIndex].ids.Count;
                                        StringEditorUtility.Open("Append New ID", "", null, (newID) => {
                                            if (idCollection.InsertNewID(catIndex, addTargetIndex, newID)) {
                                                // addTargetIndex += 1;
                                                onInsertNewID?.Invoke(catIndex, addTargetIndex, newID);
                                            } else {
                                                Debug.LogWarning("Append ID failed! [" + newID + "] [" + catIndex + "] [" + addTargetIndex + "]");
                                            }
                                        });
                                    }

                                    GUI.color = ColorPlus.SpringGreen;
                                    if (GUILayout.Button("BI", EditorStyles.miniButtonLeft, GUILayout.Width(27.0f))) {
                                        int selectingCatIndexTemp = c;
                                        int selectingIndexTemp = (selectingIndex >= 0) ? (selectingIndex) : (0);
                                        StringListEditorUtility.Open("Batch Insert", (newIDs) => {
                                            List<string> insertedIDs = new List<string>();
                                            for (int i = 0; i < newIDs.Count; i++) {
                                                if (idCollection.IsLegalToAddID(newIDs[i])) {
                                                    if (idCollection.InsertNewID(selectingCatIndexTemp, selectingIndexTemp + i, newIDs[i])) {
                                                        insertedIDs.Add(newIDs[i]);
                                                    } else {
                                                        Debug.LogWarning("Insert ID failed! [" + newIDs[i] + "]");
                                                    }
                                                    GUI.FocusControl(null);
                                                } else {
                                                    Debug.LogWarning("Illegal ID! [" + newIDs[i] + "]");
                                                }
                                            }
                                            onInsertNewIDs?.Invoke(selectingCatIndexTemp, selectingIndexTemp, insertedIDs);
                                        }
                                        );
                                    }
                                    GUI.color = Color.magenta;
                                    if (GUILayout.Button("BD", EditorStyles.miniButtonRight, GUILayout.Width(27.0f))) {
                                        int addTargetIndex = idCollection.IDIndexes[c].ids.Count;
                                        int selectingCatIndexTemp = c;
                                        int selectingIndexTemp = selectingIndex;
                                        StringListEditorUtility.Open("Batch Delete", (removingIDs) => {
                                            List<string> removedIDs = new List<string>();
                                            for (int i = 0; i < removingIDs.Count; i++) {
                                                if (idCollection.IDExist(removingIDs[i])) {
                                                    removedIDs.Add(removingIDs[i]);
                                                }
                                            }

                                            if (removedIDs.Count > 0) {
                                                onPreDeleteIDs?.Invoke(removedIDs);

                                                for (int i = 0; i < removedIDs.Count; i++) {
                                                    idCollection.RemoveID(removedIDs[i]);
                                                }

                                                onPostDeleteIDs?.Invoke(removedIDs, selectingCatIndexTemp, selectingIndexTemp);
                                            }
                                        }
                                        );
                                        selectingIndex = -1;
                                    }
                                    GUI.color = Color.white;

                                    //Edit cat name.
                                    GUI.color = Color.cyan;
                                    if (GUILayout.Button("e", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                                        int catIndex = c;
                                        StringEditorUtility.Open("Edit Category Name", idCollection.IDIndexes[c].name, (x) => idCollection.IDIndexes[catIndex].name = x);
                                    }
                                    GUI.color = ColorPlus.Lavender;
                                    if (GUILayout.Button("↑", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                        if (idCollection.MoveUpIDIndex(c)) {
                                            onMoveCatUp?.Invoke(c);
                                            if (c == selectingCatIndex) {
                                                selectingCatIndex -= 1;
                                            } else if (c == selectingCatIndex + 1) {
                                                selectingCatIndex += 1;
                                            }
                                        }
                                    }
                                    GUI.color = ColorPlus.Lavender;
                                    if (GUILayout.Button("↓", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                        if (idCollection.MoveDownIDIndex(c)) {
                                            onMoveCatDown?.Invoke(c);
                                            if (c == selectingCatIndex) {
                                                selectingCatIndex += 1;
                                            } else if (c == selectingCatIndex - 1) {
                                                selectingCatIndex -= 1;
                                            }
                                        }
                                    }
                                    GUI.color = Color.magenta;
                                    if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                                        if (idCollection.IDIndexExist(c)) {
                                            onPreDeleteCat?.Invoke(c);

                                            if (c == selectingCatIndex) {
                                                selectingCatIndex = -1;
                                                selectingIndex = -1;
                                            }

                                            idCollection.RemoveIDIndex(c);

                                            onPostDeleteCat?.Invoke(c);
                                            return;
                                        }
                                    }
                                    GUI.color = Color.white;
                                }
                            }
                            EditorGUILayout.EndHorizontal();

                            //Content.
                            if (foldOutCats[c]) {
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.BeginVertical();
                                    {
                                        //Display all ids in selecting cat.                            
                                        for (int i = 0; i < idCollection.IDIndexes[c].ids.Count; i++) {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                bool thisToggleIsOn = false;
                                                if (c == selectingCatIndex && i == selectingIndex) {
                                                    thisToggleIsOn = true;
                                                } else {
                                                    thisToggleIsOn = false;
                                                }

                                                string IDComment = (getIDComment == null) ? ("") : (getIDComment(idCollection.IDIndexes[c].ids[i]));
                                                if (GUILayout.Toggle(thisToggleIsOn, idCollection.IDIndexes[c].ids[i] + IDComment)) {
                                                    if (!thisToggleIsOn) {
                                                        newSelectingCatIndex = c;
                                                        newSelectingIndex = i;
                                                    }
                                                }

                                                if (editMode) {
                                                    GUI.color = Color.yellow;
                                                    if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                                                        int catIndex = c;
                                                        int addTargetIndex = i;
                                                        StringEditorUtility.Open("Append New ID", "", null, (newID) => {
                                                            if (idCollection.InsertNewID(catIndex, addTargetIndex, newID)) {
                                                                // addTargetIndex += 1;
                                                                onInsertNewID?.Invoke(catIndex, addTargetIndex, newID);
                                                            } else {
                                                                Debug.LogWarning("Append ID failed! [" + newID + "] [" + catIndex + "] [" + addTargetIndex + "]");
                                                            }
                                                        });
                                                    }

                                                    GUI.color = ColorPlus.LightSalmon;
                                                    if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                                        EditorGUIUtility.systemCopyBuffer = idCollection.IDIndexes[c].ids[i];
                                                    }

                                                    GUI.color = Color.cyan;
                                                    if (GUILayout.Button("e", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                                        renameingIDIndex = i;
                                                        //Open string editor utility for renaming ID.
                                                        renameingCatIndex = c;
                                                        StringEditorUtility.Open("Rename [" + idCollection.IDIndexes[c].ids[i] + "]", idCollection.IDIndexes[c].ids[i], null, onRenameID);
                                                    }

                                                    GUI.color = ColorPlus.Orange;
                                                    if (GUILayout.Button("d", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                                        //Generate and add this new ID.
                                                        string newID = idCollection.GenerateDuplicateID(idCollection.IDIndexes[c].ids[i]);
                                                        if (!idCollection.IDExist(newID)) {
                                                            idCollection.IDIndexes[c].InsertNewID(i + 1, newID);
                                                            onDuplicateID?.Invoke(c, i, newID);
                                                            newSelectingCatIndex = c;
                                                            newSelectingIndex = i + 1;
                                                        }
                                                    }

                                                    GUI.color = ColorPlus.Azure;
                                                    if (GUILayout.Button("↑", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                                        if (idCollection.IDIndexes[c].MoveUp(i)) {
                                                            onMoveIDUp?.Invoke(c, i);
                                                            if (i == selectingIndex) {
                                                                selectingIndex -= 1;
                                                            } else if (i == selectingIndex + 1) {
                                                                selectingIndex += 1;
                                                            }
                                                        }
                                                    }

                                                    if (GUILayout.Button("↓", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                                        if (idCollection.IDIndexes[c].MoveDown(i)) {
                                                            onMoveIDDown?.Invoke(c, i);
                                                            if (i == selectingIndex) {
                                                                selectingIndex += 1;
                                                            } else if (i == selectingIndex - 1) {
                                                                selectingIndex -= 1;
                                                            }
                                                        }
                                                    }

                                                    GUI.color = ColorPlus.Khaki;
                                                    if (GUILayout.Button("m", EditorStyles.miniButtonMid, GUILayout.Width(21.0f))) {
                                                        movingCatIndex = c;
                                                        movingIDIndex = i;
                                                        int catIndex = movingCatIndex;
                                                        int IDIndex = movingIDIndex;
                                                        //Open editor utility for moving to cat.
                                                        IntEditorUtility.Open("Moving [" + idCollection.IDIndexes[c].ids[i] + "] to cat: ", movingCatIndex, null, (x) => {
                                                            if (idCollection.MoveIDToCat(catIndex, IDIndex, x)) {
                                                                onMoveToCat?.Invoke(x);
                                                            }
                                                        });
                                                    }

                                                    GUI.color = Color.magenta;
                                                    if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {

                                                        if (idCollection.IDExist(c, i)) {
                                                            onPreDeleteID?.Invoke(c, i);
                                                        }
                                                        if (idCollection.RemoveID(c, i)) {
                                                            onPostDeleteID?.Invoke(c, i);
                                                        }
                                                    }
                                                } else {
                                                    GUI.color = ColorPlus.LightSalmon;
                                                    if (GUILayout.Button("c", EditorStyles.miniButton, GUILayout.Width(16.0f))) {
                                                        EditorGUIUtility.systemCopyBuffer = idCollection.IDIndexes[c].ids[i];
                                                    }
                                                    GUI.color = ColorPlus.White;

                                                    //A hack for making the layout glitch disappear.
                                                    GUILayout.Button("", EditorStyles.miniLabel, GUILayout.Width(0.0f));
                                                }

                                                GUI.color = Color.white;
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayoutPlus.HorizontalLine();
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }

                    if (editMode) {
                        GUI.color = Color.yellow;
                        if (GUILayout.Button("New Cat (T)", EditorStyles.miniButton)) {
                            //Add a new category to collection.
                            idCollection.NewIDIndex();

                            onNewCat?.Invoke();
                        }
                        GUI.color = Color.white;
                    }

                    if (newSelectingIndex != -1 || newSelectingCatIndex != -1) {
                        selectingCatIndex = newSelectingCatIndex;
                        selectingIndex = newSelectingIndex;
                        onSelectIndex?.Invoke(selectingCatIndex, selectingIndex);
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }

    static private void DrawButtonMenu(IDCollection idCollection, ref int selectingCatIndex, ref int selectingIndex, ref string newIDStr
    , ref int renameingCatIndex, ref int renameingIDIndex, ref int movingCatIndex, ref int movingIDIndex, ref bool editMode, List<bool> foldOutCats
    , Action onNewCat, Action<int> onPreDeleteCat, Action<int> onPostDeleteCat
    , Action<int, int, string> onInsertNewID, Action<int, int, List<string>> onInsertNewIDs
    , Action<int, int> onPreDeleteID, Action<List<string>> onPreDeleteIDs
    , Action<int, int> onPostDeleteID, Action<List<string>, int, int> onPostDeleteIDs
    , Action<int, int> onMoveIDUp, Action<int, int> onMoveIDDown
    , Action<int> onMoveCatUp, Action<int> onMoveCatDown, Action<int> onMoveToCat, Action<int, int, string> onDuplicateID, Action<int, int> onSelectIndex
    , Action<string> onRenameID, Action onVerifyIDs, Action onSave, Action onRevert, ref Vector2 vScollPos, Func<string, string> getIDComment) {

        EditorGUILayout.BeginVertical("HelpBox", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        {
            //Check and resize the fold vars container.
            if (foldOutCats.Count != idCollection.IDIndexes.Count) {
                Util.Resize(foldOutCats, idCollection.IDIndexes.Count);
            }

            //All ids by cats.
            EditorGUILayout.BeginVertical();
            {
                vScollPos = EditorGUILayout.BeginScrollView(vScollPos);
                {
                    //Display all flags in selecting cat.
                    int newSelectingCatIndex = -1;
                    int newSelectingIndex = -1;

                    for (int c = 0; c < idCollection.IDIndexes.Count; c++) {
                        EditorGUILayout.BeginVertical();
                        {
                            //Foldout
                            EditorGUILayout.BeginHorizontal("SelectionRect");
                            {
                                foldOutCats[c] = EditorGUILayout.Foldout(foldOutCats[c], "[" + c + "](" + idCollection.IDIndexes[c].ids.Count + ") " + idCollection.IDIndexes[c].name, true);

                                if (editMode) {
                                    GUI.color = Color.yellow;
                                    if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(17.0f))) {
                                        int catIndex = c;
                                        int addTargetIndex = idCollection.IDIndexes[catIndex].ids.Count;
                                        StringEditorUtility.Open("Append New ID", "", null, (newID) => {
                                            if (idCollection.InsertNewID(catIndex, addTargetIndex, newID)) {
                                                // addTargetIndex += 1;
                                                onInsertNewID?.Invoke(catIndex, addTargetIndex, newID);
                                            } else {
                                                Debug.LogWarning("Append ID failed! [" + newID + "] [" + catIndex + "] [" + addTargetIndex + "]");
                                            }
                                        });
                                    }

                                    GUI.color = ColorPlus.SpringGreen;
                                    if (GUILayout.Button("BI", EditorStyles.miniButtonLeft, GUILayout.Width(27.0f))) {
                                        int selectingCatIndexTemp = c;
                                        int selectingIndexTemp = idCollection.IDIndexes[c].ids.Count;
                                        StringListEditorUtility.Open("Batch Insert", (newIDs) => {
                                            List<string> insertedIDs = new List<string>();
                                            for (int i = 0; i < newIDs.Count; i++) {
                                                if (idCollection.IsLegalToAddID(newIDs[i])) {
                                                    if (idCollection.InsertNewID(selectingCatIndexTemp, selectingIndexTemp + i, newIDs[i])) {
                                                        insertedIDs.Add(newIDs[i]);
                                                    } else {
                                                        Debug.LogWarning("Insert ID failed! [" + newIDs[i] + "]");
                                                    }
                                                    GUI.FocusControl(null);
                                                } else {
                                                    Debug.LogWarning("Illegal ID! [" + newIDs[i] + "]");
                                                }
                                            }
                                            onInsertNewIDs?.Invoke(selectingCatIndexTemp, selectingIndexTemp, insertedIDs);
                                        }
                                        );
                                    }
                                    GUI.color = Color.magenta;
                                    if (GUILayout.Button("BD", EditorStyles.miniButtonRight, GUILayout.Width(27.0f))) {
                                        int addTargetIndex = idCollection.IDIndexes[c].ids.Count;
                                        int selectingCatIndexTemp = c;
                                        int selectingIndexTemp = selectingIndex;
                                        StringListEditorUtility.Open("Batch Delete", (removingIDs) => {
                                            List<string> removedIDs = new List<string>();
                                            for (int i = 0; i < removingIDs.Count; i++) {
                                                if (idCollection.IDExist(removingIDs[i])) {
                                                    removedIDs.Add(removingIDs[i]);
                                                }
                                            }

                                            if (removedIDs.Count > 0) {
                                                onPreDeleteIDs?.Invoke(removedIDs);

                                                for (int i = 0; i < removedIDs.Count; i++) {
                                                    idCollection.RemoveID(removedIDs[i]);
                                                }

                                                onPostDeleteIDs?.Invoke(removedIDs, selectingCatIndexTemp, selectingIndexTemp);
                                            }
                                        }
                                        );
                                    }
                                    GUI.color = Color.white;

                                    //Edit cat name.
                                    GUI.color = Color.cyan;
                                    if (GUILayout.Button("e", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                                        int catIndex = c;
                                        StringEditorUtility.Open("Edit Category Name", idCollection.IDIndexes[c].name, (x) => idCollection.IDIndexes[catIndex].name = x);
                                    }
                                    GUI.color = ColorPlus.Lavender;
                                    if (GUILayout.Button("↑", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                        if (idCollection.MoveUpIDIndex(c)) {
                                            onMoveCatUp?.Invoke(c);
                                        }
                                    }
                                    GUI.color = ColorPlus.Lavender;
                                    if (GUILayout.Button("↓", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                        if (idCollection.MoveDownIDIndex(c)) {
                                            onMoveCatDown?.Invoke(c);
                                        }
                                    }
                                    GUI.color = Color.magenta;
                                    if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {
                                        if (idCollection.IDIndexExist(c)) {
                                            onPreDeleteCat?.Invoke(c);

                                            idCollection.RemoveIDIndex(c);

                                            onPostDeleteCat?.Invoke(c);
                                            return;
                                        }
                                    }
                                    GUI.color = Color.white;
                                }
                            }
                            EditorGUILayout.EndHorizontal();

                            //Content.
                            if (foldOutCats[c]) {
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.BeginVertical();
                                    {
                                        //Display all ids in selecting cat.                            
                                        for (int i = 0; i < idCollection.IDIndexes[c].ids.Count; i++) {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                if (editMode) {
                                                    GUI.color = Color.white;
                                                    string IDComment = (getIDComment == null) ? ("") : (getIDComment(idCollection.IDIndexes[c].ids[i]));
                                                    if (GUILayout.Button(idCollection.IDIndexes[c].ids[i] + IDComment, "ControlLabel")) {
                                                        newSelectingCatIndex = c;
                                                        newSelectingIndex = i;
                                                    }

                                                    GUI.color = Color.yellow;
                                                    if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(16.0f))) {
                                                        int catIndex = c;
                                                        int addTargetIndex = i;
                                                        StringEditorUtility.Open("Append New ID", "", null, (newID) => {
                                                            if (idCollection.InsertNewID(catIndex, addTargetIndex, newID)) {
                                                                // addTargetIndex += 1;
                                                                onInsertNewID?.Invoke(catIndex, addTargetIndex, newID);
                                                            } else {
                                                                Debug.LogWarning("Append ID failed! [" + newID + "] [" + catIndex + "] [" + addTargetIndex + "]");
                                                            }
                                                        });
                                                    }

                                                    GUI.color = ColorPlus.LightSalmon;
                                                    if (GUILayout.Button("c", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                                        EditorGUIUtility.systemCopyBuffer = idCollection.IDIndexes[c].ids[i];
                                                    }

                                                    GUI.color = Color.cyan;
                                                    if (GUILayout.Button("e", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                                        renameingIDIndex = i;
                                                        //Open string editor utility for renaming ID.
                                                        renameingCatIndex = c;
                                                        StringEditorUtility.Open("Rename [" + idCollection.IDIndexes[c].ids[i] + "]", idCollection.IDIndexes[c].ids[i], null, onRenameID);
                                                    }

                                                    GUI.color = ColorPlus.Orange;
                                                    if (GUILayout.Button("d", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                                        //Generate and add this new ID.
                                                        string newID = idCollection.GenerateDuplicateID(idCollection.IDIndexes[c].ids[i]);
                                                        if (!idCollection.IDExist(newID)) {
                                                            idCollection.IDIndexes[c].InsertNewID(i + 1, newID);
                                                            onDuplicateID?.Invoke(c, i, newID);
                                                        }
                                                    }

                                                    GUI.color = ColorPlus.Azure;
                                                    if (GUILayout.Button("↑", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                                        if (idCollection.IDIndexes[c].MoveUp(i)) {
                                                            onMoveIDUp?.Invoke(c, i);
                                                        }
                                                    }

                                                    if (GUILayout.Button("↓", EditorStyles.miniButtonMid, GUILayout.Width(16.0f))) {
                                                        if (idCollection.IDIndexes[c].MoveDown(i)) {
                                                            onMoveIDDown?.Invoke(c, i);
                                                        }
                                                    }

                                                    GUI.color = ColorPlus.Khaki;
                                                    if (GUILayout.Button("m", EditorStyles.miniButtonMid, GUILayout.Width(21.0f))) {
                                                        movingCatIndex = c;
                                                        movingIDIndex = i;
                                                        int catIndex = movingCatIndex;
                                                        int IDIndex = movingIDIndex;
                                                        //Open editor utility for moving to cat.
                                                        IntEditorUtility.Open("Moving [" + idCollection.IDIndexes[c].ids[i] + "] to cat: ", movingCatIndex, null, (x) => {
                                                            if (idCollection.MoveIDToCat(catIndex, IDIndex, x)) {
                                                                onMoveToCat?.Invoke(x);
                                                            }
                                                        });
                                                    }

                                                    GUI.color = Color.magenta;
                                                    if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(16.0f))) {

                                                        if (idCollection.IDExist(c, i)) {
                                                            onPreDeleteID?.Invoke(c, i);
                                                        }
                                                        if (idCollection.RemoveID(c, i)) {
                                                            onPostDeleteID?.Invoke(c, i);
                                                        }
                                                    }
                                                } else {
                                                    EditorGUILayout.BeginHorizontal();
                                                    {
                                                        string IDComment = (getIDComment == null) ? ("") : (getIDComment(idCollection.IDIndexes[c].ids[i]));
                                                        if (GUILayout.Button(idCollection.IDIndexes[c].ids[i] + IDComment, "ControlLabel")) {
                                                            newSelectingCatIndex = c;
                                                            newSelectingIndex = i;
                                                        }

                                                        GUI.color = ColorPlus.LightSalmon;
                                                        if (GUILayout.Button("c", EditorStyles.miniButton, GUILayout.Width(16.0f))) {
                                                            EditorGUIUtility.systemCopyBuffer = idCollection.IDIndexes[c].ids[i];
                                                        }
                                                        GUI.color = ColorPlus.White;

                                                        //A hack for making the layout glitch disappear.
                                                        GUILayout.Button("", "ControlLabel", GUILayout.Width(0.0f));
                                                    }
                                                    EditorGUILayout.EndHorizontal();
                                                }

                                                GUI.color = Color.white;
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayoutPlus.HorizontalLine();
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }

                    if (editMode) {
                        GUI.color = Color.yellow;
                        if (GUILayout.Button("New Cat (T)", EditorStyles.miniButton)) {
                            //Add a new category to collection.
                            idCollection.NewIDIndex();

                            onNewCat?.Invoke();
                        }
                        GUI.color = Color.white;
                    }

                    if (newSelectingIndex != -1 || newSelectingCatIndex != -1) {
                        selectingCatIndex = newSelectingCatIndex;
                        selectingIndex = newSelectingIndex;
                        onSelectIndex?.Invoke(selectingCatIndex, selectingIndex);
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }

}

#endif