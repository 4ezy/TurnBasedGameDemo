using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    [Serializable]
    public class GameSerializableData
    {
        public bool ActionCompleted { get; set; }
        public bool GameEnded { get; set; }
        public bool IsPlayer1Selected { get; set; }

        public PlayerSerializableData Player1 { get; set; }
        public PlayerSerializableData Player2 { get; set; }
        public GameFieldSerializableData GameFieldSerializableData { get; set; }
    }
}
