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
            _worldData = new WorldData();
            TriggerLoadNextLevel.onWantToLoadNextLevelEvent += SavePlayerData;
            TriggerCheckPoint.onTriggerCheckPoint += SaveCheckPoint;
            PlayerController.PlayerController.onPlayerDeath += LoadCheckPoint;
        }

        private static CheckPoint _checkPoint;
        public static WorldData _worldData;
        
        private static void SavePlayerData(PlayerData playerData)
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "/PlayerData.paradox";
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, playerData);
            stream.Close();
            Debug.Log("PlayerData Saved");
        }
        
        public static void LoadPlayerData(GameObject player)
        {
            var path = Application.persistentDataPath + "/PlayerData.paradox";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                if (stream.Length > 0)
                {
                    var playerData = formatter.Deserialize(stream) as PlayerData;
                    playerData?.Load(player);
                    Debug.Log("PlayerData Loaded");
                }
                stream.Close();
            }
            else
            {
                Debug.Log("Save file not found! Are any checkpoint triggers placed on the level?");
            }
        }

        private static void SaveCheckPoint(CheckPoint checkPoint)
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "/CheckPoint.paradox";
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, checkPoint);
            stream.Close();
        }
        
        public static void LoadCheckPoint()
        {
            var path = Application.persistentDataPath + "/CheckPoint.paradox";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                if (stream.Length > 0)
                {
                    _checkPoint = formatter.Deserialize(stream) as CheckPoint;
                    _checkPoint?.Load();
                }
                stream.Close();
            }
            else
            {
                Debug.Log("Save file not found! Are any checkpoint triggers placed on the level?");
            }
        }

        public static void LoadPlayerCheckPointData(GameObject player)
        {
            _checkPoint.LoadPlayerData(player);
        }
    }
}