using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : ScriptableObject
{
    [SerializeField] internal string level;
    [SerializeField] internal Vector3 playerPosition;
    [SerializeField] internal State currentPlayerState;
    [SerializeField] internal bool hasStunGun;
    [SerializeField] internal bool hasStunGunUpgrade;
}
