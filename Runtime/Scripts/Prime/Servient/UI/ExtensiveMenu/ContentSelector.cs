using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Teamuni.Codebase;
using UnityEngine.Events;
using System.Collections.Generic;

using UnityEditor;
using System.IO;

public class ContentSelector : UIBase, IPointerClickHandler {

#if UNITY_EDITOR
    static private string PREFAB_PATH = "Prefabs/ExtensiveMenu/ContentSelector";
#endif

    public UnityEventString onIDChanged;
    public UnityEvent onClear;

    //To display the selected Content.
    public Text selectedContentNameText;

    //The opening ExtensiveMenu.
    private ExtensiveMenu m_extensiveMenu;

    private string m_selectingContentName = "";

    //The content list we are using.
    private List<string> m_contentList = new List<string>();

    void Awake() {
        UpdateSelectingDisplay();
    }

    public void Setup(List<string> contentList) {
        m_contentList.Clear();
        m_contentList.AddRange(contentList);
    }

    public void Clear() {
        m_contentList.Clear();
        m_selectingContentName = "";
        UpdateSelectingDisplay();
        onClear.Invoke();
        CloseExtensiveMenu();
    }

    /// <summary>
    /// Open or close the ExtensiveMenu.
    /// </summary>
    /// <param name="localPos"></param>
    public void ToggleExtensiveMenu(Vector2 localPos = new Vector2()) {
        if (m_extensiveMenu == null) {
            m_extensiveMenu = ExtensiveMenu.Instantiate("[ExtensiveMenu]", GetRectTransform(), localPos);

            for (int i = 0; i < m_contentList.Count; i++) {
                string path = m_contentList[i];
                string name = Path.GetFileNameWithoutExtension(path);
                bool isSelection = (!string.IsNullOrEmpty(m_selectingContentName) && m_selectingContentName == name) ? (true) : (false);
                m_extensiveMenu.AddItem(path, isSelection, OnItemSelected, name);
            }
            m_extensiveMenu.AddItem("[Copy]", false, OnCopySelected);
            m_extensiveMenu.AddItem("[Clear]", false, OnClearSelected);
        } else {
            CloseExtensiveMenu();
        }
    }

    /// <summary>
    /// Are we current opening an ExtensiveMenu?
    /// </summary>
    /// <returns></returns>
    public bool IsOpeningAnExtensiveMenu() {
        return (m_extensiveMenu != null);
    }

    /// <summary>
    /// Try close the opening extensive menu.
    /// </summary>
    public void CloseExtensiveMenu() {
        if (m_extensiveMenu != null) {
            ExtensiveMenu.DestroyUnder(GetRectTransform());
            m_extensiveMenu = null;
        }
    }

    private void UpdateSelectingDisplay() {
        if (selectedContentNameText != null) {
            if (!string.IsNullOrEmpty(m_selectingContentName)) {
                selectedContentNameText.text = m_selectingContentName;
            } else {
                selectedContentNameText.text = "[Empty]";
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        ToggleExtensiveMenu(GetRectTransform().InverseTransformPoint(eventData.pointerCurrentRaycast.worldPosition));
    }

    private void OnNotSupported() {
        CloseExtensiveMenu();
    }

    private void OnCopySelected() {
#if UNITY_EDITOR
        EditorGUIUtility.systemCopyBuffer = m_selectingContentName;
#endif
        CloseExtensiveMenu();
    }

    private void OnClearSelected() {
        m_selectingContentName = "";
        UpdateSelectingDisplay();
        onClear.Invoke();
        CloseExtensiveMenu();
    }

    private void OnItemSelected(object selectedContent) {
        if (m_selectingContentName != selectedContent as string) {
            m_selectingContentName = selectedContent as string;
            UpdateSelectingDisplay();
            onIDChanged.Invoke(m_selectingContentName);
        }
        CloseExtensiveMenu();
    }

    /// <summary>
    /// The thing we are selecting.
    /// </summary>
    /// <returns></returns>
    public string SelectingItem() {
        return m_selectingContentName;
    }

    //---------- index relative

    /// <summary>
    /// Get all possible asset names.
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllContentNames() {
        List<string> returnList = new List<string>();
        for (int i = 0; i < m_contentList.Count; i++) {
            returnList.Add(Path.GetFileNameWithoutExtension(m_contentList[i]));
        }
        return returnList;
    }

    /// <summary>
    /// Get the total count of the asset names.
    /// </summary>
    /// <returns></returns>
    public int TotalContentCount() {
        return GetAllContentNames().Count;
    }

    /// <summary>
    /// The selecting asset name in flat index.
    /// Returns -1 if not exist.
    /// </summary>
    /// <returns></returns>
    public int SelectingIndex() {
        List<string> allContentNamess = GetAllContentNames();
        return allContentNamess.IndexOf(m_selectingContentName);
    }

    /// <summary>
    /// Select an asset name by index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool SelectIndex(int index) {
        List<string> allContentNamess = GetAllContentNames();
        if (index >= 0 && index < allContentNamess.Count) {
            //Legal range
            m_selectingContentName = allContentNamess[index];
            UpdateSelectingDisplay();
            onIDChanged.Invoke(m_selectingContentName);
        }
        return false;
    }

    /// <summary>
    /// Select the next index.
    /// </summary>
    /// <returns></returns>
    public bool SelectNext() {
        int selectingIndex = SelectingIndex();
        selectingIndex += 1;
        if (selectingIndex >= TotalContentCount()) {
            selectingIndex = 0;
        }
        return SelectIndex(selectingIndex);
    }

    /// <summary>
    /// Select the previous index.
    /// </summary>
    /// <returns></returns>
    public bool SelectPrevious() {
        int selectingIndex = SelectingIndex();
        selectingIndex -= 1;
        if (selectingIndex < 0) {
            selectingIndex = TotalContentCount() - 1;
        }
        return SelectIndex(selectingIndex);
    }

    //---------- 

#if UNITY_EDITOR

    [MenuItem("GameObject/UI/ContentSelector")]
    static void SpwanContentSelector() {
        if (Selection.activeGameObject != null) {
            RectTransform rt = Selection.activeGameObject.GetRectTransform();
            if (rt != null) {
                Util.InstantiateModule<ContentSelector>(PREFAB_PATH, rt);
            }
        }
    }

    [MenuItem("GameObject/UI/ContentSelector", true)]
    static bool ValidateSpwanContentSelector() {
        if (Selection.activeGameObject != null) {
            RectTransform rt = Selection.activeGameObject.GetRectTransform();
            if (rt != null) {
                return true;
            }
        }
        return false;
    }

#endif

}
