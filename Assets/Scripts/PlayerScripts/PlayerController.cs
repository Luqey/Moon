using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Moon {

  public class PlayerController : MonoBehaviour {
    private bool inDialogue = false;

    #region Player Audio
    [SerializeField] private PlayerAudio playerAudio;
    #endregion

    [Header("Movement Controllers")]
    [SerializeField] private GridMovementController gridMovementController;

    [Header("Interact Keymap")]
    [SerializeField] private InputActionReference interact;
    [SerializeField] private LayerMask interactMask;

    #region Refs
    [Header("Refs")]
    [SerializeField] Bobbing hand;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] PlayerInventory inventory;
    [SerializeField] Highlighter highlighter;
    #endregion

    private ThresholdTrigger<float> walkingAudioTrigger;
    private ThresholdTrigger<float> bumpTrigger = null;
    void Start() {
      gridMovementController.walking += OnWalkingInGrid;
      gridMovementController.turning += OnTurning;
      gridMovementController.bumping += OnBumping;

      interact.action.performed += OnInteract;

      walkingAudioTrigger = new(0.5f, () => playerAudio.FootStepSound());
    }

    private void OnWalkingInGrid(float progress, Vector2Int gridTarget) {
      if (progress == 0) {
        walkingAudioTrigger.Reset();
      } else {
        walkingAudioTrigger.Update(progress);
      }

      if (progress == 1) {
        highlighter.HighlightShift();

        SequenceManager.Instance.StartNextActions(gridTarget);
      }

      hand.Bob();
    }

    private void OnTurning(float progress) {
      if (progress == 1) {
        highlighter.ChangeHighlightedSquare();
      }
    }

    private void OnBumping(float progress, RaycastHit hit) {
      if (progress == 0) {
        bumpTrigger = new(0.5f, () => {
          var collider = hit.collider;
          if (collider == null) return;
          var objectBumping = collider.gameObject;
          if (objectBumping == null) return;
          if (objectBumping.TryGetComponent<ObjectHealth>(out var objectHealth)) {
              StartCoroutine(playerCombat.PerformCombat(target: objectBumping));
          }
        });
      }
      bumpTrigger?.Update(progress);
    }

    private void OnInteract(InputAction.CallbackContext context) {
      Debug.Log("Trying to interact...");
      var direction = transform.forward;
      var halfHeightPosition = Vector3.Scale(transform.position, new Vector3(1, 0.5f, 1));
      var gridDelta = new Vector2Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.z));
      if (Physics.Raycast(halfHeightPosition, direction, out var hit, Services.Grid.gridSize * gridDelta.magnitude, interactMask)) {
        Debug.Log("Tried to interact with something");
        if (hit.collider.gameObject.TryGetComponent<Interactable>(out var interactable)) {
          Debug.Log("Found an interactable");
          StartCoroutine(interactable.Activate());
        }
      }
    }

    void Update() {
      inDialogue = dialogueManager.isDialogueActive;
      gridMovementController.enabled = !inDialogue;
    }
  }
}
