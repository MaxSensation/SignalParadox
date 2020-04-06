using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public static class SaveManager
{
    public static UnityEvent checkPointReached;
    private static CheckPoint _lastCheckPoint;
    private static GameObject _player;
    internal static void Init()
    {
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
        Debug.Log("CheckPoint Saved!");
        _lastCheckPoint.playerPosition = _player.transform.position;
    }

    public static CheckPoint GetLastCheckPoint()
    {
        return _lastCheckPoint;
    }
}
