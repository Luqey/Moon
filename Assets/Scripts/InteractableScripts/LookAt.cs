using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform target;

    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}