using System;
using UnityEngine.Events;

namespace McCreery.Textbox
{
    public struct TypingState
    {
        public string FullRichText { get; }
        public int CursorPos { get; }

        private string fullPlainText;
        public string FullPlainText => fullPlainText = fullPlainText ?? TextUtil.StripTags(FullRichText);

        private Substring? visiblePlainText;
        public Substring VisiblePlainText => (Substring)(visiblePlainText = visiblePlainText ?? new Substring(FullPlainText, 0, CursorPos));

        private Substring? currentWord;
        public Substring CurrentWord => (Substring)(currentWord = currentWord ?? TextUtil.GetWordAt(FullPlainText, CursorPos - 1));

        public TypingState(string richText, int cursorPos, string fullPlainText = null)
        {
            FullRichText = richText;
            CursorPos = cursorPos;

            this.fullPlainText = fullPlainText;
            visiblePlainText = null;
            currentWord = null;
        }
    }

    [Serializable]
    public class TypingStateEvent : UnityEvent<TypingState> { }
}
