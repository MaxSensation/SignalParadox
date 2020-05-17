using System.Collections;
using Interactables.Button;
using SaveSystem;
using UnityEngine;

public class CompleteGlassRoom : MonoBehaviour
{
    [SerializeField] private ButtonController _buttonController;
    private void Awake()
    {
        GetComponent<ButtonController>().onStateChangeEvent += Save;
        if (SaveManager.WorldData.PuzzleGlassRoomCompleted)
            StartCoroutine(ActivateButton());
    }

    private void Save(ButtonController.ButtonStates state)
    {
        if (state == ButtonController.ButtonStates.Activated)
            SaveManager.WorldData.PuzzleGlassRoomCompleted = true;
    }

    private IEnumerator ActivateButton()
    {
        yield return new WaitForSeconds(3);
        GetComponent<ButtonController>().ButtonPress();
        _buttonController.Lock();
    }
}
