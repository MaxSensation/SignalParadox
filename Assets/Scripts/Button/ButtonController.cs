using System.Collections;
using EventSystem;
using UnityEngine;
using EventHandler = EventSystem.EventHandler;

public class ButtonController : MonoBehaviour
{
    [SerializeField] internal GameObject[] interactableObject;
    [SerializeField] private State[] states;
    private StateMachine _stateMachine;
    private bool _offcooldown;
    private bool _onCooldown;
    private Renderer _buttonRenderer;
    private bool _isInteractable;

    private void Awake()
    {
        _buttonRenderer = GetComponent<Renderer>();
        _stateMachine = new StateMachine(this, states);

    }

    private void Start()
    {
        EventHandler.RegisterListener<OnPlayerEnteredInteractionEvent>(EnableInteraction);
        EventHandler.RegisterListener<OnPlayerExitedInteractionEvent>(DisableInteraction);
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterListener<OnPlayerEnteredInteractionEvent>(EnableInteraction);
        EventHandler.UnregisterListener<OnPlayerExitedInteractionEvent>(DisableInteraction);
    }

    internal Renderer GetRenderer()
    {
        return _buttonRenderer;
    }

    private void Update()
    {
        _stateMachine.Run();
        if (Input.GetKeyDown(KeyCode.E) && _isInteractable)
            ButtonPress();
    }

    internal bool IsOffCooldown()
    {
        return _offcooldown;
    }

    internal bool IsOnCooldown()
    {
        return _onCooldown;
    }

    internal IEnumerator ActivateButton()
    {
        yield return new WaitForSeconds(2);
        _offcooldown = false;
        _onCooldown = false;
        StopCoroutine("ActivateButton");
    }

    internal void ButtonPress()
    {
        if (_stateMachine.GetCurrentState().name.Equals("OffState(Clone)"))
        {
            _offcooldown = true;
        }
        else
        {
            _onCooldown = true;
        }
        StartCoroutine("ActivateButton");
    }
    
    private void EnableInteraction(OnPlayerEnteredInteractionEvent obj)
    {
        if (obj.interactableObject == gameObject)
        { 
            _isInteractable = true;   
        }
    }
    
    private void DisableInteraction(OnPlayerExitedInteractionEvent obj)
    {
        if (obj.interactableObject == gameObject)
        { 
            _isInteractable = false;   
        }
    }

}




