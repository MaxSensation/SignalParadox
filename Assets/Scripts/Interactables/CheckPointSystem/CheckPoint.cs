using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CheckPoint
{
    public readonly string currentScene;
    public readonly float[] playerPosition;
    public readonly float[] playerRotation;

    public CheckPoint(string currentScene, float[] playerPosition, float[] playerRotation)
    {
        this.currentScene = currentScene;
        this.playerPosition = playerPosition;
        this.playerRotation = playerRotation;
    }
}
