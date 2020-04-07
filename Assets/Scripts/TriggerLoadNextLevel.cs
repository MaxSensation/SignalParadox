using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerLoadNextLevel : MonoBehaviour
{
    public string levelToLoad;
    private Collider _collider;
    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        if (Physics.BoxCast(_collider.bounds.center, _collider.bounds.extents, Vector3.up, out var hit, _collider.transform.rotation, 5f))
        {
            if (hit.collider && hit.collider.CompareTag("Player"))
            {
                SceneManager.LoadScene(levelToLoad);
            }
        }
    }
}
