﻿//Main author: Maximiliam Rosén

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Interactables.Triggers;
using UnityEngine;

namespace SaveSystem.CheckPointSystem
{
    public static class SaveManager
    {
        public static void Init()
        {
            TriggerLoadNextLevel.onLoadedNextLevelEvent += SavePlayerData;
            TriggerCheckPoint.onTriggerCheckPoint += SaveCheckPoint;
            PlayerController.PlayerController.onPlayerDeath += LoadCheckPoint;
        }


        private static void SavePlayerData(PlayerData playerData)
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "/PlayerData.paradox";
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, playerData);
            stream.Close();
            Debug.Log("PlayerData Saved");
            playerData?.WaitForLoaded();
        }
        
        private static void LoadPlayerData()
        {
            var path = Application.persistentDataPath + "/PlayerData.paradox";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                if (stream.Length > 0)
                {
                    var playerData = formatter.Deserialize(stream) as PlayerData;
                    playerData?.WaitForLoaded();
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
            Debug.Log("CheckPoint Saved");
        }
        
        private static void LoadCheckPoint()
        {
            var path = Application.persistentDataPath + "/CheckPoint.paradox";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                if (stream.Length > 0)
                {
                    var checkPoint = formatter.Deserialize(stream) as CheckPoint;
                    checkPoint?.WaitForLoaded();
                    Debug.Log("CheckPoint Loaded");
                }
                stream.Close();
            }
            else
            {
                Debug.Log("Save file not found! Are any checkpoint triggers placed on the level?");
            }
        }
    }
}