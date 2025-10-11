using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] public int type; // 1 - Sign, 2 - Pickup, 3 - IDK

    #region Dialogue Stuff
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] Dialogue[] dialogueObjs;

    private int line = 0;
    #endregion

    #region Item Stuff

    [SerializeField] AudioSource openSound;

    [SerializeField] private bool isOpened = false;
    [SerializeField] private Sprite openedSprite;

    [SerializeField] private GameObject heldObject;

    [SerializeField] private string[] empty;

    [SerializeField] PlayerInventory pInventory;

    [SerializeField] private GameObject itemNeeded;

    #endregion


    #region fucked up messy shit
    public GameObject inputFieldObject;
    public TMPro.TMP_InputField inputField;

    public Animator animator; 
    #endregion

    [SerializeField] bool doSomethingElse = false;

    public IEnumerator Activate()
    {
        if (type == 1)
        {
            inputFieldObject.SetActive(false);
            yield return StartCoroutine(dialogueManager.ReadText(dialogueObjs[line].dialogue, dialogueObjs[line].faceSprite, dialogueObjs[line].barkClip, dialogueObjs[line].lowPitch, dialogueObjs[line].highPitch, dialogueObjs[line].typeSpeed, true));
            if (line < dialogueObjs.Length - 1)
            {
                line++;
            }
        }
        else if (type == 2)
        {
            inputFieldObject.SetActive(false);
            if (!isOpened)
            {
                this.GetComponent<SpriteRenderer>().sprite = openedSprite;
                isOpened = true;
                openSound.Play();
                ObtainItem();

                yield return StartCoroutine(dialogueManager.ReadText(dialogueObjs[line].dialogue, dialogueObjs[line].faceSprite, dialogueObjs[line].barkClip, dialogueObjs[line].lowPitch, dialogueObjs[line].highPitch, dialogueObjs[line].typeSpeed, true));
            }
            else
            {
                yield return StartCoroutine(dialogueManager.ReadText(empty, dialogueObjs[line].faceSprite, dialogueObjs[line].barkClip, dialogueObjs[line].lowPitch, dialogueObjs[line].highPitch, dialogueObjs[line].typeSpeed, true));
            }
        }
        else if (type == 3)
        {
            inputFieldObject.SetActive(false);
            if (!isOpened && pInventory.inventoryItems.Contains(itemNeeded))
            {


            }
        }
        else if (type == 4)
        {
            Coroutine currentC;
            currentC = StartCoroutine(dialogueManager.ReadText(dialogueObjs[line].dialogue, dialogueObjs[line].faceSprite, dialogueObjs[line].barkClip, dialogueObjs[line].lowPitch, dialogueObjs[line].highPitch, dialogueObjs[line].typeSpeed, true));

            inputFieldObject.SetActive(true);
            inputField.text = "";
            inputField.ActivateInputField();

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
            StopCoroutine(currentC);

            inputFieldObject.SetActive(false);

            animator.Play("Stab");
        }

        if (doSomethingElse)
        {

        }

    }

    private void ObtainItem()
    {
        pInventory.inventoryItems.Add(heldObject);
        heldObject = null;
    }
}
