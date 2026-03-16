using UnityEngine;
using System.Collections;

namespace Moon {

  public class NpcAI : MonoBehaviour {
    [Header("NPC Types")]
    [SerializeField] private bool hostile = false;

    [SerializeField] private float attDmg = 5;


    [Header("AI Brain")]
    [SerializeField] public int NPCTurnTimer;
    private int timer;

    [SerializeField] private bool decidingToMove = true;
    [SerializeField] private bool decidingToAttack = false;
    [SerializeField] private bool facingPlayer = false;
    [SerializeField] private bool inCombat = false;

    public NPCCombatClass nPCCombatClass;
    [System.Serializable]
    public class NPCCombatClass {
      [SerializeField] public float attDist = 1.5f;

      [SerializeField] public float attForDur = .15f;
      [SerializeField] public float attBackDur = .1f;
    }

    [SerializeField] LayerMask pLayerMask;

    [Header("Refs")]
    [SerializeField] private GameObject player;

    private void Start() {
      timer = NPCTurnTimer;
    }

    public void CountdownTurnTimer(Vector3 playerMovePos) {
      timer -= 1;
      Debug.Log(timer);
      if (timer <= 0) {
        PerformAction(playerMovePos);
        timer = NPCTurnTimer;
      }
    }

    public void PerformAction(Vector3 playerMovePos) {
      CheckIfFacingPlayer();
      if (decidingToMove) {
        StartCoroutine(ShifterMove(CalculateWhereToMove(), playerMovePos));
      } else if (decidingToAttack) {
        StartCoroutine(Attack());
      }
    }

    private IEnumerator ShifterMove(Vector3 destination, Vector3 playerMovePos) {
      if (destination.x == playerMovePos.x && destination.z == playerMovePos.z) {
        Debug.Log("Same Position! Attacking Instead");
        StartCoroutine(Attack());
      } else {
        float dur = 0.3f;
        Vector3 start = transform.position;

        float elapsed = 0f;

        while (elapsed < dur) {
          transform.position = Vector3.Lerp(start, destination, elapsed / dur);
          elapsed += Time.deltaTime;
          yield return null;
        }

        transform.position = destination;
        CheckIfFacingPlayer();
      }
    }

    private Vector3 CalculateWhereToMove() {
      Vector2Int step = new(0, 0);

      var myGrid = Services.Grid.ToGrid(transform.position);

      if (hostile) {
        // Take unpredictable steps towards the player
        var diff = Services.Grid.ToGrid(player.transform.position) - myGrid;
        if (diff.x > diff.y)
          step.x = Math.Sign(diff.x);
        step.y = Math.Sign(diff.y);
      } else {
        step.x = Random.Range(-1, 2);
        step.y = Random.Range(-1, 2);
      }

      return Services.Grid.FromGrid(myGrid + step);
    }

    private void CheckIfFacingPlayer() {
      var playerGrid = Services.Grid.ToGrid(player.transform.position);
      var diff = Services.Grid.ToGrid(transform.position) - playerGrid;
      if (diff.sqrMagnitude <= 2f) {
        decidingToMove = false;
        decidingToAttack = true;
        facingPlayer = true;
      } else {
        decidingToMove = true;
        facingPlayer = false;
      }
    }

    private IEnumerator Attack() {
      yield return new WaitForSeconds(.8f);
      CheckIfFacingPlayer();
      if (facingPlayer) {
        player.GetComponent<PlayerCombat>().currentHealth -= attDmg;
        player.GetComponent<PlayerCombat>().UIFill(attDmg);
        Debug.Log($"Player hit by {gameObject} for {attDmg}, Player Health: {player.GetComponent<PlayerCombat>().currentHealth}/{player.GetComponent<PlayerCombat>().maxHealth}");

      }
    }
  }
}
