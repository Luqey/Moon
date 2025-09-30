using TMPro;
using UnityEngine;

public class Cords : MonoBehaviour
{
    [SerializeField] GameObject player;

    private TextMeshProUGUI cords;

    private void Start()
    {
        cords = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        cords.text = (player.transform.position.x/3).ToString("F0") + ", " + (player.transform.position.z/3).ToString("F0");
    }
}
