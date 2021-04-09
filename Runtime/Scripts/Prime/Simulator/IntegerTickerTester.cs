using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegerTickerTester : MonoBehaviour {

    public IntegerTicker integerTicker;
    public int initialNumber = 0;
    public int goalNumber = 100;
    public float durationTime = 5.0f;

    public void OnGoClicked() {
        integerTicker.StartTicking(initialNumber, goalNumber, durationTime);
    }

    public void OnTickStart(int value) {
        Debug.Log("OnTickStart [" + value + "]");
    }

    public void OnTick(int value) {
        Debug.Log("OnTick [" + value + "]");
    }

    public void OnTickComplete(int value) {
        Debug.Log("OnTickComplete [" + value + "]");
    }

}
