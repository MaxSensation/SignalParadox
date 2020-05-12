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
        ThrowDecoyGrenade.OnThrowEvent += Deactivate;
    }

    public void Activate(int obj)
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
        ThrowDecoyGrenade.OnThrowEvent -= Deactivate;
    }
}
