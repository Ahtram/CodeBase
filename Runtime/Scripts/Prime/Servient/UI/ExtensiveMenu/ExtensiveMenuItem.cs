using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

/// <summary>
/// A single ExtensiveMenu item.
/// </summary>
public class ExtensiveMenuItem : UIBase {

    public const string PATH = "Prefabs/ExtensiveMenu/ExtensiveMenuItem";

    public Action<ExtensiveMenu.ItemContent> onItemClick;
    public Action<ExtensiveMenu.ItemContent, ExtensiveMenuItem> onItemEnter;

    public TernaryButton button;
    public RawImage toggle;
    public RawImage arrow;

    public RectTransform subMenuLeftAnchor;
    public RectTransform subMenuRightAnchor;

    //Displaying content.
    private ExtensiveMenu.ItemContent m_content = null;

    public void Setup(ExtensiveMenu.ItemContent content) {
        if (content != null) {
            m_content = content;
            button.SetupStr(m_content.DisplayStr);
            toggle.enabled = m_content.on;
            arrow.enabled = content.HasSubList();
        }
    }

    public void SetupAsEmpty() {
        Clear();
        button.SetupStr("-- Empty --");
    }

    public void Clear() {
        m_content = null;
        button.SetupStr("--");
        toggle.enabled = false;
        arrow.enabled = false;
    }

    public void OnItemClick() {
        onItemClick?.Invoke(m_content);
    }

    public void OnItemEnter() {
        onItemEnter?.Invoke(m_content, this);
    }

    //給子選單用
    public void OnItemClick(ExtensiveMenu.ItemContent itemContent) {
        onItemClick?.Invoke(itemContent);
    }

    //----------------------------------------------------------

    //Create an item with content.
    static public ExtensiveMenuItem Instantiate(string gameObjectName, RectTransform parent, ExtensiveMenu.ItemContent content,
    Action<ExtensiveMenu.ItemContent> onItemClick, Action<ExtensiveMenu.ItemContent, ExtensiveMenuItem> onItemEnter) {
        ExtensiveMenuItem item = Util.InstantiateModule<ExtensiveMenuItem>(PATH, parent);
        if (item != null) {
            item.Setup(content);
            item.onItemClick = onItemClick;
            item.onItemEnter = onItemEnter;
            return item;
        } else {
            Debug.LogWarning("Oops! ExtensiveMenuItem Instantiate failed?!");
            return null;
        }
    }

    //Create an empty item.
    static public ExtensiveMenuItem InstantiateAsEmpty(string gameObjectName, RectTransform parent,
    Action<ExtensiveMenu.ItemContent> onItemClick, Action<ExtensiveMenu.ItemContent, ExtensiveMenuItem> onItemEnter) {
        ExtensiveMenuItem item = Util.InstantiateModule<ExtensiveMenuItem>(PATH, parent);
        if (item != null) {
            item.SetupAsEmpty();
            item.onItemClick = onItemClick;
            item.onItemEnter = onItemEnter;
            return item;
        } else {
            Debug.LogWarning("Oops! ExtensiveMenuItem Instantiate failed?!");
            return null;
        }
    }

    //This will return all performers found under the parent.
    static public bool DestroyUnder(RectTransform parent) {
        bool hasDestroy = false;
        ExtensiveMenuItem[] items = parent.GetComponentsInChildren<ExtensiveMenuItem>();
        for (int i = 0; i < items.Length; i++) {
            items[i].Clear();
            GameObject.Destroy(items[i].gameObject);
            hasDestroy = true;
        }
        return hasDestroy;
    }

}
