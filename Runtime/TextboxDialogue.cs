using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TextboxDialogue : MonoBehaviour
{
    [SerializeField]
    [Multiline]
    private string[] messages;

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
    private UnityEvent dialogueStart = default;
    public UnityEvent DialogueStart => dialogueStart;

    [SerializeField]
    private UnityEvent dialogueEnd = default;
    public UnityEvent DialogueEnd => dialogueEnd;

    private void Start()
    {
        if (playOnStart) StartDialogue();
    }

    public void StartDialogue()
    {
        StartCoroutine(Dialogue());
    }

    private IEnumerator Dialogue()
    {
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
        }

        dialogueEnd.Invoke();
    }
}
