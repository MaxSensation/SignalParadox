//Main author: Maximiliam Rosén

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactables.Triggers
{
    public class TriggerLoadNextLevel : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
