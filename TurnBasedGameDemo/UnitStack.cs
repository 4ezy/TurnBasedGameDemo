using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TurnBasedGameDemo
{
    public class UnitStack : IComparable<UnitStack>
    {
        public int UnitsCapacity { get; }
        public Stack<Unit> Units { get; set; }
        public GameFieldCell Cell { get; set; }
        public int CellIndex { get; set; }
        public UnitType UnitType { get; }

        public UnitStack(UnitType unitType, int unitsCapacity)
        {
            UnitsCapacity = unitsCapacity;
            UnitType = unitType;
        }

        public UnitStack(UnitType unitType, int numberOfUnits, GameFieldCell cell)
        {
            UnitType = unitType;
            Units = new Stack<Unit>(numberOfUnits);

            for (int i = 0; i < numberOfUnits; i++)
            {
                Units.Push(SimpleUnitFactory.CreateUnit(unitType));
            }

            UnitsCapacity = numberOfUnits;
            Cell = cell;
        }

        public string Attack(UnitStack unitStack)
        {
            if (unitStack == null)
                return "Unitstack attacked air and deals 0 damage.";
            else if (unitStack == this)
                return "Unitstack don't want attack oneself, general.";

            var random = new Random(DateTime.Today.Second);
            Unit unit = Units.FirstOrDefault();
            int attackValue = random.Next(unit.MinDamage, unit.MaxDamage) * Units.Count;
            string ret = $"Unitstack attacked and deals {attackValue} damage.";

            while (attackValue != 0 && unitStack.Units.Count != 0)
            {
                int res = unitStack.Units.Peek().CurrentHitPoints - attackValue;

                if (res <= 0)
                {
                    unitStack.Units.Pop();
                    attackValue = res * -1;
                }
                else
                {
                    unitStack.Units.Peek().CurrentHitPoints = res;
                    attackValue = 0;
                }
            }

            return ret;
        }

        public int CompareTo(UnitStack other)
        {
            if (other == null)
                return 1;
            else
                return Units.FirstOrDefault().Speed.CompareTo(
                    other.Units.FirstOrDefault().Speed);
        }
    }
}
