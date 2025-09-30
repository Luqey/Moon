using UnityEngine;

public class BloodChecker : MonoBehaviour
{
    private BloodCheckManager bloodCheckManager;
    [SerializeField] AudioSource correctNoise;

    private void Awake()
    {
        bloodCheckManager = GameObject.FindGameObjectWithTag("BloodCheckManager").GetComponent<BloodCheckManager>();
    }

    public bool iHaveBlood = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Blood") && !iHaveBlood)
        {
            correctNoise.Play();
            iHaveBlood = true;
            bloodCheckManager.BloodCheck();
            Debug.Log(gameObject + "I have Blood");
        }
    }
}
