using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Teamuni.Codebase;

using UnityEditor;
using System.IO;

public class AssetNameSelector : UIBase, IPointerClickHandler {

#if UNITY_EDITOR
    static private string PREFAB_PATH = "Prefabs/ExtensiveMenu/AssetNameSelector";
#endif

    public enum AssetType {
        prefab,
        texture2D,
        audioClip,
        font,
        sprite,
        script,
    }

    [Tooltip("Which type of asset we are targeting?")]
    public AssetType assetType = AssetType.prefab;

    [Tooltip("Start with \"Assets/\" and \"SHOULD NOT\" end with \" / \"")]
    public string assetsFolderPath = "";

    //To display the selected Asset.
    public Text selectedAssetText;

    //The opening ExtensiveMenu.
    private ExtensiveMenu m_extensiveMenu;

    private string m_selectingAssetName = "";

    void Awake() {
        UpdateSelectingIDDisplay();
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
            m_extensiveMenu.AddItem("[Clear]", false, OnItemSelected, "");
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

    private void UpdateSelectingIDDisplay() {
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

    }

    private void OnCopySelected() {
#if UNITY_EDITOR
        EditorGUIUtility.systemCopyBuffer = m_selectingAssetName;
#endif
    }

    private void OnItemSelected(object selectedAssetName) {
        m_selectingAssetName = selectedAssetName as string;
        UpdateSelectingIDDisplay();
        CloseExtensiveMenu();
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
