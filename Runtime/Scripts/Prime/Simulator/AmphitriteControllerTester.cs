using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmphitriteControllerTester : MonoBehaviour {

    //Ordered by KeyConfig.ControllerKey.
    public Image[] controllerKeyBtnImages;

    public GameObject LJTracker;
    public GameObject LRTracker;
    public GameObject CJTracker;
    public GameObject CBJTracker;
    public GameObject VJTracker;

    public Slider LTSlider;
    public Slider RTSlider;

    private float TRACKER_RANGE = 40.0f;

    void Start () {
        //Listen to Amphitrite.
        Amphitrite.onControllerLeftAxis = OnLeftJoystick;
        Amphitrite.onControllerRightAxis = OnRightJoystick;
        Amphitrite.onControllerCrossAxis = OnCrossJoystick;
        Amphitrite.onControllerComboAxis = OnComboJoystick;

        Amphitrite.onControllerVirtualAxis = OnVirtualJoystick;

        Amphitrite.onControllerDown = OnControllerDown;
        Amphitrite.onControllerKeep = OnControllerKeep;
        Amphitrite.onControllerUp = OnControllerUp;

        Amphitrite.onControllerLTAxis = OnControllerLTAxis;
        Amphitrite.onControllerRTAxis = OnControllerRTAxis;
    }
	
    private void OnControllerDown(KeyConfig.Key controllerKey) {
        controllerKeyBtnImages[(int)controllerKey].color = Color.gray;
    }

    private void OnControllerKeep(KeyConfig.Key controllerKey) {
        controllerKeyBtnImages[(int)controllerKey].color = Color.yellow;
    }

    private void OnControllerUp(KeyConfig.Key controllerKey) {
        controllerKeyBtnImages[(int)controllerKey].color = Color.white;
    }

    private void OnLeftJoystick(Vector2 vec) {
        LJTracker.transform.localPosition = vec * TRACKER_RANGE;
    }

    private void OnRightJoystick(Vector2 vec) {
		    LRTracker.transform.localPosition = vec * TRACKER_RANGE;
    }

    private void OnCrossJoystick(Vector2 vec) {
		    CJTracker.transform.localPosition = vec * TRACKER_RANGE;
    }

    private void OnComboJoystick(Vector2 vec) {
		    CBJTracker.transform.localPosition = vec * TRACKER_RANGE;
    }

    private void OnVirtualJoystick(Vector2 vec) {
        VJTracker.transform.localPosition = vec * TRACKER_RANGE;
    }

    private void OnControllerLTAxis(float value) {
        LTSlider.value = value;
    }

    private void OnControllerRTAxis(float value) {
        RTSlider.value = value;
    }
}
