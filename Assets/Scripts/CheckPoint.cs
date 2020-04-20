using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CheckPoint
{
    public readonly string currentScene;
    public readonly float[] playerPosition;
    public readonly float[] playerRotation;
    public readonly bool hasStunBaton;
    public readonly bool hasStunGunUpgrade;

    public CheckPoint(string currentScene, float[] playerPosition, float[] playerRotation, bool hasStunBaton, bool hasStunGunUpgrade)
    {
        this.currentScene = currentScene;
        this.playerPosition = playerPosition;
        this.playerRotation = playerRotation;
        this.hasStunBaton = hasStunBaton;
        this.hasStunGunUpgrade = hasStunGunUpgrade;
    }
}
