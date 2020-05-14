//Main author: Maximiliam Rosén

using System;
using PlayerController;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class PlayerData
    {
        private readonly int health;
        private readonly int amountOfDecoys;
        
        public PlayerData(GameObject player, bool savedAtCheckpoint)
        {
            var healthSystem = player.GetComponent<HealthSystem>();
            health = savedAtCheckpoint ? healthSystem.GetMaxHP() : healthSystem.CurrentHealth;
            amountOfDecoys = player.GetComponent<ThrowDecoyGrenade>().GetCurrentAmountOfGrenades();
        }

        public void Load(GameObject player)
        {
            player.GetComponent<HealthSystem>().SetHealth(health);
            player.GetComponent<ThrowDecoyGrenade>().SetCurrentAmountOfGrenades(amountOfDecoys);
            if (amountOfDecoys > 0)
                GameObject.Find("DecoyGrenadeProp").GetComponent<PropDecoyGrenade>().Activate(1);
        }

        public void LoadPosition(GameObject player, Vector3 position, Quaternion rotation)
        {
            player.transform.position = position;
            player.transform.rotation = rotation;
        }
    }
}
