using EventSystem;
using UnityEngine;

public class LaserWallController : MonoBehaviour
{
    private Collider _collider;
    [SerializeField] private PlayerController.PlayerController player; //Det här för göras om senare.
    private GameObject _lasers;
    private bool _active;

    void Awake()
    {
        _lasers = transform.Find("TempShowCaseLaser").gameObject;
        _active = true;
        _collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayerAndBox();
    }

    private void CheckForPlayerAndBox()
    {
        Physics.BoxCast(_collider.bounds.center, _collider.bounds.extents, Vector3.up, out RaycastHit _hit, _collider.transform.rotation, 5f); //Kan ändra och göra smalare casts om vi vill
        if (_active && _hit.collider)
        {
            if (_hit.collider.CompareTag("Player"))
            {
                //PlayerEvents.Init();
                Debug.Log("Player killed by lazer");
                EventHandler.InvokeEvent(new OnPlayerDieEvent());
            }
            else if (_hit.collider.CompareTag("PushableBox"))
            {
                Debug.Log("lazer Deactivated");
                _active = false;
                _lasers.SetActive(false);
            }
        }
        else if (!_active && (!_hit.collider || _hit.collider.CompareTag("Untagged")) /*&&harknapp*/)
        {
            Debug.Log("lazer back on");
            _active = true;
            _lasers.SetActive(true);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    //Check if there has been a hit yet
    //    if (_hit.collider)
    //    {
    //        //Draw a Ray forward from GameObject toward the hit
    //        Gizmos.DrawRay(transform.position, Vector3.up * 5f);
    //        //Draw a cube that extends to where the hit exists
    //        Gizmos.DrawWireCube(transform.position + Vector3.up * 5f, _hit.collider.bounds.size);
    //    }
    //    //If there hasn't been a hit yet, draw the ray at the maximum distance
    //    else
    //    {
    //        //Draw a Ray forward from GameObject toward the maximum distance
    //        Gizmos.DrawRay(transform.position, Vector3.up * 5f);
    //        //Draw a cube at the maximum distance
    //        Gizmos.DrawWireCube(transform.position + Vector3.up * 5f, _hit.collider.bounds.size);
    //    }
    //}

}
