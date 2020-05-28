//Main author: Maximiliam Rosén

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
    
        private Renderer buttonScreenrenderer;
        private ButtonController buttonController;
        private Dictionary<ButtonController.ButtonStates, Color> colorMap;

        private void Awake()
        {
            colorMap = new Dictionary<ButtonController.ButtonStates, Color>
            {
                {ButtonController.ButtonStates.Standby, standbyColor},
                {ButtonController.ButtonStates.Activated, activatedColor},
                {ButtonController.ButtonStates.Locked, lockedColor}
            };
            buttonScreenrenderer = GetComponent<MeshRenderer>();
            buttonController = GetComponentInParent<ButtonController>();
            buttonController.onStateChangeEvent += UpdateColor;
        }

        private void OnDestroy() => buttonController.onStateChangeEvent -= UpdateColor; 

        private void UpdateColor(ButtonController.ButtonStates state)
        {
            var block = new MaterialPropertyBlock();
            block.SetColor("BaseColor", colorMap[state]);
            buttonScreenrenderer.SetPropertyBlock(block);
        }
    }
}