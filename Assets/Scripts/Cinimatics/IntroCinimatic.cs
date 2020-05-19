using SaveSystem;
using UnityEngine;
using UnityEngine.Playables;

public class IntroCinimatic : MonoBehaviour
{
    private PlayableDirector _playableDirector;
    private void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        if (SaveManager.WorldEventsData == null) return;
        if (!SaveManager.WorldEventsData.CinematicHasPlayed)
            _playableDirector.Play();
        SaveManager.WorldEventsData.CinematicHasPlayed = true;
    }
}
