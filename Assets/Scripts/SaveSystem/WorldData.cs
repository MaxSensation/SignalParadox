using System;

namespace SaveSystem
{
    [Serializable]
    public class WorldData
    {
        //Level 3 Special Events
        public bool PuzzleLabyrinthCompleted { get; set; }
        public bool PuzzleGlassRoomCompleted { get; set; }
    }
}