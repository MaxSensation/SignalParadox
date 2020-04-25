using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerLoadNextLevel : MonoBehaviour
{
    public string levelToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
