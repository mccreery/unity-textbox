using System.Collections;
using UnityEngine;

namespace McCreery.Textbox
{
    public class BasicDialogue : MonoBehaviour, IDialogue
    {
        [SerializeField]
        [Multiline]
        private string[] messages = default;

        [SerializeField]
        private bool automatic = false;
        [SerializeField]
        private float automaticDelay = 0.0f;
        [SerializeField]
        private bool useScaledTime = true;

        [SerializeField]
        private bool clearOnFinish = true;

        public IEnumerator Play(DialogueManager manager, int id)
        {
            foreach (string message in messages)
            {
                yield return manager.Prompt.Text(message, !automatic);
                if (automatic)
                {
                    yield return YieldUtil.WaitForSecondsScaled(automaticDelay, useScaledTime);
                }

                if (manager.RunningId != id) yield break;
            }

            if (clearOnFinish)
            {
                yield return manager.Prompt.Text("", false);
            }
        }
    }
}
