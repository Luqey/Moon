using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

#region Bark
    [Header("Bark")]
    [SerializeField] AudioSource bark;
    #endregion

    #region Text Components
    [Header("Text Componenets")]
    public GameObject dialogueCanvas;
    public Image dialoguePortrait;
    public TextMeshProUGUI textObject;

    [SerializeField] float delay;
    [SerializeField] float typingSpeed = 0.05f;
    #endregion

    private string currentLineHolder;
    private int currentLineIndex;
    public bool isDialogueActive;
    private bool typing;

    public Coroutine currentTextTypingCoroutine;

    private void Awake()
    {
        isDialogueActive = false;
        dialogueCanvas.SetActive(false);
        textObject.text = "";
        dialoguePortrait.sprite = null;
    }

    public IEnumerator ReadText(string[] dialogue, Sprite portrait, AudioClip barkClip, float lowPitch, float highPitch, float typeSpeed, bool startedByInteraction)
    {
        if (isDialogueActive)
        {
            yield break;
        }

        if (portrait != null)
        {
            dialoguePortrait.sprite = portrait;
            dialoguePortrait.enabled = true;
        }
        else
        {
            dialoguePortrait.enabled = false;
        }

        isDialogueActive = true;
        dialogueCanvas.SetActive(true);

        if (startedByInteraction)
        {
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E));
        }
        
        for (currentLineIndex = 0; currentLineIndex < dialogue.Length; currentLineIndex++)
        {
            if (!isDialogueActive)
            {
                yield break;
            }

            currentLineHolder = dialogue[currentLineIndex];
            currentTextTypingCoroutine = StartCoroutine(TypeText(dialogue[currentLineIndex], barkClip, lowPitch, highPitch, typeSpeed));

            //yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E));

            bool lineFinished = false;
            while (!lineFinished)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (typing)
                    {
                        StopCoroutine(currentTextTypingCoroutine);
                        textObject.text = currentLineHolder;
                        typing = false;
                    }
                    else
                    {
                        lineFinished = true;
                        yield return null;
                    }
                }
                yield return null;
            }
        }
        isDialogueActive = false;
        textObject.text = "";
        dialoguePortrait.sprite = null;
        dialogueCanvas.SetActive(false);
    }

    private IEnumerator TypeText(string line, AudioClip barkClip, float lowPitch, float highPitch, float typeSpeed) // types text one character at a time
    {
        typing = true;
        textObject.text = "";

        bark.clip = barkClip;
        foreach (char letter in line.ToCharArray())
        {
            textObject.text += letter;
            bark.pitch = Random.Range(lowPitch, highPitch);

            bark.PlayOneShot(bark.clip);
            yield return new WaitForSeconds(typeSpeed);
        }
        typing = false;
    }
}
