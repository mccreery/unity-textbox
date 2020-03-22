using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class ScriptedDialogue : MonoBehaviour
{
    public void StartCoroutine(UnityAction endCallback = null)
    {
        StartCoroutine(Coroutine(endCallback));
    }

    public IEnumerator Coroutine(UnityAction endCallback = null)
    {
        yield return Coroutine();

        if (endCallback != null)
        {
            endCallback.Invoke();
        }
    }

    protected abstract IEnumerator Coroutine();
}
