using System.Collections;
using Interactables.Button;
using SaveSystem;
using UnityEngine;

public class CompleteLabyrinth : MonoBehaviour
{
    [SerializeField] private ButtonController _buttonController;
    private void Awake()
    {
        DisablePlattforms.onDisablePlattformsEvent += () => SaveManager._worldData.PuzzleLabyrinthCompleted = true;
        if (SaveManager._worldData.PuzzleLabyrinthCompleted)
            StartCoroutine(ActivateButton());
    }

    private IEnumerator ActivateButton()
    {
        yield return new WaitForSeconds(3);
        GetComponent<ButtonController>().ButtonPress();
        _buttonController.Lock();
    }
}
