//Main author: Andreas Berzelius

using DecoyGrenade;
using UnityEngine;

public class PropDecoyGrenade : MonoBehaviour
{
    private MeshRenderer propGrenade; 

    private void Awake()
    {
        propGrenade = GetComponent<MeshRenderer>();
        if (propGrenade.enabled)
            propGrenade.enabled = false;
        PickupDecoyGrenade.onGrenadePickup += Activate;
        ThrowDecoyGrenade.onThrowEvent += Deactivate;
    }

    public void Activate()
    {
        propGrenade.enabled = true;
    }

    private void Deactivate()
    {
        propGrenade.enabled = false;
    }

    private void OnDestroy()
    {
        PickupDecoyGrenade.onGrenadePickup -= Activate;
        ThrowDecoyGrenade.onThrowEvent -= Deactivate;
    }
}
