using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndGameTrigger : MonoBehaviour
{

    [SerializeField] private Animator textAnimation;
    [SerializeField] private Animator imageAnimation;
    [SerializeField] private float endGameDelay;
    [SerializeField] private AudioSource _audioSource;
    private WaitForSeconds _endGameDelay;
    private bool triggerd;

    private void Awake()
    {
        _endGameDelay = new WaitForSeconds(endGameDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            EndGame();
    }

    private IEnumerator EndGameTimer()
    {
        yield return _endGameDelay;
        Application.Quit();
    }

    private void EndGame()
    {
        triggerd = true;
        _audioSource.Play();
        StartCoroutine("EndGameTimer");
        textAnimation.SetBool("ReachedEnd", true);
        imageAnimation.SetBool("ReachedEnd", true);
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        if (triggerd && context.performed)
        {
            Application.Quit();
        }
    }
}
