using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace McCreery.Textbox
{
    public class TextboxDialogue : MonoBehaviour
    {
        [SerializeField]
        [Multiline]
        private string[] messages = default;

        [SerializeField]
        private bool playOnStart = true;

        [SerializeField]
        private Textbox textbox;
        private Textbox Textbox => this.LazyGet(ref textbox, true);

        [SerializeField]
        private TextboxPrompt prompt;
        private TextboxPrompt Prompt => this.LazyGet(ref prompt, true);

        [SerializeField]
        private bool automatic = false;
        [SerializeField]
        private float automaticDelay = 0.0f;
        [SerializeField]
        private bool useScaledTime = true;

        [SerializeField]
        private bool clearOnFinish = true;

        [SerializeField]
        private UnityEvent dialogueStart = default;
        public UnityEvent DialogueStart => dialogueStart;

        [SerializeField]
        private UnityEvent dialogueEnd = default;
        public UnityEvent DialogueEnd => dialogueEnd;

        private int runningId = 0;
        public bool Running => runningId != 0;

        private void Start()
        {
            if (playOnStart) StartDialogue();
        }

        public void StartDialogue()
        {
            StartCoroutine(Dialogue());
        }

        public IEnumerator Dialogue()
        {
            int myId = ++runningId;
            dialogueStart.Invoke();

            foreach (string message in messages)
            {
                if (automatic)
                {
                    yield return Textbox.Text(message);
                    yield return YieldUtil.WaitForSecondsScaled(automaticDelay, useScaledTime);
                }
                else
                {
                    yield return Prompt.TextAndWait(message);
                }

                if (runningId != myId) yield break;
            }

            runningId = 0;
            if (clearOnFinish)
            {
                yield return Textbox.Text(string.Empty);
            }
            dialogueEnd.Invoke();
        }
    }
}
