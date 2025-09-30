using UnityEngine;

#region Sound Clips Class
[System.Serializable]
public class Clips
{
    [Header("Ground Sound Clips")]
    [SerializeField] public AudioClip[] grassFootstep;
    [SerializeField] public AudioClip[] woodFootstep;
    [SerializeField] public AudioClip[] waterFootstep;

    [Header("Blood Drip Sound Clip")]
    [SerializeField] public AudioClip bloodDrip;
}
#endregion

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource playerSFX;

    [Header("Ground Type Bools")]
    [SerializeField] public bool onGrass;
    [SerializeField] public bool onWood;
    [SerializeField] public bool onWater;
    [SerializeField] public bool onBlood;

    [Header("Ground Masks")]
    [SerializeField] private LayerMask bloodMask;
    [SerializeField] LayerMask surfaceMask;

    public Clips clips;

    private void Update()
    {
        #region Sound Check System

        ResetAudioBools();

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit bloodHit, 1.5f, bloodMask))
        {
            onBlood = true;
        }

        else if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, 1.5f, surfaceMask))
        {
            switch (groundHit.collider.tag)
            {
                case "Grass": onGrass = true; break;
                case "Wood": onWood = true; break;
                case "Water": onWater = true; break;
            }
        }
        #endregion
    }

    public void FootStepSound()
    {
        if (onGrass)
        {
            playerSFX.pitch = Random.Range(.8f, 1.3f);
            playerSFX.PlayOneShot(clips.grassFootstep[Random.Range(0, clips.grassFootstep.Length)]);
        }
        else if (onWood)
        {
            playerSFX.pitch = Random.Range(.8f, 1.3f);
            playerSFX.PlayOneShot(clips.woodFootstep[Random.Range(0, clips.woodFootstep.Length)]);
        }
        else if (onWater)
        {
            playerSFX.pitch = Random.Range(.8f, 1.3f);
            playerSFX.PlayOneShot(clips.waterFootstep[Random.Range(0, clips.waterFootstep.Length)]);
        }
    }

    #region Audio Bools
    private void ResetAudioBools()
    {
        onGrass = false;
        onWood = false;
        onWater = false;
        onBlood = false;
    }
    #endregion

    #region Blood Drip
    public void PlayDrip()
    {
        playerSFX.PlayOneShot(clips.bloodDrip);
    }
    #endregion
}
