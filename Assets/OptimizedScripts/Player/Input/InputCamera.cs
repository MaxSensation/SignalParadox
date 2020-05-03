using UnityEngine;
using UnityEngine.InputSystem;

public class InputCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 1.0f;
    
    // private string mouseHorizontalAxis, mouseVerticalAxis;
    private Vector2 _mouseInput;

    private void Awake()
    {
        // mouseHorizontalAxis = "Mouse X";
        // mouseVerticalAxis = "Mouse Y";
    }

    public void UpdateCameraInput(InputAction.CallbackContext context)
    {
        _mouseInput = context.ReadValue<Vector2>();
    }
    
    public float GetHorizontalCameraInput()
    {
        return _mouseInput.x * mouseSensitivity;
    }

    public float GetVerticalCameraInput()
    {
        return -_mouseInput.y * mouseSensitivity;
    }

    public bool IsCrouching()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }
}
