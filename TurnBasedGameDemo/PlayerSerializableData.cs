using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    [Serializable]
    public class PlayerSerializableData
    {
        public Uri SwordsmanImageUri { get; set; }
        public Uri ArcherImageUri { get; set; }
        public Uri PeasantImageUri { get; set; }
        public List<UnitStackSerializableData> UnitStacksSerializableData { get; set; }
    }
}
