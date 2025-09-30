using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Bird : MonoBehaviour
{
    [SerializeField] AudioSource caw;

    [SerializeField] Transform[] nextPos;

    [SerializeField] float duration = 3;

    [SerializeField] float time;

    private int goToPos = 1;

    //Bird Behavior
    [SerializeField] private Animator animator;
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;
    [SerializeField, Range(0f, 1f)] private float chanceToPlay = 0.5f; // 50% chance
    private bool moving;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(RandomIdleBehavior());
    }

    private IEnumerator RandomIdleBehavior()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            if (Random.value < chanceToPlay)
            {
                animator.SetTrigger("Move");

                AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
                yield return new WaitForSeconds(state.length);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(FlyAway());
            caw.Play();
        }
    }

    private IEnumerator FlyAway()
    {
        animator.Play("BirdFly");
        gameObject.GetComponent<Collider>().enabled = false;

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(startPos, nextPos[goToPos].position, t);

            yield return null;
        }

        animator.Play("BirdIdle");

        transform.position = nextPos[goToPos].position;

        gameObject.GetComponent<Collider>().enabled = true;


        if (goToPos < nextPos.Length - 1)
        {
            goToPos++;
        }
        else
        {
            goToPos = 0;
        }
    }
}
