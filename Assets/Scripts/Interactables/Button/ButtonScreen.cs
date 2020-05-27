﻿//Main author: Maximiliam Rosén

using System.Collections.Generic;
using UnityEngine;

namespace Interactables.Button
{
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
            var block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", _colorMap[state]);
            _renderer.SetPropertyBlock(block);
        }
    }
}
