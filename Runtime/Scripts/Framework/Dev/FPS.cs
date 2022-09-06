using UnityEngine;
using UnityEngine.UI;
using TMPro;

//A class for showing FPS in game.
public class FPS : MonoBehaviour {

    public TextMeshProUGUI fpsText;
    public CanvasGroup canvasGroup;

    public float m_refreshTime = 0.5F;
    private float m_accumulateTime = 0;
    private int m_accumulateFrames = 0;
    private float m_timeLeft;

    static private FPS instance = null;

    void Awake() {
        instance = this;
    }

    void Start() {
        m_timeLeft = m_refreshTime;
    }

    void Update() {
        if (instance != null) {
            m_timeLeft -= Time.deltaTime;
            m_accumulateTime += Time.timeScale / Time.deltaTime;
            ++m_accumulateFrames;

            // Interval ended - update GUI text and start new interval
            if (m_timeLeft <= 0.0) {
                // display two fractional digits (f2 format)
                float fps = m_accumulateTime / m_accumulateFrames;
                instance.fpsText.text = Mathf.Round(fps).ToString();

                if (fps >= 40.0f) {
                    instance.fpsText.color = Color.green;
                } else if (fps >= 30.0f) {
                    instance.fpsText.color = Color.cyan;
                } else if (fps >= 20.0f) {
                    instance.fpsText.color = Color.yellow;
                } else {
                    instance.fpsText.color = Color.red;
                }

                m_timeLeft = m_refreshTime;
                m_accumulateTime = 0.0F;
                m_accumulateFrames = 0;
            }
        }
    }

    //當前Console是否被顯示?
    static public bool IsShowing() {
        if (instance != null) {
            return (instance.canvasGroup.alpha != 0);
        } else {
            return false;
        }
    }

    //顯示Console?
    static public void Show() {
        if (instance != null) {
            instance.canvasGroup.alpha = 1;
        }
    }

    //隱藏Console?
    static public void Hide() {
        if (instance != null) {
            instance.canvasGroup.alpha = 0;
        }
    }

    //切換隱藏顯示
    static public void SwitchVisible() {
        if (instance != null) {
            if (instance.canvasGroup.alpha == 0) {
                instance.canvasGroup.alpha = 1;
            } else {
                instance.canvasGroup.alpha = 0;
            }
        }
    }

}
