using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using CodeBaseExtensions;

/// <summary>
/// 可展開式選單
/// </summary>
public class ExtensiveMenu : UIBase {

    public const string PATH = "Prefabs/ExtensiveMenu/ExtensiveMenu";
    
    //代表一個節點用的資料
    public class ItemContent {
        //顯示在選單上的字串
        public string DisplayStr = "--";

        //這個Item持有的userData (只有在無SubList時有可能有用)
        public object userData = null;

        //當按下此選項時要callback? (只有在無SubList時有可能有用)
        public Action onItemSelected1 = null;

        //當按下此選項時要callback? (只有在無SubList時有可能有用)
        public Action<object> onItemSelected2 = null;

        //是否被勾選
        public bool on = false;

        //要往下展開的列表 (有可能為空)
        public Dictionary<string, ItemContent> SubList = new Dictionary<string, ItemContent>();
        // public List<ItemContent> SubList = new List<ItemContent>();

        //是否有向下展開的列表
        public bool HasSubList() {
            return (SubList != null && SubList.Count > 0) ? (true) : (false);
        }
    }

    //選項掛載點
    public RectTransform itemMountPoint;

    //子選單掛載點
    public RectTransform subMenuMountPoint;

    //主捲動框
    public ScrollRect scrollRect;

    //捲動框自動縮放用 contentSizeFitter
    public ContentSizeFitter scrollRectContentSizeFitter;

    //正在顯示的子選單
    private ExtensiveMenu _displayingSubMenu = null;

    //未加處理的原輸入資料 (只有字串)
    private List<string> _rawContents = new List<string>();

    //已解析過資料
    private Dictionary<string, ItemContent> _parsedContents = new Dictionary<string, ItemContent>();
    // private List<ItemContent> _parsedContents = new List<ItemContent>();

    //已經被顯示出來的Item物件
    private List<ExtensiveMenuItem> _items = new List<ExtensiveMenuItem>();

    //是否需要重建Items.
    private bool _needRebuild = false;

    //------ 為了解決沒有 MaxSize 可以用來限制 ScrollRect 的問題而存在的參數 --------
    private const int MAX_VIEWABLE_ITEM_COUNT = 16;
    private const float MAX_SCROLLRECT_HEIGHT = 358.0f;

    //直接指定一組 Content 並且設定好 (這是給非 Root 用的)
    public void Setup(Dictionary<string, ItemContent> contents) {
        _parsedContents = new Dictionary<string, ItemContent>(contents);
        Rebuild(_parsedContents);
    }

    //---------------- Adjust anchor (you should do this befoare adda items!) -----------------

    public void SetPivot(PivotPresets pivotPreset) {
        this.GetRectTransform().SetPivot(pivotPreset);
    }

    //嘗試新增一個選單選項 (這是給 Root 用的)
    public bool AddItem(string path, bool on, Action func) {
        //確保路徑不會有空白字元。
        path = path.Replace(' ', '_');
        if (!string.IsNullOrEmpty(path) && !_rawContents.Contains(path)) {
            //加入此原輸入資料.
            _rawContents.Add(path);
            ParseAppend(_parsedContents, path, on, func);
            _needRebuild = true;
            return true;
        }
        Debug.LogWarning("[ExtensiveMenu] AddItem failed! path: [" + path + "]");
        return false;
    }

    //嘗試新增一個選單選項 (這是給 Root 用的)
    public bool AddItem(string path, bool on, Action<object> func, object userData) {
        //確保路徑不會有空白字元。
        path = path.Replace(' ', '_');
        if (!string.IsNullOrEmpty(path) && !_rawContents.Contains(path)) {
            //加入此原輸入資料.
            _rawContents.Add(path);
            ParseAppend(_parsedContents, path, on, func, userData);
            _needRebuild = true;
            return true;
        }
        Debug.LogWarning("[ExtensiveMenu] AddItem failed! path: [" + path + "]");
        return false;
    }

    public void Clear() {
        _rawContents.Clear();
        _parsedContents.Clear();
        Rebuild(_parsedContents);
    }

    //用輸入資料建立當前選單 (會先清掉舊的 UIItem )
    public void Rebuild(Dictionary<string, ItemContent> itemContents) {
        ClearItems();
        if (itemContents.Count > 0) {
            foreach (KeyValuePair<string, ItemContent> kvp in itemContents) {
                _items.Add(ExtensiveMenuItem.Instantiate("[Item]", itemMountPoint, kvp.Value, OnItemClick, OnItemEnter));
            }
            // itemContents.ForEach((content) => {
            //     _items.Add(ExtensiveMenuItem.Instantiate("[Item]", itemMountPoint, content, OnItemClick, OnItemEnter));
            // });
        } else {
            //這是個空列表，顯示一個示意用Item.
            ExtensiveMenuItem.InstantiateAsEmpty("[EmptyItem]", itemMountPoint, OnItemClick, OnItemEnter);
        }
        LimitScrollRectHeight();
    }

