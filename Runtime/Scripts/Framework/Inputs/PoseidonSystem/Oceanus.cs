using UnityEngine;
using System;

/// <summary>
/// This is actually a layer for processing Unity controller inputs.
/// </summary>
static public class Oceanus {

    //XBoX and PS4 behave differently to Unity. We provide a mode switch to dynamicly switch between them.
    public enum Mode {
        XBoxOne,
        PS4
    };

    //All "button" type. We also mapping PS4 buttons at the same location on the joystick with these enums.
    //Note that LT/RT is not act like a button for XBoxOne controller so we don't do it here. However, Nereus will handle this problem properly.
    public enum Button {
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
        Empty10,    // LT
        Empty11,    // RT
        Empty12,
        Empty13,
        Empty14,
        Empty15,
        Empty16,
        Empty17,
        Empty18,
        Empty19
    };

	//For performance sake.
	static public int USED_BUTTON_ENUM_COUNT = 10;

    //For simulate LT/RT button up/down.
    private const float AXIS_DOWN_VALUE = 0.1f;

	//http://wiki.unity3d.com/index.php?title=Xbox360Controller
	//[Mode][Button]
    static private string[][] UNITY_JOYSTICK_BUTTON_MAPPING = new string[][] {
		new string[] {
			"0",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"10",
			"11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19"
		}, 	//XBoxOne
		new string[] {
            "2",    //Invert A/B for PS4
			"1",    //Invert A/B for PS4
            "0",
            "3",
            "4",
            "5",
			"8",
            "9",
            "10",
            "11",
			"6",
            "7",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19"
		}	//PS4
    };

    //Axis (sticks and cross with -1 ~ 1 output values)
    public enum Axis {
        LJ,
        RJ,
		Cross
    };

    //[Performance trick]: Optimize for enum conversion
    static private EnumUtil<KeyCode> _keyCodeEnumUtil = null;
    static private EnumUtil<Axis> _axisEnumUtil = null;

    static private EnumUtil<KeyCode> KeyCodeUtil {
        get {
            if (_keyCodeEnumUtil == null) {
                _keyCodeEnumUtil = new EnumUtil<KeyCode>();
            }
            return _keyCodeEnumUtil;
        }
    }

    static private EnumUtil<Axis> AxisUtil {
        get {
            if (_axisEnumUtil == null) {
                _axisEnumUtil = new EnumUtil<Axis>();
            }
            return _axisEnumUtil;
        }
    }

    //The Input Manager should manage the following define to properly use these axis.
    //C[N][X/P][Axis]Horizontal
    //C[N][X/P][Axis]Vertical
    //C[N][X/P]LT
    //C[N][X/P]RT

    //N = 1 ~ 8
    //X = XBoxOne / P = PS4
    //Axis = enum name
    //Total = 128 defines in Unity Input Manager. (Damn)

    //Check if a joystick button is down.

    //Init stuffs for Oceanus here.
    static public void Init() {
        //[Performance trick]
        if (_keyCodeEnumUtil == null) {
            _keyCodeEnumUtil = new EnumUtil<KeyCode>();
        }
        if (_axisEnumUtil == null) {
            _axisEnumUtil = new EnumUtil<Axis>();
        }
    }

    //--------- Button APIs

    //controllerNum: 1 ~ 8
    static public bool GetButtonDown(Mode mode, int controllerNum, Button button) {
        Init();
        try {
            KeyCode keyCode = (KeyCode)KeyCodeUtil.ToEnum("Joystick" + controllerNum.ToString() + "Button" + UNITY_JOYSTICK_BUTTON_MAPPING[(int)mode][(int)button]);
			return Input.GetKeyDown(keyCode);
        } catch {
            Debug.LogError("Not a valid button for [" + controllerNum + "] [" + button + "]");
            return false;
        }
    }

    //Check if a joystick button is Up.
    //controllerNum: 1 ~ 8
    static public bool GetButtonUp(Mode mode, int controllerNum, Button button) {
        Init();
		try {
            KeyCode keyCode = (KeyCode)KeyCodeUtil.ToEnum("Joystick" + controllerNum.ToString() + "Button" + UNITY_JOYSTICK_BUTTON_MAPPING[(int)mode][(int)button]);
            return Input.GetKeyUp(keyCode);
        } catch {
            Debug.LogError("Not a valid button for [" + controllerNum + "] [" + button + "]");
            return false;
        }
    }

	//--------- Axis APIs

    //controllerNum: 1 ~ 8
    static public float GetAxisHorizontal(Mode mode, int controllerNum, Axis axis) {
        Init();
		try {
            string axisName = "C" + controllerNum.ToString() + ((mode == Mode.XBoxOne) ? ("X") : ("P")) + AxisUtil.ToString(axis) + "Horizontal";
			return Input.GetAxis(axisName);
        } catch {
            Debug.LogError("Not a valid axis for [" + controllerNum + "] [" + axis + "]");
            return 0.0f;
        }
    }

    //controllerNum: 1 ~ 8
    static public float GetAxisVertical(Mode mode, int controllerNum, Axis axis) {
        Init();
        try {
            string axisName = "C" + controllerNum.ToString() + ((mode == Mode.XBoxOne) ? ("X") : ("P")) + AxisUtil.ToString(axis) + "Vertical";
            return Input.GetAxis(axisName);
        } catch {
            Debug.LogError("Not a valid axis for [" + controllerNum + "] [" + axis + "]");
            return 0.0f;
        }
    }

    //controllerNum: 1 ~ 8
    static public float GetAxisLT(Mode mode, int controllerNum) {
        try {
            string axisName = "C" + controllerNum.ToString() + ((mode == Mode.XBoxOne) ? ("X") : ("P")) + "LT";
            return Input.GetAxis(axisName);
        } catch {
            Debug.LogError("Not a valid LT for [" + controllerNum + "]");
            return 0.0f;
        }
    }

    //controllerNum: 1 ~ 8
    static public float GetAxisRT(Mode mode, int controllerNum) {
        try {
            string axisName = "C" + controllerNum.ToString() + ((mode == Mode.XBoxOne) ? ("X") : ("P")) + "RT";
            return Input.GetAxis(axisName);
        } catch {
            Debug.LogError("Not a valid RT for [" + controllerNum + "]");
            return 0.0f;
        }
    }

}
