using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

            yield return YieldUtil.WaitForSecondsScaled(GetDelay(typingState), scaledTime);
            // Stop typing if anyone else has started
            if (typingId != myTypingId) yield break;

            TextComponent.text = TextUtil.InsertTagRichText(richText, visibleLength, plainText.Length, "<color=#fff0>", "</color>");
            characterTyped.Invoke(typingState);
        }

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
