using System.Collections;
using UnityEngine;

public class TextboxPrompt : MonoBehaviour
{
    [SerializeField]
    private Textbox textbox;
    public Textbox Textbox => this.LazyGet(ref textbox, true);

    [SerializeField]
    private string buttonName = "Submit";

    [SerializeField]
    private float turboDelayScale = 1.0f;

    public IEnumerator Wait()
    {
        Textbox.delayScale = 1.0f;

        while (true)
        {
            if (Input.GetButtonDown(buttonName))
            {
                if (Textbox.Typing)
                {
                    Textbox.delayScale = turboDelayScale;
                    yield return null;
                }
                else
                {
                    Textbox.delayScale = 1.0f;
                    yield break;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    public IEnumerator TextAndWait(string richText)
    {
        Coroutine text = StartCoroutine(Textbox.Text(richText));

        // Start waiting 1 frame later to avoid double button press
        yield return null;
        Coroutine wait = StartCoroutine(Wait());

        yield return text;
        yield return wait;
    }
}
