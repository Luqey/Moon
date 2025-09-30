using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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