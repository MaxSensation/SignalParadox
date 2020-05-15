//Main author: Maximiliam Rosén

using System.Collections.Generic;
using Interactables.Button;
using UnityEngine;

public class ButtonScreen : MonoBehaviour
{
    [Header("Color")] 
    [Tooltip("The default Color of the buttonScreen")][SerializeField] private Color standbyColor;
    [Tooltip("The activated Color of the buttonScreen")][SerializeField] private Color activatedColor;
    [Tooltip("The locked Color of the buttonScreen")][SerializeField] private Color lockedColor;
    
    private Renderer _renderer;
    private ButtonController _buttonController;
    private Dictionary<ButtonController.ButtonStates, Color> _colorMap;

    private void Awake()
    {
        _colorMap = new Dictionary<ButtonController.ButtonStates, Color>
        {
            {ButtonController.ButtonStates.Standby, standbyColor},
            {ButtonController.ButtonStates.Activated, activatedColor},
            {ButtonController.ButtonStates.Locked, lockedColor}
        };
        _renderer = GetComponent<MeshRenderer>();
        _buttonController = GetComponentInParent<ButtonController>();
        _buttonController.onStateChangeEvent += UpdateColor;
    }

    private void OnDestroy() => _buttonController.onStateChangeEvent -= UpdateColor; 

    private void UpdateColor(ButtonController.ButtonStates state)
    {
        _renderer.material.color = _colorMap[state];
    }
}
