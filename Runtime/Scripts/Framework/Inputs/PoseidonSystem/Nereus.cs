using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// A component for listen to a specific controller. 
/// Provide actions and enum for the user.
/// This component should exist mutiple instance and managed by the Poseidon.
/// </summary>
public class Nereus : MonoBehaviour {

    //All "button" type. (Some of these are emulated axis directions as buttons)
    public enum InputType {
        A,
        B,
        X,
        Y,
        LB,
        RB,
        Select,
        Start,
        LJ,
        RJ,

        //Special button
        LT,
        RT,

        //Left axis
        LJL,    //Left
        LJR,    //Right
        LJD,    //Down
        LJU,    //Up

        LJLD,   //Left-Down
        LJRD,   //Right-Down
        LJLU,   //Left-Up
        LJRU,   //Right-Up

        //Right axis
        RJL,    //Left
        RJR,    //Right
        RJD,    //Down
        RJU,    //Up

        RJLD,   //Left-Down
        RJRD,   //Right-Down
        RJLU,   //Left-Up
        RJRU,   //Right-Up

        //Cross axis
        CL,     //Left
        CR,     //Right
        CD,     //Down
        CU,     //Up

        CLD,    //Left-Down
        CRD,    //Right-Down
        CLU,    //Left-Up
        CRU,    //Right-Up

        //Combo axis
        CBL,     //Left
        CBR,     //Right
        CBD,     //Down
        CBU,     //Up

        CBLD,    //Left-Down
        CBRD,    //Right-Down
        CBLU,    //Left-Up
        CBRU,    //Right-Up

        None    //This will not appear by logic.
    };

    //Also we have 3 differenct joystick. But I don't want to give them enums.
    //They are Left Joystick / Right Joystick / Cross Joystick.

    //The controller number we are listening. (1 ~ 8) (Don't use the wrong number or I'll kill you)
    public int controllerNum = 1;
    public Oceanus.Mode controllerMode = Oceanus.Mode.XBoxOne;

    //Normalize PS4 LT/RT value from -1~1 to 0~1?
    public bool normalizeLTRTValue = true;

    //These will be called only when pressed/released.
    public Action<int, InputType> onButtonDown;
    public Action<int, InputType> onButtonKeep;
    public Action<int, InputType> onButtonUp;

    //These will be called contineuesly.
    public Action<int, Vector2> onLeftJoystick;
    public Action<int, Vector2> onRightJoystick;
    public Action<int, Vector2> onCrossJoystick;
    //Combo = LeftJoystick and CrossJoystick and VirtualJoystick.
    public Action<int, Vector2> onComboJoystick;

    //Support for Third-party virtual joystick. (We need a reference to work)
    public VariableJoystick variableJoystick;
    public Action<int, Vector2> onVJoystick;

    //LT and RT are special. They are actually axis and behave differently on XBox and PS controllers.
    public Action<int, float> onLT;
    public Action<int, float> onRT;

    //The virtual button we're checking in Update()
    public List<NereusVirtualButton> virtualButtons = new List<NereusVirtualButton>();

    //For simulate LT/RT button up/down.
    public const float AXIS_DOWN_VALUE = 0.1f;
    private float m_ltValue = 0.0f;
    private float m_rtValue = 0.0f;

    //For simulate LJ and RJ direction down.
    private float m_ljHorizontalValue = 0.0f;
    private float m_ljVerticalValue = 0.0f;
    private float m_rjHorizontalValue = 0.0f;
    private float m_rjVerticalValue = 0.0f;
    private float m_cHorizontalValue = 0.0f;
    private float m_cVerticalValue = 0.0f;

    //For simulate VJ direction down.
    private float m_vHorizontalValue = 0.0f;
    private float m_vVerticalValue = 0.0f;

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

    public float buttonEnterRepeatDelay = 0.2f;
    public float buttonRepeatInterval = 0.1f;

