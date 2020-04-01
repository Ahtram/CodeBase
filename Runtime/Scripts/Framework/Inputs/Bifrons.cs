using UnityEngine;
using System;
using System.Collections.Generic;

//The high level input event interface. Provide actions and enum for user.
public class Bifrons : MonoBehaviour {

    //All "button" type. (Some of these are emulated axis directions as buttons)
    public enum ButtonType {
        A,
        B,
        X,
        Y,
        Select,
        Start,
        LB,
        RB,
        LJ,
        RJ,
        LT,
        RT,

        //Left stick
        LJL,    //Left
        LJR,    //Right
        LJD,    //Down
        LJU,    //Up

        LJLD,   //Left-Down
        LJRD,   //Right-Down
        LJLU,   //Left-Up
        LJRU,   //Right-Up

        //Right stick
        RJL,    //Left
        RJR,    //Right
        RJD,    //Down
        RJU,    //Up

        RJLD,   //Left-Down
        RJRD,   //Right-Down
        RJLU,   //Left-Up
        RJRU,   //Right-Up

        //Cross stick
        CL,     //Left
        CR,     //Right
        CD,     //Down
        CU,     //Up

        CLD,    //Left-Down
        CRD,    //Right-Down
        CLU,    //Left-Up
        CRU     //Right-Up
    }

    //Also we have 3 differenct joystick. But I don't want to give them enums.
    //They are Left Joystick / Right Joystick / Cross Joystick.

    //The controller number we are listening.
    public Janus.ControllerNum controllerNum = Janus.ControllerNum.One;
    public Janus.Mode controllerMode = Janus.Mode.XBoxOne;

    //These will be called only when pressed/released.
    public Action<Janus.ControllerNum, ButtonType> onButtonDown;
    public Action<Janus.ControllerNum, ButtonType> onButtonKeep;
    public Action<Janus.ControllerNum, ButtonType> onButtonUp;

    //These will be called contineuesly.
    public Action<Janus.ControllerNum, Vector2> onLeftJoystick;
    public Action<Janus.ControllerNum, Vector2> onRightJoystick;
    public Action<Janus.ControllerNum, Vector2> onCrossJoystick;
    //Combo = LeftJoystick and CrossJoystick.
    public Action<Janus.ControllerNum, Vector2> onComboJoystick;

    //LT and RT are special. They are actually axis and behave differently on XBox and PS controllers.
    public Action<Janus.ControllerNum, float> onLT;
    public Action<Janus.ControllerNum, float> onRT;

    //For simulate LT/RT button up/down.
    private const float AXIS_DOWN_VALUE = 0.1f;
    private float m_ltValue = 0.0f;
    private float m_rtValue = 0.0f;

    //For simulate LJ and RJ direction down.
    private float m_ljHorizontalValue = 0.0f;
    private float m_ljVerticalValue = 0.0f;
    private float m_rjHorizontalValue = 0.0f;
    private float m_rjVerticalValue = 0.0f;
    private float m_cHorizontalValue = 0.0f;
    private float m_cVerticalValue = 0.0f;

    private class ButtonStatus {
        public enum State {
            NotDown,
            Down,
            Keep
        };

        public State state = State.NotDown;
        public bool keepIsActive = false;
        public float accumulateDelay = 0.0f;
    };

    //Ordered by ButtonType.
    private List<ButtonStatus> buttonStatus = new List<ButtonStatus>();

    private const float BUTTON_KEEP_ACTIVE_DELAY = 0.2f;
    private const float BUTTON_KEEP_REPEAT_DELAY = 0.1f;

    private void Awake() {
        buttonStatus.Clear();
        int buttonTypeCount = Enum.GetNames(typeof(ButtonType)).Length;
        for (int i = 0; i < buttonTypeCount; i++) {
            buttonStatus.Add(new ButtonStatus());
        }
    }

    /// <summary>
    /// This will reset all actions to null. Also clear the cache LT/RT value.
    /// </summary>
    public void Reset() {
        onButtonDown = null;
        onButtonKeep = null;
        onButtonUp = null;
        onLeftJoystick = null;
        onRightJoystick = null;
        onCrossJoystick = null;
        onComboJoystick = null;
        m_ltValue = 0.0f;
        m_rtValue = 0.0f;
        m_ljHorizontalValue = 0.0f;
        m_ljVerticalValue = 0.0f;
        m_rjHorizontalValue = 0.0f;
        m_rjVerticalValue = 0.0f;
        m_cHorizontalValue = 0.0f;
        m_cVerticalValue = 0.0f;
    }

