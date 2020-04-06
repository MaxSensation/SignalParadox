using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject _player;
    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        SaveManager.Init();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadCheckPoint();
        }
    }

    private void LoadCheckPoint()
    {
        CheckPoint lastCheckPoint = SaveManager.GetLastCheckPoint();
        var spawnPosition = lastCheckPoint.playerPosition;
        spawnPosition.y += 0.2f;
        _player.transform.position = spawnPosition;
        _player.GetComponent<PlayerController>().SetVelocity(Vector3.zero);
    }
}
