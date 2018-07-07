using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo.ViewModels
{
    public class AddUnitWindowViewModel
    {
        public UnitType SelectedUnitType { get; set; }
        public List<string> UnitTypes { get; set; }
        public int NumberOfUnits { get; set; }

        public AddUnitWindowViewModel()
        {
            UnitTypes = Enum.GetNames(typeof(UnitType)).ToList();
            NumberOfUnits = 1;
        }
    }
}
