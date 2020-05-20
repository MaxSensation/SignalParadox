//Main author: Maximiliam Rosén

using System;
using Player;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class PlayerData
    {
        public int Health { get; set; }
        private readonly int amountOfDecoys;
        
        public PlayerData(GameObject player)
        {
            var healthSystem = player.GetComponent<HealthSystem>();
            Health = healthSystem.CurrentHealth;
            amountOfDecoys = player.GetComponent<ThrowDecoyGrenade>().GetCurrentAmountOfGrenades();
        }

        public void Load(GameObject player)
        {
            player.GetComponent<HealthSystem>().SetHealth(Health);
            player.GetComponent<ThrowDecoyGrenade>().SetCurrentAmountOfGrenades(amountOfDecoys);
            if (amountOfDecoys > 0)
                GameObject.Find("DecoyGrenadeProp").GetComponent<PropDecoyGrenade>().Activate();
        }

        public void LoadPosition(GameObject player, Vector3 position, Quaternion rotation)
        {
            player.transform.position = position;
            player.transform.rotation = rotation;
        }
    }
}
