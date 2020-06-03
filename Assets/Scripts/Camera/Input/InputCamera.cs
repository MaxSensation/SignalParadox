//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using SaveSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCamera : MonoBehaviour
{
    private float mouseSensitivity;

    private void Start()
    {
        mouseSensitivity = SaveManager.Settings != null ? SaveManager.Settings.Sensitivity : 0.2f;
    }

    private Vector2 mouseInput;

    public void UpdateCameraInput(InputAction.CallbackContext context) => mouseInput = context.ReadValue<Vector2>();
    
    public float GetHorizontalCameraInput() => mouseInput.x * mouseSensitivity;

    public float GetVerticalCameraInput() => -mouseInput.y * mouseSensitivity;
}
