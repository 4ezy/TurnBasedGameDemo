using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TurnBasedGameDemo
{
    public abstract class Unit
    {
        public int HitPoints { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int Speed { get; set; }
        public int MovementPoints { get; set; }

        protected Unit(int hitPoints, int minDamage,
            int maxDamage, int speed, int movementPoints)
        {
            HitPoints = hitPoints;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Speed = speed;
            MovementPoints = movementPoints;
        }
    }
}
