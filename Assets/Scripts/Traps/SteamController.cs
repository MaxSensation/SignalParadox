using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SteamController : MonoBehaviour
{
    [SerializeField] private bool startSteamOn;
    [SerializeField] private string secretCode;
    [SerializeField] private float delay;
    public UnityEvent turnOn;
    public UnityEvent turnOff;
    private ParticleSystem _particleSystem;
    private bool _isRunning;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        if (startSteamOn)
            ActivateSteam();
        else
            DeactivateSteam();
        turnOn.AddListener(ActivateSteam);
        turnOff.AddListener(DeactivateSteam);
    }

    private void Update()
    {
        if (secretCode.Length > 0 && !_isRunning)
        {
            StartCoroutine("SecretCodeTimer");
        }
    }

    private IEnumerator SecretCodeTimer()
    {
        _isRunning = true;
        var splitedSecretCode = secretCode.Split(',');
        foreach (var letter in splitedSecretCode)
        {
            if (letter == "1")
            {
                if (!startSteamOn)
                {
                    ActivateSteam();
                }
            }
            else
            {
                if (startSteamOn)
                {
                    DeactivateSteam();
                }
            }
            yield return new WaitForSeconds(delay);
        }
        _isRunning = false;
    }

    private void ActivateSteam()
    {
        _particleSystem.Play();
        startSteamOn = true;
    }
    
    private void DeactivateSteam()
    {
        _particleSystem.Stop();
        startSteamOn = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Hit by Steam");
                other.GetComponent<PlayerController>().Die();
            }
        }
    }
}
