using UnityEngine;
using System.Collections;

public class NpcAI : MonoBehaviour
{
    [Header("NPC Types")]
    [SerializeField] private bool hostile = false;


    [Header("AI Brain")]
    [SerializeField] private bool decidingToMove = true;
    [SerializeField] private bool decidingToAttack = false;

    [SerializeField] LayerMask pLayerMask;

    [Header("Refs")]
    [SerializeField] private GameObject player;

    public void PerfomAction()
    {
        CheckIfFacingPlayer();
        if (decidingToMove)
        {
            StartCoroutine(ShifterMove(CalculateWhereToMove()));
        }
    }

    private IEnumerator ShifterMove(Vector3 destination)
    {
        float dur = 0.3f;
        Vector3 start = transform.position;

        float elapsed = 0f;

        while (elapsed < dur)
        {
            transform.position = Vector3.Lerp(start, destination, elapsed / dur);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
        CheckIfFacingPlayer();
    }

    private Vector3 CalculateWhereToMove()
    {
        Vector3 target;

        if (hostile)
        {
            Vector3 playerGrid = player.transform.position / 3f;
            Vector3 myGrid = transform.position / 3f;

            Vector3 diff = playerGrid - myGrid;

            Vector3 step = Vector3.zero;

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
                step.x = Mathf.Sign(diff.x);

            step.z = Mathf.Sign(diff.z);

            target = (myGrid + step) * 3f;
        }
        else
        {
            target = NextPathingPoint();
        }
        return target;
    }

    private Vector3 NextPathingPoint()
    {
        Vector3 currentGrid = transform.position / 3f;


        int dx = Random.Range(-1, 2);
        int dz = Random.Range(-1, 2);

        Vector3 nextGrid = new Vector3(currentGrid.x + dx, currentGrid.y, currentGrid.z + dz);
        return nextGrid * 3f;
    }

    private void CheckIfFacingPlayer()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 4, pLayerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); // Raycast logic used from Unity Documentation
            decidingToMove = false;
        }
        else
        {
            decidingToMove = true;
        }
    }
}
