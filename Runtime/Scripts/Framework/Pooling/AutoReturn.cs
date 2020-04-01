using UnityEngine;
using System;

public class AutoReturn : MonoBehaviour {

    public Action onReturning = null;
    public float time = 3.0f;
    private bool m_isCounting = false;

    void Awake() {
        StartCount();
    }

    public void StartCount(float countTime) {
        time = countTime;
        m_isCounting = true;
        StopCount();
        Invoke("ReturnNow", time);
    }

    public void StartCount() {
        StopCount();
        Invoke("ReturnNow", time);
    }

    public void StopCount() {
        m_isCounting = false;
        CancelInvoke("ReturnNow");
    }

    public bool IsCounting() {
        return m_isCounting;
    }

    public void ReturnNow() {
        if(onReturning != null) {
            onReturning();
            onReturning = null;
        }

        StopCount();
        GameObjectPool.Return(gameObject);
    }

}
