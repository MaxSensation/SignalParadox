//Main author: Maximiliam Rosén

using SaveSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace Cinimatics
{
    public class IntroCinematic : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        [SerializeField] private CameraMover cameraMover;
        private PlayableDirector playableDirector;
        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
            if (SaveManager.WorldEventsData != null)
            {
                if (SaveManager.WorldEventsData.CinematicHasPlayed == false)
                {
                    playableDirector.Play();
                    SaveManager.WorldEventsData.CinematicHasPlayed = true;
                }
                else
                {
                    cameraController.enabled = true;
                    cameraMover.enabled = true;
                }
            }
            else
            {
                cameraController.enabled = true;
                cameraMover.enabled = true;
            }
        }
    }
}
