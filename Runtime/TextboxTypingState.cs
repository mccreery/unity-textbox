using UnityEngine;

public readonly struct TextboxTypingState
{
    public readonly string richText;
    public string PlainText => RichText.StripTags(richText);

    public readonly int length;
    public readonly RangeInt currentWord;

    public bool InsideWord => length > currentWord.start && length <= currentWord.end;
}
