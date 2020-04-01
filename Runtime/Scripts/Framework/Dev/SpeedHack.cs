using UnityEngine;

/// <summary>
/// This is an example speed hack stuff.
/// </summary>
public class SpeedHack : MonoBehaviour {
    private bool m_isSpeedUp = false;

    private const float TIME_SCALE_UP = 2.0f;
    private const float TIME_SCALE_NORMAL = 1.0f;

    void Update() {
        //For PC
        if (Input.GetKeyUp(KeyCode.H)) {
            if (!m_isSpeedUp) {
                m_isSpeedUp = true;
                TheStar.SetMulti(TIME_SCALE_UP);
                Console.OutGood("Speed Hack On");
            } else {
                m_isSpeedUp = false;
                TheStar.SetMulti(TIME_SCALE_NORMAL);
                Console.OutGood("Speed Hack Off");
            }
        }

        //For mobile devices
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) {
            if (!m_isSpeedUp && Input.GetTouch(0).deltaPosition.y > 3 && Input.GetTouch(1).deltaPosition.y > 3) {
                m_isSpeedUp = true;
                TheStar.SetMulti(TIME_SCALE_UP);
                Console.OutGood("Speed Hack On");
            } else if (m_isSpeedUp && Input.GetTouch(0).deltaPosition.y < 3 && Input.GetTouch(1).deltaPosition.y < 3) {
                m_isSpeedUp = false;
                TheStar.SetMulti(TIME_SCALE_NORMAL);
                Console.OutGood("Speed Hack Off");
            }
        }
    }
}
