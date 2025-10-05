using System;
using UnityEngine;
using System.Collections;

public class ObjectHealth : MonoBehaviour
{
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth;

    [SerializeField] public float dodgeChance;

    [Header("HealthUI")]
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private Transform healthFill;

    [Header("Takes Damage From:")]

    [SerializeField] public bool unarmed;
    [SerializeField] public bool lightWpn;
    [SerializeField] public bool heavyWpn;
    [SerializeField] public bool magic;

    public void HealthUpdate(float damage, DamageType type, bool isCrit)
    {
        if (UnityEngine.Random.value <= dodgeChance)
        {
            Debug.Log("Miss!");
            Popup(0, isCrit, "Miss!");
            return;
        }

        if (CanTakeDamage(type))
        {
            UIFill(damage);
            currentHealth -= damage;
            Debug.Log($"{gameObject.name} took {damage} {type} damage! Health: {currentHealth}/{maxHealth}");
            Popup(((int)damage), isCrit, "");
        }
        else
        {
            Debug.Log($"Cannot Damage {gameObject} with {type}");
            Popup(0, isCrit, "Can't Damage This Type");
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private bool CanTakeDamage(DamageType type)
    {
        return type switch
        {
            DamageType.Unarmed => unarmed,
            DamageType.LightWeapon => lightWpn,
            DamageType.HeavyWeapon => heavyWpn,
            DamageType.Magic => magic,
            _ => false
        };
    }

    private void Popup(int damage, bool isCrit, string altText)
    {
        Vector3 spawnPos = new Vector3(0, 1.5f, 0);
        spawnPos = transform.position + spawnPos;

        SpawnsDamagePopups.Instance.DamageDone(damage, spawnPos, isCrit, altText);
    }

    private void UIFill(float damage)
    {
        healthBarUI.SetActive(true);
        Vector3 currentFill = healthFill.localScale;
        float percChange = (currentHealth - damage) / maxHealth;

        currentFill = new Vector3(percChange, currentFill.y, currentFill.z);
        healthFill.localScale = currentFill;
    }
}

