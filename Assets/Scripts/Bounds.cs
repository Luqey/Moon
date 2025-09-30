using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Bounds : MonoBehaviour
{
    [SerializeField] Transform player;

    [Header("Shake Settings")]
    [SerializeField] float maxShakeIntensity = 0.7f;
    [SerializeField] float maxEdgeDistance = 5f; // Distance from wall where shake fades in

    private Vector3 originalPos;
    private BoxCollider box;

    [SerializeField] private Image redFilter;
    [SerializeField] private float maxAlpha = 0.7f;

    void Start()
    {
        Color c = redFilter.color;
        c.a = 0f;
        redFilter.color = c;

        originalPos = Camera.main.transform.localPosition;
        box = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (box.bounds.Contains(player.position))
        {
            // Convert player position to local space relative to box center
            Vector3 localPos = box.transform.InverseTransformPoint(player.position);

            // Half-size of box in local space
            Vector3 halfSize = box.size * 0.5f;

            // Distances to each wall
            float distX = halfSize.x - Mathf.Abs(localPos.x);
            float distY = halfSize.y - Mathf.Abs(localPos.y);
            float distZ = halfSize.z - Mathf.Abs(localPos.z);

            // Smallest distance = how close we are to the nearest wall
            float distanceToEdge = Mathf.Min(distX, distY, distZ);

            if (distanceToEdge <= maxEdgeDistance)
            {
                // Normalize (closer to wall = stronger shake)
                float t = Mathf.InverseLerp(maxEdgeDistance, 0f, distanceToEdge);
                float intensity = t * maxShakeIntensity;

                float alpha = t * maxAlpha;

                Color c = redFilter.color;
                c.a = alpha;
                redFilter.color = c;

                Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * intensity;
            }
            else
            {
                Camera.main.transform.localPosition = originalPos;
                Color c = redFilter.color;
                c.a = 0f;
                redFilter.color = c;
            }
        }
        else
        {
            Camera.main.transform.localPosition = originalPos;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player left");
        }
    }
}