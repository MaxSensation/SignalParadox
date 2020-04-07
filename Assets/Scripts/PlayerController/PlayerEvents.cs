using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    public static UnityEvent KillPlayer;
    private PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    internal static void Init()
    {
        //KillPlayer.AddListener(player.Die());
        KillPlayer.AddListener(SaveManager.Init);
    }
}
