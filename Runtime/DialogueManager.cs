using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace McCreery.Textbox
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField]
        private TextboxPrompt prompt = default;
        public TextboxPrompt Prompt => this.LazyGet<TextboxPrompt>(ref prompt);

        [SerializeField]
        private UnityEvent beforeDialogue = default;

        /// <value>Event called immediately before starting a root dialogue</value>
        public UnityEvent BeforeDialogue => beforeDialogue;

        [SerializeField]
        private UnityEvent afterDialogue = default;

        /// <value>Event called immediately after a root dialogue completes without interruption</value>
        public UnityEvent AfterDialogue => afterDialogue;

        /// <value>The ID of the current dialogue coroutine, for checking interruption</value>
        public int RunningId { get; private set; }
        /// <value><c>true</c> when there is a dialogue running</value>
        public bool IsRunning => RunningId != 0;

        /// <summary>
        /// Starts a coroutine to play a dialogue sequence. Also used for nested dialogues.
        /// </summary>
        /// <param name="dialogue">The dialogue to play</param>
        /// <param name="parentId">If nesting, the ID from the parent dialogue</param>
        /// <returns>A coroutine to optionally <c>yield return</c> from</returns>
        public Coroutine StartDialogue(IDialogue dialogue, int parentId = 0)
        {
            return StartCoroutine(Coro(dialogue, parentId));
        }

        private IEnumerator Coro(IDialogue dialogue, int parentId = 0)
        {
            if (parentId == 0) BeforeDialogue.Invoke();

            int id = ++RunningId;
            yield return dialogue.Play(this, id);

            // No interruption
            if (RunningId == id)
            {
                RunningId = parentId;
                if (parentId == 0) AfterDialogue.Invoke();
            }
        }
    }
}
