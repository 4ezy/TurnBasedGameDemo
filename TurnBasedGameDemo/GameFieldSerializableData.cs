using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    [Serializable]
    public class GameFieldSerializableData
    {
        public List<GameFieldCellSerializableData> GameFieldCellsSerializableData { get; set; }
    }
}
