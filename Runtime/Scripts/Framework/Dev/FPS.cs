using UnityEngine;
using UnityEngine.UI;

//A class for showing FPS in game.
public class FPS : MonoBehaviour {

    public Text fpsText;

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

    static public void Show() {
        if (instance != null) {
            instance.fpsText.enabled = true;
        }
    }

    static public void Hide() {
        if (instance != null) {
            instance.fpsText.enabled = false;
        }
    }

    static public void SwitchVisible() {
        if (instance != null) {
            if (instance.fpsText.enabled) {
                instance.fpsText.enabled = false;
            } else {
                instance.fpsText.enabled = true;
            }
        }
    }

}
