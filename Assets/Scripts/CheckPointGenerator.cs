using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointGenerator
{
    private static GameObject _player;
    public static CheckPoint Generate()
    {
        _player = GameObject.FindWithTag("Player");
        if (_player == null) return null;
        var playerTransform = _player.transform;
        var position = playerTransform.position;
        var rotation = playerTransform.rotation;
        return new CheckPoint(
            SceneManager.GetActiveScene().name, 
            new []{position.x, position.y, position.z}, 
            new []{rotation.x, rotation.y, rotation.z}, 
            false, 
            false);
    }
}
