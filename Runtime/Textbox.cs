using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace McCreery.Textbox
{
    public class Textbox : MonoBehaviour
    {
        [SerializeField]
        private Text textComponent = default;
        private Text TextComponent => this.LazyGet(ref textComponent);

        [SerializeField]
        private float characterDelay = 0.05f;
        [SerializeField]
        private float whitespaceDelay = 0.075f;
        [SerializeField]
        private bool scaledTime = true;

        public float delayScale = 1.0f;

        [SerializeField]
        private UnityEvent startTyping = new UnityEvent();
        public UnityEvent StartTyping => startTyping;

        [SerializeField]
        private TypingStateEvent characterTyped = new TypingStateEvent();
        public TypingStateEvent CharacterTyped => characterTyped;

        [SerializeField]
        private UnityEvent finishTyping = new UnityEvent();
        public UnityEvent FinishTyping => finishTyping;

        private int typingId;
        public bool Typing => typingId != 0;

        public void StartText(string richText) => StartCoroutine(Text(richText));

        public IEnumerator Text(string richText)
        {
            // Announce new ID is typing
            int myTypingId = ++typingId;
            startTyping.Invoke();
            TextComponent.text = string.Empty;

            string plainText = TextUtil.StripTags(richText);

            for (int visibleLength = 1; visibleLength <= plainText.Length; visibleLength++)
            {
                TypingState typingState = new TypingState(richText, visibleLength, plainText);
                float delay = GetDelay(typingState) * delayScale;

                // Finish typing (with cleanup) if delay is 0
                if (delay == 0)
                {
                    break;
                }
                else
                {
                    yield return YieldUtil.WaitForSecondsScaled(delay, scaledTime);
                }

                // Stop typing if anyone else has started
                if (typingId != myTypingId) yield break;

                TextComponent.text = TextUtil.InsertTagRichText(richText, visibleLength, plainText.Length, "<color=#fff0>", "</color>");
                characterTyped.Invoke(typingState);
            }

            TextComponent.text = richText;
            typingId = 0;
            finishTyping.Invoke();
        }

        private float GetDelay(TypingState typingState)
        {
            return typingState.CurrentWord.Contains(typingState.CursorPos - 1)
                ? characterDelay : whitespaceDelay;
        }
    }

    [Serializable]
    public struct RangeFloat
    {
        public float min, max;
    }
}
