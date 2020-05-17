using System;

namespace SaveSystem
{
    [Serializable]
    internal class SaveGame
    {
        public WorldData WorldData { get; }
        public CheckPoint CheckPoint { get; }
        public PlayerData PlayerData { get; }

        public SaveGame(WorldData worldData, CheckPoint checkPoint, PlayerData playerData)
        {
            WorldData = worldData;
            CheckPoint = checkPoint;
            PlayerData = playerData;
        }
    }
}