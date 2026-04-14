using UnityEngine;

#region Sound Clips Class
[System.Serializable]
public class Clips {
  [Header("Ground Sound Clips")]
  [SerializeField] public AudioClip[] grassFootstep;
  [SerializeField] public AudioClip[] woodFootstep;
  [SerializeField] public AudioClip[] waterFootstep;

  [Header("Blood Drip Sound Clip")]
  [SerializeField] public AudioClip bloodDrip;
}
#endregion

public enum GroundType {
  none,
  grass,
  wood,
  water,
  blood
}

public class PlayerAudio : MonoBehaviour {
  [Header("Audio Source")]
  [SerializeField] private AudioSource playerSFX;

  [Header("Ground Masks")]
  [SerializeField] private LayerMask bloodMask;
  [SerializeField] private LayerMask surfaceMask;

  public Clips clips;

  public GroundType CheckGround() {
    if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, 1.5f, surfaceMask)) {
      return groundHit.collider.tag switch {
        "Grass" => GroundType.grass,
        "Wood" => GroundType.wood,
        "Water" => GroundType.water,
        _ => GroundType.none
      };
    }
    return GroundType.none;
  }

  public void FootStepSound() {
    var groundType = CheckGround();
    switch (groundType) {
      case GroundType.grass:
        playerSFX.pitch = Random.Range(.8f, 1.3f);
        playerSFX.PlayOneShot(clips.grassFootstep[Random.Range(0, clips.grassFootstep.Length)]);
        break;
      case GroundType.wood:
        playerSFX.pitch = Random.Range(.8f, 1.3f);
        playerSFX.PlayOneShot(clips.woodFootstep[Random.Range(0, clips.woodFootstep.Length)]);
        break;
      case GroundType.water:
        playerSFX.pitch = Random.Range(.8f, 1.3f);
        playerSFX.PlayOneShot(clips.waterFootstep[Random.Range(0, clips.waterFootstep.Length)]);
        break;
    }
  }

  #region Blood Drip
  public void PlayDrip() {
    playerSFX.PlayOneShot(clips.bloodDrip);
  }
  #endregion
}
