using UnityEngine;
using UnityEngine.SceneManagement;

public class StabSequence : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
