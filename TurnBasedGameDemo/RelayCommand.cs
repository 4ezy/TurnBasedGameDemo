using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TurnBasedGameDemo
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public event Action ExecutedCommand;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ExecutedCommand?.Invoke();
        }
    }
}
