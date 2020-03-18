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
    private UnityEvent startTyping;
    public UnityEvent StartTyping => startTyping;

    [SerializeField]
    private TypingStateEvent characterTyped;
    public TypingStateEvent CharacterTyped => characterTyped;

    [SerializeField]
    private UnityEvent finishTyping;
    public UnityEvent FinishTyping => finishTyping;

    public bool Typing { get; private set; }

    public void Text(string text)
    {
        StopAllCoroutines();
        StartCoroutine(TextCoro(text));
    }

    private IEnumerator TextCoro(string richText)
    {
        Typing = true;
        startTyping.Invoke();

        string plainText = RichText.StripTags(richText);

        for (int visibleLength = 1; visibleLength <= plainText.Length; visibleLength++)
        {
            TypingState typingState = new TypingState(richText, visibleLength, plainText);

            yield return YieldUtil.WaitForSecondsScaled(GetDelay(typingState), scaledTime);

            TextComponent.text = RichText.InsertTagRichText(richText, visibleLength, plainText.Length, "<color=#fff0>", "</color>");
            characterTyped.Invoke(typingState);
        }

        Typing = false;
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
