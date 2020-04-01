using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmphitriteKeyboardTester : MonoBehaviour {

    //Ordered by KeyConfig.KeyboardKey.
    public Image[] keyboardKeyKeyBtnImages;
    public GameObject axisTracker;

    private float TRACKER_RANGE = 40.0f;

    void Start() {
        //Listen to Amphitrite.
        Amphitrite.onKeyboardDown = OnKeyboardDown;
        Amphitrite.onKeyboardKeep = OnKeyboardKeep;
        Amphitrite.onKeyboardUp = OnKeyboardUp;
        Amphitrite.onKeyboardAxis = OnKeyboardAxis;
    }

	private void OnKeyboardDown(KeyConfig.Key keyboardKey) {
		keyboardKeyKeyBtnImages[(int)keyboardKey].color = Color.gray;
	}

	private void OnKeyboardKeep(KeyConfig.Key keyboardKey) {
		keyboardKeyKeyBtnImages[(int)keyboardKey].color = Color.yellow;
    }

	private void OnKeyboardUp(KeyConfig.Key keyboardKey) {
		keyboardKeyKeyBtnImages[(int)keyboardKey].color = Color.white;
    }

    private void OnKeyboardAxis(Vector2 vec){
        axisTracker.transform.localPosition = vec * TRACKER_RANGE;
    }

}
