using UnityEngine;

/// <summary>
/// This is a simple time scale manager.
/// </summary>
public class TheStar : MonoBehaviour {

    static private float BaseTimeScale = 1.0f;
    static private float Mod = 0.0f;
    static private float Multi = 1.0f;
    static private TheStar Instance = null;

    //Time.timeScale = (BaseTimeScale + Mod) * Multi 

    void Awake() {
        Instance = this;
    }

    /// <summary>
    /// Internal use.
    /// </summary>
    static private void UpdateTimeScale() {
        Time.timeScale = (BaseTimeScale + Mod) * Multi;
    }

    /// <summary>
    /// Note your scale down/up rate better be a pair of inverse numbers.
    /// </summary>
    /// <param name="rate"></param>
    static public void SetMulti(float multi) {
        Multi = multi;
        UpdateTimeScale();
    }

    /// <summary>
    /// Get the current Multi.
    /// </summary>
    /// <returns></returns>
    static public float GetMulti() {
        return Multi;
    }

    /// <summary>
    /// Reset the Multi var.
    /// </summary>
    static public void ResetMulti() {
        Multi = 1.0f;
    }

    /// <summary>
    /// Set timeScale directly.
    /// </summary>
    /// <param name="value"></param>
    static public void SetBase(float value) {
        BaseTimeScale = value;
        UpdateTimeScale();
    }

    /// <summary>
    /// Get the current time scale rate.
    /// </summary>
    /// <returns></returns>
    static public float GetBase() {
        return BaseTimeScale;
    }

    /// <summary>
    /// Reset the Base var.
    /// </summary>
    static public void ResetBase() {
        BaseTimeScale = 1.0f;
    }

    /// <summary>
    /// Add timeScale directly.
    /// </summary>
    /// <param name="value"></param>
    static public void SetMod(float mod) {
        Mod = mod;
        UpdateTimeScale();
    }

    /// <summary>
    /// Get the currnet mod.
    /// </summary>
    /// <returns></returns>
    static public float GetMod() {
        return Mod;
    }

    /// <summary>
    /// Reset the Mod var.
    /// </summary>
    static public void ResetMod() {
        Mod = 0.0f;
        UpdateTimeScale();
    }

    /// <summary>
    /// Reset the time scale.
    /// </summary>
    static public void Reset() {
        ResetBase();
        ResetMod();
        ResetMulti();
        UpdateTimeScale();
    }

    /// <summary>
    /// The the current time scale.
    /// </summary>
    /// <returns></returns>
    static public float CurrentTimeScale() {
        return Time.timeScale;
    }

    /// <summary>
    /// Are we pausing the time?
    /// </summary>
    /// <returns></returns>
    static public bool IsPausingTime() {
        if (Time.timeScale > 0.0f) {
            return false;
        } else {
            return true;
        }
    }

    /// <summary>
    /// Temperary set the time scale to 0.
    /// </summary>
    static public void PauseTime() {
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// Resture time scale.
    /// </summary>
    static public void ResumeTime() {
        UpdateTimeScale();
    }
}
