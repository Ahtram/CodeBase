using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// A component for manage key-mapping class the provide interface and events for keyboard and controllers.
/// This is a system core component and should exist only one instance in the world.
/// Note that this one only consider single player situation.
/// </summary>
public class Amphitrite : MonoBehaviour {

    //Get informed if a controller is pluged-in.
    static public Action<Triton.ControllerType> onControllerChanged = null;

    public enum KeyDevice {
        Keyboard,
        Controller,
        VirtualController
    };

    //These will be called only when pressed/released.
    //[Keyboard]
    static public Action<KeyConfig.Key> onKeyboardDown;
    static public Action<KeyConfig.Key> onKeyboardKeep;
    static public Action<KeyConfig.Key> onKeyboardUp;
    //These will be called contineuesly.
    static public Action<Vector2> onKeyboardAxis;

    //[Controller]
    static public Action<KeyConfig.Key> onControllerDown;
    static public Action<KeyConfig.Key> onControllerKeep;
    static public Action<KeyConfig.Key> onControllerUp;

    //These will be called contineuesly.
    //[Controller]
    static public Action<Vector2> onControllerLeftAxis;
    static public Action<Vector2> onControllerRightAxis;
    static public Action<Vector2> onControllerCrossAxis;
    //Combo = LeftJoystick and CrossJoystick.
    static public Action<Vector2> onControllerComboAxis;

    //Virtual = On screen virtual joystick.
    static public Action<Vector2> onControllerVirtualAxis;

    //The virtual button we're checking in Update()
    public List<AmphitriteVirtualButton> virtualButtons = new List<AmphitriteVirtualButton>();

    //If not normalized:
    //The LT/RT value for XBox is 0~1
    //The LT/RT value for PS4 is -1~1
    static public Action<float> onControllerLTAxis;
    static public Action<float> onControllerRTAxis;

    //These will be called both when a keyboard key or a controller key is pressed.
    static public Action<KeyConfig.Key> onKeyDown;
    static public Action<KeyConfig.Key> onKeyKeep;
    static public Action<KeyConfig.Key> onKeyUp;

    //This will be called when focus key device changed.
    static public Action<KeyDevice> onFocusKeyDeviceChanged;

    //Ordered by KeyConfig.Key enum. (This is for implement onKeyboardKeyKeep onVirtualKeyKeep)
    private List<KeyStatus> virtualButtonKeyStatus = new List<KeyStatus>();

    public float virtualButtonEnterRepeatDelay = 0.35f;
    public float virtualButtonRepeatInterval = 0.1f;

    private class KeyStatus {
        public enum State {
            NotDown,
            Down,
            Keep
        };

        public State state = State.NotDown;
        public bool keepIsActive = false;
        public float accumulateDelay = 0.0f;
    };

    //Ordered by KeyConfig.Key enum. (This is for implement onKeyboardKeyKeep onVirtualKeyKeep)
    private List<KeyStatus> keyboardKeyStatus = new List<KeyStatus>();

    public float keyboardKeyEnterRepeatDelay = 0.35f;
    public float keyboardKeyRepeatInterval = 0.1f;

    //Main controller guy here.
    public Nereus nereus;

    //The keyConfig data.
    private KeyConfig keyConfigCache = null;

    //This will cache the most recent used key device type.
    private KeyDevice focusingKeyDevice = KeyDevice.Keyboard;

    static public Amphitrite instance = null;

    //===================================

    static public void ReloadKeyConfig() {
        if (instance != null) {
            instance.keyConfigCache = KeyConfig.Get();
        }
    }

    //===================================

    //Start listen to main controller.
    void Awake() {
        instance = this;
        ReloadKeyConfig();
        if (nereus != null) {
            nereus.onButtonDown += OnNereusButtonDown;
            nereus.onButtonKeep += OnNereusButtonKeep;
            nereus.onButtonUp += OnNereusButtonUp;

            nereus.onLeftJoystick += OnNereusLeftJoystick;
            nereus.onRightJoystick += OnNereusRightJoystick;
            nereus.onCrossJoystick += OnNereusCrossJoystick;
            nereus.onComboJoystick += OnNereusComboJoystick;
            nereus.onVJoystick += OnNereusVJoystick;

            nereus.onLT += OnNereusLT;
            nereus.onRT += OnNereusRT;
        } else {
            Console.OutWarning("Oops! Amphitrite need a Nereus to work properly!");
        }

        //Initialize status container.
        keyboardKeyStatus.Clear();
        int inputTypeCount = Enum.GetNames(typeof(KeyConfig.Key)).Length;

        for (int i = 0; i < inputTypeCount; i++) {
            keyboardKeyStatus.Add(new KeyStatus());
        }

        virtualButtonKeyStatus.Clear();
        for (int i = 0; i < inputTypeCount; i++) {
            virtualButtonKeyStatus.Add(new KeyStatus());
        }
    }

