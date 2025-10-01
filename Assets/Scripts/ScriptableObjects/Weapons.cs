using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/Weapons")]
public class Weapons : ScriptableObject
{
    [Header("Stats")]
    [SerializeField] public float highDmg;
    [SerializeField] public float lowDmg;

    [SerializeField] public float critChance = 0.0f;
    [SerializeField] public float critMult = 1.5f;

    [Header("Damage Type")]
    public DamageType damageType;
}
