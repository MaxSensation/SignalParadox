//Main author: Maximiliam Rosén
//Secondary Author: Andreas Berzelius

using Interactables.Button;
using Interactables.Triggers.Platform;
using SaveSystem;
using UnityEngine;

public class CompletedPuzzelSaver : MonoBehaviour
{
    [SerializeField] private ButtonController buttonToLock;
    [SerializeField] private float delay;
    [SerializeField] [Tooltip("puzzle to save upon completion")] private Puzzle puzzleToSave;
    private float currentTime;
    private bool completedPuzzle;

    private enum Puzzle { glassroom, labyrinth };

    private void Awake()
    {
        if (puzzleToSave == (Puzzle.labyrinth))
        {
            DisablePlattforms.onDisablePlattformsEvent += () => SaveManager.WorldEventsData.PuzzleLabyrinthCompleted = true;
            if (SaveManager.WorldEventsData != null && SaveManager.WorldEventsData.PuzzleLabyrinthCompleted)
                completedPuzzle = true;
        }
        else
        {
            GetComponent<ButtonController>().onStateChangeEvent += SaveGlassroomSolved;
            if (SaveManager.WorldEventsData != null && SaveManager.WorldEventsData.PuzzleGlassRoomCompleted)
                completedPuzzle = true;
        }
    }

    private void Update()
    {
        if (!completedPuzzle) return;

            currentTime += Time.deltaTime;
            if (currentTime >= delay)
            {
                GetComponent<ButtonController>().ButtonPress();
                buttonToLock.Lock();
                completedPuzzle = false;
            }
    }

    private void SaveGlassroomSolved(ButtonController.ButtonStates state)
    {
        if (state == ButtonController.ButtonStates.Activated && SaveManager.WorldEventsData != null)
            SaveManager.WorldEventsData.PuzzleGlassRoomCompleted = true;
    }

}
