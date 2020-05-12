using System;
using System.Linq;
using UnityEngine;

public class GlassWallTrigger : MonoBehaviour
{
    private BoxCollider[] _colliders;
    private Animator _animator;
    private AudioSource _audioSource;
    private void Start()
    {
        _colliders = GetComponents<BoxCollider>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        _colliders.ToList().ForEach(i => i.enabled = false);
        _animator.SetTrigger("GlassBreak");
        _audioSource.Play();
    }
}
