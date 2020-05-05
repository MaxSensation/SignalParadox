//Main author: Ferreira Dos Santos Keziah

using UnityEngine;
using PlayerController.PlayerStateMachine;
namespace FootStepSound
{
    public class DynamicFootStep : MonoBehaviour
    {

        private AudioSource _source;
        [SerializeField] private AudioClip Footsteps;
        [SerializeField] private AudioClip Glass;
        [SerializeField] private AudioClip Metal;

        private double time;
        private float filterTime;
        private bool hasEnterd;


        private string colliderType;



        private void Start()
        {
            _source = GetComponent<AudioSource>();
            time = AudioSettings.dspTime;
            filterTime = 0.2f;


        }

        private void Awake()
        {

            CrouchState.onEnteredCrouchEvent += EnteredCrouch;
            CrouchState.onExitCrouchEvent += ExitedCrouch;


        }

        private void OnDestroy()
        {
            CrouchState.onEnteredCrouchEvent -= EnteredCrouch;
            CrouchState.onExitCrouchEvent -= ExitedCrouch;
        }

        private void EnteredCrouch()
        {
            _source.volume = 0.2f;
        }

        private void ExitedCrouch()
        {
            _source.volume = 0.5f;
        }

        private void OnTriggerEnter(Collider col)
        {

            if (col.CompareTag("FootSounds"))
            {
                hasEnterd = true;
                var act = col.gameObject.GetComponent<Collider>().gameObject.GetComponent<SurfaceColliderType>();
                if (act)
                    colliderType = act.GetTerrainType();
            }



        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("FootSounds"))
            {

                hasEnterd = false;

            }


        }


        public void PlayFootstepSound()
        {

            if (hasEnterd)
            {
                if (AudioSettings.dspTime < time + filterTime)

                    return;
                time = AudioSettings.dspTime;
                switch (colliderType)
                {
                    case "Footsteps":
                        _source.PlayOneShot(Footsteps);
                        break;
                    case "Grass":
                        _source.PlayOneShot(Glass);
                        break;
                    case "Dirt":
                        _source.PlayOneShot(Metal);
                        break;
                }

            }
        }




    }
}
