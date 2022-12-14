using UnityEngine;

/// <summary>
/// Contains convenient API and interface for receive controller inputs.
/// </summary>
public class UIControllable : UIBase {

    /// <summary>
    /// A logic bool which should be used to block user inputs when it's false.
    /// </summary>
    protected bool m_enableOperation = false;

    /// <summary>
    /// Turn on the enable user operation flag.
    /// </summary>
    public void EnableOperation() {
        m_enableOperation = true;
    }

    /// <summary>
    /// Turn off the enable user operation flag.
    /// </summary>
    public void DisableOperation() {
        m_enableOperation = false;
    }

    /// <summary>
    /// Is currently enable operation?
    /// </summary>
    /// <returns></returns>
    public bool IsEnableOperation() {
        return m_enableOperation;
    }

    // Poseidon -> 8 joysticks

    protected void StartListenToPoseidon() {
        Nereus[] controllers = Poseidon.Controllers;
        for (int i = 0; i < controllers.Length; i++) {
            controllers[i].onButtonDown = OnPoseidonControllerDown;
            controllers[i].onButtonKeep = OnPoseidonControllerKeep;
            controllers[i].onButtonUp = OnPoseidonControllerUp;

            controllers[i].onLeftJoystick = OnPoseidonControllerLeftJoystick;
            controllers[i].onRightJoystick = OnPoseidonControllerRightJoystick;
            controllers[i].onCrossJoystick = OnPoseidonControllerCrossJoystick;
            controllers[i].onComboJoystick = OnPoseidonControllerComboJoystick;

            controllers[i].onLT = OnPoseidonControllerLT;
            controllers[i].onRT = OnPoseidonControllerRT;
        }
    }

    protected void StopListenToPoseidon() {
        Nereus[] controllers = Poseidon.Controllers;
        for (int i = 0; i < controllers.Length; i++) {
            controllers[i].onButtonDown -= OnPoseidonControllerDown;
            controllers[i].onButtonKeep -= OnPoseidonControllerKeep;
            controllers[i].onButtonUp -= OnPoseidonControllerUp;

            controllers[i].onLeftJoystick -= OnPoseidonControllerLeftJoystick;
            controllers[i].onRightJoystick -= OnPoseidonControllerRightJoystick;
            controllers[i].onCrossJoystick -= OnPoseidonControllerCrossJoystick;
            controllers[i].onComboJoystick -= OnPoseidonControllerComboJoystick;

            controllers[i].onLT -= OnPoseidonControllerLT;
            controllers[i].onRT -= OnPoseidonControllerRT;
        }
    }

    //----------------------------------------

    virtual protected void OnPoseidonControllerDown(int controllerNum, Nereus.InputType inputType) {

    }

    virtual protected void OnPoseidonControllerKeep(int controllerNum, Nereus.InputType inputType) {

    }

    virtual protected void OnPoseidonControllerUp(int controllerNum, Nereus.InputType inputType) {

    }

    virtual protected void OnPoseidonControllerLeftJoystick(int controllerNum, Vector2 vec) {

    }

    virtual protected void OnPoseidonControllerRightJoystick(int controllerNum, Vector2 vec) {

    }

    virtual protected void OnPoseidonControllerCrossJoystick(int controllerNum, Vector2 vec) {

    }

    virtual protected void OnPoseidonControllerComboJoystick(int controllerNum, Vector2 vec) {

    }

    //value = 0~1
    virtual protected void OnPoseidonControllerLT(int controllerNum, float value) {

    }

    //value = 0~1
    virtual protected void OnPoseidonControllerRT(int controllerNum, float value) {

    }

    //----------------------------------------

    // Amphitrite -> Main joystick + Main keyboard

