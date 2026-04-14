using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerCombat : MonoBehaviour {
  private static WaitForSeconds _waitForSeconds_4 = new(.4f);

  private static WaitForSeconds _waitForSeconds_3 = new(.3f);

  [Header("Stats")]

  [SerializeField] public float currentHealth = 100f;
  [SerializeField] public float maxHealth = 100f;

  [SerializeField] public float flatArmor = 0.0f;
  [SerializeField] public float percArmor = 0.0f;

  [SerializeField] public float unarmedDamage = 5.0f;
  [SerializeField] public float equipDamage;

  [SerializeField] public float hitChange = 1.0f;

  [SerializeField] public float dodgeChance = 0.0f;

  private bool crit = false;

  [Header("Equipment")]
  [SerializeField] Weapon currWeapon;
  [SerializeField] Animator currWpnAnim; //CHANGE

  [Header("HealthUI")]
  [SerializeField] private GameObject healthBarUI;
  [SerializeField] private Transform healthFill;


  public IEnumerator PerformCombat(GameObject target) {

    if (target.TryGetComponent<ObjectHealth>(out var targetHealth)) {
      float dmg = DamageRoll();
      DamageType type = currWeapon != null ? currWeapon.damageType : DamageType.Unarmed;

      yield return StartCoroutine(PlayAnim());

      targetHealth.HealthUpdate(dmg, type, crit);
    }

    crit = false;

    yield return _waitForSeconds_3;
  }

  private float DamageRoll() {
    if (currWeapon != null) //Has Weapon
    {
      float dmg = Random.Range(currWeapon.lowDmg, currWeapon.highDmg);

      //Check crit
      if (Random.value <= currWeapon.critChance) {
        dmg *= currWeapon.critMult;
        crit = true;
        Debug.Log("CRTI!");
      }

      return Mathf.Round(dmg);
    } else {
      return unarmedDamage;
    }
  }
  private IEnumerator PlayAnim() {
    int layerIndex = 0;
    currWpnAnim.Play("AxeSwing", layerIndex, 0f);
    yield return _waitForSeconds_4;
  }

  public void UIFill(float damage) {
    healthBarUI.SetActive(true);
    Vector3 currentFill = healthFill.localScale;
    float percChange = (currentHealth - damage) / maxHealth;

    currentFill = new Vector3(percChange, currentFill.y, currentFill.z);
    healthFill.localScale = currentFill;
  }
}