    //For ControllerIdentifier (Triton)
    //index: 0~n
    public void OnControllerChanged(int index, Triton.ControllerType controllerType) {
        if (index == 0) {
            switch (controllerType) {
                case Triton.ControllerType.XBox:
                    nereus.controllerMode = Oceanus.Mode.XBoxOne;
                    break;
                case Triton.ControllerType.PS:
                    nereus.controllerMode = Oceanus.Mode.PS4;
                    break;
                case Triton.ControllerType.None:
                    nereus.controllerMode = Oceanus.Mode.XBoxOne;
                    break;
                default:
                    nereus.controllerMode = Oceanus.Mode.XBoxOne;
                    break;
            }

            onControllerChanged?.Invoke(controllerType);
        }
    }

    //[Controller]
    private void OnNereusButtonDown(int controllerNum, Nereus.InputType inputType) {
        if (keyConfigCache != null) {
            if (onControllerDown != null || onKeyDown != null) {
                int keyIndex = keyConfigCache.controllerSetting.IndexOf(inputType);
                if (keyIndex != -1 & keyIndex < KeyConfig.KEY_COUNT) {
                    onControllerDown?.Invoke((KeyConfig.Key)keyIndex);
                    onKeyDown?.Invoke((KeyConfig.Key)keyIndex);

                    CheckSwitchFocusKeyDevice(KeyDevice.Controller);
                }
            }
        }
    }

    //[Controller]
    private void OnNereusButtonKeep(int controllerNum, Nereus.InputType inputType) {
        if (keyConfigCache != null) {
            if (onControllerKeep != null || onKeyKeep != null) {
                if (onControllerKeep != null && keyConfigCache != null) {
                    int keyIndex = keyConfigCache.controllerSetting.IndexOf(inputType);
                    if (keyIndex != -1 & keyIndex < KeyConfig.KEY_COUNT) {
                        onControllerKeep?.Invoke((KeyConfig.Key)keyIndex);
                        onKeyKeep?.Invoke((KeyConfig.Key)keyIndex);
                    }
                }
            }
        }
    }

    //[Controller]
    private void OnNereusButtonUp(int controllerNum, Nereus.InputType inputType) {
        if (keyConfigCache != null) {
            if (onControllerUp != null || onKeyUp != null) {
                if (onControllerUp != null && keyConfigCache != null) {
                    int keyIndex = keyConfigCache.controllerSetting.IndexOf(inputType);
                    if (keyIndex != -1 & keyIndex < KeyConfig.KEY_COUNT) {
                        onControllerUp?.Invoke((KeyConfig.Key)keyIndex);
                        onKeyUp?.Invoke((KeyConfig.Key)keyIndex);
                    }
                }
            }
        }
    }

    //[Controller]
    private void OnNereusLeftJoystick(int controllerNum, Vector2 axisValue) {
        onControllerLeftAxis?.Invoke(axisValue);

        if (axisValue != Vector2.zero) {
            CheckSwitchFocusKeyDevice(KeyDevice.Controller);
        }
    }

    //[Controller]
    private void OnNereusRightJoystick(int controllerNum, Vector2 axisValue) {
        onControllerRightAxis?.Invoke(axisValue);

        if (axisValue != Vector2.zero) {
            CheckSwitchFocusKeyDevice(KeyDevice.Controller);
        }
    }

    //[Controller]
    private void OnNereusCrossJoystick(int controllerNum, Vector2 axisValue) {
        onControllerCrossAxis?.Invoke(axisValue);

        if (axisValue != Vector2.zero) {
            CheckSwitchFocusKeyDevice(KeyDevice.Controller);
        }
    }

    //[Controller]
    private void OnNereusComboJoystick(int controllerNum, Vector2 axisValue) {
        onControllerComboAxis?.Invoke(axisValue);

        if (axisValue != Vector2.zero) {
            CheckSwitchFocusKeyDevice(KeyDevice.Controller);
        }
    }

