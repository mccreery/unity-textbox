using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

public class RichText
{
    private static readonly Regex Tags = new Regex(@"((?:<[^<>]+>)+)", RegexOptions.Compiled);

    public static string InsertTagRichText(string richText, int start, int length, string openTag, string closeTag)
    {
        string[] tokens = Tags.Split(richText);
        int plainTextLength = 0;

        // Ignore odd tokens (tags)
        for (int i = 0; i < tokens.Length; i += 2)
        {
            string token = InsertTagPlainText(tokens[i], start - plainTextLength, length, "<color=#fff0>", "</color>");
            plainTextLength += tokens[i].Length;
            tokens[i] = token;
        }
        return string.Concat(tokens);
    }

    private static string InsertTagPlainText(string text, int start, int length, string openTag, string closeTag)
    {
        // Clamp to ends of string
        if (start < 0)
        {
            length += start;
            start = 0;
        }
        length = Mathf.Min(length, text.Length - start);

        if (length > 0)
        {
            return text.Substring(0, start) + openTag + text.Substring(start, length) + closeTag + text.Substring(start + length);
        }
        else
        {
            return text;
        }
    }

    public static string StripTags(string richText)
    {
        return Tags.Replace(richText, "");
    }

    private static readonly Regex WordStart = new Regex(@"(?<=\s|^)(?=[^\s])", RegexOptions.Compiled | RegexOptions.RightToLeft);
    private static readonly Regex WordEnd = new Regex(@"(?<=[^\s])(?=\s|$)", RegexOptions.Compiled);

    public static Substring? GetWordAt(string plainText, int i)
    {
        Match startMatch = WordStart.Match(plainText, i);
        if (!startMatch.Success) return null;

        Match endMatch = WordEnd.Match(plainText, startMatch.Index);
        Assert.IsTrue(endMatch.Success);

        return new Substring(plainText, startMatch.Index, endMatch.Index - startMatch.Index);
    }
}
