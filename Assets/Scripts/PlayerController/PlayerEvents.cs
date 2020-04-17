using UnityEditor;
using UnityEngine;
using System;
using Managers;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    public static UnityEvent KillPlayer;
    private PlayerController.PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController.PlayerController>();
    }

    internal static void Init()
    {
        //KillPlayer.AddListener(player.Die());
        KillPlayer.AddListener(SaveManager.Init);
    }
}
