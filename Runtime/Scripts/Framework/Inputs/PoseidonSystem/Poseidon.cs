using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A singleton component for manage exist Nereus instances (Controller listener) in scene.
/// This is a system core component and should exist only one instance in the world.
/// </summary>
public class Poseidon : MonoBehaviour {

    //Get informed if a controller is pluged-in.
    static public Action<int, Triton.ControllerType> onControllerChanged = null;

    //The custom input manager for multiple players.
    public Nereus[] nereus;

	static public Nereus[] Controllers {
        get {
            if (instance != null) {
                return instance.nereus;
            } else {
                return null;
            }
        }
    }
	
    static public Nereus MainController {
        get {
            if (instance != null) {
                return instance.nereus[0];
            } else {
                return null;
            }
        }
    }
	
	//0~n
    static public Nereus GetNereus(int controllerIndex) {
        if (instance != null) {
            if ((int)controllerIndex < instance.nereus.Length) {
                return instance.nereus[(int)controllerIndex];
            } else {
                return null;
            }
        } else {
            return null;
        }
    }

    static public Poseidon instance = null;

    void Awake() {
        instance = this;
    }

    //For ControllerIdentifier
    //index: 0~n
    public void OnControllerChanged(int index, Triton.ControllerType controllerType) {
        if (SetControllerType(index, controllerType) && onControllerChanged != null) {
            onControllerChanged(index, controllerType);
        }
    }

    //------------------------------------------------

    //index: 0~n
    static private bool SetControllerType(int index, Triton.ControllerType controllerType) {
        if (instance != null) {
            if (instance.nereus.Length > index && index >= 0) {
                Console.Out("SetupNereus: [" + index + "] = " + controllerType);
                if (controllerType == Triton.ControllerType.PS) {
                    instance.nereus[index].SetJoystickMode(Oceanus.Mode.PS4);
                } else if (controllerType == Triton.ControllerType.XBox) {
                    instance.nereus[index].SetJoystickMode(Oceanus.Mode.XBoxOne);
                } else {
                    instance.nereus[index].SetJoystickMode(Oceanus.Mode.XBoxOne);
                }
                return true;
            } else {
                Console.OutWarning("Not enough Nereus in the scene for joystick index: " + index);
                return false;
            }
        } else {
            Console.OutWarning("Poseidon instance not initialized yet. Cannot set joystick mode for: " + index);
            return false;
        }
    }
}
