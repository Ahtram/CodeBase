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

    public UnityEvent onPreToggle;
    public UnityEventString onContentChanged;
    public UnityEvent onClear;

    //To display the selected Content.
    public Text selectedContentNameText;

    //This will be use if assigned.
    public RectTransform customMountPoint;

    //The opening ExtensiveMenu.
    private ExtensiveMenu m_extensiveMenu;

    private string m_selectingContentName = "";

    //The content list we are using.
    private List<string> m_contentList = new List<string>();

    void Awake() {
        UpdateSelectingDisplay();
    }

    /// <summary>
    /// The content string support sub file pathes as sub menus.
    /// </summary>
    /// <param name="contentList"></param>
    public void Setup(List<string> contentList, bool selectFirstWhenSetup = true) {
        m_contentList.Clear();
        m_contentList.AddRange(contentList);
        if (m_contentList.Count > 0) {
            if (selectFirstWhenSetup) {
                m_selectingContentName = m_contentList[0];
                UpdateSelectingDisplay();
                onContentChanged.Invoke(m_selectingContentName);
            }
        } else {
            m_selectingContentName = "";
            UpdateSelectingDisplay();
        }
        CloseExtensiveMenu();
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
            onPreToggle?.Invoke();
            m_extensiveMenu = ExtensiveMenu.Instantiate("[ExtensiveMenu]", GetRectTransform(), localPos);
            if (customMountPoint != null) {
                m_extensiveMenu.transform.SetParent(customMountPoint);
            }

            for (int i = 0; i < m_contentList.Count; i++) {
                string path = m_contentList[i];
                bool isSelection = (!string.IsNullOrEmpty(m_selectingContentName) && m_selectingContentName == path) ? (true) : (false);
                m_extensiveMenu.AddItem(path, isSelection, OnItemSelected, path);
            }
            m_extensiveMenu.AddItem("[Copy]", false, OnCopySelected);
            m_extensiveMenu.AddItem("[Clear]", false, OnClearSelected);
            m_extensiveMenu.EnableOnBackgroundClickEventListener(OnRootMenuBackgroundClick);
        } else {
            CloseExtensiveMenu();
        }
    }

    public RectTransform GetMountPoint() {
        if (customMountPoint != null) {
            return customMountPoint;
        }
        return GetRectTransform();
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
            ExtensiveMenu.DestroyUnder(GetMountPoint());
            m_extensiveMenu = null;
        }
    }

    private void OnRootMenuBackgroundClick() {
        CloseExtensiveMenu();
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
            onContentChanged.Invoke(m_selectingContentName);
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
        List<string> allContentNames = GetAllContentNames();
        return allContentNames.IndexOf(m_selectingContentName);
    }

    /// <summary>
    /// Select an asset name by index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool SelectIndex(int index) {
        List<string> allContentNames = GetAllContentNames();
        if (index >= 0 && index < allContentNames.Count) {
            //Legal range
            m_selectingContentName = allContentNames[index];
            UpdateSelectingDisplay();
            onContentChanged.Invoke(m_selectingContentName);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Try select a name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool SelectName(string name) {
        List<string> allContentNames = GetAllContentNames();
        for (int i = 0; i < allContentNames.Count; ++i) {
            if (allContentNames[i] == name) {
                m_selectingContentName = name;
                UpdateSelectingDisplay();
                onContentChanged.Invoke(m_selectingContentName);
                return true;
            }
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
