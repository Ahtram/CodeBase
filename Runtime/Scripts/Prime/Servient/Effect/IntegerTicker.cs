using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Teamuni.Codebase;

/// <summary>
/// A simple effect component for tick a number text component.
/// This component can work independently without a Text.
/// </summary>
public class IntegerTicker : MonoBehaviour {

    public UnityEventInt onTickStart;
    public UnityEventInt onTick;
    public UnityEventInt onTickComplete;

    public Text targetText;

    private int m_initialNumber = 0;
    private int m_goalNumber = 100;
    private float m_currentNumber = 0.0f;

    public int CurrentNumber {
        get {
            return Mathf.RoundToInt(m_currentNumber);
        }
    }

    private float m_stepNumber = 0.0f;

    //In seconds.
    private float m_durationTime = 5.0f;
    private bool m_isTicking = false;

    //The progress time after start ticking.
    private float m_progressTime = 0.0f;

    void Awake() {
        targetText = GetComponent<Text>();
    }

    /// <summary>
    /// This will start the number ticking.
    /// </summary>
    public bool StartTicking(int initialNumber, int goalNumber, float durationTime) {
        if (durationTime > 0) {
            m_initialNumber = initialNumber;
            m_goalNumber = goalNumber;
            m_currentNumber = initialNumber;
            m_durationTime = durationTime;
            m_progressTime = 0.0f;
            m_isTicking = true;

            onTickStart.Invoke(CurrentNumber);

            //Pre-calculate number increse per seconds.
            m_stepNumber = (m_goalNumber - m_initialNumber) / m_durationTime;

            return true;
        }
        Debug.LogWarning("Oops! StartTicking() failed! durationTime must be greater than 0!");
        return false;
    }

    /// <summary>
    /// Are we ticking a number?
    /// </summary>
    /// <returns></returns>
    public bool IsTicking() {
        return m_isTicking;
    }

    void Update() {
        if (m_isTicking) {
            m_progressTime += Time.deltaTime;
            int currentNumberOld = CurrentNumber;
            m_currentNumber = m_currentNumber + m_stepNumber * Time.deltaTime;

            if (m_stepNumber > 0.0f) {
                //This is a increment tick.
                if (m_currentNumber >= m_goalNumber) {
                    m_currentNumber = m_goalNumber;
                    //Finished
                }
            } else {
                //This is a decrement tick.
                if (m_currentNumber <= m_goalNumber) {
                    m_currentNumber = m_goalNumber;
                    //Finished
                }
            }

            int currentNumberNew = CurrentNumber;
            if (currentNumberOld != currentNumberNew) {
                UpdateDisplay();
                onTick.Invoke(currentNumberNew);
            }

            if (m_currentNumber == m_goalNumber) {
                //Finished
                m_isTicking = false;
                onTickComplete.Invoke(CurrentNumber);
            }
        }
    }

    private void UpdateDisplay() {
        if (targetText != null) {
            targetText.text = CurrentNumber.ToString();
        }
    }

}