    //[Controller]
    private void OnNereusVJoystick(int controllerNum, Vector2 axisValue) {
        onControllerVirtualAxis?.Invoke(axisValue);

        if (axisValue != Vector2.zero) {
            CheckSwitchFocusKeyDevice(KeyDevice.VirtualController);
        }
    }

    //[Controller]: LT
    //If not normalized:
    //The LT/RT value for XBox is 0~1
    //The LT/RT value for PS4 is -1~1
    private void OnNereusLT(int controllerNum, float value) {
        onControllerLTAxis?.Invoke(value);

        if (value >= Nereus.AXIS_DOWN_VALUE) {
            CheckSwitchFocusKeyDevice(KeyDevice.Controller);
        }
    }

    //[Controller]: RT
    //If not normalized:
    //The LT/RT value for XBox is 0~1
    //The LT/RT value for PS4 is -1~1
    private void OnNereusRT(int controllerNum, float value) {
        onControllerRTAxis?.Invoke(value);

        if (value >= Nereus.AXIS_DOWN_VALUE) {
            CheckSwitchFocusKeyDevice(KeyDevice.Controller);
        }
    }

    private void Update() {
        if (keyConfigCache != null) {
            //Keyboard Down
            for (int i = 0; i < keyConfigCache.keyboardSetting.Count; i++) {
                if (Input.GetKeyDown(keyConfigCache.keyboardSetting[i]) || Input.GetKeyDown(keyConfigCache.keyboardSettingSub[i])
                || Input.GetKeyDown(keyConfigCache.keyboardSettingExtra[i]) || Input.GetKeyDown(keyConfigCache.keyboardSettingExtand[i])) {
                    OnKeyboardDown((KeyConfig.Key)i);
                }
            }

            //Keyboard Keep
            OnKeyboardKeep(Time.unscaledDeltaTime);

            //Keyboard simulate axises.
            OnKeyboardAxis();

            //Keyboard Up
            //Check Keyboard KeyCode.
            for (int i = 0; i < keyConfigCache.keyboardSetting.Count; i++) {
                if (Input.GetKeyUp(keyConfigCache.keyboardSetting[i]) || Input.GetKeyUp(keyConfigCache.keyboardSettingSub[i])
                || Input.GetKeyUp(keyConfigCache.keyboardSettingExtra[i]) || Input.GetKeyUp(keyConfigCache.keyboardSettingExtand[i])) {
                    OnKeyboardUp((KeyConfig.Key)i);
                }
            }
        }

        //Virtual Buttons down.
        virtualButtons.ForEach((vButton) => {
            if (vButton.CurrentState == AmphitriteVirtualButton.State.Down) {
                OnVirtualButtonDown(vButton.key);
            }
        });

        OnVirtualButtonKeep(Time.unscaledDeltaTime);

        //Virtual Buttons up.
        virtualButtons.ForEach((vButton) => {
            if (vButton.CurrentState != AmphitriteVirtualButton.State.Down) {
                OnVirtualButtonUp(vButton.key);
            }
        });
    }

    //========= Keyboard

    private void OnKeyboardDown(KeyConfig.Key key) {
        if (keyboardKeyStatus[(int)key].state == KeyStatus.State.NotDown) {
            keyboardKeyStatus[(int)key].state = KeyStatus.State.Down;
            onKeyboardDown?.Invoke(key);
            onKeyDown?.Invoke(key);
        }

        CheckSwitchFocusKeyDevice(KeyDevice.Keyboard);
    }

    private void OnKeyboardUp(KeyConfig.Key key) {
        if (keyboardKeyStatus[(int)key].state != KeyStatus.State.NotDown) {
            keyboardKeyStatus[(int)key].state = KeyStatus.State.NotDown;
            keyboardKeyStatus[(int)key].accumulateDelay = 0.0f;
            onKeyboardUp?.Invoke(key);
            onKeyUp?.Invoke(key);
        }
    }

    private void OnKeyboardKeep(float deltaTime) {
        for (int i = 0; i < keyboardKeyStatus.Count; i++) {
            if (keyboardKeyStatus[i].state == KeyStatus.State.Down) {
                keyboardKeyStatus[i].accumulateDelay += deltaTime;
                if (keyboardKeyStatus[i].accumulateDelay >= keyboardKeyEnterRepeatDelay) {
                    keyboardKeyStatus[i].state = KeyStatus.State.Keep;
                    keyboardKeyStatus[i].accumulateDelay -= keyboardKeyEnterRepeatDelay;
                    onKeyboardKeep?.Invoke((KeyConfig.Key)i);
                    onKeyKeep?.Invoke((KeyConfig.Key)i);
                }
            } else if (keyboardKeyStatus[i].state == KeyStatus.State.Keep) {
                keyboardKeyStatus[i].accumulateDelay += deltaTime;
                if (keyboardKeyStatus[i].accumulateDelay >= keyboardKeyRepeatInterval) {
                    keyboardKeyStatus[i].accumulateDelay -= keyboardKeyRepeatInterval;
                    onKeyboardKeep?.Invoke((KeyConfig.Key)i);
                    onKeyKeep?.Invoke((KeyConfig.Key)i);
                }
            }
        }
    }

