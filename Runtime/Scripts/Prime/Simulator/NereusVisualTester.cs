using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NereusVisualTester : MonoBehaviour {

    //Btn images ordered by ButtonType enum.
    public Image[] btnImages;

    //Controller status text ordered by detected controllers.
    public Text controllerStatus;

    private float TRACKER_RANGE = 40.0f;

    public GameObject LJTracker;
    public GameObject RJTracker;
    public GameObject CJTracker;
    public GameObject CBJTracker;

    public Image LTFill;
    public Image RTFill;

    public int controllerNum = 1;
    private Nereus controller = null;

    public void Setup(Nereus nereus, Triton.ControllerType type) {
        controller = nereus;
        controller.Reset();

        if(type != Triton.ControllerType.None) {
            controller.onButtonDown += OnButtonDown;
            controller.onButtonKeep += OnButtonKeep;
            controller.onButtonUp += OnButtonUp;

            controller.onLeftJoystick += OnLeftJoystick;
            controller.onRightJoystick += OnRightJoystick;
            controller.onCrossJoystick += OnCrossJoystick;
            controller.onComboJoystick += OnComboJoystick;

            controller.onLT += OnLT;
            controller.onRT += OnRT;
        }

        controllerStatus.text = type.ToString();
    }

    private void OnButtonDown(int controllerNum, Nereus.InputType btn) {
        if (controllerNum == controller.controllerNum)
            btnImages[(int)btn].color = Color.gray;
    }

    private void OnButtonKeep(int controllerNum, Nereus.InputType btn) {
        if (controllerNum == controller.controllerNum)
            btnImages[(int)btn].color = Color.yellow;
    }

    private void OnButtonUp(int controllerNum, Nereus.InputType btn) {
        if (controllerNum == controller.controllerNum)
            btnImages[(int)btn].color = Color.white;
    }

    private void OnLeftJoystick(int controllerNum, Vector2 vec) {
        if (controllerNum == controller.controllerNum)
            LJTracker.transform.localPosition = vec * TRACKER_RANGE;
    }

    private void OnRightJoystick(int controllerNum, Vector2 vec) {
        if (controllerNum == controller.controllerNum)
            RJTracker.transform.localPosition = vec * TRACKER_RANGE;
    }

    private void OnCrossJoystick(int controllerNum, Vector2 vec) {
        if (controllerNum == controller.controllerNum)
            CJTracker.transform.localPosition = vec * TRACKER_RANGE;
    }

    private void OnComboJoystick(int controllerNum, Vector2 vec) {
        if (controllerNum == controller.controllerNum)
            CBJTracker.transform.localPosition = vec * TRACKER_RANGE;
    }

    private void OnLT(int controllerNum, float value) {
        LTFill.fillAmount = value;
    }

    private void OnRT(int controllerNum, float value) {
        RTFill.fillAmount = value;
    }

}
