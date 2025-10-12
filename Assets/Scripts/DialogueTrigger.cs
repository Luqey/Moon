using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    #region Dialogue Stuff
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] Dialogue[] dialogues;
    #endregion

    [SerializeField] private bool activated;

    private int line;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !activated)
        {
            StartCoroutine(BeginInteraction());
            if (line < dialogues.Length - 1)
            {
                line++;
            }
            activated = true;
        }
    }

    private IEnumerator BeginInteraction()
    {
        yield return new WaitForSeconds(.3f);
        yield return StartCoroutine(dialogueManager.ReadText(dialogues[line].dialogue, dialogues[line].faceSprite, dialogues[line].barkClip, dialogues[line].lowPitch, dialogues[line].highPitch, dialogues[line].typeSpeed, false));
    }
}
