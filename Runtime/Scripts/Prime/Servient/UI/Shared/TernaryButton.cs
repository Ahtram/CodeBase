using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class TernaryButton : UIBase, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public enum State {
        Normal,
        Focus,
        Down
    };

    public Image buttonImage;
    public bool focusOnMouseEnter = false;

    protected bool m_isPointerDown = false;
    protected bool m_isFocused = false;
    protected State m_currentState = State.Normal;

    public State CurrentState {
        get {
            return m_currentState;
        }
    }

    public Sprite spriteButtonNormal;
    public Color colorButtonNormal = Color.white;

    public Sprite spriteButtonFocus;
    public Color colorButtonFocus = Color.white;

    public Sprite spriteButtonDown;
    public Color colorButtonDown = Color.white;

    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;
    public UnityEvent onPointerClick;
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;

    public RectTransform rectTransform { get { return (RectTransform)transform; } }

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

    virtual public void SetFocus(bool b) {
        m_isFocused = b;
        if (!m_isPointerDown) {
            if (m_isFocused) {
                SetButtonState(State.Focus);
            } else {
                SetButtonState(State.Normal);
            }
        }
    }

    public bool IsFocused() {
        return m_isFocused;
    }

    private void SetButtonState(State state) {
        m_currentState = state;
        switch(state) {
            case State.Normal:
                buttonImage.sprite = spriteButtonNormal;
                buttonImage.color = colorButtonNormal;
                break;
            case State.Focus:
                buttonImage.sprite = spriteButtonFocus;
                buttonImage.color = colorButtonFocus;
                break;
            case State.Down:
                buttonImage.sprite = spriteButtonDown;
                buttonImage.color = colorButtonDown;
                break;
        }
    }

    virtual public void OnPointerDown(PointerEventData eventData) {
        m_isPointerDown = true;
        SetButtonState(State.Down);
        onPointerDown.Invoke();

        if (vibrateOnDown) {
            Vibration.Vibrate(vibrateDuration);
        }
    }

    virtual public void OnPointerUp(PointerEventData eventData) {
        m_isPointerDown = false;
        if (m_isFocused) {
            SetButtonState(State.Focus);
        } else {
            SetButtonState(State.Normal);
        }
        onPointerUp.Invoke();
    }

    virtual public void OnPointerClick(PointerEventData eventData) {
        onPointerClick.Invoke();
    }

    virtual public void OnPointerEnter(PointerEventData eventData) {
        if (focusOnMouseEnter) {
            SetFocus(true);
        }
        onPointerEnter.Invoke();
    }

    virtual public void OnPointerExit(PointerEventData eventData) {
        if (focusOnMouseEnter) {
            SetFocus(false);
        }
        onPointerExit.Invoke();
    }

}
