using EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventHandler = EventSystem.EventHandler;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
            SaveManager.Init();
            //Register Events
            EventHandler.RegisterListener<OnButtonStartEvent>(LoadLevel);
            EventHandler.RegisterListener<OnTriggerEnteredCheckPointEvent>(SavePlayerData);
            EventHandler.RegisterListener<OnCheckPointLoadedEvent>(LoadPlayerData);
        }

        private void LoadLevel(OnButtonStartEvent obj)
        {
            SceneManager.LoadScene(obj.level);
        }

        private void OnDestroy()
        {
            //Unregister Events
            EventHandler.UnregisterListener<OnButtonStartEvent>(LoadLevel);
        }
        
        private void SavePlayerData(OnTriggerEnteredCheckPointEvent obj)
        {
            var player = GameObject.FindWithTag("Player");
            var playerTransform = player.transform;
            var position = playerTransform.position;
            var rotation = playerTransform.rotation;
            var checkPoint = new CheckPoint(
                SceneManager.GetActiveScene().name,
                new[] {position.x, position.y, position.z},
                new[] {rotation.x, rotation.y, rotation.z},
                false,
                false
            );
            EventHandler.InvokeEvent(new OnCheckPointSaveEvent(checkPoint));
        }
        
        private void LoadPlayerData(OnCheckPointLoadedEvent obj)
        {
            SceneManager.LoadScene(obj.checkPoint.currentScene);
            var player = GameObject.FindWithTag("Player");
            player.transform.position = new Vector3(obj.checkPoint.playerPosition[0], obj.checkPoint.playerPosition[1], obj.checkPoint.playerPosition[2]);
            //transform.rotation = new Vector3(obj.checkPoint.playerRotation[0], obj.checkPoint.playerRotation[1], obj.checkPoint.playerRotation[2]);
            // hasStunBaton = obj.checkPoint.hasStunBaton;
            // hasStunGunUpgrade = obj.checkPoint.hasStunGunUpgrade;
        }
    }
}
