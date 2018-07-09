using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TurnBasedGameDemo
{
    [Serializable]
    public class Swordsman : Unit
    {
        public Swordsman()
            : base(25, 2, 3, 3, 2) { }
    }
}
