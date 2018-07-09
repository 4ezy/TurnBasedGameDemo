using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TurnBasedGameDemo
{
    [Serializable]
    public abstract class Unit
    {
        public int HitPoints { get; }
        public int CurrentHitPoints { get; set; }
        public int MinDamage { get; }
        public int MaxDamage { get; }
        public int Speed { get; }
        public int MovementPoints { get; }

        protected Unit(int hitPoints, int minDamage,
            int maxDamage, int speed, int movementPoints)
        {
            HitPoints = hitPoints;
            CurrentHitPoints = hitPoints;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Speed = speed;
            MovementPoints = movementPoints;
        }
    }
}
