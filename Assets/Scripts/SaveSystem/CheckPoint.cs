//Main author: Maximiliam Rosén

using System;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    [Serializable]
    public class CheckPoint
    {
        private readonly PlayerData playerData;
        private readonly string currentScene;
        private readonly float[] position;
        private readonly float[] rotation;

        public void LoadScene()
        {
            SceneManager.LoadScene(currentScene);
        }

        public void Load(GameObject player)
        {
            playerData.Load(player);
            playerData.LoadPosition(player,
                new Vector3(position[0], position[1], position[2]), 
                new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]));
        }

        public CheckPoint(GameObject player, Transform checkPointTransform)
        {
            playerData = new PlayerData(player);
            var checkPointPosition = checkPointTransform.position;
            var checkPointRotation = checkPointTransform.rotation;
            position = new []{checkPointPosition.x, checkPointPosition.y, checkPointPosition.z};
            rotation = new []{checkPointRotation.x, checkPointRotation.y, checkPointRotation.z, checkPointRotation.w};
            currentScene = SceneManager.GetActiveScene().name;
        }

        public void ResetHealth() => playerData.Health = HealthSystem.GetMaxHP();
    }
}
