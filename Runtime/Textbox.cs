using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Textbox : MonoBehaviour
{
    [SerializeField]
    private Text textComponent = default;
    private Text TextComponent => LazyGet(ref textComponent);

    [SerializeField]
    private float characterDelay = 0.05f;
    [SerializeField]
    private float whitespaceDelay = 0.075f;
    [SerializeField]
    private bool scaledTime = true;

    [Header("Typing Sound")]
    [SerializeField]
    private AudioSource audioSourceComponent = default;
    private AudioSource AudioSourceComponent => LazyGet(ref audioSourceComponent, true);

    [SerializeField]
    private AudioClip audioClip = default;
    [SerializeField]
    private bool playOnWhitespace = false;

    [SerializeField]
    private RangeFloat randomPitchShift = default;

    [Header("Pitch shift window")]
    [SerializeField]
    private int windowSize = 2;
    [SerializeField]
    private float windowPitchShift = 0.0f;

    public void Text(string text)
    {
        StopAllCoroutines();
        StartCoroutine(TextCoro(text));
    }

    private IEnumerator TextCoro(string richText)
    {
        Queue<int> whitespaceQueue = new Queue<int>(RichText.Find(richText, c => char.IsWhiteSpace(c)));
        int length = RichText.Length(richText);
        whitespaceQueue.Enqueue(length);

        for (int i = 0; i < length; i++)
        {
            TextComponent.text = RichText.Format(richText, i + 1, length, "<color=#fff0>", "</color>");

            int distanceToWhitespace = whitespaceQueue.Peek() - i;
            bool isWhitespace = distanceToWhitespace == 0;

            if (isWhitespace)
            {
                whitespaceQueue.Dequeue();
            }

            if (!isWhitespace || playOnWhitespace)
            {
                PlayTypingSound(distanceToWhitespace);
            }

            float nextDelay = isWhitespace ? whitespaceDelay : characterDelay;
            if (scaledTime)
            {
                yield return new WaitForSeconds(nextDelay);
            }
            else
            {
                yield return new WaitForSecondsRealtime(nextDelay);
            }
        }
    }

    private void PlayTypingSound(int distanceToWhitespace)
    {
        if (audioClip != null)
        {
            AudioSourceComponent.pitch = 1.0f
                + GetWindowPitchShift(distanceToWhitespace)
                + Random.Range(randomPitchShift.min, randomPitchShift.max);

            AudioSourceComponent.PlayOneShot(audioClip);
        }
    }

    private float GetWindowPitchShift(int distanceToWhitespace)
    {
        if (windowSize == 0)
        {
            return 0.0f;
        }
        else
        {
            float t = (windowSize + 1 - distanceToWhitespace) / (float)windowSize;
            return windowPitchShift * Mathf.Clamp01(t);
        }
    }

    private T LazyGet<T>(ref T inspectorField, bool create = false) where T : Component
    {
        if (inspectorField == null)
        {
            inspectorField = GetComponent<T>();

            if (create && inspectorField == null)
            {
                inspectorField = gameObject.AddComponent<T>();
            }
        }
        return inspectorField;
    }
}

[Serializable]
public struct RangeFloat
{
    public float min, max;
}
