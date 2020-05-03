using System;
using System.Collections;
using PlayerController;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] internal GameObject[] interactableObjects;
    private StateMachine _stateMachine;
    private bool _isInRangeOfPlayer;
    private bool _isInteractable;
    private WaitForSeconds buttonDelay;
    
    public static Action<GameObject[]> onButtonPressed;

    private void Awake()
    {
        _isInteractable = true;
        buttonDelay = new WaitForSeconds(2);
        PlayerInteractionTrigger.onInteracted += OnButtonPressed;
    }

    private void OnButtonPressed(GameObject button)
    {
        if (button == gameObject)
        {
            ButtonPress();
        }
    }

    private void OnDestroy()
    {
        PlayerInteractionTrigger.onInteracted -= OnButtonPressed;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isInRangeOfPlayer && _isInteractable)
            ButtonPress();
    }
    
    private IEnumerator ActivateButton()
    {
        yield return buttonDelay;
        _isInteractable = true;
    }

    private void ButtonPress()
    {
        _isInteractable = false;
        onButtonPressed?.Invoke(interactableObjects);
        StartCoroutine("ActivateButton");
    }
}




