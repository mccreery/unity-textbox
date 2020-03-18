using System.Collections;
using UnityEngine;

public class TextboxPrompt : MonoBehaviour
{
    [SerializeField]
    private Textbox textbox;
    public Textbox Textbox => this.LazyGet(ref textbox, true);

    [SerializeField]
    private string buttonName = "Submit";

    public IEnumerator Wait()
    {
        while (Textbox.Typing || !Input.GetButton(buttonName))
        {
            yield return null;
        }
    }

    public IEnumerator TextAndWait(string richText)
    {
        yield return Textbox.Text(richText);
        yield return Wait();
    }
}
