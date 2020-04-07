using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject _player;
    private void Start()
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("Level_1");
        SaveManager.Init();
    }
}
