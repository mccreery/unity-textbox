using System.Collections;

namespace McCreery.Textbox
{
    /// <summary>
    /// A dialogue sequence. Designed to control a <c>TextboxPrompt</c>
    /// and any other game objects for animations, sound effects etc.
    /// </summary>
    /// <seealso cref="DialogueManager.Prompt"/>
    public interface IDialogue
    {
        /// <summary>
        /// <para>Coroutine to play the dialogue. This coroutine will never be forcibly stopped,
        /// but interruptible dialogues should periodically check <c>manager.RunningId == id</c>
        /// and <c>yield break</c> if false.</para>
        /// <para>Nested calls will work, but you should never call this method directly.
        /// Instead, <c>yield return</c> <see cref="DialogueManager.StartDialogue(IDialogue, int)"/>
        /// with <c>id</c> as parent.</para>
        /// </summary>
        /// <param name="manager">The dialogue manager maintaining the current dialogue</param>
        /// <param name="id">Run ID, for interruption and nested calls to <c>manager.StartDialogue</c></param>
        /// <returns>A coroutine iterator</returns>
        IEnumerator Play(DialogueManager manager, int id);
    }
}
