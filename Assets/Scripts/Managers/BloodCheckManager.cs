    using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class BloodCheckManager : MonoBehaviour
{
    [SerializeField] bool allFilled = false;

    private bool activated = false;

    [SerializeField] private GameObject[] bloodCheckObj;

    [SerializeField] private PlayerController playerController;

    void Awake()
    {
        bloodCheckObj = GameObject.FindGameObjectsWithTag("Checker");
    }

    public void BloodCheck()
    {
        Debug.Log("checking blood");
        foreach (GameObject checker in bloodCheckObj)
        {
            if (!checker.GetComponent<BloodChecker>().iHaveBlood)
            {
                allFilled = false;
                return;
            }
        }

        allFilled = true;
        Debug.Log("Everything is FULL!!!!!");

    }

    private void Update()
    {
        if (allFilled && !activated)
        {
            activated = true;
        }
    }
}
