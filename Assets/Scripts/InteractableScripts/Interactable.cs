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
    [SerializeField] bool doSomethingElse = false;

    public IEnumerator Activate()
    {
        if (type == 1)
        {
            yield return StartCoroutine(dialogueManager.ReadText(dialogueObjs[line].dialogue, dialogueObjs[line].faceSprite, dialogueObjs[line].barkClip, dialogueObjs[line].lowPitch, dialogueObjs[line].highPitch, dialogueObjs[line].typeSpeed, true));
            if (line < dialogueObjs.Length - 1)
            {
                line++;
            }
        }
        else if (type == 2)
        {
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
            if (!isOpened && pInventory.inventoryItems.Contains(itemNeeded))
            {


            }
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
