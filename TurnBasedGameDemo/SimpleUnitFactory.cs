using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    public static class SimpleUnitFactory
    {
        public static Unit CreateUnit(UnitType unitType)
        {
            Unit unit = null;

            switch (unitType)
            {
                case UnitType.Swordsman:
                    unit = new Swordsman();
                    break;
                case UnitType.Archer:
                    unit = new Archer();
                    break;
                case UnitType.Peasant:
                    unit = new Peasant();
                    break;
                default:
                    break;
            }

            return unit;
        }
    }
}
