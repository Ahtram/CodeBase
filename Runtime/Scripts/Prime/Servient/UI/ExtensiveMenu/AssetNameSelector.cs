using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Teamuni.Codebase;
using UnityEngine.Events;
using System.Collections.Generic;

using UnityEditor;
using System.IO;

public class AssetNameSelector : UIBase, IPointerClickHandler {

#if UNITY_EDITOR
    static private string PREFAB_PATH = "Prefabs/ExtensiveMenu/AssetNameSelector";
#endif

    public UnityEventString onIDChanged;
    public UnityEvent onClear;

    public enum AssetType {
        AnimationClip,
        AudioClip,
        AudioMixer,
        ComputeShader,
        Font,
        GUISkin,
        Material,
        Mesh,
        Model,
        PhysicMaterial,
        Prefab,
        Scene,
        Script,
        Shader,
        Sprite,
        Texture,
        VideoClip,
        Texture2D,
        TextAsset
    }

    [Tooltip("Which type of asset we are targeting?")]
    public AssetType assetType = AssetType.Prefab;

    [Tooltip("Start with \"Assets/\" and \"SHOULD NOT\" end with \" / \"")]
    public string assetsFolderPath = "";

    //To display the selected Asset.
    public Text selectedAssetText;

    //The opening ExtensiveMenu.
    private ExtensiveMenu m_extensiveMenu;

    private string m_selectingAssetName = "";

    void Awake() {
        UpdateSelectingDisplay();
    }

    /// <summary>
    /// Open or close the ExtensiveMenu.
    /// </summary>
    /// <param name="localPos"></param>
    public void ToggleExtensiveMenu(Vector2 localPos = new Vector2()) {
        if (m_extensiveMenu == null) {
#if UNITY_EDITOR
            m_extensiveMenu = ExtensiveMenu.Instantiate("[ExtensiveMenu]", GetRectTransform(), localPos);

            string[] guids = AssetDatabase.FindAssets("t:" + assetType.ToString(), new string[] { assetsFolderPath });
            for (int i = 0; i < guids.Length; i++) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                string assetName = Path.GetFileNameWithoutExtension(assetPath);
                string extensiotn = Path.GetExtension(assetPath);
                string displayGUIContent = assetPath.Remove(assetPath.Length - extensiotn.Length, extensiotn.Length).Remove(0, assetsFolderPath.Length + 1);
                bool isSelection = (!string.IsNullOrEmpty(m_selectingAssetName) && m_selectingAssetName == assetName) ? (true) : (false);
                m_extensiveMenu.AddItem(displayGUIContent, isSelection, OnItemSelected, assetName);
            }
            m_extensiveMenu.AddItem("[Copy]", false, OnCopySelected);
            m_extensiveMenu.AddItem("[Clear]", false, OnClearSelected);
#else
            m_extensiveMenu.AddItem("[Not support on non editor env]", false, OnNotSupported);
#endif

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
        if (selectedAssetText != null) {
            if (!string.IsNullOrEmpty(m_selectingAssetName)) {
                selectedAssetText.text = m_selectingAssetName;
            } else {
                selectedAssetText.text = "[Empty]";
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
        EditorGUIUtility.systemCopyBuffer = m_selectingAssetName;
#endif
        CloseExtensiveMenu();
    }

    private void OnClearSelected() {
        m_selectingAssetName = "";
        UpdateSelectingDisplay();
        onClear.Invoke();
        CloseExtensiveMenu();
    }

    private void OnItemSelected(object selectedAssetName) {
        if (m_selectingAssetName != selectedAssetName as string) {
            m_selectingAssetName = selectedAssetName as string;
            UpdateSelectingDisplay();
            onIDChanged.Invoke(m_selectingAssetName);
        }
        CloseExtensiveMenu();
    }

    /// <summary>
    /// The thing we are selecting.
    /// </summary>
    /// <returns></returns>
    public string SelectingItem() {
        return m_selectingAssetName;
    }

    //---------- index relative

    /// <summary>
    /// Get all possible asset names.
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllAssetNames() {
        List<string> returnList = new List<string>();
#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("t:" + assetType.ToString(), new string[] { assetsFolderPath });
        for (int i = 0; i < guids.Length; i++) {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            string assetName = Path.GetFileNameWithoutExtension(assetPath);
            returnList.Add(assetName);
        }
#endif
        return returnList;
    }

    /// <summary>
    /// Get the total count of the asset names.
    /// </summary>
    /// <returns></returns>
    public int TotalAssetNameCount() {
        return GetAllAssetNames().Count;
    }

    /// <summary>
    /// The selecting asset name in flat index.
    /// Returns -1 if not exist.
    /// </summary>
    /// <returns></returns>
    public int SelectingIndex() {
        List<string> allAssetNames = GetAllAssetNames();
        return allAssetNames.IndexOf(m_selectingAssetName);
    }

    /// <summary>
    /// Select an asset name by index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool SelectIndex(int index) {
        List<string> allAssetNames = GetAllAssetNames();
        if (index >= 0 && index < allAssetNames.Count) {
            //Legal range
            m_selectingAssetName = allAssetNames[index];
            UpdateSelectingDisplay();
            onIDChanged.Invoke(m_selectingAssetName);
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
        if (selectingIndex >= TotalAssetNameCount()) {
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
            selectingIndex = TotalAssetNameCount() - 1;
        }
        return SelectIndex(selectingIndex);
    }

    //---------- 

#if UNITY_EDITOR

    [MenuItem("GameObject/UI/AssetNameSelector")]
    static void SpwanAssetNameSelector() {
        if (Selection.activeGameObject != null) {
            RectTransform rt = Selection.activeGameObject.GetRectTransform();
            if (rt != null) {
                Util.InstantiateModule<AssetNameSelector>(PREFAB_PATH, rt);
            }
        }
    }

    [MenuItem("GameObject/UI/AssetNameSelector", true)]
    static bool ValidateSpwanAssetNameSelector() {
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