    private void ClearItems() {
        ExtensiveMenuItem.DestroyUnder(itemMountPoint);
        _items.Clear();
    }

    private void OnItemEnter(ItemContent content, ExtensiveMenuItem item) {
        //Clear all displaying submenus.
        DestroySubMenu();
        if (content != null && content.SubList.Count > 0) {
            //Todo: 這邊要判斷位置(以Item本身給的資訊)
            CreateSubMenu(content, item.subMenuAnchor);
        }
    }

    private void OnItemClick(ItemContent content) {
        // onItemClick?.Invoke(content);
        if (content != null) {
            content.onItemSelected1?.Invoke();
            content.onItemSelected2?.Invoke(content.userData);
        }
    }

    private void Update() {
        //偵測並且開啟關閉 ScrollRect 自動縮放功能
        LimitScrollRectHeight();

        //偵測是否執行 Rebuild.
        if (_needRebuild) {
            Rebuild(_parsedContents);
            _needRebuild = false;
        }
    }

    private void LimitScrollRectHeight() {
        if (_items.Count > MAX_VIEWABLE_ITEM_COUNT) {
            //關閉ScrollRect自動縮放, 同時設定最大Size.
            Vector2 size = scrollRect.GetRectTransform().sizeDelta;
            scrollRectContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            scrollRect.GetRectTransform().sizeDelta = new Vector2(size.x, MAX_SCROLLRECT_HEIGHT);
        } else {
            //開啟ScrollRect自動縮放
            scrollRectContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
    }

    //---------------- Sub Menu -----------------

    public void CreateSubMenu(ExtensiveMenu.ItemContent content, RectTransform anchor) {
        DestroySubMenu();
        Vector3 calculatedAnchorPos = subMenuMountPoint.InverseTransformPoint(anchor.position);
        _displayingSubMenu = ExtensiveMenu.Instantiate("[ExtensiveMenu]", subMenuMountPoint, calculatedAnchorPos);
        _displayingSubMenu.Setup(content.SubList);
    }

    public void DestroySubMenu() {
        ExtensiveMenu.DestroyUnder(subMenuMountPoint);
        _displayingSubMenu = null;
    }

    //----------------------------------------------------------------------------------

    //將輸入資料解析為 ItemContent 集合 (簡易版本)
    static public void ParseAppend(Dictionary<string, ItemContent> itemContents, string path, bool on, Action func) {
        Dictionary<string, ItemContent> parsedContent = new Dictionary<string, ItemContent>();

        //首先取得 path 的最前面一個 Item.
        string remainPath = "";
        string firstItem = CutFirstItem(path, ref remainPath);

        //尋找是否已經存在這個 FirstItem.
        bool appendOnExist = false;
        if (itemContents.ContainsKey(firstItem)) {
            //存在，使用這個既有的 Item 往下長
            ParseAppend(itemContents[firstItem].SubList, remainPath, on, func);
            appendOnExist = true;
        }

        //不存在，我們得要加一個新的
        if (!appendOnExist) {
            ItemContent newItemContent = new ItemContent {
                DisplayStr = firstItem
            };
            if (!string.IsNullOrEmpty(remainPath)) {
                //還有路徑，繼續往下長
                ParseAppend(newItemContent.SubList, remainPath, on, func);
            } else {
                //無路徑，這是一個選項，因此要賦予他 callback.
                newItemContent.on = on;
                newItemContent.onItemSelected1 = func;
                newItemContent.userData = null;
            }
            itemContents.Add(firstItem, newItemContent);
        }
    }

    //將輸入資料解析為 ItemContent 集合 (進階版本)
    static public void ParseAppend(Dictionary<string, ItemContent> itemContents, string path, bool on, Action<object> func, object userData) {
       Dictionary<string, ItemContent> parsedContent = new Dictionary<string, ItemContent>();

        //首先取得 path 的最前面一個 Item.
        string remainPath = "";
        string firstItem = CutFirstItem(path, ref remainPath);

        //尋找是否已經存在這個 FirstItem.
        bool appendOnExist = false;
        if (itemContents.ContainsKey(firstItem)) {
            //存在，使用這個既有的 Item 往下長
            ParseAppend(itemContents[firstItem].SubList, remainPath, on, func, userData);
            appendOnExist = true;
        }

        //不存在，我們得要加一個新的
        if (!appendOnExist) {
            ItemContent newItemContent = new ItemContent {
                DisplayStr = firstItem
            };
            if (!string.IsNullOrEmpty(remainPath)) {
                //還有路徑，繼續往下長
                ParseAppend(newItemContent.SubList, remainPath, on, func, userData);
            } else {
                //無路徑，這是一個選項，因此要賦予他 callback.
                newItemContent.on = on;
                newItemContent.onItemSelected2 = func;
                newItemContent.userData = userData;
            }
            itemContents.Add(firstItem, newItemContent);
        }
    }

    static public string CutFirstItem(string path, ref string remain) {
        int index = path.IndexOf('/');
        if (index == -1) {
            //找不到, 已經沒有殘餘物
            remain = "";
            return path;
        } else {
            //找到一個字元
            remain = path.Substring(index + 1);
            return path.Substring(0, index);
        }
    }

    //---------------- Auto fix into UICanvas bound ---------------------

    public void OnScrollRectRectTransformDimensionsChange(RectTransform rectTransform) {
        //Check and fix self position.
        
        //Check the size and see if we need to fix our position.

        RectTransform selfRectTransform = this.GetRectTransform();
        if (selfRectTransform != null) {
            RectTransform canvasRectTransform = selfRectTransform.GetCanvasRectTransform();
            if (canvasRectTransform != null) {
                //Vector2 canvasSize = canvasRectTransform.sizeDelta;
                //Debug.Log("OnScrollRectRectTransformDimensionsChange size: " + rectTransform.sizeDelta + " | selfRectTransform anchoredPosition: " + selfRectTransform.anchoredPosition + " | canvasRectTransform.sizeDelta: " + canvasRectTransform.sizeDelta);
                Vector2 localPosOnCanvas = canvasRectTransform.InverseTransformPoint(selfRectTransform.position);
                float horizontalFix = 0.0f;
                float verticalFix = 0.0f;

                ////Check top.
                //if (localPosOnCanvas.y > canvasRectTransform.sizeDelta.y * 0.5f) {
                //    verticalFix = canvasRectTransform.sizeDelta.y * 0.5f - localPosOnCanvas.y;
                //}

                //Check left.
                if (localPosOnCanvas.x < -canvasRectTransform.sizeDelta.x * 0.5f) {
                    horizontalFix = -canvasRectTransform.sizeDelta.x * 0.5f - localPosOnCanvas.x;
                }

                ////Check bottom.
                //if (localPosOnCanvas.y - rectTransform.sizeDelta.y < -canvasRectTransform.sizeDelta.y * 0.5f) {
                //    verticalFix = -canvasRectTransform.sizeDelta.y * 0.5f - (localPosOnCanvas.y - rectTransform.sizeDelta.y);
                //}

                //Check right.
                if (localPosOnCanvas.x + rectTransform.sizeDelta.x > canvasRectTransform.sizeDelta.x * 0.5f) {
                    horizontalFix = canvasRectTransform.sizeDelta.x * 0.5f - (localPosOnCanvas.x + rectTransform.sizeDelta.x);
                }

                //Fix position.
                selfRectTransform.anchoredPosition = new Vector2(selfRectTransform.anchoredPosition.x + horizontalFix, selfRectTransform.anchoredPosition.y + verticalFix);
            }
        }
    }

    //----------------------------------------------------------------------------------

    //Create an item with content.
    static public ExtensiveMenu Instantiate(string gameObjectName, RectTransform parent, Vector2 anchoredPosition) {
        ExtensiveMenu menu = Util.InstantiateModule<ExtensiveMenu>(PATH, parent);
        if (menu != null) {
            menu.GetRectTransform().anchoredPosition = anchoredPosition;

            //我們很愚蠢的使用wordposition y 來判斷 pivot 要在哪邊
            Vector2 worldPos = parent.TransformPoint(anchoredPosition);

            //Decide pivor (Menu relative position to cursor)
            if (worldPos.y > 0.0f) {
                menu.GetRectTransform().SetPivot(PivotPresets.TopLeft);
                menu.scrollRect.GetRectTransform().SetPivot(PivotPresets.TopLeft);
            } else {
                menu.GetRectTransform().SetPivot(PivotPresets.BottomLeft);
                menu.scrollRect.GetRectTransform().SetPivot(PivotPresets.BottomLeft);
            }

            return menu;
        } else {
            Debug.LogWarning("Oops! ExtensiveMenu Instantiate failed?!");
            return null;
        }
    }

    //This will return all performers found under the parent.
    static public bool DestroyUnder(RectTransform parent) {
        bool hasDestroy = false;
        ExtensiveMenu[] menus = parent.GetComponentsInChildren<ExtensiveMenu>();
        for (int i = 0; i < menus.Length; i++) {
            menus[i].Clear();
            GameObject.Destroy(menus[i].gameObject);
            hasDestroy = true;
        }
        return hasDestroy;
    }

}
