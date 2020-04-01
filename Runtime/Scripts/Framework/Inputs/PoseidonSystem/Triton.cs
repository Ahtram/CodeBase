using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

[System.Serializable]
public class ControllerChangedEvent : UnityEvent<int, Triton.ControllerType> {
	
}

/// <summary>
/// This class will keep detect if the controller has changed.
/// Event will be invoked in case changed was made.
/// </summary>
public class Triton : MonoBehaviour {

    public ControllerChangedEvent onControllerChangedEvent;

    public enum ControllerType {
        XBox,
        PS,
        None
    };

    //The PC version should set this to true and consoles to false.
#if UNITY_PS4
    public bool resident = false;
#elif UNITY_XBOXONE
    public bool resident = false;
#else
    //PC.
    public bool resident = true;
#endif

    static private string XBOXONE_CONTROLLER_NAMES = "Controller (Xbox One For Windows)";
    static private string XBOX360_CONTROLLER_NAMES = "Controller (XBOX 360 For Windows)";
    static private string PS4_CONTROLLER_NAMES = "Wireless Controller";

    //Store detected controller types.
    static private List<string> m_controllerNamesCache = new List<string>();

    void Awake() {
        SetupInformChange();
        if (resident) {
            StartCoroutine(DetectChangeCoroutine());
        }
    }

    private IEnumerator DetectChangeCoroutine() {
        while (true) {
            ScanForChange();
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void ScanForChange() {
        string[] names = Input.GetJoystickNames();

        if (!names.SequenceEqual(m_controllerNamesCache)) {
            Console.Out("== Controller Change Detected ==");
            m_controllerNamesCache = new List<string>(names);
            PrintControllerTypes();
            Console.Out("=======================");
            SetupInformChange();
        }
    }

    private void SetupInformChange() {
		for (int i = 0; i < m_controllerNamesCache.Count; i++) {
            onControllerChangedEvent.Invoke(i, GetControllerType(i));
        }
    }

    static private ControllerType IdentifyType(string controllerName) {
        if (string.IsNullOrEmpty(controllerName)) {
            return ControllerType.None;
        }

        if (controllerName == XBOXONE_CONTROLLER_NAMES) {
            return ControllerType.XBox;
        }

        if (controllerName == XBOX360_CONTROLLER_NAMES) {
            return ControllerType.XBox;
        }

        if (controllerName == PS4_CONTROLLER_NAMES) {
            return ControllerType.PS;
        }

        //Unknowen name = treat it as a xbox controller.
        return ControllerType.XBox;
    }

    static private void PrintControllerTypes() {
        for (int i = 0; i < m_controllerNamesCache.Count; i++) {
            Console.Out("[" + i + "]:" + IdentifyType(m_controllerNamesCache[i]));
        }
    }

	//0~n
    static public ControllerType GetControllerType(int i) {
#if UNITY_PS4
        return ControllerType.PS;
#elif UNITY_XBOXONE
        return ControllerType.XBox;
#else
        ////PC.
        //for (int i = 0; i < m_controllerNamesCache.Count; i++) {
        //    ControllerType thisType = IdentifyType(m_controllerNamesCache[i]);
        //    //Return the first found.
        //    if (thisType != ControllerType.None) {
        //        return thisType;
        //    }
        //}

        //return ControllerType.None;

        if (m_controllerNamesCache.Count > i) {
            return IdentifyType(m_controllerNamesCache[i]);
        }
        return ControllerType.None;

#endif
    }

}
