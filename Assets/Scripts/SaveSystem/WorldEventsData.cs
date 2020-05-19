//Main author: Maximiliam Rosén

using System;

namespace SaveSystem
{
    [Serializable]
    public class WorldEventsData
    {
        //Level 1 Cinematic has played
        public bool CinematicHasPlayed { get; set; }
        
        //Level 3 Special Events
        public bool PuzzleLabyrinthCompleted { get; set; }
        public bool PuzzleGlassRoomCompleted { get; set; }
    }
}