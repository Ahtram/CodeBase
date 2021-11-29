using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Teamuni.Codebase;

/// <summary>
/// For test ExtensiveMenu
/// </summary>
public class ExtensiveMenuTester : UIControllable {
    public RectTransform mountPoint;
    public IDSelector iDSelector;
    public AssetNameSelector assetNameSelector;
    public ContentSelector contentSelector;

    private ExtensiveMenu extensiveMenu;

    void Awake() {
        StartListenToAmphitrite();
        List<string> testContent = new List<string>() {
            "ABC/DEF/BBB.txt",
            "ABC/DEF/CCC.txt",
            "ABC/123/111.txt",
            "ABC/123/222.txt",
            "ABC/123/333.json",
        };
        contentSelector.Setup(testContent);
    }

    public void OnMountPointClicked(BaseEventData baseEventData) {
        if (iDSelector.IsOpeningAnExtensiveMenu()) {
            // iDSelector.CloseExtensiveMenu();
        } else if (assetNameSelector.IsOpeningAnExtensiveMenu()) {
            // assetNameSelector.CloseExtensiveMenu();
        } else if (contentSelector.IsOpeningAnExtensiveMenu()) {
            // contentSelector.CloseExtensiveMenu();
        } else {
            PointerEventData pointerEventData = (PointerEventData)baseEventData;
            if (extensiveMenu == null) {
                OpenMenu(mountPoint.InverseTransformPoint(pointerEventData.pointerCurrentRaycast.worldPosition));
            } else {
                CloseMenu();
            }
        }
    }

    private void OpenMenu(Vector2 position) {
        if (extensiveMenu == null) {
            extensiveMenu = ExtensiveMenu.Instantiate("[ExtensiveMenu]", mountPoint, position);

            //測試隨便資料
            extensiveMenu.AddItem("AAADEFGHIJKLMNOPQRSTUVWXYZ/DEF/AAADEFGHIJKLMNOPQRSTUVWXYZ", false, OnStrSelect, "AAA");
            extensiveMenu.AddItem("ABC/DEF/BBB", false, OnStrSelect, "BBB");
            extensiveMenu.AddItem("ABC/DEF/CCC", false, OnStrSelect, "CCC");
            extensiveMenu.AddItem("ABC/123/111", false, OnNumSelect, "111");
            extensiveMenu.AddItem("ABC/123/222", false, OnNumSelect, "222");
            extensiveMenu.AddItem("ABC/123/333", false, OnNumSelect, "333");
            extensiveMenu.AddItem("ABC/ASDF", false, OnStrSelect, "ASDF");
            extensiveMenu.AddItem("XYZ/FDS/222", false, OnNumSelect, "222");
            extensiveMenu.AddItem("XYZ/FDS/333", false, OnNumSelect, "333");
            extensiveMenu.AddItem("XYZ/BVNM", false, OnStrSelect, "BVMN");
            extensiveMenu.AddItem("9876543210", false, OnNumSelect, "9876543210");
            extensiveMenu.AddItem("11", false, OnNumSelect, "11");
            extensiveMenu.AddItem("22", false, OnNumSelect, "22");
            extensiveMenu.AddItem("33", false, OnNumSelect, "33");
            extensiveMenu.AddItem("44", false, OnNumSelect, "44");
            extensiveMenu.AddItem("55", false, OnNumSelect, "55");
            extensiveMenu.AddItem("66", false, OnNumSelect, "66");
            extensiveMenu.AddItem("77", false, OnNumSelect, "77");
            extensiveMenu.AddItem("88", false, OnNumSelect, "88");
            extensiveMenu.AddItem("99", false, OnNumSelect, "99");
            extensiveMenu.AddItem("00", false, OnNumSelect, "00");
            extensiveMenu.AddItem("01", false, OnNumSelect, "01");
            extensiveMenu.AddItem("02", false, OnNumSelect, "02");
            extensiveMenu.AddItem("03", false, OnNumSelect, "03");
            extensiveMenu.AddItem("04", false, OnNumSelect, "04");
            extensiveMenu.AddItem("05", false, OnNumSelect, "05");
            extensiveMenu.AddItem("06", true, OnNumSelect, "06");
            extensiveMenu.AddItem("07", false, OnNumSelect, "07");
            extensiveMenu.AddItem("08", false, OnNumSelect, "08");
            extensiveMenu.AddItem("09", false, OnNumSelect, "09");
        }
    }

    private void OnStrSelect(object obj) {
        Debug.Log("OnStrSelect: " + ((string)obj).ToString());
        CloseMenu();
    }

    private void OnNumSelect(object obj) {
        Debug.Log("OnNumSelect: " + ((string)obj).ToString());
        CloseMenu();
    }

    private void CloseMenu() {
        if (extensiveMenu != null) {
            ExtensiveMenu.DestroyUnder(mountPoint);
            extensiveMenu = null;
        }
    }

    override protected void OnAmphitriteKeyDown(KeyConfig.Key key) {
        if (key == KeyConfig.Key.Down) {
            iDSelector.SelectNext();
        }

        if (key == KeyConfig.Key.Up) {
            iDSelector.SelectPrevious();
        }

        if (key == KeyConfig.Key.Left) {
            assetNameSelector.SelectNext();
        }

        if (key == KeyConfig.Key.Right) {
            assetNameSelector.SelectPrevious();
        }

        if (key == KeyConfig.Key.Next) {
            contentSelector.SelectNext();
        }

        if (key == KeyConfig.Key.Previous) {
            contentSelector.SelectPrevious();
        }
    }

    override protected void OnAmphitriteKeyKeep(KeyConfig.Key key) {
        if (key == KeyConfig.Key.Down) {
            iDSelector.SelectNext();
        }

        if (key == KeyConfig.Key.Up) {
            iDSelector.SelectPrevious();
        }

        if (key == KeyConfig.Key.Left) {
            assetNameSelector.SelectNext();
        }

        if (key == KeyConfig.Key.Right) {
            assetNameSelector.SelectPrevious();
        }

        if (key == KeyConfig.Key.Next) {
            contentSelector.SelectNext();
        }

        if (key == KeyConfig.Key.Previous) {
            contentSelector.SelectPrevious();
        }
    }

}
