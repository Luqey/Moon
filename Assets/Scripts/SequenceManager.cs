using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public static SequenceManager Instance { get; private set; }
    [SerializeField] private List<Entity> entities = new List<Entity>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Register(Entity entity) => entities.Add(entity);
    public void Unregister(Entity entity) => entities.Remove(entity);

    public void StartNextActions()
    {
        foreach (Entity entity in entities)
        {
            entity.GetComponent<NpcAI>().PerfomAction();
        }
    }
}
