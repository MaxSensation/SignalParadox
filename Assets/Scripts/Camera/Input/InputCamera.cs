//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using Menu;
using SaveSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCamera : MonoBehaviour
{
    private float mouseSensitivity;
    private Vector2 mouseInput;
    
    
    private void Start()
    {
        SetSensitivity.onUpdateEvent += UpdateSensitivity;
        mouseSensitivity = SaveManager.Settings != null ? SaveManager.Settings.Sensitivity : 0.2f;
    }


    private void UpdateSensitivity(float sens)
    {
        mouseSensitivity = sens;
    }
    
    public void UpdateCameraInput(InputAction.CallbackContext context) => mouseInput = context.ReadValue<Vector2>();
    
    public float GetHorizontalCameraInput() => mouseInput.x * mouseSensitivity;

    public float GetVerticalCameraInput() => -mouseInput.y * mouseSensitivity;
}