    private void Awake() {
        buttonStatus.Clear();
        int inputTypeCount = Enum.GetNames(typeof(InputType)).Length;
        for (int i = 0; i < inputTypeCount; i++) {
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
        onVJoystick = null;
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
    /// A convenient function for setting Oceanus's joystick input mode.
    /// And surely you can alter the mode on Oceanus directly. It will not cause any problem.
    /// </summary>
    public void SetJoystickMode(Oceanus.Mode mode) {
        controllerMode = mode;
    }

    //Yup, this is how Unity gets input.
    private void Update() {
        //Axis: 8

        float thisLJHorizontalValue = 0.0f;
        float thisLJVerticalValue = 0.0f;
        float thisRJHorizontalValue = 0.0f;
        float thisRJVerticalValue = 0.0f;
        float thisCHorizontalValue = 0.0f;
        float thisCVerticalValue = 0.0f;

        if (onLeftJoystick != null || onComboJoystick != null || onButtonDown != null || onButtonUp != null) {
            thisLJHorizontalValue = Oceanus.GetAxisHorizontal(controllerMode, controllerNum, Oceanus.Axis.LJ);
            thisLJVerticalValue = Oceanus.GetAxisVertical(controllerMode, controllerNum, Oceanus.Axis.LJ);
        }

        if (onRightJoystick != null || onButtonDown != null || onButtonUp != null) {
            thisRJHorizontalValue = Oceanus.GetAxisHorizontal(controllerMode, controllerNum, Oceanus.Axis.RJ);
            thisRJVerticalValue = Oceanus.GetAxisVertical(controllerMode, controllerNum, Oceanus.Axis.RJ);
        }

        if (onCrossJoystick != null || onComboJoystick != null || onButtonDown != null || onButtonUp != null) {
            thisCHorizontalValue = Oceanus.GetAxisHorizontal(controllerMode, controllerNum, Oceanus.Axis.Cross);
            thisCVerticalValue = Oceanus.GetAxisVertical(controllerMode, controllerNum, Oceanus.Axis.Cross);
        }

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
            if (Util.VecEqual(ljAxisVec, Vector2.zero)) {
                //Return VJ?
                if (variableJoystick == null || Util.VecEqual(variableJoystick.Direction, Vector2.zero)) {
                    Vector2 cAxisVec = new Vector2(thisCHorizontalValue, thisCVerticalValue);
                    if (cAxisVec.magnitude > 1.0f) {
                        cAxisVec.Normalize();
                    }
                    //Retrun CJ.
                    onComboJoystick(controllerNum, cAxisVec);
                } else {
                    //Retrun VJ.
                    onComboJoystick(controllerNum, variableJoystick.Direction);
                }
            } else {
                //Return LJ.
                if (ljAxisVec.magnitude > 1.0f) {
                    ljAxisVec.Normalize();
                }
                onComboJoystick(controllerNum, ljAxisVec);
            }
        }

        if (onVJoystick != null && variableJoystick != null) {
            onVJoystick(controllerNum, variableJoystick.Direction);
        }

        //Scan LT/RT value
        float thisLTValue = 0.0f;
        float thisRTValue = 0.0f;

        if (onLT != null || onButtonDown != null || onButtonUp != null) {
            //The LT/RT value for XBox is 0~1
            //The LT/RT value for PS4 is -1~1
            thisLTValue = Oceanus.GetAxisLT(controllerMode, controllerNum);

            //Normalize PS4 LT/RT value from -1~1 to 0~1?
            if (normalizeLTRTValue && controllerMode == Oceanus.Mode.PS4) {
                thisLTValue = (thisLTValue + 1.0f) * 0.5f;
            }
        }

        if (onRT != null || onButtonDown != null || onButtonUp != null) {
            //The LT/RT value for XBox is 0~1
            //The LT/RT value for PS4 is -1~1
            thisRTValue = Oceanus.GetAxisRT(controllerMode, controllerNum);

            //Normalize PS4 LT/RT value from -1~1 to 0~1?
            if (normalizeLTRTValue && controllerMode == Oceanus.Mode.PS4) {
                thisRTValue = (thisRTValue + 1.0f) * 0.5f;
            }
        }

        onLT?.Invoke(controllerNum, thisLTValue);
        onRT?.Invoke(controllerNum, thisRTValue);

        if (onButtonDown != null || onButtonUp != null) {
            //Buttons: 10

            //Combo button down detect.
            bool CBLDown = false;
            bool CBRDown = false;
            bool CBDDown = false;
            bool CBUDown = false;
            bool CBLDDown = false;
            bool CBRDDown = false;
            bool CBLUDown = false;
            bool CBRUDown = false;

            for (int i = 0; i < Oceanus.USED_BUTTON_ENUM_COUNT; i++) {
                if (Oceanus.GetButtonDown(controllerMode, controllerNum, (Oceanus.Button)i)) {
                    OnButtonDown(controllerNum, (InputType)i);
                }
            }

            //Special: LT and RT as buttons.
            if (m_ltValue < AXIS_DOWN_VALUE) {
                if (thisLTValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.LT);
                }
            }
            if (m_rtValue < AXIS_DOWN_VALUE) {
                if (thisRTValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.RT);
                }
            }

            //LJL/LJR/LJD/LJU as buttons.
            if (m_ljHorizontalValue < AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.LJR);
                    CBRDown = true;
                }
            }
            if (m_ljHorizontalValue > -AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.LJL);
                    CBLDown = true;
                }
            }
            if (m_ljVerticalValue < AXIS_DOWN_VALUE) {
                if (thisLJVerticalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.LJU);
                    CBUDown = true;
                }
            }
            if (m_ljVerticalValue > -AXIS_DOWN_VALUE) {
                if (thisLJVerticalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.LJD);
                    CBDDown = true;
                }
            }

            //LJLD/LJRD/LJLU/LJRU as buttons.
            if (m_ljHorizontalValue > -AXIS_DOWN_VALUE || m_ljVerticalValue > -AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue <= -AXIS_DOWN_VALUE && thisLJVerticalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.LJLD);
                    CBLDDown = true;
                }
            }
            if (m_ljHorizontalValue < AXIS_DOWN_VALUE || m_ljVerticalValue > -AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue >= AXIS_DOWN_VALUE && thisLJVerticalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.LJRD);
                    CBRDDown = true;
                }
            }
            if (m_ljHorizontalValue > -AXIS_DOWN_VALUE || m_ljVerticalValue < AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue <= -AXIS_DOWN_VALUE && thisLJVerticalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.LJLU);
                    CBLUDown = true;
                }
            }
            if (m_ljHorizontalValue < AXIS_DOWN_VALUE || m_ljVerticalValue < AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue >= AXIS_DOWN_VALUE && thisLJVerticalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.LJRU);
                    CBRUDown = true;
                }
            }

            //RJL/RJR/RJD/RJU as buttons.
            if (m_rjHorizontalValue < AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.RJR);
                }
            }
            if (m_rjHorizontalValue > -AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.RJL);
                }
            }
            if (m_rjVerticalValue < AXIS_DOWN_VALUE) {
                if (thisRJVerticalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.RJU);
                }
            }
            if (m_rjVerticalValue > -AXIS_DOWN_VALUE) {
                if (thisRJVerticalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.RJD);
                }
            }

            //RJLD/RJRD/RJLU/RJRU as buttons.
            if (m_rjHorizontalValue > -AXIS_DOWN_VALUE || m_rjVerticalValue > -AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue <= -AXIS_DOWN_VALUE && thisRJVerticalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.RJLD);
                }
            }
            if (m_rjHorizontalValue < AXIS_DOWN_VALUE || m_rjVerticalValue > -AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue >= AXIS_DOWN_VALUE && thisRJVerticalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.RJRD);
                }
            }
            if (m_rjHorizontalValue > -AXIS_DOWN_VALUE || m_rjVerticalValue < AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue <= -AXIS_DOWN_VALUE && thisRJVerticalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.RJLU);
                }
            }
            if (m_rjHorizontalValue < AXIS_DOWN_VALUE || m_rjVerticalValue < AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue >= AXIS_DOWN_VALUE && thisRJVerticalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.RJRU);
                }
            }

            //CL/CR/CD/CU as buttons.
            if (m_cHorizontalValue < AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.CR);
                    CBRDown = true;
                }
            }
            if (m_cHorizontalValue > -AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.CL);
                    CBLDown = true;
                }
            }
            if (m_cVerticalValue < AXIS_DOWN_VALUE) {
                if (thisCVerticalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.CU);
                    CBUDown = true;
                }
            }
            if (m_cVerticalValue > -AXIS_DOWN_VALUE) {
                if (thisCVerticalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.CD);
                    CBDDown = true;
                }
            }

            //CLD/CRD/CLU/CRU as buttons.
            if (m_cHorizontalValue > -AXIS_DOWN_VALUE || m_cVerticalValue > -AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue <= -AXIS_DOWN_VALUE && thisCVerticalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.CLD);
                    CBLDDown = true;
                }
            }
            if (m_cHorizontalValue < AXIS_DOWN_VALUE || m_cVerticalValue > -AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue >= AXIS_DOWN_VALUE && thisCVerticalValue <= -AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.CRD);
                    CBRDDown = true;
                }
            }
            if (m_cHorizontalValue > -AXIS_DOWN_VALUE || m_cVerticalValue < AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue <= -AXIS_DOWN_VALUE && thisCVerticalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.CLU);
                    CBLUDown = true;
                }
            }
            if (m_cHorizontalValue < AXIS_DOWN_VALUE || m_cVerticalValue < AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue >= AXIS_DOWN_VALUE && thisCVerticalValue >= AXIS_DOWN_VALUE) {
                    OnButtonDown(controllerNum, InputType.CRU);
                    CBRUDown = true;
                }
            }

            //Virtual joystick as combo button.
            if (variableJoystick != null) {
                //VL/VR/VD/VU as buttons.            
                if (m_vHorizontalValue < AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x >= AXIS_DOWN_VALUE) {
                        CBRDown = true;
                    }
                }
                if (m_vHorizontalValue > -AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x <= -AXIS_DOWN_VALUE) {
                        CBLDown = true;
                    }
                }
                if (m_vVerticalValue < AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.y >= AXIS_DOWN_VALUE) {
                        CBUDown = true;
                    }
                }
                if (m_vVerticalValue > -AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.y <= -AXIS_DOWN_VALUE) {
                        CBDDown = true;
                    }
                }

                //VLD/VRD/VLU/VRU as buttons.
                if (m_vHorizontalValue > -AXIS_DOWN_VALUE || m_vVerticalValue > -AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x <= -AXIS_DOWN_VALUE && variableJoystick.Direction.y <= -AXIS_DOWN_VALUE) {
                        CBLDDown = true;
                    }
                }
                if (m_vHorizontalValue < AXIS_DOWN_VALUE || m_vVerticalValue > -AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x >= AXIS_DOWN_VALUE && variableJoystick.Direction.y <= -AXIS_DOWN_VALUE) {
                        CBRDDown = true;
                    }
                }
                if (m_vHorizontalValue > -AXIS_DOWN_VALUE || m_vVerticalValue < AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x <= -AXIS_DOWN_VALUE && variableJoystick.Direction.y >= AXIS_DOWN_VALUE) {
                        CBLUDown = true;
                    }
                }
                if (m_vHorizontalValue < AXIS_DOWN_VALUE || m_vVerticalValue < AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x >= AXIS_DOWN_VALUE && variableJoystick.Direction.y >= AXIS_DOWN_VALUE) {
                        CBRUDown = true;
                    }
                }
            }

            //Combo button down detect.

            //CBL/CBR/CBD/CBU
            if (CBRDown)
                OnButtonDown(controllerNum, InputType.CBR);

            if (CBLDown)
                OnButtonDown(controllerNum, InputType.CBL);

            if (CBUDown)
                OnButtonDown(controllerNum, InputType.CBU);

            if (CBDDown)
                OnButtonDown(controllerNum, InputType.CBD);

            //CBLD/CBRD/CBLU/CBRU
            if (CBLDDown)
                OnButtonDown(controllerNum, InputType.CBLD);

            if (CBRDDown)
                OnButtonDown(controllerNum, InputType.CBRD);

            if (CBLUDown)
                OnButtonDown(controllerNum, InputType.CBLU);

            if (CBRUDown)
                OnButtonDown(controllerNum, InputType.CBRU);

            //Check virtual buttons down.
            virtualButtons.ForEach((vButton) => {
                if (vButton.CurrentState == NereusVirtualButton.State.Down) {
                    OnButtonDown(controllerNum, vButton.inputType);
                }
            });
        }

        //======================================================================

        if (onButtonKeep != null) {
            OnKeep(Time.unscaledDeltaTime);
        }

        //======================================================================

        if (onButtonDown != null || onButtonUp != null) {
            //Buttons: 10

            //Combo button down detect.
            bool CBLUp = false;
            bool CBRUp = false;
            bool CBDUp = false;
            bool CBUUp = false;
            bool CBLDUp = false;
            bool CBRDUp = false;
            bool CBLUUp = false;
            bool CBRUUp = false;

            for (int i = 0; i < Oceanus.USED_BUTTON_ENUM_COUNT; i++) {
                if (Oceanus.GetButtonUp(controllerMode, controllerNum, (Oceanus.Button)i)) {
                    OnButtonUp(controllerNum, (InputType)i);
                }
            }

            //Special: LT and RT as buttons.
            if (m_ltValue >= AXIS_DOWN_VALUE) {
                if (thisLTValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.LT);
                }
            }
            if (m_rtValue >= AXIS_DOWN_VALUE) {
                if (thisRTValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.RT);
                }
            }

            //LJL/LJR/LJD/LJU as buttons.
            if (m_ljHorizontalValue >= AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.LJR);
                    CBRUp = true;
                }
            }
            if (m_ljHorizontalValue < -AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue >= -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.LJL);
                    CBLUp = true;
                }
            }
            if (m_ljVerticalValue >= AXIS_DOWN_VALUE) {
                if (thisLJVerticalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.LJU);
                    CBUUp = true;
                }
            }
            if (m_ljVerticalValue <= -AXIS_DOWN_VALUE) {
                if (thisLJVerticalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.LJD);
                    CBDUp = true;
                }
            }

            //LJLD/LJRD/LJLU/LJRU as buttons.
            if (m_ljHorizontalValue <= -AXIS_DOWN_VALUE && m_ljVerticalValue <= -AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue > -AXIS_DOWN_VALUE || thisLJVerticalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.LJLD);
                    CBLDUp = true;
                }
            }
            if (m_ljHorizontalValue >= AXIS_DOWN_VALUE && m_ljVerticalValue <= -AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue < AXIS_DOWN_VALUE || thisLJVerticalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.LJRD);
                    CBRDUp = true;
                }
            }
            if (m_ljHorizontalValue <= -AXIS_DOWN_VALUE && m_ljVerticalValue >= AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue > -AXIS_DOWN_VALUE || thisLJVerticalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.LJLU);
                    CBLUUp = true;
                }
            }
            if (m_ljHorizontalValue >= AXIS_DOWN_VALUE && m_ljVerticalValue >= AXIS_DOWN_VALUE) {
                if (thisLJHorizontalValue < AXIS_DOWN_VALUE || thisLJVerticalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.LJRU);
                    CBRUUp = true;
                }
            }

            //RJL/RJR/RJD/RJU as buttons.
            if (m_rjHorizontalValue >= AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.RJR);
                }
            }
            if (m_rjHorizontalValue <= -AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.RJL);
                }
            }
            if (m_rjVerticalValue >= AXIS_DOWN_VALUE) {
                if (thisRJVerticalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.RJU);
                }
            }
            if (m_rjVerticalValue <= -AXIS_DOWN_VALUE) {
                if (thisRJVerticalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.RJD);
                }
            }

            //RJLD/RJRD/RJLU/RJRU as buttons.
            if (m_rjHorizontalValue <= -AXIS_DOWN_VALUE && m_rjVerticalValue <= -AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue > -AXIS_DOWN_VALUE || thisRJVerticalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.RJLD);
                }
            }
            if (m_rjHorizontalValue >= AXIS_DOWN_VALUE && m_rjVerticalValue <= -AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue < AXIS_DOWN_VALUE || thisRJVerticalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.RJRD);
                }
            }
            if (m_rjHorizontalValue <= -AXIS_DOWN_VALUE && m_rjVerticalValue >= AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue > -AXIS_DOWN_VALUE || thisRJVerticalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.RJLU);
                }
            }
            if (m_rjHorizontalValue >= AXIS_DOWN_VALUE && m_rjVerticalValue >= AXIS_DOWN_VALUE) {
                if (thisRJHorizontalValue < AXIS_DOWN_VALUE || thisRJVerticalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.RJRU);
                }
            }

            //Combo button down detect.

            //CL/CR/CD/CU as buttons.
            if (m_cHorizontalValue >= AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.CR);
                    CBRUp = true;
                }
            }
            if (m_cHorizontalValue <= -AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.CL);
                    CBLUp = true;
                }
            }
            if (m_cVerticalValue >= AXIS_DOWN_VALUE) {
                if (thisCVerticalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.CU);
                    CBUUp = true;

                }
            }
            if (m_cVerticalValue <= -AXIS_DOWN_VALUE) {
                if (thisCVerticalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.CD);
                    CBDUp = true;
                }
            }

            //CLD/CRD/CLU/CRU as buttons.
            if (m_cHorizontalValue <= -AXIS_DOWN_VALUE && m_cVerticalValue <= -AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue > -AXIS_DOWN_VALUE || thisCVerticalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.CLD);
                    CBLDUp = true;
                }
            }
            if (m_cHorizontalValue >= AXIS_DOWN_VALUE && m_cVerticalValue <= -AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue < AXIS_DOWN_VALUE || thisCVerticalValue > -AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.CRD);
                    CBRDUp = true;
                }
            }
            if (m_cHorizontalValue <= -AXIS_DOWN_VALUE && m_cVerticalValue >= AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue > -AXIS_DOWN_VALUE || thisCVerticalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.CLU);
                    CBLUUp = true;
                }
            }
            if (m_cHorizontalValue >= AXIS_DOWN_VALUE && m_cVerticalValue >= AXIS_DOWN_VALUE) {
                if (thisCHorizontalValue < AXIS_DOWN_VALUE || thisCVerticalValue < AXIS_DOWN_VALUE) {
                    OnButtonUp(controllerNum, InputType.CRU);
                    CBRUUp = true;
                }
            }

            //Virtual joystick as combo button.
            if (variableJoystick != null) {
                //VL/VR/VD/VU as buttons.
                if (m_vHorizontalValue >= AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x < AXIS_DOWN_VALUE) {
                        CBRUp = true;
                    }
                }
                if (m_vHorizontalValue <= -AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x > -AXIS_DOWN_VALUE) {
                        CBLUp = true;
                    }
                }
                if (m_vVerticalValue >= AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.y < AXIS_DOWN_VALUE) {
                        CBUUp = true;

                    }
                }
                if (m_vVerticalValue <= -AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.y > -AXIS_DOWN_VALUE) {
                        CBDUp = true;
                    }
                }

                //VLD/VRD/VLU/VRU as buttons.
                if (m_vHorizontalValue <= -AXIS_DOWN_VALUE && m_vVerticalValue <= -AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x > -AXIS_DOWN_VALUE || variableJoystick.Direction.y > -AXIS_DOWN_VALUE) {
                        CBLDUp = true;
                    }
                }
                if (m_vHorizontalValue >= AXIS_DOWN_VALUE && m_vVerticalValue <= -AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x < AXIS_DOWN_VALUE || variableJoystick.Direction.y > -AXIS_DOWN_VALUE) {
                        CBRDUp = true;
                    }
                }
                if (m_vHorizontalValue <= -AXIS_DOWN_VALUE && m_vVerticalValue >= AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x > -AXIS_DOWN_VALUE || variableJoystick.Direction.y < AXIS_DOWN_VALUE) {
                        CBLUUp = true;
                    }
                }
                if (m_vHorizontalValue >= AXIS_DOWN_VALUE && m_vVerticalValue >= AXIS_DOWN_VALUE) {
                    if (variableJoystick.Direction.x < AXIS_DOWN_VALUE || variableJoystick.Direction.y < AXIS_DOWN_VALUE) {
                        CBRUUp = true;
                    }
                }
            }

            //Combo button down detect.

            //CBL/CBR/CBD/CBU
            if (CBRUp)
                OnButtonUp(controllerNum, InputType.CBR);

            if (CBLUp)
                OnButtonUp(controllerNum, InputType.CBL);

            if (CBUUp)
                OnButtonUp(controllerNum, InputType.CBU);

            if (CBDUp)
                OnButtonUp(controllerNum, InputType.CBD);

            //CBLD/CBRD/CBLU/CBRU
            if (CBLDUp)
                OnButtonUp(controllerNum, InputType.CBLD);

            if (CBRDUp)
                OnButtonUp(controllerNum, InputType.CBRD);

            if (CBLUUp)
                OnButtonUp(controllerNum, InputType.CBLU);

            if (CBRUUp)
                OnButtonUp(controllerNum, InputType.CBRU);

            //Check virtual buttons up.
            virtualButtons.ForEach((vButton) => {
                if (vButton.CurrentState != NereusVirtualButton.State.Down) {
                    OnButtonUp(controllerNum, vButton.inputType);
                }
            });
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

        //Virtual joystick
        if (variableJoystick != null) {
            m_vHorizontalValue = variableJoystick.Direction.x;
            m_vVerticalValue = variableJoystick.Direction.y;
        }
    }

    private void OnButtonDown(int controllerNum, InputType inputType) {
        if (buttonStatus[(int)inputType].state == ButtonStatus.State.NotDown) {
            buttonStatus[(int)inputType].state = ButtonStatus.State.Down;
            onButtonDown?.Invoke(controllerNum, inputType);
        }
    }

    private void OnButtonUp(int controllerNum, InputType inputType) {
        if (buttonStatus[(int)inputType].state != ButtonStatus.State.NotDown) {
            buttonStatus[(int)inputType].state = ButtonStatus.State.NotDown;
            buttonStatus[(int)inputType].accumulateDelay = 0.0f;
            onButtonUp?.Invoke(controllerNum, inputType);
        }
    }

    private void OnKeep(float deltaTime) {
        if (onButtonKeep != null) {
            for (int i = 0; i < buttonStatus.Count; i++) {
                if (buttonStatus[i].state == ButtonStatus.State.Down) {
                    buttonStatus[i].accumulateDelay += deltaTime;
                    if (buttonStatus[i].accumulateDelay >= buttonEnterRepeatDelay) {
                        buttonStatus[i].state = ButtonStatus.State.Keep;
                        buttonStatus[i].accumulateDelay -= buttonEnterRepeatDelay;
                        onButtonKeep(controllerNum, (InputType)i);
                    }
                } else if (buttonStatus[i].state == ButtonStatus.State.Keep) {
                    buttonStatus[i].accumulateDelay += deltaTime;
                    if (buttonStatus[i].accumulateDelay >= buttonRepeatInterval) {
                        buttonStatus[i].accumulateDelay -= buttonRepeatInterval;
                        onButtonKeep(controllerNum, (InputType)i);
                    }
                }
            }
        }
    }

}
