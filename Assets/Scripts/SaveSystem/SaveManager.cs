﻿//Main author: Maximiliam Rosén
//Secondary athor: Andreas Berzelius

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Interactables.Triggers.Events;
using Player;
using UnityEngine;
using UnityEngine.Audio;

namespace SaveSystem
{
    public static class SaveManager
    {
        public static WorldEventsData WorldEventsData { get; private set; }
        public static GameSettingData Settings { get; private set; }
        private static CheckPoint checkPoint;
        private static PlayerData betweenLevelData;

        public static void Init()
        {
            if (HasSettings())
                LoadSettings();
            else
                Settings = new GameSettingData();
            WorldEventsData = new WorldEventsData();
            TriggerLoadNextLevel.onTriggerdNextLevelEvent += SaveBetweenLevelData;
            TriggerLoadNextLevel.onTriggerdNextLevelEvent += LoadPlayerDataNextPlayerInstance;
            TriggerCheckPoint.onTriggerCheckPoint += SaveCheckPoint;
            PlayerController.onPlayerDeathEvent += LoadCheckPointScene;
            PlayerController.onPlayerDeathEvent += SpawnWithFullHealthNextPlayerInstance;
            PlayerController.onPlayerDeathEvent += LoadCheckPointNextPlayerInstance;
        }

        private static void LoadCheckPointNextPlayerInstance() => PlayerController.onPlayerInitEvent += LoadCheckPoint;
        private static void SaveBetweenLevelData(PlayerData betweenLevelData) => SaveManager.betweenLevelData = betweenLevelData;
        private static void LoadPlayerDataNextPlayerInstance(PlayerData playerData) => PlayerController.onPlayerInitEvent += LoadPlayerData;
        private static void SaveCheckPoint(CheckPoint checkPoint) => SaveManager.checkPoint = checkPoint;
        private static void LoadCheckPointScene() => checkPoint?.LoadScene();

        public static void SpawnWithFullHealthNextPlayerInstance()
        {
            checkPoint?.ResetHealth();
            PlayerController.onPlayerInitEvent += SpawnWithFullHealth;
        }

        private static void SpawnWithFullHealth(GameObject player)
        {
            player.GetComponent<HealthSystem>().ResetHealth();
            PlayerController.onPlayerInitEvent -= SpawnWithFullHealth;
        }

        private static void LoadPlayerData(GameObject player)
        {
            PlayerController.onPlayerInitEvent -= LoadPlayerData;
            betweenLevelData?.Load(player);
        }

        private static void LoadCheckPoint(GameObject player)
        {
            PlayerController.onPlayerInitEvent -= LoadCheckPoint;
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
        
        public static void SaveSettings()
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "/Settings.paradox";
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, Settings);
            stream.Close();
        }

        private static void LoadSettings()
        {
            var path = Application.persistentDataPath + "/Settings.paradox";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                if (stream.Length > 0)
                {
                    if (formatter.Deserialize(stream) is GameSettingData settings)
                        Settings = settings;
                }
                stream.Close();
            }
            else
                Debug.Log("Save file not found!");
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

        public static bool HasSettings()
        {
            var path = Application.persistentDataPath + "/Settings.paradox";
            return File.Exists(path);
        }
        
        public static void Reset()
        {
            SpawnWithFullHealthNextPlayerInstance();
            WorldEventsData = new WorldEventsData();
        }
    }
}