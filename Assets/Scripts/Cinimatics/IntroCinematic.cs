using SaveSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace Cinimatics
{
    public class IntroCinematic : MonoBehaviour
    {
        private PlayableDirector playableDirector;
        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
            if (SaveManager.WorldEventsData == null) return;
            if (SaveManager.WorldEventsData.CinematicHasPlayed) return;
            playableDirector.Play();
            SaveManager.WorldEventsData.CinematicHasPlayed = true;
        }
    }
}
