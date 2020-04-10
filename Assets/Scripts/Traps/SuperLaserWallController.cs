using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SuperLaserWallController : MonoBehaviour
{
    [SerializeField] public UnityEvent turnOn;
    [SerializeField] public UnityEvent turnOff;
    [SerializeField] private LaserController[] lasers;
    [SerializeField] private bool onStartLaserWallOn;
    [SerializeField] private float delay;

    private void Awake()
    {
        turnOn.AddListener(ActivateLasers);
        turnOff.AddListener(DeactivateLasers);
    }

    private void Start()
    {
        StartCoroutine("WaitForStart");
    }

    private IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(1f);
        if (onStartLaserWallOn)
            ActivateLasers();
        else
            DeactivateLasers();
    } 
    
    public void ActivateLasers()
    {
        StartCoroutine("ActivateWithDelay");
    }
    
    public void DeactivateLasers()
    {
        StartCoroutine("DeactivateWithDelay");
    }

    private IEnumerator ActivateWithDelay()
    {
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].turnOn.Invoke();
            yield return new WaitForSeconds(delay);
        }
        onStartLaserWallOn = true;
    }
    
    private IEnumerator DeactivateWithDelay()
    {
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].turnOff.Invoke();
            yield return new WaitForSeconds(delay);
        }
        onStartLaserWallOn = false;
    }
}
