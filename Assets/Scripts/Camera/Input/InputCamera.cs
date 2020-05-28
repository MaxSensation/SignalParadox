//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;
using UnityEngine.InputSystem;

public class InputCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 1.0f;
    
    private Vector2 mouseInput;

    public void UpdateCameraInput(InputAction.CallbackContext context) => mouseInput = context.ReadValue<Vector2>();
    
    public float GetHorizontalCameraInput() => mouseInput.x * mouseSensitivity;

    public float GetVerticalCameraInput() => -mouseInput.y * mouseSensitivity;
}
