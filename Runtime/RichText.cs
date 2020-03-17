using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RichText
{
    private static readonly Regex Tags = new Regex("((?:<[^<>]+>)+)", RegexOptions.Compiled);

    public static IEnumerable<int> Find(string richText, Predicate<char> predicate)
    {
        string[] tokens = Tags.Split(richText);

        int length = 0;
        for (int i = 0; i < tokens.Length; i += 2)
        {
            for (int j = 0; j < tokens[i].Length; j++)
            {
                if (predicate.Invoke(tokens[i][j]))
                {
                    yield return length + j;
                }
            }
            length += tokens[i].Length;
        }
    }

    public static int Length(string richText)
    {
        string[] tokens = Tags.Split(richText);

        int length = 0;
        for (int i = 0; i < tokens.Length; i += 2)
        {
            length += tokens[i].Length;
        }
        return length;
    }

    public static string Format(string richText, int lo, int hi, string openTag, string closeTag)
    {
        string[] tokens = Tags.Split(richText);

        // Even indexed tokens are text nodes, odd are consecutive groups of tags
        int length = 0;
        for (int i = 0; i < tokens.Length; i += 2)
        {
            // Find position of tags inside text node
            int loRelative = Mathf.Clamp(lo - length, 0, tokens[i].Length);
            int hiRelative = Mathf.Clamp(hi - length, 0, tokens[i].Length);

            // Add to plaintext length before modifying token
            length += tokens[i].Length;

            // Add tags to text node
            if (loRelative < hiRelative)
            {
                tokens[i] = FormatPlainText(tokens[i], loRelative, hiRelative, openTag, closeTag);
            }
        }
        return string.Concat(tokens);
    }

    public static string FormatPlainText(string text, int lo, int hi, string openTag, string closeTag)
    {
        return text.Substring(0, lo) + openTag + text.Substring(lo, hi - lo) + closeTag + text.Substring(hi);
    }
}
