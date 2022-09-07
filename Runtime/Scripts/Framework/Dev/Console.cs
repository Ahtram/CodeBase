using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

//使用UGUI系統製作的Console類別，可用在執行階段於銀幕上顯示字串。您不需要手動新增這個Component，請愛用Project內的Console Prefab。
public class Console : MonoBehaviour {

    //Console Text主體
    public TextMeshProUGUI consoleText;
    public CanvasGroup canvasGroup;

    //同時也顯示在 Unity console 視窗內。
    public bool unityConsoleLog = true;

    //記錄睛顯示的所有字串。做成Queue型式以便先進先出。
    static private Queue<string> m_consoleStrings = new Queue<string>();

    //最多顯示幾筆字串?
    public int maxConsoleLine = 30;

    static private Console instance = null;

    void Awake() {
        instance = this;
    }

    //清除畫面上的所有字串。
    static public void CLR() {
        if (instance != null) {
            //清除所有顯示中字串。包含Queue與顯示用Component。
            m_consoleStrings.Clear();
            instance.consoleText.text = "";
        }
    }

    //顯示一行白色字串。
    static public void Out(string strIn) {
        if (instance != null) {
            //限叔摦示筆數。
            if (m_consoleStrings.Count >= instance.maxConsoleLine) {
                m_consoleStrings.Dequeue();
            }

            Color consoleColor = Color.white;
            Color editorColor = Color.black;
#if UNITY_EDITOR
            if (EditorGUIUtility.isProSkin) {
                editorColor = ColorPlus.WhiteSmoke;
            } else {
                editorColor = ColorPlus.Black;
            }
#endif

            string consoleStrIn = "<color=#" + ColorUtility.ToHtmlStringRGB(consoleColor) + ">" + strIn + "</color>";
            m_consoleStrings.Enqueue(consoleStrIn);

            if (instance.unityConsoleLog) {
                string editorStrIn = "<color=#" + ColorUtility.ToHtmlStringRGB(editorColor) + ">" + strIn + "</color>";
                Debug.Log(editorStrIn);
            }

            //輸出字串於Component上。
            RefreshConsoleText();
        }
    }

    //顯示一行綠色字串。
    static public void OutGood(string strIn) {
        if (instance != null) {
            if (m_consoleStrings.Count >= instance.maxConsoleLine) {
                m_consoleStrings.Dequeue();
            }

            Color consoleColor = ColorPlus.Chartreuse;
            Color editorColor = ColorPlus.Chartreuse;
#if UNITY_EDITOR
            if (EditorGUIUtility.isProSkin) {
                editorColor = ColorPlus.Chartreuse;
            } else {
                editorColor = ColorPlus.DarkGreen;
            }
#endif

            string consoleStrIn = "<color=#" + ColorUtility.ToHtmlStringRGB(consoleColor) + ">" + strIn + "</color>";
            m_consoleStrings.Enqueue(consoleStrIn);

            if (instance.unityConsoleLog) {
                string editorStrIn = "<color=#" + ColorUtility.ToHtmlStringRGB(editorColor) + ">" + strIn + "</color>";
                Debug.Log(editorStrIn);
            }

            RefreshConsoleText();
        }
    }

    //顯示一行黃色字串。
    static public void OutWarning(string strIn) {
        if (instance != null) {
            if (m_consoleStrings.Count >= instance.maxConsoleLine) {
                m_consoleStrings.Dequeue();
            }

            Color consoleColor = ColorPlus.LightGoldenrodYellow;
            Color editorColor = ColorPlus.LightGoldenrodYellow;
#if UNITY_EDITOR
            if (EditorGUIUtility.isProSkin) {
                editorColor = ColorPlus.LightGoldenrodYellow;
            } else {
                editorColor = ColorPlus.Brown;
            }
#endif

            string consoleStrIn = "<color=#" + ColorUtility.ToHtmlStringRGB(consoleColor) + ">" + strIn + "</color>";
            m_consoleStrings.Enqueue(consoleStrIn);

            if (instance.unityConsoleLog) {
                string editorStrIn = "<color=#" + ColorUtility.ToHtmlStringRGB(editorColor) + ">" + strIn + "</color>";
                Debug.LogWarning(editorStrIn);
            }

            RefreshConsoleText();
        }
    }

    //顯示一行錯誤字串。
    static public void OutError(string strIn) {
        if (instance != null) {
            if (m_consoleStrings.Count >= instance.maxConsoleLine) {
                m_consoleStrings.Dequeue();
            }

            Color consoleColor = ColorPlus.PaleVioletRed;
            Color editorColor = ColorPlus.PaleVioletRed;
#if UNITY_EDITOR
            if (EditorGUIUtility.isProSkin) {
                editorColor = ColorPlus.PaleVioletRed;
            } else {
                editorColor = ColorPlus.DarkRed;
            }
#endif

            string consoleStrIn = "<color=#" + ColorUtility.ToHtmlStringRGB(consoleColor) + ">" + strIn + "</color>";
            m_consoleStrings.Enqueue(consoleStrIn);

            if (instance.unityConsoleLog) {
                string editorStrIn = "<color=#" + ColorUtility.ToHtmlStringRGB(editorColor) + ">" + strIn + "</color>";
                Debug.LogError(editorStrIn);
            }

            RefreshConsoleText();
        }
    }

    //換行。
    static public void NewLine() {
        if (instance != null) {
            Out("");
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
            instance.canvasGroup.interactable = true;
            instance.canvasGroup.blocksRaycasts = true;
        }
    }

    //隱藏Console?
    static public void Hide() {
        if (instance != null) {
            instance.canvasGroup.alpha = 0;
            instance.canvasGroup.interactable = false;
            instance.canvasGroup.blocksRaycasts = false;
        }
    }

    //切換隱藏顯示
    static public void SwitchVisible() {
        if (instance != null) {
            if (instance.canvasGroup.alpha == 0) {
                instance.canvasGroup.alpha = 1;
                instance.canvasGroup.interactable = true;
                instance.canvasGroup.blocksRaycasts = true;
            } else {
                instance.canvasGroup.alpha = 0;
                instance.canvasGroup.interactable = false;
                instance.canvasGroup.blocksRaycasts = false;
            }
        }
    }

    //更新當前Queue內的所有字串至Component上。
    static private void RefreshConsoleText() {
        if (instance != null) {
            string outString = "";
            foreach (string str in m_consoleStrings) {
                outString += ":";
                outString += str;
                outString += System.Environment.NewLine;
            }

            instance.consoleText.text = outString;
        }
    }

}
