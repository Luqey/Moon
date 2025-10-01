using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool somethingHappening = false;

    private bool inDialogue = false;

    #region Player Audio
    private PlayerAudio playerAudio;
    #endregion

    #region Movement Variables
    public MovementVariables moveVar;
    [System.Serializable]
    public class MovementVariables
    {
        [SerializeField] public float moveDist = 3f;
        [SerializeField] public float angledMoveDist = 4.24f;
        [SerializeField] public int rotateDist = 45;

        [SerializeField] public float moveDuration = .3f;
        [SerializeField] public float angledMoveDur = .45f;
        [SerializeField] public float rotateDuration = .2f;

        [Header("Bools")]
        public bool angled = false;
        public bool canMoveForward = true, canMoveBackward = true, canMoveLeft = true, canMoveRight = true;
    }

    private float dist;

    LayerMask layerMask;
    LayerMask interactableMask;
    #endregion

    #region Bump Variables
    public BumpClass bumpClass;
    [System.Serializable]
    public class BumpClass
    {
        [SerializeField] public bool bumping;

        [SerializeField] public float bumpDamage;

        [SerializeField] public float bumpDist = 1.5f;

        [SerializeField] public float bumpForDur = .15f;
        [SerializeField] public float bumpBackDur = .1f;
    }
    #endregion

    #region Interactable
    [Header("InteractDebug")]
    [SerializeField] private GameObject objectLookingAt;
    private bool lookingAtInteractable = false;
    #endregion

    #region Refs
    [Header("Refs")]
    [SerializeField] Bobbing hand;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] PlayerInventory inventory;
    #endregion

    void Awake()
    {
        layerMask = LayerMask.GetMask("Wall", "Interactable", "Character");
        interactableMask = LayerMask.GetMask("Interactable", "Character");
        playerAudio = this.GetComponent<PlayerAudio>();
    }

    void Start()
    {
        Check();
    }

    void Update()
    {

        #region Movement System
        if (!somethingHappening && !inDialogue)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (moveVar.canMoveForward)
                {
                    StartCoroutine(PerformMovement(dist = moveVar.angled ? moveVar.angledMoveDist : moveVar.moveDist, false));
                }
                else
                {
                    bumpClass.bumping = true;
                    StartCoroutine(PerformMovement(dist = bumpClass.bumpDist, false));
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (moveVar.canMoveBackward)
                {
                    StartCoroutine(PerformMovement(dist = moveVar.angled ? -moveVar.angledMoveDist : -moveVar.moveDist, false));
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A))
            {
                if (moveVar.canMoveLeft)
                {
                    StartCoroutine(PerformMovement(dist = moveVar.angled ? -moveVar.angledMoveDist : -moveVar.moveDist, true));
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(RotateScreen(-moveVar.rotateDist));
            }
            else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D))
            {
                if (moveVar.canMoveRight)
                {
                    StartCoroutine(PerformMovement(dist = moveVar.angled ? 4.24f : moveVar.moveDist, true));
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                StartCoroutine(RotateScreen(moveVar.rotateDist));
            }
        }
        #endregion

        #region UI and Interactables
        if (objectLookingAt != null)
        {
            if (objectLookingAt.GetComponent<Interactable>() != null)
            {
                int type = objectLookingAt.GetComponent<Interactable>().type;
            }
        }

        if (lookingAtInteractable && Input.GetKeyDown(KeyCode.E) && !inDialogue)
        {
            StartCoroutine(objectLookingAt.GetComponent<Interactable>().Activate());
        }

        inDialogue = dialogueManager.isDialogueActive;

        #endregion
    }

    #region Movement Function
    private IEnumerator PerformMovement(float dist, bool strafe)
    {
        somethingHappening = true;
        SequenceManager.Instance.StartNextActions();

        Vector3 destination = strafe ? transform.position + transform.right * dist : transform.position + transform.forward * dist ;

        destination = new Vector3(Mathf.Round(destination.x), destination.y, Mathf.Round(destination.z));

        playerAudio.FootStepSound();

        //Bob
        hand.Bob();

        if (!bumpClass.bumping)
        {
            yield return StartCoroutine(ShifterMove(destination));
        }
        else
        {
            yield return StartCoroutine(Bump(destination));
        }
        bumpClass.bumping = false;

        somethingHappening = false;

        Check();
    }

    private IEnumerator ShifterMove(Vector3 destination)
    {
        float dur;
        Vector3 start = transform.position;

        float elapsed = 0f;

        while (elapsed < (dur = moveVar.angled ? moveVar.angledMoveDur : moveVar.moveDuration))
        {
            transform.position = Vector3.Lerp(start, destination, elapsed / dur);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
    }
    #endregion

    #region Bump Function
    private IEnumerator Bump(Vector3 destination)
    {
        GameObject target;

        Vector3 start = transform.position;

        float elapsed = 0f;

        StartCoroutine(playerCombat.PerformCombat(target = objectLookingAt != null ? objectLookingAt : null));
        while (elapsed < bumpClass.bumpForDur)
        {
            transform.position = Vector3.Lerp(start, destination, elapsed / bumpClass.bumpForDur);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = destination;

        float elapsed2 = 0f;
        while (elapsed2 < bumpClass.bumpBackDur)
        {
            //Debug.Log(start);
            transform.position = Vector3.Lerp(transform.position, start, elapsed2 / bumpClass.bumpBackDur);
            elapsed2 += Time.deltaTime;
            yield return null;
        }
        transform.position = start;
    }
    #endregion

    #region Rotate Function
    private IEnumerator RotateScreen(int angle)
    {
        somethingHappening = true;

        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, angle, 0);

        yield return StartCoroutine(ShifterRotate(targetRotation));

        somethingHappening = false;

        Check();
    }

    private IEnumerator ShifterRotate(Quaternion targetRotation)
    {
        Quaternion start = transform.rotation;
        float elapsed = 0f;

        while (elapsed < moveVar.rotateDuration)
        {
            transform.rotation = Quaternion.Lerp(start, targetRotation, elapsed / moveVar.rotateDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Snap to final
        moveVar.angled = !moveVar.angled;
    }
    #endregion

    #region Directional Check Function
    public void Check()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 4, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); // Raycast logic used from Unity Documentation
            moveVar.canMoveForward = false;
        }
        else
        {
            moveVar.canMoveForward = true;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, 4, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hit.distance, Color.yellow); // Raycast logic used from Unity Documentation
            moveVar.canMoveBackward = false;
        }
        else
        {
            moveVar.canMoveBackward = true;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 4, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit.distance, Color.yellow); // Raycast logic used from Unity Documentation
            moveVar.canMoveLeft = false;
        }
        else
        {
            moveVar.canMoveLeft = true;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 4, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow); // Raycast logic used from Unity Documentation
            moveVar.canMoveRight = false;
        }
        else
        {
            moveVar.canMoveRight = true;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 4))
        {
            objectLookingAt = hit.transform.gameObject;
            if (objectLookingAt.GetComponent<Interactable>() != null)
            {
                lookingAtInteractable = true;
            }
            else
            {
                lookingAtInteractable = false;
            }
        }
        else
        {
            lookingAtInteractable = false;
            objectLookingAt = null;
        }

        #region Debug
        string forward, backward, right, left, empty;
        forward = !moveVar.canMoveForward ? "Forward, " : null;
        backward = !moveVar.canMoveBackward ? "Backward, " : null;
        right = !moveVar.canMoveRight ? "Right, " : null;
        left = !moveVar.canMoveLeft ? "Left." : null;

        if (forward == null && backward == null && right == null && left == null) { empty = "Nothing!"; } else { empty = null; }
        Debug.Log("Hit: " + forward + backward + right + left + empty);
        #endregion
    }
    #endregion
}