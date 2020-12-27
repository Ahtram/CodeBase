using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This is a very simple button which has only two state.
/// </summary>
[RequireComponent(typeof(Image))]
public class BinaryButton : PassiveToggle, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {

    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;
    public UnityEvent onPointerClick;

    public Text btnText;
    public TextMeshProUGUI btnTMP;

    public bool vibrateOnDown = false;
    public long vibrateDuration = 10;

    public void SetupStr(string str) {
        if (btnText != null) {
            btnText.text = str;
        }
        if (btnTMP != null) {
            btnTMP.text = str;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        SetToggleOn();
        onPointerDown.Invoke();

        if (vibrateOnDown) {
            Vibration.Vibrate(vibrateDuration);
        }
    }

	public void OnPointerUp(PointerEventData eventData) {
		SetToggleOff();
        onPointerUp.Invoke();
    }

	public void OnPointerClick(PointerEventData eventData) {
        onPointerClick.Invoke();
    }

}
