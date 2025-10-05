using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerCombat : MonoBehaviour
{
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
    [SerializeField] Weapons currWeapon;
    [SerializeField] Animator currWpnAnim; //CHANGE

    [Header("HealthUI")]
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private Transform healthFill;


    public IEnumerator PerformCombat(GameObject target)
    {
        ObjectHealth targetHealth = target.GetComponent<ObjectHealth>();

        if (targetHealth != null)
        {
            float dmg = DamageRoll();
            DamageType type = currWeapon != null ? currWeapon.damageType : DamageType.Unarmed;

            yield return StartCoroutine(PlayAnim());

            targetHealth.HealthUpdate(dmg, type, crit);
        }

        crit = false;

        yield return new WaitForSeconds(.3f);
    }

    private float DamageRoll()
    {
        if (currWeapon != null) //Has Weapon
        {
            float dmg = Random.Range(currWeapon.lowDmg, currWeapon.highDmg);

            //Check crit
            if (Random.value <= currWeapon.critChance)
            {
                dmg *= currWeapon.critMult;
                crit = true;
                Debug.Log("CRTI!");
            }

            return Mathf.Round(dmg);
        }
        else
        {
            return unarmedDamage;
        }
    }
    private IEnumerator PlayAnim()
    {
        int layerIndex = 0;
        currWpnAnim.Play("AxeSwing", layerIndex, 0f);
        yield return new WaitForSeconds(.4f);
    }

        public void UIFill(float damage)
    {
        healthBarUI.SetActive(true);
        Vector3 currentFill = healthFill.localScale;
        float percChange = (currentHealth - damage) / maxHealth;

        currentFill = new Vector3(percChange, currentFill.y, currentFill.z);
        healthFill.localScale = currentFill;
    }
}
