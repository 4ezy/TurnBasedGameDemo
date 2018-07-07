using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    public class UnitStack
    {
        public Stack<Unit> Units { get; set; }

        public UnitStack(UnitType unitType, int numberOfUnits)
        {
            Units = new Stack<Unit>(numberOfUnits);

            for (int i = 0; i < numberOfUnits; i++)
            {
                Units.Push(SimpleUnitFactory.CreateUnit(unitType));
            }
        }

        public void Attack(UnitStack unitStack) { }

        public void Wait() { }

        public void Move() { }
    }
}