    /// <summary>
    /// A convenient function for setting Janus's joystick input mode.
    /// And surely you can alter the mode on Janus directly. It will not cause any problem.
    /// </summary>
    public void SetJoystickMode(Janus.Mode mode) {
        controllerMode = mode;
    }

    //Yup, this is how Unity gets input.
	private void Update () {
        //Axis: 8

        float thisLJHorizontalValue = Janus.LJHorizontal(controllerMode, controllerNum);
        float thisLJVerticalValue = Janus.LJVertical(controllerMode, controllerNum);
        float thisRJHorizontalValue = Janus.RJHorizontal(controllerMode, controllerNum);
        float thisRJVerticalValue = Janus.RJVertical(controllerMode, controllerNum);
        float thisCHorizontalValue = Janus.CJHorizontal(controllerMode, controllerNum);
        float thisCVerticalValue = Janus.CJVertical(controllerMode, controllerNum);

        if (onLeftJoystick != null) {
            Vector2 ljAxisVec = new Vector2(thisLJHorizontalValue, thisLJVerticalValue);
            if (ljAxisVec.magnitude > 1.0f) {
                ljAxisVec.Normalize();
            }
            onLeftJoystick(controllerNum, ljAxisVec);
        }

        if (onRightJoystick != null) {
            Vector2 rjAxisVec = new Vector2(thisRJHorizontalValue, thisRJVerticalValue);
            if (rjAxisVec.magnitude > 1.0f) {
                rjAxisVec.Normalize();
            }
            onRightJoystick(controllerNum, rjAxisVec);
        }

        if (onCrossJoystick != null) {
            Vector2 cAxisVec = new Vector2(thisCHorizontalValue, thisCVerticalValue);
            if (cAxisVec.magnitude > 1.0f) {
                cAxisVec.Normalize();
            }
            onCrossJoystick(controllerNum, cAxisVec);
        }

        if (onComboJoystick != null) {
            Vector2 ljAxisVec = new Vector2(thisLJHorizontalValue, thisLJVerticalValue);
            if(Util.VecEqual(ljAxisVec, Vector2.zero)) {
                Vector2 cAxisVec = new Vector2(thisCHorizontalValue, thisCVerticalValue);
                if (cAxisVec.magnitude > 1.0f) {
                    cAxisVec.Normalize();
                }
                //Retrun CJ.
                onComboJoystick(controllerNum, cAxisVec);
            } else {
                //Return LJ.
                if (ljAxisVec.magnitude > 1.0f) {
                    ljAxisVec.Normalize();
                }
                onComboJoystick(controllerNum, ljAxisVec);
            }
        }

        //Scan LT/RT value
        float thisLTValue = Janus.LT(controllerMode, controllerNum);
        float thisRTValue = Janus.RT(controllerMode, controllerNum);

        if (onLT != null) {
            onLT(controllerNum, thisLTValue);
        }

        if (onRT != null) {
            onRT(controllerNum, thisRTValue);
        }

        //Buttons: 10

        if (Janus.AButtonDown(controllerMode, controllerNum)) {
            OnButtonDown(controllerNum, ButtonType.A);
        }

        if (Janus.BButtonDown(controllerMode, controllerNum)) {
            OnButtonDown(controllerNum, ButtonType.B);
        }

        if (Janus.XButtonDown(controllerMode, controllerNum)) {
            OnButtonDown(controllerNum, ButtonType.X);
        }

        if (Janus.YButtonDown(controllerMode, controllerNum)) {
            OnButtonDown(controllerNum, ButtonType.Y);
        }

        if (Janus.SelectButtonDown(controllerMode, controllerNum)) {
            OnButtonDown(controllerNum, ButtonType.Select);
        }

        if (Janus.StartButtonDown(controllerMode, controllerNum)) {
            OnButtonDown(controllerNum, ButtonType.Start);
        }

        if (Janus.LBButtonDown(controllerMode, controllerNum)) {
            OnButtonDown(controllerNum, ButtonType.LB);
        }

        if (Janus.RBButtonDown(controllerMode, controllerNum)) {
            OnButtonDown(controllerNum, ButtonType.RB);
        }

        if (Janus.LJButtonDown(controllerMode, controllerNum)) {
            OnButtonDown(controllerNum, ButtonType.LJ);
        }

        if (Janus.RJButtonDown(controllerMode, controllerNum)) {
            OnButtonDown(controllerNum, ButtonType.RJ);
        }

        //Special: LT and RT as buttons.
        if (m_ltValue < AXIS_DOWN_VALUE) {
            if (thisLTValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.LT);
            }
        }
        if (m_rtValue < AXIS_DOWN_VALUE) {
            if (thisRTValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.RT);
            }
        }

        //LJL/LJR/LJD/LJU as buttons.
        if (m_ljHorizontalValue < AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.LJR);
            }
        }
        if (m_ljHorizontalValue > -AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.LJL);
            }
        }
        if (m_ljVerticalValue < AXIS_DOWN_VALUE) {
            if (thisLJVerticalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.LJU);
            }
        }
        if (m_ljVerticalValue > -AXIS_DOWN_VALUE) {
            if (thisLJVerticalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.LJD);
            }
        }

        //LJLD/LJRD/LJLU/LJRU as buttons.
        if (m_ljHorizontalValue > -AXIS_DOWN_VALUE || m_ljVerticalValue > -AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue <= -AXIS_DOWN_VALUE && thisLJVerticalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.LJLD);
            }
        }
        if (m_ljHorizontalValue < AXIS_DOWN_VALUE || m_ljVerticalValue > -AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue >= AXIS_DOWN_VALUE && thisLJVerticalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.LJRD);
            }
        }
        if (m_ljHorizontalValue > -AXIS_DOWN_VALUE || m_ljVerticalValue < AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue <= -AXIS_DOWN_VALUE && thisLJVerticalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.LJLU);
            }
        }
        if (m_ljHorizontalValue < AXIS_DOWN_VALUE || m_ljVerticalValue < AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue >= AXIS_DOWN_VALUE && thisLJVerticalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.LJRU);
            }
        }

        //RJL/RJR/RJD/RJU as buttons.
        if (m_rjHorizontalValue < AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.RJR);
            }
        }
        if (m_rjHorizontalValue > -AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.RJL);
            }
        }
        if (m_rjVerticalValue < AXIS_DOWN_VALUE) {
            if (thisRJVerticalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.RJU);
            }
        }
        if (m_rjVerticalValue > -AXIS_DOWN_VALUE) {
            if (thisRJVerticalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.RJD);
            }
        }

        //RJLD/RJRD/RJLU/RJRU as buttons.
        if (m_rjHorizontalValue > -AXIS_DOWN_VALUE || m_rjVerticalValue > -AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue <= -AXIS_DOWN_VALUE && thisRJVerticalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.RJLD);
            }
        }
        if (m_rjHorizontalValue < AXIS_DOWN_VALUE || m_rjVerticalValue > -AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue >= AXIS_DOWN_VALUE && thisRJVerticalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.RJRD);
            }
        }
        if (m_rjHorizontalValue > -AXIS_DOWN_VALUE || m_rjVerticalValue < AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue <= -AXIS_DOWN_VALUE && thisRJVerticalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.RJLU);
            }
        }
        if (m_rjHorizontalValue < AXIS_DOWN_VALUE || m_rjVerticalValue < AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue >= AXIS_DOWN_VALUE && thisRJVerticalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.RJRU);
            }
        }

        //CL/CR/CD/CU as buttons.
        if (m_cHorizontalValue < AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.CR);
            }
        }
        if (m_cHorizontalValue > -AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.CL);
            }
        }
        if (m_cVerticalValue < AXIS_DOWN_VALUE) {
            if (thisCVerticalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.CU);
            }
        }
        if (m_cVerticalValue > -AXIS_DOWN_VALUE) {
            if (thisCVerticalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.CD);
            }
        }

        //CLD/CRD/CLU/CRU as buttons.
        if (m_cHorizontalValue > -AXIS_DOWN_VALUE || m_cVerticalValue > -AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue <= -AXIS_DOWN_VALUE && thisCVerticalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.CLD);
            }
        }
        if (m_cHorizontalValue < AXIS_DOWN_VALUE || m_cVerticalValue > -AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue >= AXIS_DOWN_VALUE && thisCVerticalValue <= -AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.CRD);
            }
        }
        if (m_cHorizontalValue > -AXIS_DOWN_VALUE || m_cVerticalValue < AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue <= -AXIS_DOWN_VALUE && thisCVerticalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.CLU);
            }
        }
        if (m_cHorizontalValue < AXIS_DOWN_VALUE || m_cVerticalValue < AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue >= AXIS_DOWN_VALUE && thisCVerticalValue >= AXIS_DOWN_VALUE) {
                OnButtonDown(controllerNum, ButtonType.CRU);
            }
        }

        //======================================================================

        OnKeep(Time.deltaTime);

        //======================================================================

        if (Janus.AButtonUp(controllerMode, controllerNum)) {
            OnButtonUp(controllerNum, ButtonType.A);
        }

        if (Janus.BButtonUp(controllerMode, controllerNum)) {
            OnButtonUp(controllerNum, ButtonType.B);
        }

        if (Janus.XButtonUp(controllerMode, controllerNum)) {
            OnButtonUp(controllerNum, ButtonType.X);
        }

        if (Janus.YButtonUp(controllerMode, controllerNum)) {
            OnButtonUp(controllerNum, ButtonType.Y);
        }

        if (Janus.SelectButtonUp(controllerMode, controllerNum)) {
            OnButtonUp(controllerNum, ButtonType.Select);
        }

        if (Janus.StartButtonUp(controllerMode, controllerNum)) {
            OnButtonUp(controllerNum, ButtonType.Start);
        }

        if (Janus.LBButtonUp(controllerMode, controllerNum)) {
            OnButtonUp(controllerNum, ButtonType.LB);
        }

        if (Janus.RBButtonUp(controllerMode, controllerNum)) {
            OnButtonUp(controllerNum, ButtonType.RB);
        }

        if (Janus.LJButtonUp(controllerMode, controllerNum)) {
            OnButtonUp(controllerNum, ButtonType.LJ);
        }

        if (Janus.RJButtonUp(controllerMode, controllerNum)) {
            OnButtonUp(controllerNum, ButtonType.RJ);
        }

        //Special: LT and RT as buttons.
        if (m_ltValue >= AXIS_DOWN_VALUE) {
            if (thisLTValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.LT);
            }
        }
        if (m_rtValue >= AXIS_DOWN_VALUE) {
            if (thisRTValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.RT);
            }
        }

        //LJL/LJR/LJD/LJU as buttons.
        if (m_ljHorizontalValue >= AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.LJR);
            }
        }
        if (m_ljHorizontalValue < -AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue >= -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.LJL);
            }
        }
        if (m_ljVerticalValue >= AXIS_DOWN_VALUE) {
            if (thisLJVerticalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.LJU);
            }
        }
        if (m_ljVerticalValue <= -AXIS_DOWN_VALUE) {
            if (thisLJVerticalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.LJD);
            }
        }

        //LJLD/LJRD/LJLU/LJRU as buttons.
        if (m_ljHorizontalValue <= -AXIS_DOWN_VALUE && m_ljVerticalValue <= -AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue > -AXIS_DOWN_VALUE || thisLJVerticalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.LJLD);
            }
        }
        if (m_ljHorizontalValue >= AXIS_DOWN_VALUE && m_ljVerticalValue <= -AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue < AXIS_DOWN_VALUE || thisLJVerticalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.LJRD);
            }
        }
        if (m_ljHorizontalValue <= -AXIS_DOWN_VALUE && m_ljVerticalValue >= AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue > -AXIS_DOWN_VALUE || thisLJVerticalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.LJLU);
            }
        }
        if (m_ljHorizontalValue >= AXIS_DOWN_VALUE && m_ljVerticalValue >= AXIS_DOWN_VALUE) {
            if (thisLJHorizontalValue < AXIS_DOWN_VALUE || thisLJVerticalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.LJRU);
            }
        }

        //RJL/RJR/RJD/RJU as buttons.
        if (m_rjHorizontalValue >= AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.RJR);
            }
        }
        if (m_rjHorizontalValue <= -AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.RJL);
            }
        }
        if (m_rjVerticalValue >= AXIS_DOWN_VALUE) {
            if (thisRJVerticalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.RJU);
            }
        }
        if (m_rjVerticalValue <= -AXIS_DOWN_VALUE) {
            if (thisRJVerticalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.RJD);
            }
        }

        //RJLD/RJRD/RJLU/RJRU as buttons.
        if (m_rjHorizontalValue <= -AXIS_DOWN_VALUE && m_rjVerticalValue <= -AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue > -AXIS_DOWN_VALUE || thisRJVerticalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.RJLD);
            }
        }
        if (m_rjHorizontalValue >= AXIS_DOWN_VALUE && m_rjVerticalValue <= -AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue < AXIS_DOWN_VALUE || thisRJVerticalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.RJRD);
            }
        }
        if (m_rjHorizontalValue <= -AXIS_DOWN_VALUE && m_rjVerticalValue >= AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue > -AXIS_DOWN_VALUE || thisRJVerticalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.RJLU);
            }
        }
        if (m_rjHorizontalValue >= AXIS_DOWN_VALUE && m_rjVerticalValue >= AXIS_DOWN_VALUE) {
            if (thisRJHorizontalValue < AXIS_DOWN_VALUE || thisRJVerticalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.RJRU);
            }
        }

        //CL/CR/CD/CU as buttons.
        if (m_cHorizontalValue >= AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.CR);
            }
        }
        if (m_cHorizontalValue <= -AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.CL);
            }
        }
        if (m_cVerticalValue >= AXIS_DOWN_VALUE) {
            if (thisCVerticalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.CU);
            }
        }
        if (m_cVerticalValue <= -AXIS_DOWN_VALUE) {
            if (thisCVerticalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.CD);
            }
        }

        //CLD/CRD/CLU/CRU as buttons.
        if (m_cHorizontalValue <= -AXIS_DOWN_VALUE && m_cVerticalValue <= -AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue > -AXIS_DOWN_VALUE || thisCVerticalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.CLD);
            }
        }
        if (m_cHorizontalValue >= AXIS_DOWN_VALUE && m_cVerticalValue <= -AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue < AXIS_DOWN_VALUE || thisCVerticalValue > -AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.CRD);
            }
        }
        if (m_cHorizontalValue <= -AXIS_DOWN_VALUE && m_cVerticalValue >= AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue > -AXIS_DOWN_VALUE || thisCVerticalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.CLU);
            }
        }
        if (m_cHorizontalValue >= AXIS_DOWN_VALUE && m_cVerticalValue >= AXIS_DOWN_VALUE) {
            if (thisCHorizontalValue < AXIS_DOWN_VALUE || thisCVerticalValue < AXIS_DOWN_VALUE) {
                OnButtonUp(controllerNum, ButtonType.CRU);
            }
        }

        //!! Do these last !!! Cache axis value for each frame for emulate axis direction as buttons!
        m_ltValue = thisLTValue;
        m_rtValue = thisRTValue;
        m_ljHorizontalValue = thisLJHorizontalValue;
        m_ljVerticalValue = thisLJVerticalValue;
        m_rjHorizontalValue = thisRJHorizontalValue;
        m_rjVerticalValue = thisRJVerticalValue;
        m_cHorizontalValue = thisCHorizontalValue;
        m_cVerticalValue = thisCVerticalValue;
	}

    private void OnButtonDown(Janus.ControllerNum controllerNum, ButtonType buttonType) {
        if(buttonStatus[(int)buttonType].state == ButtonStatus.State.NotDown) {
            buttonStatus[(int)buttonType].state = ButtonStatus.State.Down;
        }

        if (onButtonDown != null) {
            onButtonDown(controllerNum, buttonType);
        }
    }

    private void OnButtonUp(Janus.ControllerNum controllerNum, ButtonType buttonType) {
        if (buttonStatus[(int)buttonType].state != ButtonStatus.State.NotDown) {
            buttonStatus[(int)buttonType].state = ButtonStatus.State.NotDown;
            buttonStatus[(int)buttonType].accumulateDelay = 0.0f;
        }

        if (onButtonUp != null) {
            onButtonUp(controllerNum, buttonType);
        }
    }

    private void OnKeep(float deltaTime) {
        if (onButtonKeep != null) {
            for (int i = 0; i < buttonStatus.Count; i++) {
                if (buttonStatus[i].state == ButtonStatus.State.Down) {
                    buttonStatus[i].accumulateDelay += deltaTime;
                    if (buttonStatus[i].accumulateDelay >= BUTTON_KEEP_ACTIVE_DELAY) {
                        buttonStatus[i].state = ButtonStatus.State.Keep;
                        buttonStatus[i].accumulateDelay -= BUTTON_KEEP_ACTIVE_DELAY;
                        onButtonKeep(controllerNum, (ButtonType)i);
                    }
                } else if (buttonStatus[i].state == ButtonStatus.State.Keep) {
                    buttonStatus[i].accumulateDelay += deltaTime;
                    if (buttonStatus[i].accumulateDelay >= BUTTON_KEEP_REPEAT_DELAY) {
                        buttonStatus[i].accumulateDelay -= BUTTON_KEEP_REPEAT_DELAY;
                        onButtonKeep(controllerNum, (ButtonType)i);
                    }
                }
            }
        }
    }

}
