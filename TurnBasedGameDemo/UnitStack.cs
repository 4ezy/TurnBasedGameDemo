using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    public class UnitStack
    {
        public Stack<Character> CharacterUnitStack { get; set; }

        public void Attack(UnitStack unitStack) { }

        public void Wait() { }

        public void Move() { }
    }
}
