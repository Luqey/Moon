using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    [SerializeField] private GameObject highligher;
    private Transform[] highlightSprites;
    private string currentDirection;

    void Start()
    {
        highlightSprites = new Transform[highligher.transform.childCount];
        for(int i = 0; i < highlightSprites.Length; i++)
        {
            highlightSprites[i] = highligher.transform.GetChild(i);
        }

        HighlightShift();
        ChangeHighlightedSquare();
    }

    public void HighlightShift()
    {
        Vector3 fixedPos = new Vector3(transform.position.x, 0.01f, transform.position.z);
        highligher.transform.position = fixedPos;
    }

    public void ChangeHighlightedSquare()
    {
        float yRot = transform.eulerAngles.y;
        string newDirection = GetDirection(yRot);
        if (newDirection != currentDirection)
        {
            currentDirection = newDirection;
            foreach (var child in highlightSprites)
            {
                Animator anim = child.GetComponent<Animator>();
                if (child.name == newDirection)
                {
                    anim.Play("BorderAnimated");
                }
                else
                {
                    anim.Play("BorderIdle");
                }
            }
        }
    }
    private string GetDirection(float yRot)
    {
        float snapped = Mathf.Round(yRot / 45f) * 45f % 360;

        switch (snapped)
        {
            case 0: return "N";
            case 45: return "NE";
            case 90: return "E";
            case 135: return "SE";
            case 180: return "S";
            case 225: return "SW";
            case 270: return "W";
            case 315: return "NW";
            default: return "N";
        }
    }
}
