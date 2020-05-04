//Main author: Maximiliam Rosén

namespace Interactables.CheckPointSystem
{
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
}
