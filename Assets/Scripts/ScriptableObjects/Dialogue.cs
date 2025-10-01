using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Sprite faceSprite;

    [TextArea(minLines: 2, maxLines: 4)]
    public string[] dialogue;
    public AudioClip barkClip;

    public float highPitch, lowPitch;

    public float typeSpeed = .05f;
}
