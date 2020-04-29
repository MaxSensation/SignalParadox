using UnityEngine;

public class InputCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 1.0f;
    
    private string mouseHorizontalAxis, mouseVerticalAxis;

    private void Awake()
    {
        mouseHorizontalAxis = "Mouse X";
        mouseVerticalAxis = "Mouse Y";
    }

    public float GetHorizontalCameraInput()
    {
        return Input.GetAxisRaw(mouseHorizontalAxis) * mouseSensitivity;
    }

    public float GetVerticalCameraInput()
    {
        return -Input.GetAxisRaw(mouseVerticalAxis) * mouseSensitivity;
    }

    public bool IsCrouching()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }
}
