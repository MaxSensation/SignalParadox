//Main author: Ferreira Dos Santos Keziah
using UnityEngine;
namespace FootStepSound
{
    public class DynamicFootStep : MonoBehaviour
    {

        private AudioSource _source;
        public AudioClip Default;
        public AudioClip Grass;
        public AudioClip Dirt;

        private double time;
        private float filterTime;

        private string colliderType;

        private void Start()
        {
            _source = GetComponent<AudioSource>();
            time = AudioSettings.dspTime;
            filterTime = 0.2f;
        }
    
        private void OnCollisionEnter(Collision col)
        {
            var act = col.gameObject.GetComponent<Collider>().gameObject.GetComponent<SurfaceColliderType>();
            if (act)
                colliderType = act.GetTerrainType();
        }

        private void PlayFootstepSound(int foot_number)
        {
            if(AudioSettings.dspTime < time + filterTime)
        
                return;
            time = AudioSettings.dspTime;
            switch (colliderType) {
                case "Default":
                    _source.PlayOneShot(Default);
                    break;
                case "Grass":
                    _source.PlayOneShot(Grass);
                    break;
                case "Dirt":
                    _source.PlayOneShot(Dirt);
                    break;
            }
        }
    }
}
