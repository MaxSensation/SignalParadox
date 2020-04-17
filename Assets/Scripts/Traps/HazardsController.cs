using EventSystem;
using UnityEngine;

public class HazardsController : MonoBehaviour
{
    private Collider _collider;
    [SerializeField] private PlayerController.PlayerController player;

    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        CheckForPlayer();
        //Behövs animation här för att bestämma vad för slags Hazard det är
    }

    private void CheckForPlayer()
    {
        Physics.BoxCast(_collider.bounds.center, _collider.bounds.extents, Vector3.up, out RaycastHit _hit, _collider.transform.rotation, 5f); //Kan ändra och göra smalare casts om vi vill
        if (_hit.collider)
        {
            if (_hit.collider.CompareTag("Player"))
            {
                //PlayerEvents.Init();
                Debug.Log("Player killed by Hazard");
                EventHandler.InvokeEvent(new OnPlayerDieEvent());
            }
            //else if (_hit.collider.CompareTag("PushableBox"))
            //{
            //    Debug.Log("lazer Deactivated");
            //    _active = false;
            //    _lasers.SetActive(false);
            //}
        }
        //else if (!_active && (!_hit.collider || _hit.collider.CompareTag("Untagged")) /*&&harknapp*/)
        //{
        //    Debug.Log("lazer back on");
        //    _active = true;
        //    _lasers.SetActive(true);
        //}
    }

}
