using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using EventSystem;
using UnityEngine;

namespace Managers
{
    public static class SaveManager
    {
        public static void Init()
        {
            EventHandler.RegisterListener<OnCheckPointSaveEvent>(SaveCheckPoint);
            EventHandler.RegisterListener<OnCheckPointLoadEvent>(LoadCheckPoint);
        }
        
        private static void SaveCheckPoint(OnCheckPointSaveEvent data)
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "/SaveGame.paradox";
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data.checkPoint);
            stream.Close();
        }
        
        private static void LoadCheckPoint(OnCheckPointLoadEvent data)
        {
            var path = Application.persistentDataPath + "/SaveGame.paradox";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                EventHandler.InvokeEvent(new OnCheckPointLoadedEvent(formatter.Deserialize(stream) as CheckPoint));
                stream.Close();
            }
            else
            {
                Debug.Log("Save file not found! Are any checkpoint triggers placed on the level?");
            }
        }
    }
}