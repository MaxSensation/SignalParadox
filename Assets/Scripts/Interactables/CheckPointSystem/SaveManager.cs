//Main author: Maximiliam Rosén

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Interactables.Triggers;
using UnityEngine;

namespace Interactables.CheckPointSystem
{
    public static class SaveManager
    {
        public static Action<CheckPoint> onLoadCheckPoint;
        public static void Init()
        {
            TriggerCheckPoint.onTriggerCheckPoint += SaveCheckPoint;
            PlayerController.PlayerController.onPlayerDeath += LoadCheckPoint;
        }
        
        private static void SaveCheckPoint(CheckPoint checkPoint)
        {
            if (checkPoint != null)
            {
                var formatter = new BinaryFormatter();
                var path = Application.persistentDataPath + "/SaveGame.paradox";
                var stream = new FileStream(path, FileMode.Create);
                formatter.Serialize(stream, checkPoint);
                stream.Close();
                Debug.Log("CheckPoint Saved");
            }
        }
        
        private static void LoadCheckPoint()
        {
            var path = Application.persistentDataPath + "/SaveGame.paradox";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                if (stream.Length > 0)
                {
                    var checkPoint = formatter.Deserialize(stream) as CheckPoint;
                    onLoadCheckPoint?.Invoke(checkPoint);
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