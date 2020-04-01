using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

/// <summary>
/// A simple effect component for typing effect for UGUI text.
/// This version do pre-display transparant character.
/// </summary>
[RequireComponent(typeof(Text))]
public class Typer : MonoBehaviour {

    public UnityEvent onTypeStart;
    public UnityEvent onTypeText;
    public UnityEvent onTypeComplete;

    //Speed settings.
    public float typingTextIntervalNormal = 0.04f;
    public float typingTextIntervalSpeedUp = 0.01f;

    private Text targetText;

    private float m_typingTextInterval = 0.04f;

    private bool m_isTypingText = false;
    private string m_completeString = "";
    private string m_typingString = "";
    private int m_typingIndex = 1;
    private float m_typingDelay = 0.0f;
    private bool m_speedUp = false;

    //The queue for put a phrase into Text component one at a time.
    private Queue<string> phrases = new Queue<string>();

    void Awake() {
        targetText = GetComponent<Text>();
        if (targetText == null) {
            Debug.LogWarning("Typer need to be with a UGUI Text to work properly!");
        }
    }

    public void TypeText(string str) {
        if (targetText != null) {
            Initialize();
            ParseIntoPhrased(str);
            m_completeString = str;
            m_typingString = string.Empty;
            m_isTypingText = true;
            onTypeStart.Invoke();
        }
    }

