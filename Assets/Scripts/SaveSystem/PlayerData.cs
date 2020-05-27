//Main author: Maximiliam Rosén

using System;
using Interactables.DecoyGrenade;
using Player;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class PlayerData
    {
        public int Health { get; set; }
        private readonly bool hasGrenade;

        public PlayerData(GameObject player)
        {
            var healthSystem = player.GetComponent<HealthSystem>();
            Health = healthSystem.CurrentHealth;
            hasGrenade = player.GetComponent<ThrowDecoyGrenade>().HasGrenade;
        }

        public void Load(GameObject player)
        {
            player.GetComponent<HealthSystem>().SetHealth(Health);
            var throwDecoyGrenade = player.GetComponent<ThrowDecoyGrenade>();
            if (hasGrenade)
            {
                throwDecoyGrenade.HasGrenade = hasGrenade;
                throwDecoyGrenade.EnableGrenade();
                GameObject.Find("propGrenade").GetComponent<PropDecoyGrenade>().Activate();
            }

        }

        public void LoadPosition(GameObject player, Vector3 position, Quaternion rotation)
        {
            player.transform.position = position;
            player.transform.rotation = rotation;
        }
    }
}
