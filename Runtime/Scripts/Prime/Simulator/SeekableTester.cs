using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekableTester : UIControllable {

    private List<SeekableTesterToggle> toggleList = null;

    void Awake() {
        toggleList = new List<SeekableTesterToggle>(GetComponentsInChildren<SeekableTesterToggle>());
        StartListenToAmphitrite();
    }

    override protected void OnAmphitriteKeyDown(KeyConfig.Key key) {
        if (key == KeyConfig.Key.Right) {
            MoveFocus(UISeekable.SeekDirection.Right);
        }

        if (key == KeyConfig.Key.Left) {
            MoveFocus(UISeekable.SeekDirection.Left);
        }

        if (key == KeyConfig.Key.Up) {
            MoveFocus(UISeekable.SeekDirection.Up);
        }

        if (key == KeyConfig.Key.Down) {
            MoveFocus(UISeekable.SeekDirection.Down);
        }
    }

    override protected void OnAmphitriteKeyKeep(KeyConfig.Key key) {
        if (key == KeyConfig.Key.Right) {
            MoveFocus(UISeekable.SeekDirection.Right);
        }

        if (key == KeyConfig.Key.Left) {
            MoveFocus(UISeekable.SeekDirection.Left);
        }

        if (key == KeyConfig.Key.Up) {
            MoveFocus(UISeekable.SeekDirection.Up);
        }

        if (key == KeyConfig.Key.Down) {
            MoveFocus(UISeekable.SeekDirection.Down);
        }
    }

	private void MoveFocus(UISeekable.SeekDirection direction) {
        SeekableTesterToggle currentFocus = CurrentFocus();
		if (currentFocus == null) {
            toggleList[0].SetToggle(true);
        } else {
            SeekableTesterToggle nextFocus = currentFocus.SeekNeighbor(direction) as SeekableTesterToggle;
			if (nextFocus != null) {
                for (int i = 0; i < toggleList.Count; i++) {
					if (toggleList[i] == nextFocus) {
                        toggleList[i].SetToggle(true);
                    } else {
                        toggleList[i].SetToggle(false);
                    }
                }
			}
        }
    }

	private SeekableTesterToggle CurrentFocus() {
		for (int i = 0; i < toggleList.Count; i++) {
			if (toggleList[i].IsToggled()) {
                return toggleList[i];
            }
		}
        return null;
    }

}
