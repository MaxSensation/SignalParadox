//Main author: Maximiliam Rosén

using System;
using Managers;
using PlayerController;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    [Serializable]
    public class PlayerData
    {
        private readonly int health;
        private readonly int amountOfDecoys;
        
        public PlayerData(bool savedAtCheckpoint)
        {
            var player = GameManager.GetPlayer();
            var healthSystem = player.GetComponent<HealthSystem>();
            health = savedAtCheckpoint ? healthSystem.GetMaxHP() : healthSystem.CurrentHealth;
            amountOfDecoys = player.GetComponent<ThrowDecoyGrenade>().GetCurrentAmountOfGrenades();
        }

        public void WaitForLoaded()
        {
            SceneManager.sceneLoaded += Load;
        }

        private void Load(Scene arg0, LoadSceneMode arg1)
        {
            Load();
            SceneManager.sceneLoaded -= Load;
        }

        public void Load()
        {
            var player = GameManager.GetPlayer();
            player.GetComponent<HealthSystem>().SetHealth(health);
            player.GetComponent<ThrowDecoyGrenade>().SetCurrentAmountOfGrenades(amountOfDecoys);
            if (amountOfDecoys > 0)
                GameObject.Find("DecoyGrenadeProp").GetComponent<PropDecoyGrenade>().Activate(1);
        }

        public void LoadPosition(Vector3 position, Quaternion rotation)
        {
            var player = GameObject.FindWithTag("Player");
            player.transform.position = position;
            player.transform.rotation = rotation;
        }
    }
}
