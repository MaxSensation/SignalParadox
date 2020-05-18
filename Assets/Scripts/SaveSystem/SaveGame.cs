//Main author: Maximiliam Rosén

using System;

namespace SaveSystem
{
    [Serializable]
    internal class SaveGame
    {
        public WorldEventsData WorldEventsData { get; }
        public CheckPoint CheckPoint { get; }
        public PlayerData PlayerData { get; }

        public SaveGame(WorldEventsData worldEventsData, CheckPoint checkPoint, PlayerData playerData)
        {
            WorldEventsData = worldEventsData;
            CheckPoint = checkPoint;
            PlayerData = playerData;
        }
    }
}