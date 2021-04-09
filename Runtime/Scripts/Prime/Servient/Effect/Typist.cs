using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using Teamuni.Codebase;

/// <summary>
/// A simple effect component for typing effect for UGUI text.
/// </summary>
[RequireComponent(typeof(Text))]
public class Typist : MonoBehaviour {

    public UnityEvent onTypeStart;
    public UnityEvent onTypeText;
    public UnityEvent onTypeComplete;

    /// <summary>
    /// Typing Progress: 0.0 ~ 1.0
    /// </summary>
    public UnityEventFloat onTypeProgress;

    //Speed settings.
    public float typingTextIntervalNormal = 0.04f;
    public float typingTextIntervalSpeedUp = 0.01f;

    private Text targetText;

    private float m_typingTextInterval = 0.04f;

    private bool m_isTypingText = false;
    private string m_completeString = "";
    private int m_typingIndex = 1;
    private float m_typingDelay = 0.0f;
    private bool m_speedUp = false;

    void Awake() {
        targetText = GetComponent<Text>();
        if (targetText == null) {
            Debug.LogWarning("Typist need to be with an UGUI Text to work properly!");
        }
    }

    public void TypeText(string str) {
        if (targetText != null) {
            Initialize();
            m_completeString = str;
            m_isTypingText = true;
            onTypeStart.Invoke();
            InvokeProgress();
        }
    }

    void Update() {
        //If is typing text.
        if (m_isTypingText) {
            //Check end.
            if (m_typingIndex > m_completeString.Length) {
                //End typing.
                m_isTypingText = false;
                onTypeComplete.Invoke();
            } else {
                //Check delay.
                if (m_typingDelay <= 0.0f) {

                    //[err...tricky]: Display multiple character in a frame if TYPING_TEXT_DELAY is too small.
                    while (m_typingDelay <= 0.0f && m_typingIndex <= m_completeString.Length) {
                        //type a text.
                        string currentDisplayString = m_completeString.Substring(0, m_typingIndex);
                        //Check the new character and see if it is '<'
                        if (currentDisplayString.EndsWith("<")) {
                            //This is the beginning of a Rich text element tag. We should find the tag name.
                            int matchIndexOfEndOpenTag = m_completeString.IndexOf(">", m_typingIndex);
                            string openTagContent = m_completeString.Substring(m_typingIndex, matchIndexOfEndOpenTag - m_typingIndex);
                            //Debug.Log("Tag content : " + m_typingString.Substring(m_typingIndex, matchIndexOfEndOpenTag - m_typingIndex));
                            string openTagName = "";
                            if (openTagContent.Contains("=")) {
                                openTagName = openTagContent.Substring(0, openTagContent.IndexOf("="));
                                //Debug.Log("openTagName: " + openTagName);
                            } else {
                                openTagName = openTagContent;
                            }

                            //Form the close tag.
                            string closeTag = "</" + openTagName + ">";

                            //Find the end of the element.
                            int startIndexOfCloseTag = m_completeString.IndexOf(closeTag, m_typingIndex);

                            //Set the new m_typingIndex directly to the close tag to display the whole rich text.
                            m_typingIndex = startIndexOfCloseTag + closeTag.Length;
                            string completeRichTextDisplayString = m_completeString.Substring(0, m_typingIndex);
                            targetText.text = completeRichTextDisplayString;
                        } else {
                            //Nope, just display it.
                            targetText.text = currentDisplayString;
                        }

                        //Debug.Log("LineCount:" + m_targetText.cachedTextGenerator.lineCount);

                        //Incerment m_typingTextIndex.
                        m_typingIndex++;

                        m_typingDelay = m_typingTextInterval + m_typingDelay;

                        onTypeText.Invoke();
                        InvokeProgress();
                    }//End while.

                    //Just delay and nothing.
                    m_typingDelay -= Time.deltaTime;
                } else {
                    //Just delay and nothing.
                    m_typingDelay -= Time.deltaTime;
                }
            }
        }
    }

    public void Initialize() {
        //Clear display text and start typing.
        targetText.text = "";
        m_typingDelay = 0.0f;
        m_typingIndex = 1;
        RefreshTypingTextInterval();
    }

    public bool IsTyping() {
        return m_isTypingText;
    }

    public void SetSpeedUp(bool b) {
        m_speedUp = b;
        RefreshTypingTextInterval();
    }

    public bool InstantComplete(bool invokeTypeComplete = true) {
        if (m_isTypingText) {
            m_isTypingText = false;
            targetText.text = m_completeString;
            if (invokeTypeComplete) {
                onTypeComplete.Invoke();
                InvokeProgress();
            }
            return true;
        }
        return false;
    }

    public void Clear() {
        m_isTypingText = false;
        if (targetText != null) {
            targetText.text = string.Empty;
        }
    }

    public void SetFontSize(int size) {
        targetText.fontSize = size;
    }

    //===========================================

    private void InvokeProgress() {
        if (m_completeString.Length > 0) {
            onTypeProgress.Invoke((float)targetText.text.Length / (float)m_completeString.Length);
        } else {
            onTypeProgress.Invoke(1.0f);
        }
    }

    private void RefreshTypingTextInterval() {
        if (m_speedUp) {
            m_typingTextInterval = typingTextIntervalSpeedUp;
        } else {
            m_typingTextInterval = typingTextIntervalNormal;
        }
    }
}
