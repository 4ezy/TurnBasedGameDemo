using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TurnBasedGameDemo
{
    [Serializable]
    public class GameFieldCellSerializableData
    {
        public bool IsSelected { get; set; }
        public int CurrentUnitNumber { get; set; }
        public int MaxUnitNumber { get; set; }
        public UnitStackSerializableData UnitStack { get; set; }
    }
}
