//Main author: Maximiliam Rosén

using PlayerController;
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
                HealthSaver.SaveInt(other.GetComponent<HealthSystem>().CurrentHealth);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
