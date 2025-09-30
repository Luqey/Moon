using UnityEngine;

public class CompassHand : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;

    [SerializeField] RectTransform rect;
    private Quaternion rotation;


    void Update()
    {
        float playerY = playerTrans.eulerAngles.y;

        rect.rotation = Quaternion.Euler(0f, 0f, playerY);
    }
}
