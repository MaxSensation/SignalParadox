//Main author: Maximiliam Rosén

using System;
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

        public void WaitForLoaded()
        {
            SceneManager.LoadScene(currentScene);
            SceneManager.sceneLoaded += Load;
        }

        private void Load(Scene arg0, LoadSceneMode arg1)
        {
            playerData.Load();
            playerData.LoadPosition(
                new Vector3(position[0], position[1], position[2]), 
                new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]));
            SceneManager.sceneLoaded -= Load;
        }

        public CheckPoint(Transform checkPointTransform)
        {
            playerData = new PlayerData(true);
            var checkPointPosition = checkPointTransform.position;
            var checkPointRotation = checkPointTransform.rotation;
            position = new []{checkPointPosition.x, checkPointPosition.y, checkPointPosition.z};
            rotation = new []{checkPointRotation.x, checkPointRotation.y, checkPointRotation.z, checkPointRotation.w};
            currentScene = SceneManager.GetActiveScene().name;
        }
    }
}
