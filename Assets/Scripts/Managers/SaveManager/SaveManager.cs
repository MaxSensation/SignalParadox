using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class SaveManager
{
    public static UnityEvent checkPointReached;
    private static CheckPoint _lastCheckPoint;
    private static GameObject _player;
    private static bool hasInitilized;
    internal static void Init()
    {
        hasInitilized = true;
        _player = GameObject.FindWithTag("Player");
        _lastCheckPoint = (CheckPoint)AssetDatabase.LoadAssetAtPath("Assets/ScriptObjects/Saves/lastCheckPoint.asset", typeof(CheckPoint));
        if (_lastCheckPoint == null)
        {
            _lastCheckPoint = ScriptableObject.CreateInstance<CheckPoint>();
            AssetDatabase.CreateAsset(_lastCheckPoint, "Assets/ScriptObjects/Saves/lastCheckPoint.asset");            
        }
        if (checkPointReached == null)
            checkPointReached = new UnityEvent();
        checkPointReached.AddListener(SaveCheckPoint);
    }

    private static void SaveCheckPoint()
    {
        Init();
        Debug.Log("CheckPoint Saved!");
        _lastCheckPoint.playerPosition = _player.transform.position;
        _lastCheckPoint.level = SceneManager.GetActiveScene().name;
    }

    public static void LoadLastCheckPoint()
    {
        SceneManager.LoadScene(_lastCheckPoint.level);
        var spawnPosition = _lastCheckPoint.playerPosition;
        spawnPosition.y += 0.2f;
        _player.transform.position = spawnPosition;
        _player.GetComponent<PlayerController>().SetVelocity(Vector3.zero);
    }

    public static bool HasCheckpoint()
    {
        return hasInitilized;
    }
}
