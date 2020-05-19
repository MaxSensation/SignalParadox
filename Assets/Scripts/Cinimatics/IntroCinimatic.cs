using Cinemachine;
using SaveSystem;
using UnityEngine;
using UnityEngine.Playables;

public class IntroCinimatic : MonoBehaviour
{
    [SerializeField] private GameObject playerMesh;
    [SerializeField] private GameObject cinimaticPlayer;
    [SerializeField] private GameObject camera;
    private PlayableDirector _playableDirector;
    private void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        if (!SaveManager.WorldEventsData.CinematicHasPlayed)
            _playableDirector.Play();
        else
        {
            playerMesh.SetActive(true);
            playerMesh.GetComponent<TurnWithCamera>().enabled = true;
            camera.GetComponent<CinemachineBrain>().enabled = false;
            cinimaticPlayer.SetActive(false);
        }
        SaveManager.WorldEventsData.CinematicHasPlayed = true;
    }
}
