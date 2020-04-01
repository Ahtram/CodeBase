using UnityEngine;
using System;

public class SelfDestroy : MonoBehaviour {

    public Action onSuiciding = null;
    public float time = 1.0f;
    private bool m_isCounting = false;

    void Awake() {
        StartCount();
    }

    public void StartCount(float countTime) {
        time = countTime;
        m_isCounting = true;
        StopCount();
        Invoke("Suicide", time);
    }

    public void StartCount() {
        StopCount();
        Invoke("Suicide", time);
    }

    public void StopCount() {
        m_isCounting = true;
        CancelInvoke("Suicide");
    }

    public bool IsCounting() {
        return m_isCounting;
    }

    public void Suicide() {
        if(onSuiciding != null) {
            onSuiciding();
            onSuiciding = null;
        }

        StopCount();
        GameObject.Destroy(gameObject);
    }

}
