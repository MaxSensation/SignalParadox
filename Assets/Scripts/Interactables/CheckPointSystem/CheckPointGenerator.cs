//Main author: Maximiliam Rosén

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactables.CheckPointSystem
{
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
            var playerController = _player.GetComponent<PlayerController.PlayerController>();
            return new CheckPoint(
                SceneManager.GetActiveScene().name,
                new[] {position.x, position.y, position.z},
                new[] {rotation.x, rotation.y, rotation.z});
        }
    }
}
