using System.Collections;
using UnityEngine;

namespace McCreery.Textbox
{
    public class TextboxPrompt : MonoBehaviour
    {
        [SerializeField]
        private Textbox textbox;
        private Textbox Textbox => this.LazyGet(ref textbox, true);

        [SerializeField]
        private string buttonName = "Submit";

        [SerializeField]
        private float turboDelayScale = 1.0f;

        public IEnumerator Text(string richText, bool wait = true)
        {
            Coroutine textCoro = StartCoroutine(Textbox.Text(richText));

            // Start waiting 1 frame later to avoid double button press
            yield return null;
            // Wait coroutine also handles turbo
            Coroutine waitCoro = StartCoroutine(Wait());

            if (wait)
            {
                // Implicitly waits for text
                yield return waitCoro;
            }
            else
            {
                yield return textCoro;
            }
        }

        private IEnumerator Wait()
        {
            Textbox.DelayScale = 1.0f;

            while (Textbox.Typing || !Input.GetButtonDown(buttonName))
            {
                if (Input.GetButtonDown(buttonName))
                {
                    Textbox.DelayScale = turboDelayScale;
                }
                yield return null;
            }

            Textbox.DelayScale = 1.0f;
        }
    }
}
