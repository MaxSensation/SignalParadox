//Main author: Maximiliam Rosén

using System;
namespace SaveSystem
{
    [Serializable]
    public class GameSettingData
    {
        public float Volume { get; set; }
        public float Sensitivity { get; set; }

        public GameSettingData()
        {
            Volume = 1f;
            Sensitivity = 0.5f;
        }
    }
}