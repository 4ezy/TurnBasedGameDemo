using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    public class Game : INotifyPropertyChanged
    {
        private GameField _gameField;

        public GameField GameField
        {
            get { return _gameField; }
            set
            {
                _gameField = value;
                NotifyPropertyChanged("GameField");
            }
        }

        public List<Player> Players { get; set; }

        public Game()
        {
            GameField = new GameField();
            Players = new List<Player>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
