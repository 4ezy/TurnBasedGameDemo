using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    [Serializable]
    public class UnitStackSerializableData
    {
        public int UnitsCapacity { get; set; }
        public Stack<Unit> Units { get; set; }
        public GameFieldCellSerializableData Cell { get; set; }
    }
}
