using System.Collections;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{

    [SerializeField] private Animator textAnimation;
    [SerializeField] private Animator imageAnimation;
    [SerializeField] private float endGameDelay;
    private WaitForSeconds _endGameDelay;

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
        StartCoroutine("EndGameTimer");
        textAnimation.SetBool("ReachedEnd", true);
        imageAnimation.SetBool("ReachedEnd", true);
    }
}
