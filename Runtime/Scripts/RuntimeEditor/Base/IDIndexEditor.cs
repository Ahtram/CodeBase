#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A base class for IDIndex serious editors. 
/// To use this class: Derive from it for making individual window of different IDIndex object. (There's a tricky reason for use the deriving here)
/// Or: Use the Draw function directly for embed the editor drawing UI. In this case you need to store all necessary state outside the function.
/// </summary>
abstract public class IDIndexEditor : UniEditorWindow {

    public Action<int, string> onInsertNewID = null;
    public Action<int> onDeleteID = null;
    public Action<int> onMoveIDUp = null;
    public Action<int> onMoveIDDown = null;
    public Action<int, string> onDuplicateID = null;
    public Action<int> onSelectIndex = null;
    public Action onSave = null;

    public IDIndex editingIDIndex = null;

    private Vector2 scrollPos = Vector2.zero;
    private int selectingIDIndex = -1;
    private string newIDStr = "";

    override public void OnGUI() {
        base.OnGUI();
        if (editingIDIndex != null) {
            Draw(editingIDIndex, ref selectingIDIndex, ref newIDStr, onInsertNewID, onDeleteID, onMoveIDUp, onMoveIDDown, onDuplicateID, onSelectIndex, onSave, ref scrollPos);
        }
    }

    public int SelectingIDIndex() {
        return selectingIDIndex;
    }

    public void UnSelectID() {
        selectingIDIndex = -1;
    }

    //Static UI Drawer. Can be used in any editor class if needed.
    static public void Draw(IDIndex idIndex, ref int selectingIndex, ref string newIDStr, Action<int, string> onInsertNewID, Action<int> onDeleteID
        , Action<int> onMoveUp, Action<int> onMoveDown, Action<int, string> onDuplicateID, Action<int> onSelectIndex, Action onSave, ref Vector2 vScrollPos) {
        EditorGUILayout.BeginVertical("HelpBox");
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUI.color = Color.yellow;
                    if (GUILayout.Button("Insert New", EditorStyles.miniButton)) {
                        if (idIndex.IsLegalToAddID(newIDStr)) {
                            //Insert a new flag to selecting flag index
                            if (idIndex.InsertNewID(selectingIndex + 1, newIDStr)) {
                                GUI.FocusControl("");
                                //Callback.
                                if (onInsertNewID != null) {
                                    onInsertNewID(selectingIndex + 1, newIDStr);
                                }
                            }
                        } else {
                            Debug.LogWarning("Illegal ID!");
                        }
                    }

                    GUI.color = Color.white;
                    newIDStr = EditorGUILayout.TextField(newIDStr, EditorStyles.foldout);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    GUI.color = ColorPlus.Orange;
                    if (GUILayout.Button("d", EditorStyles.miniButtonLeft)) {
                        if (selectingIndex != -1 && selectingIndex < idIndex.ids.Count) {
                            //Generate and add this new ID.
                            string newID = idIndex.GenerateDuplicateID(idIndex.ids[selectingIndex]);
                            if (!idIndex.IDExist(newID)) {
                                idIndex.InsertNewID(selectingIndex + 1, newID);

                                if (onDuplicateID != null) {
                                    onDuplicateID(selectingIndex, newID);
                                }

                                selectingIndex += 1;
                            }
                        }
                    }

                    GUI.color = ColorPlus.Azure;
                    if (GUILayout.Button("↑", EditorStyles.miniButtonMid)) {
                        if (idIndex.MoveUp(selectingIndex)) {
                            if (onMoveUp != null) {
                                onMoveUp(selectingIndex);
                            }
                            selectingIndex -= 1;
                        }
                    }

                    if (GUILayout.Button("↓", EditorStyles.miniButtonMid)) {
                        if (idIndex.MoveDown(selectingIndex)) {
                            if (onMoveDown != null) {
                                onMoveDown(selectingIndex);
                            }
                            selectingIndex += 1;
                        }
                    }

                    GUI.color = Color.magenta;
                    if (GUILayout.Button("-", EditorStyles.miniButtonRight)) {
                        if (selectingIndex >= 0) {
                            if (idIndex.RemoveID(selectingIndex)) {
                                if (onDeleteID != null) {
                                    onDeleteID(selectingIndex);
                                }
                                selectingIndex = -1;
                            }
                        }
                    }

                    GUI.color = Color.white;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            //All IDs
            EditorGUILayout.BeginVertical();
            {
                vScrollPos = EditorGUILayout.BeginScrollView(vScrollPos);
                {
                    int newSelectingIDIndex = -1;
                    //Display all ids in selecting cat.
                    newSelectingIDIndex = EditorGUILayoutPlus.Tabs(idIndex.ids.ToArray(), selectingIndex, EditorStyles.radioButton);

                    if (newSelectingIDIndex != -1 && newSelectingIDIndex != selectingIndex) {
                        selectingIndex = newSelectingIDIndex;
                        if (onSelectIndex != null) {
                            onSelectIndex(newSelectingIDIndex);
                        }
                    }

                    GUI.color = Color.white;
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        {
            GUI.color = Color.green;
            if (GUILayout.Button("Save", EditorStyles.miniButton)) {
                if (onSave != null) {
                    onSave();
                }
            }
            GUI.color = Color.white;
        }
        EditorGUILayout.EndVertical();
    }

}

#endif