using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    public interface IAttackBehavior
    {
        int Attack(UnitStack unitStack);
    }
}