    void Update() {
        //If is typing text.
        if (m_isTypingText) {
            //Check end.
            if (m_typingIndex > m_typingString.Length) {
                if (phrases.Count > 0) {
                    //Add a new phrase.
                    m_typingString = m_typingString + phrases.Dequeue();
                } else {
                    //No phrase left. End typing.
                    m_isTypingText = false;
                    onTypeComplete.Invoke();
                }
            } else {
                //Check delay.
                if (m_typingDelay <= 0.0f) {

                    //[err...tricky]: Display multiple character in a frame if TYPING_TEXT_DELAY is too small.
                    while (m_typingDelay <= 0.0f && m_typingIndex <= m_typingString.Length) {
                        //type a text.
                        string currentDisplayString = m_typingString.Substring(0, m_typingIndex);
                        //Check the new character and see if it is '<'
                        if (currentDisplayString.EndsWith("<")) {
                            //This is the beginning of a Rich text element tag. We should find the tag name.
                            int matchIndexOfEndOpenTag = m_typingString.IndexOf(">", m_typingIndex);
                            string openTagContent = m_typingString.Substring(m_typingIndex, matchIndexOfEndOpenTag - m_typingIndex);
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
                            int startIndexOfCloseTag = m_typingString.IndexOf(closeTag, m_typingIndex);

                            //Set the new m_typingIndex directly to the close tag to display the whole rich text.
                            m_typingIndex = startIndexOfCloseTag + closeTag.Length;
                            targetText.text = FormDisplayingStringWithTransparantTail(m_typingString, m_typingIndex);
                        } else {
                            //Nope, just display it.
                            targetText.text = FormDisplayingStringWithTransparantTail(m_typingString, m_typingIndex);
                        }

                        //Incerment m_typingTextIndex.
                        m_typingIndex++;

                        //Check Add new phrase.
                        if (m_typingIndex > m_typingString.Length && phrases.Count > 0) {
                            //Add a new phrase.
                            m_typingString = m_typingString + phrases.Dequeue();
                        }

                        m_typingDelay = m_typingTextInterval + m_typingDelay;

                        onTypeText.Invoke();
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

    //===========================================

    private void RefreshTypingTextInterval() {
        if (m_speedUp) {
            m_typingTextInterval = typingTextIntervalSpeedUp;
        } else {
            m_typingTextInterval = typingTextIntervalNormal;
        }
    }

    private bool ParseIntoPhrased(string processingString) {
        phrases.Clear();

        while (!string.IsNullOrEmpty(processingString)) {

            //Find Open/Close Tag and treat them and the stuff inside as a phrases.

            int indexOfSeperateCharacter = processingString.IndexOfAny(new char[] { ' ', '\n', '<' });
            int indexOfFirstSpace = processingString.IndexOf(" ");
            int indexOfFirstNewLine = processingString.IndexOf('\n');
            int indexOfFirstopenTagStarting = processingString.IndexOf("<");

            if (indexOfSeperateCharacter == -1) {
                //No rest space and openTag found.
                //Chip the whole string.
                phrases.Enqueue(processingString);
                processingString = string.Empty;
            } else if (indexOfSeperateCharacter != -1 && indexOfSeperateCharacter == indexOfFirstSpace) {
                //Space comes first.
                //Just sub string till the space.
                phrases.Enqueue(processingString.Substring(0, indexOfFirstSpace + 1));
                processingString = processingString.Remove(0, indexOfFirstSpace + 1);
                //...and the while loop goes on...
            } else if (indexOfSeperateCharacter != -1 && indexOfSeperateCharacter == indexOfFirstNewLine) {
                //Newline comes first.
                //Just sub string till the space.
                phrases.Enqueue(processingString.Substring(0, indexOfFirstNewLine + 1));
                processingString = processingString.Remove(0, indexOfFirstNewLine + 1);
                //...and the while loop goes on...
            } else if (indexOfSeperateCharacter != -1 && indexOfSeperateCharacter == indexOfFirstopenTagStarting) {
                //OpenTagStarting comes first.
                if (indexOfFirstopenTagStarting == 0) {
                    //"<" at index 0. We need to find the whole ELEMENT and treat them as a phrase...

                    //This is the beginning of a Rich text element tag. We should find the tag name.
                    int matchIndexOfEndOpenTag = processingString.IndexOf(">");
                    if (matchIndexOfEndOpenTag != -1) {
                        //Found the end of open tag index.
                        string openTagContent = processingString.Substring(1, matchIndexOfEndOpenTag - 1);
                        // Debug.Log("Tag content : " + openTagContent);
                        string openTagName = "";
                        if (openTagContent.Contains("=")) {
                            openTagName = openTagContent.Substring(0, openTagContent.IndexOf("="));
                            // Debug.Log("openTagName: " + openTagName);
                        } else {
                            openTagName = openTagContent;
                        }

                        //Form the close tag.
                        string closeTag = "</" + openTagName + ">";

                        //Find the end of the element.
                        int startIndexOfCloseTag = processingString.IndexOf(closeTag, 0);

                        if (startIndexOfCloseTag != -1) {
                            //Set the new m_typingIndex directly to the close tag to display the whole rich text.
                            int endIndexOfCloseTag = startIndexOfCloseTag + closeTag.Length;

                            phrases.Enqueue(processingString.Substring(0, endIndexOfCloseTag));
                            processingString = processingString.Remove(0, endIndexOfCloseTag);
                            // Debug.Log("processingString = " + processingString);
                        } else {
                            //Cannot find the close tag!
                            Debug.LogWarning("Cannot find a close tag: [" + closeTag + "] Make sure you use it right!");
                            phrases.Enqueue(processingString.Substring(0, matchIndexOfEndOpenTag));
                            processingString = processingString.Remove(0, matchIndexOfEndOpenTag);
                            return false;
                        }
                    } else {
                        //No end. This is a pure symbol. Just add this symbol
                        phrases.Enqueue(processingString.Substring(0, 1));
                        processingString = processingString.Remove(0, 1);
                        // Debug.Log("processingString = " + processingString);
                    }
                } else {
                    //Sub string till the indexOfFirstopenTagStarting. Since we want to just stop before the ELEMENT
                    phrases.Enqueue(processingString.Substring(0, indexOfFirstopenTagStarting));
                    processingString = processingString.Remove(0, indexOfFirstopenTagStarting);
                }
            }
        }

        // //Print phrases for debug.
        // Debug.Log("========================");
        // foreach (string item in phrases) {
        //     Debug.Log("[" + item + "]");
        // }
        // Debug.Log("========================");

        return true;
    }

    //Form a displaying string with a transparant tail
    private string FormDisplayingStringWithTransparantTail(string originalText, int startTransparentIndex) {
        // Debug.Log("FormDisplayingStringWithTransparantTail: [" + originalText + "] [" + startTransparentIndex + "]");
        string returnString = originalText;
        //Insert open transparant color tag.
        string openTag = "<color=#00000000>";
        returnString = returnString.Insert(startTransparentIndex, openTag);

        //Insert close transparant color tag.
        string closeTag = "</color>";
        returnString = returnString.Insert(returnString.Length, closeTag);

        return returnString;
    }


}
