//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using UnityEngine;

namespace Interactables.DecoyGrenade
{
    public class PropDecoyGrenade : MonoBehaviour
    {
        private MeshRenderer propGrenade; 

        private void Awake()
        {
            propGrenade = GetComponent<MeshRenderer>();
            if (propGrenade.enabled)
                propGrenade.enabled = false;
            PickupDecoyGrenade.onGrenadePickupEvent += Activate;
            ThrowDecoyGrenade.onThrowEvent += DisableProp;
        }
    
        private void OnDestroy()
        {
            PickupDecoyGrenade.onGrenadePickupEvent -= Activate;
            ThrowDecoyGrenade.onThrowEvent -= DisableProp;
        }

        private void DisableProp()
        {
            propGrenade.enabled = false;
        }

        public void Activate()
        {
            propGrenade.enabled = true;
        }
    }
}
