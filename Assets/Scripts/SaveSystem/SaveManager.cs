//Main author: Maximiliam Rosén

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Interactables.Triggers;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveManager
    {
        public static void Init()
        {
            WorldData = new WorldData();
            TriggerLoadNextLevel.onWantToLoadNextLevelEvent += SavePlayerData;
            TriggerCheckPoint.onTriggerCheckPoint += SaveCheckPoint;
            PlayerController.PlayerController.onPlayerDeath += LoadCheckPoint;
        }

        public static bool HasSaveGame { get; private set; }
        public static WorldData WorldData { get; private set; }
        private static CheckPoint _checkPoint;
        private static PlayerData _playerData;

        private static void SavePlayerData(PlayerData playerData) => _playerData = playerData;
        public static void LoadPlayerData(GameObject player) => _playerData?.Load(player);
        private static void SaveCheckPoint(CheckPoint checkPoint) => _checkPoint = checkPoint;
        public static void LoadCheckPoint() => _checkPoint?.Load();
        public static void LoadPlayerCheckPointData(GameObject player) => _checkPoint.LoadPlayerData(player);
        
        public static void SaveGame()
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "/SaveGame.paradox";
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, new SaveGame(WorldData, _checkPoint, _playerData));
            stream.Close();
        }
        
        public static void LoadSaveGame()
        {
            var path = Application.persistentDataPath + "/SaveGame.paradox";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                if (stream.Length > 0)
                {
                    if (formatter.Deserialize(stream) is SaveGame saveGame)
                    {
                        WorldData = saveGame.WorldData;
                        _checkPoint = saveGame.CheckPoint;
                        _playerData = saveGame.PlayerData;
                        HasSaveGame = true;
                    }
                }
                stream.Close();
            }
            else
            {
                Debug.Log("Save file not found!");
            }
        }
    }
}