    private void OnKeyboardAxis() {
        if (onKeyboardAxis != null) {
            //Get the stats of the 4 direction keys.
            //Left
            //Right
            //Up
            //Down

            bool left = !(keyboardKeyStatus[(int)KeyConfig.Key.Left].state == KeyStatus.State.NotDown);
            bool right = !(keyboardKeyStatus[(int)KeyConfig.Key.Right].state == KeyStatus.State.NotDown);
            bool up = !(keyboardKeyStatus[(int)KeyConfig.Key.Up].state == KeyStatus.State.NotDown);
            bool down = !(keyboardKeyStatus[(int)KeyConfig.Key.Down].state == KeyStatus.State.NotDown);

            float horizontalAxis = 0.0f;
            if (left) {
                horizontalAxis -= 1.0f;
            }
            if (right) {
                horizontalAxis += 1.0f;
            }

            float verticalAxis = 0.0f;
            if (down) {
                verticalAxis -= 1.0f;
            }
            if (up) {
                verticalAxis += 1.0f;
            }

            onKeyboardAxis((new Vector2(horizontalAxis, verticalAxis).normalized));

            if (left || right || down || up) {
                CheckSwitchFocusKeyDevice(KeyDevice.Keyboard);
            }
        }
    }

    //========= Virtual Buttons ===========

    private void OnVirtualButtonDown(KeyConfig.Key key) {
        if (virtualButtonKeyStatus[(int)key].state == KeyStatus.State.NotDown) {
            virtualButtonKeyStatus[(int)key].state = KeyStatus.State.Down;
            onControllerDown?.Invoke(key);
            onKeyDown?.Invoke(key);
        }

        CheckSwitchFocusKeyDevice(KeyDevice.VirtualController);
    }

    private void OnVirtualButtonUp(KeyConfig.Key key) {
        if (virtualButtonKeyStatus[(int)key].state != KeyStatus.State.NotDown) {
            virtualButtonKeyStatus[(int)key].state = KeyStatus.State.NotDown;
            virtualButtonKeyStatus[(int)key].accumulateDelay = 0.0f;
            onControllerUp?.Invoke(key);
            onKeyUp?.Invoke(key);
        }
    }

    private void OnVirtualButtonKeep(float deltaTime) {
        for (int i = 0; i < virtualButtonKeyStatus.Count; i++) {
            if (virtualButtonKeyStatus[i].state == KeyStatus.State.Down) {
                virtualButtonKeyStatus[i].accumulateDelay += deltaTime;
                if (virtualButtonKeyStatus[i].accumulateDelay >= virtualButtonEnterRepeatDelay) {
                    virtualButtonKeyStatus[i].state = KeyStatus.State.Keep;
                    virtualButtonKeyStatus[i].accumulateDelay -= virtualButtonEnterRepeatDelay;
                    onControllerKeep?.Invoke((KeyConfig.Key)i);
                    onKeyKeep?.Invoke((KeyConfig.Key)i);
                }
            } else if (virtualButtonKeyStatus[i].state == KeyStatus.State.Keep) {
                virtualButtonKeyStatus[i].accumulateDelay += deltaTime;
                if (virtualButtonKeyStatus[i].accumulateDelay >= virtualButtonRepeatInterval) {
                    virtualButtonKeyStatus[i].accumulateDelay -= virtualButtonRepeatInterval;
                    onControllerKeep?.Invoke((KeyConfig.Key)i);
                    onKeyKeep?.Invoke((KeyConfig.Key)i);
                }
            }
        }
    }

    //==

    private void CheckSwitchFocusKeyDevice(KeyDevice keyDevice) {
        if (keyDevice != focusingKeyDevice) {
            focusingKeyDevice = keyDevice;
            onFocusKeyDeviceChanged?.Invoke(focusingKeyDevice);
        }
    }

}
