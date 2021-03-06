﻿using UnityEngine;

namespace McCreery.Textbox
{
    public class TextboxTypingSound : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSourceComponent = default;
        private AudioSource AudioSourceComponent => this.LazyGet(ref audioSourceComponent, true);

        [SerializeField]
        private Textbox textbox = default;
        private Textbox Textbox => this.LazyGet(ref textbox, true);

        [SerializeField]
        private AudioClip audioClip = default;
        [SerializeField]
        private bool playOnWhitespace = false;

        [SerializeField]
        private float minimumDelay = 0.0f;

        [SerializeField]
        private RangeFloat randomPitchShift = default;

        [Header("Pitch shift window")]
        [SerializeField]
        private int windowSize = 2;
        [SerializeField]
        private float windowPitchShift = 0.0f;

        private void Start()
        {
            Textbox.CharacterTyped.AddListener(typingState =>
            {
                int distanceToWhitespace = typingState.CurrentWord.End - (typingState.CursorPos - 1);

                if (playOnWhitespace || distanceToWhitespace != 0)
                {
                    PlayTypingSound(distanceToWhitespace);
                }
            });
        }

        private float lastPlayTime;

        private void PlayTypingSound(int distanceToWhitespace)
        {
            if (isActiveAndEnabled && audioClip != null
                && Time.unscaledTime - lastPlayTime >= minimumDelay)
            {
                AudioSourceComponent.pitch = 1.0f
                    + GetWindowPitchShift(distanceToWhitespace)
                    + Random.Range(randomPitchShift.min, randomPitchShift.max);

                lastPlayTime = Time.unscaledTime;
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
    }
}
