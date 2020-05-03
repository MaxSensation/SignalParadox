﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class PlatformTrigger : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Material on;
    [SerializeField] private Material off;
    [SerializeField] private GameObject[] interactables;
    private MeshRenderer _meshRenderer;
    private static readonly int IsPressed = Animator.StringToHash("IsPressed");
    public UnityEvent isOn;
    public UnityEvent isOff;
    public static Action<GameObject[]> onButtonPressed;
    private int objectsOnButton;

    private void Awake()
    {
        objectsOnButton = 0;
        var parent = transform.parent;
        _animator = parent.GetComponent<Animator>();
        _meshRenderer = parent.transform.Find("PlatformButton").GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) return;
        objectsOnButton++;
        if (objectsOnButton != 1) return;
        _animator.SetBool(IsPressed, true);
        _meshRenderer.material = @on;
        isOn.Invoke();
        onButtonPressed?.Invoke(interactables);
    }

    private void OnTriggerExit(Collider other)
    {
        if ((!other.CompareTag("Player") && !other.CompareTag("Enemy"))) return;
        objectsOnButton--;
        if (objectsOnButton != 0) return;
        _animator.SetBool(IsPressed, false);
        _meshRenderer.material = off;
        isOff.Invoke();
        onButtonPressed?.Invoke(interactables);
    }
}