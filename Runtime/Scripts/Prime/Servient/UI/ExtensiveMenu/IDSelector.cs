using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Teamuni.Codebase;

using UnityEditor;

public class IDSelector : UIBase, IPointerClickHandler {

#if UNITY_EDITOR
    static private string PREFAB_PATH = "Prefabs/ExtensiveMenu/IDSelector";
#endif

    [Tooltip("Input a IDCollection file name under your [Resources//Data//Trivial//] without extenstion file name.")]
    public string initIDCollectionFileName = "";

    //To display the selected ID.
    public Text selectedIDText;

    //The temp IDCollection loaded on this selector.
    private IDCollection m_loadedIDCollection;

    //The opening ExtensiveMenu.
    private ExtensiveMenu m_extensiveMenu;

    private string m_selectingID = "";

    void Awake() {
        //Try load the initial IDCollection.
        if (!string.IsNullOrEmpty(initIDCollectionFileName)) {
            LoadIDCollection(initIDCollectionFileName);
            UpdateSelectingIDDisplay();
        }
    }

    /// <summary>
    /// Load the IDCollection by file name.
    /// </summary>
    /// <param name="IDColelctionFilename"></param>
    public void LoadIDCollection(string IDColelctionFilename) {
        m_loadedIDCollection = DataUtil.GetDataFromResource<IDCollection>(SysPath.TrivialDataPath + IDColelctionFilename);
    }

    /// <summary>
    /// Just set the loaded IDCollection.
    /// </summary>
    /// <param name="idCollection"></param>
    public void LoadIDCollection(IDCollection idCollection) {
        m_loadedIDCollection = idCollection;
    }

    /// <summary>
    /// Release the loaded IDCollection.
    /// </summary>
    public void UnloadIDCollection() {
        m_loadedIDCollection = null;
    }

    /// <summary>
    /// Open or close the ExtensiveMenu.
    /// </summary>
    /// <param name="localPos"></param>
    public void ToggleExtensiveMenu(Vector2 localPos = new Vector2()) {
        if (m_extensiveMenu == null) {
            if (m_loadedIDCollection != null) {
                m_extensiveMenu = ExtensiveMenu.Instantiate("[ExtensiveMenu]", GetRectTransform(), localPos);
                for (int i = 0; i < m_loadedIDCollection.IDIndexes.Count; i++) {
                    for (int j = 0; j < m_loadedIDCollection.IDIndexes[i].ids.Count; j++) {
                        m_extensiveMenu.AddItem("[" + i + "](" + m_loadedIDCollection.IDIndexes[i].ids.Count + ") " + m_loadedIDCollection.IDIndexes[i].name + "/" + m_loadedIDCollection.IDIndexes[i].ids[j],
                        (m_selectingID == m_loadedIDCollection.IDIndexes[i].ids[j]), OnItemSelected, m_loadedIDCollection.IDIndexes[i].ids[j]);
                    }
                }
                m_extensiveMenu.AddItem("[Copy]", false, OnCopySelected);
                m_extensiveMenu.AddItem("[Clear]", false, OnItemSelected, "");
            }
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
        if (selectedIDText != null) {
            if (!string.IsNullOrEmpty(m_selectingID)) {
                selectedIDText.text = m_selectingID;
            } else {
                selectedIDText.text = "[Empty]";
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        ToggleExtensiveMenu(GetRectTransform().InverseTransformPoint(eventData.pointerCurrentRaycast.worldPosition));
    }

    private void OnCopySelected() {
#if UNITY_EDITOR
        EditorGUIUtility.systemCopyBuffer = m_selectingID;
#endif
        CloseExtensiveMenu();
    }

    private void OnItemSelected(object selectedID) {
        m_selectingID = selectedID as string;
        UpdateSelectingIDDisplay();
        CloseExtensiveMenu();
    }

    //---------- 

#if UNITY_EDITOR

    [MenuItem("GameObject/UI/IDSelector")]
    static void SpwanIDSelector() {
        if (Selection.activeGameObject != null) {
            RectTransform rt = Selection.activeGameObject.GetRectTransform();
            if (rt != null) {
                Util.InstantiateModule<IDSelector>(PREFAB_PATH, rt);
            }
        }
    }

    [MenuItem("GameObject/UI/IDSelector", true)]
    static bool ValidateSpwanIDSelector() {
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