    protected void StartListenToAmphitrite() {
        Amphitrite.onKeyboardDown = OnAmphitriteKeyboardDown;
        Amphitrite.onKeyboardKeep = OnAmphitriteKeyboardKeep;
        Amphitrite.onKeyboardUp = OnAmphitriteKeyboardUp;
        Amphitrite.onKeyboardAxis = OnAmphitriteKeyboardAxis;

        Amphitrite.onControllerDown = OnAmphitriteControllerDown;
        Amphitrite.onControllerKeep = OnAmphitriteControllerKeep;
        Amphitrite.onControllerUp = OnAmphitriteControllerUp;
        Amphitrite.onControllerLeftAxis = OnAmphitriteControllerLeftAxis;
        Amphitrite.onControllerRightAxis = OnAmphitriteControllerRightAxis;
        Amphitrite.onControllerCrossAxis = OnAmphitriteControllerCrossAxis;
        Amphitrite.onControllerComboAxis = OnAmphitriteControllerComboAxis;

        Amphitrite.onKeyDown = OnAmphitriteKeyDown;
        Amphitrite.onKeyKeep = OnAmphitriteKeyKeep;
        Amphitrite.onKeyUp = OnAmphitriteKeyUp;

        Amphitrite.onFocusKeyDeviceChanged = OnAmphitriteFocusKeyDeviceChanged;
    }

    protected void StopListenToAmphitrite() {
        Amphitrite.onKeyboardDown -= OnAmphitriteKeyboardDown;
        Amphitrite.onKeyboardKeep -= OnAmphitriteKeyboardKeep;
        Amphitrite.onKeyboardUp -= OnAmphitriteKeyboardUp;
        Amphitrite.onKeyboardAxis -= OnAmphitriteKeyboardAxis;

        Amphitrite.onControllerDown -= OnAmphitriteControllerDown;
        Amphitrite.onControllerKeep -= OnAmphitriteControllerKeep;
        Amphitrite.onControllerUp -= OnAmphitriteControllerUp;
        Amphitrite.onControllerLeftAxis -= OnAmphitriteControllerLeftAxis;
        Amphitrite.onControllerRightAxis -= OnAmphitriteControllerRightAxis;
        Amphitrite.onControllerCrossAxis -= OnAmphitriteControllerCrossAxis;
        Amphitrite.onControllerComboAxis -= OnAmphitriteControllerComboAxis;

        Amphitrite.onKeyDown -= OnAmphitriteKeyDown;
        Amphitrite.onKeyKeep -= OnAmphitriteKeyKeep;
        Amphitrite.onKeyUp -= OnAmphitriteKeyUp;

        Amphitrite.onFocusKeyDeviceChanged -= OnAmphitriteFocusKeyDeviceChanged;
    }

    //----------------------------------------

    //Impl these interface to properly receive controller signal.
    virtual protected void OnAmphitriteKeyboardDown(KeyConfig.Key key) {

    }

    virtual protected void OnAmphitriteKeyboardKeep(KeyConfig.Key key) {

    }

    virtual protected void OnAmphitriteKeyboardUp(KeyConfig.Key key) {

    }

    virtual protected void OnAmphitriteKeyboardAxis(Vector2 vec) {

    }

    //LeftJoystick + Cross
    virtual protected void OnAmphitriteControllerDown(KeyConfig.Key key) {

    }

    virtual protected void OnAmphitriteControllerKeep(KeyConfig.Key key) {

    }

    virtual protected void OnAmphitriteControllerUp(KeyConfig.Key key) {

    }

    virtual protected void OnAmphitriteControllerLeftAxis(Vector2 vec) {

    }

    virtual protected void OnAmphitriteControllerRightAxis(Vector2 vec) {

    }

    virtual protected void OnAmphitriteControllerCrossAxis(Vector2 vec) {

    }

    virtual protected void OnAmphitriteControllerComboAxis(Vector2 vec) {

    }

    //For both controller and keyboard. (These should be the most important methods)
    virtual protected void OnAmphitriteKeyDown(KeyConfig.Key key) {

    }

    virtual protected void OnAmphitriteKeyKeep(KeyConfig.Key key) {

    }

    virtual protected void OnAmphitriteKeyUp(KeyConfig.Key key) {

    }

    //For KeyDevice changed.
    virtual protected void OnAmphitriteFocusKeyDeviceChanged(Amphitrite.KeyDevice keyDevice) {

    }

}
