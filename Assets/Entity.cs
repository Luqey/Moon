using UnityEngine;

public class Entity : MonoBehaviour
{
    private void OnEnable()
    {
        SequenceManager.Instance.Register(this);
    }
    private void OnDisable()
    {
        if (SequenceManager.Instance != null)
        {
            SequenceManager.Instance.Unregister(this);
        }
    }
}
