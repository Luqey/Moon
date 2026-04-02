using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Stats")]
    public float highDmg;
    public float lowDmg;

    public float critChance = 0.0f;
    public float critMult = 1.5f;

    [Header("Damage Type")]
    public DamageType damageType;
}
