using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    public abstract class Character
    {

        public int HitPoints { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int Speed { get; set; }
        public int MovementPoints { get; set; }

        protected Character(int hitPoints, int minDamage,
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
