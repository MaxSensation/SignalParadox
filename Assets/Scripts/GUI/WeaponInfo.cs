using Managers;
using Pickups;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    [SerializeField] private GameObject stunGunGui, stunGunUpgradeGui;
    [SerializeField] private bool stunGunGUION, stunGunUpgradeGUION;
    private void Awake()
    {
        PickupStunBaton.onStunBatonPickup += ActivateStunGunGUI;
        PickupStunGunUpgrade.onStunGunUpgradePickup += ActivateStunUpgradeGunGUI;
        SaveManager.onLoadCheckPoint += LoadCheckPointData;
        if (stunGunGUION)
            ActivateStunGunGUI();
        if (stunGunUpgradeGUION)
            ActivateStunUpgradeGunGUI();
    }

    private void OnDestroy()
    {
        PickupStunGunUpgrade.onStunGunUpgradePickup -= ActivateStunGunGUI;
        PickupStunGunUpgrade.onStunGunUpgradePickup -= ActivateStunUpgradeGunGUI;
        SaveManager.onLoadCheckPoint -= LoadCheckPointData;
    }
    
    private void LoadCheckPointData(CheckPoint checkPoint)
    {
        Debug.Log("Hello");
        if(checkPoint.hasStunBaton)
            ActivateStunGunGUI();
        if(checkPoint.hasStunGunUpgrade)
            ActivateStunUpgradeGunGUI();
    }      

    private void ActivateStunGunGUI() => stunGunGui.SetActive(true);

    private void ActivateStunUpgradeGunGUI() => stunGunUpgradeGui.SetActive(true);
}
