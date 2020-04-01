namespace RedBlueGames.Tools.TextTyper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    /// <summary>
    /// Type text component types out Text one character at a time. Heavily adapted from synchrok's GitHub project.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public sealed class TextTyper : MonoBehaviour
    {
        /// <summary>
        /// The print delay setting. Could make this an option some day, for fast readers.
        /// </summary>
        // private float PrintDelaySetting = 0.04f;

        /// [Hack by Neo] Speed settings.
        public float typingTextIntervalNormal = 0.04f;
        public float typingTextIntervalSpeedUp = 0.01f;

        // Characters that are considered punctuation in this language. TextTyper pauses on these characters
        // a bit longer by default. Could be a setting sometime since this doesn't localize.
        private readonly List<char> punctutationCharacters = new List<char>
        {
            '.',
            ',',
            '!',
            '?'
        };

        [SerializeField]
        [Tooltip("Event that's called when the text has start printing.")]
        private UnityEvent printStart = new UnityEvent();

        [SerializeField]
        [Tooltip("Event that's called when the text has finished printing.")]
        private UnityEvent printCompleted = new UnityEvent();

        [SerializeField]
        [Tooltip("Event called when a character is printed. Inteded for audio callbacks.")]
        private CharacterPrintedEvent characterPrinted = new CharacterPrintedEvent();

        private Text textComponent;
        private string printingText;
        private float defaultPrintDelay;

        private bool m_isTypingText = false;
        private int m_typingIndex = 1;
        private float m_typingDelay = 0.0f;
        private TypedTextGenerator generator = null;

        /// <summary>
        /// Gets the PrintCompleted callback event.
        /// </summary>
        /// <value>The print completed callback event.</value>
        public UnityEvent PrintStart {
            get {
                return this.printStart;
            }
        }

        /// <summary>
        /// Gets the PrintCompleted callback event.
        /// </summary>
        /// <value>The print completed callback event.</value>
        public UnityEvent PrintCompleted
        {
            get
            {
                return this.printCompleted;
            }
        }

        /// <summary>
        /// Gets the CharacterPrinted event, which includes a string for the character that was printed.
        /// </summary>
        /// <value>The character printed event.</value>
        public CharacterPrintedEvent CharacterPrinted
        {
            get
            {
                return this.characterPrinted;
            }
        }

        private Text TextComponent
        {
            get
            {
                if (this.textComponent == null)
                {
                    this.textComponent = this.GetComponent<Text>();
                }

                return this.textComponent;
            }
        }

        /// <summary>
        /// Types the text into the Text component character by character, using the specified (optional) print delay per character.
        /// </summary>
        /// <param name="text">Text to type.</param>
        /// <param name="printDelay">Print delay (in seconds) per character.</param>
        public void TypeText(string text, float printDelay = -1)
        {
            this.defaultPrintDelay = printDelay > 0 ? printDelay : typingTextIntervalNormal;
            this.printingText = text;

            this.OnTypewritingStart();
            this.TextComponent.text = string.Empty;
            m_typingDelay = 0.0f;
            m_typingIndex = 1;

            m_isTypingText = true;
            generator = new TypedTextGenerator();
        }

        // [Hack By Neo] 
        public void SetSpeedUp(bool b) {
            if (b) {
                defaultPrintDelay = typingTextIntervalSpeedUp;
            } else {
                defaultPrintDelay = typingTextIntervalNormal;
            }
        }

        /// <summary>
        /// Skips the typing to the end.
        /// </summary>
        public void Skip()
        {
            // this.Cleanup();
            var generator = new TypedTextGenerator();
            var typedText = generator.GetCompletedText(this.printingText);
            this.TextComponent.text = typedText.TextToPrint;
            m_isTypingText = false;
            this.OnTypewritingComplete();
        }

        public void Clear() {
            m_isTypingText = false;
            this.TextComponent.text = string.Empty;
        }

        public bool IsTyping() {
            return m_isTypingText;
        }

        void Update() {
            //If is typing text.
            if (m_isTypingText) {
                //Check end.
                if (m_typingIndex > this.printingText.Length) {
                    //End typing.
                    m_isTypingText = false;
                    this.OnTypewritingComplete();
                } else {
                    //Check delay.
                    if (m_typingDelay <= 0.0f) {

                        //[err...tricky]: Display multiple character in a frame if TYPING_TEXT_DELAY is too small.
                        while (m_typingDelay <= 0.0f && m_typingIndex <= this.printingText.Length) {

                            TypedTextGenerator.TypedText typedText = generator.GetTypedTextAt(this.printingText, m_typingIndex);
                            this.TextComponent.text = typedText.TextToPrint;
                            this.OnCharacterPrinted(typedText.LastPrintedChar.ToString());

                            ++m_typingIndex;

                            float delay = typedText.Delay > 0 ? typedText.Delay : this.GetPrintDelayForCharacter(typedText.LastPrintedChar);

                            m_typingDelay = delay + m_typingDelay;

                            this.OnCharacterPrinted(typedText.LastPrintedChar.ToString());
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

        private float GetPrintDelayForCharacter(char characterToPrint)
        {
            // Then get the default print delay for the current character
            float punctuationDelay = this.defaultPrintDelay * 8.0f;
            if (this.punctutationCharacters.Contains(characterToPrint))
            {
                return punctuationDelay;
            }
            else
            {
                return this.defaultPrintDelay;
            }
        }

        private void OnTypewritingStart() 
        {
            if (this.PrintStart != null) 
            {
                this.PrintStart.Invoke();
            }
        }

        private void OnCharacterPrinted(string printedCharacter)
        {
            if (this.CharacterPrinted != null)
            {
                this.CharacterPrinted.Invoke(printedCharacter);
            }
        }

        private void OnTypewritingComplete()
        {
            if (this.PrintCompleted != null)
            {
                this.PrintCompleted.Invoke();
            }
        }

        /// <summary>
        /// Event that signals a Character has been printed to the Text component.
        /// </summary>
        [System.Serializable]
        public class CharacterPrintedEvent : UnityEvent<string>
        {
        }
    }
}