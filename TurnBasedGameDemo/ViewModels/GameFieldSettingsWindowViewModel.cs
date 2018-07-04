using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo.ViewModels
{
    public class GameFieldSettingsViewModel : INotifyPropertyChanged
    {
        private int _gameFieldWidth;
        private int _gameFiledHeight;

        public int GameFieldWidth
        {
            get { return _gameFieldWidth; }
            set
            {
                _gameFieldWidth = value;
                OnPropertyChanged("GameFieldWidth");
            }
        }

        public int GameFieldHeight {
            get { return _gameFiledHeight; }
            set
            {
                _gameFiledHeight = value;
                OnPropertyChanged("GameFieldHeight");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
