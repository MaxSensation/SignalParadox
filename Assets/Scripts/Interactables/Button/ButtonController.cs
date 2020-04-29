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
        PlayerInteractionTrigger.onEnteredInteractionRange += EnableInteraction;
        PlayerInteractionTrigger.onExitedInteractionRange += DisableInteraction;
    }

    private void OnDestroy()
    {
        PlayerInteractionTrigger.onEnteredInteractionRange -= EnableInteraction;
        PlayerInteractionTrigger.onExitedInteractionRange -= DisableInteraction;
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
    
    
    private void EnableInteraction(GameObject o)
    {
        if (o == gameObject)
            _isInRangeOfPlayer = true;   
        
    }
    
    private void DisableInteraction(GameObject o)
    {
        if (o == gameObject)
            _isInRangeOfPlayer = false;
    }
}




