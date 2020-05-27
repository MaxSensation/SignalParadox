//Main author: Maximiliam Rosén

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Interactables.Triggers;
using Interactables.Triggers.Events;
using Player;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveManager
    {
        public static WorldEventsData WorldEventsData { get; private set; }
        private static CheckPoint _checkPoint;
        private static PlayerData _betweenLevelData;
        
        public static void Init()
        {
            WorldEventsData = new WorldEventsData();
            TriggerLoadNextLevel.onTriggerdNextLevelEvent += SaveBetweenLevelData;
            TriggerLoadNextLevel.onTriggerdNextLevelEvent += LoadPlayerDataNextPlayerInstance;
            TriggerCheckPoint.onTriggerCheckPoint += SaveCheckPoint;
            PlayerController.onPlayerDeath += LoadCheckPointScene;
            PlayerController.onPlayerDeath += SpawnWithFullHealthNextPlayerInstance;
            PlayerController.onPlayerDeath += LoadCheckPointNextPlayerInstance;
        }

        private static void LoadCheckPointNextPlayerInstance() => PlayerController.onPlayerInit += LoadCheckPoint;

        private static void SaveBetweenLevelData(PlayerData betweenLevelData) => _betweenLevelData = betweenLevelData;

        private static void LoadPlayerDataNextPlayerInstance(PlayerData playerData) => PlayerController.onPlayerInit += LoadPlayerData;
        private static void SaveCheckPoint(CheckPoint checkPoint) => _checkPoint = checkPoint;
        private static void LoadCheckPointScene() => _checkPoint?.LoadScene();

        public static void SpawnWithFullHealthNextPlayerInstance()
        {
            _checkPoint?.ResetHealth();
            PlayerController.onPlayerInit += SpawnWithFullHealth;
        }

        private static void SpawnWithFullHealth(GameObject player)
        {
            player.GetComponent<HealthSystem>().ResetHealth();
            PlayerController.onPlayerInit -= SpawnWithFullHealth;
        }

        private static void LoadPlayerData(GameObject player)
        {
            PlayerController.onPlayerInit -= LoadPlayerData;
            _betweenLevelData?.Load(player);
        }

        private static void LoadCheckPoint(GameObject player)
        {
            PlayerController.onPlayerInit -= LoadCheckPoint;
            _checkPoint.Load(player);
        }

        public static void SaveGame()
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "/SaveGame.paradox";
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, new SaveGame(WorldEventsData, _checkPoint, _betweenLevelData));
            stream.Close();
        }
        
        public static void LoadSavedGame()
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
                        WorldEventsData = saveGame.WorldEventsData;
                        _checkPoint = saveGame.CheckPoint;
                        _betweenLevelData = saveGame.PlayerData;
                        LoadCheckPointNextPlayerInstance();
                        LoadCheckPointScene();
                    }
                }
                stream.Close();
            }
            else
                Debug.Log("Save file not found!");
        }

        public static bool HasSavedGame()
        {
            var path = Application.persistentDataPath + "/SaveGame.paradox";
            return File.Exists(path);
        }

        public static void Reset()
        {
            SpawnWithFullHealthNextPlayerInstance();
            WorldEventsData = new WorldEventsData();
        }
    }
}