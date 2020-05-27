//Main author: Maximiliam Rosén
//Secondary athor: Andreas Berzelius

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Interactables.Triggers.Events;
using Player;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveManager
    {
        public static WorldEventsData WorldEventsData { get; private set; }
        private static CheckPoint checkPoint;
        private static PlayerData betweenLevelData;
        
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
        private static void SaveBetweenLevelData(PlayerData betweenLevelData) => SaveManager.betweenLevelData = betweenLevelData;
        private static void LoadPlayerDataNextPlayerInstance(PlayerData playerData) => PlayerController.onPlayerInit += LoadPlayerData;
        private static void SaveCheckPoint(CheckPoint checkPoint) => SaveManager.checkPoint = checkPoint;
        private static void LoadCheckPointScene() => checkPoint?.LoadScene();

        public static void SpawnWithFullHealthNextPlayerInstance()
        {
            checkPoint?.ResetHealth();
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
            betweenLevelData?.Load(player);
        }

        private static void LoadCheckPoint(GameObject player)
        {
            PlayerController.onPlayerInit -= LoadCheckPoint;
            checkPoint.Load(player);
        }

        public static void SaveGame()
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "/SaveGame.paradox";
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, new SaveGame(WorldEventsData, checkPoint, betweenLevelData));
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
                        checkPoint = saveGame.CheckPoint;
                        betweenLevelData = saveGame.PlayerData;
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