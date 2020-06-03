//Main author: Maximiliam Rosén

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactables.Triggers.Events
{
    public class EndGameTrigger : MonoBehaviour
    {
        [SerializeField] private Animator textAnimation;
        [SerializeField] private Animator imageAnimation;
        [SerializeField] private float endGameDelay;
        [SerializeField] private AudioSource endingAudioSource;
        private WaitForSeconds endGameDelaySeconds;
        private bool isTriggerd;

        private void Awake() => endGameDelaySeconds = new WaitForSeconds(endGameDelay);
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isTriggerd) EndGame();
        }

        private IEnumerator EndGameTimer()
        {
            yield return endGameDelaySeconds;
            Application.Quit();
        }

        private void EndGame()
        {
            isTriggerd = true;
            endingAudioSource.Play();
            StartCoroutine("EndGameTimer");
            textAnimation.SetBool("ReachedEnd", true);
            imageAnimation.SetBool("ReachedEnd", true);
        }

        private void HandleInput(InputAction.CallbackContext context)
        {
            if (isTriggerd && context.performed)
                Application.Quit();
        }
    }
}