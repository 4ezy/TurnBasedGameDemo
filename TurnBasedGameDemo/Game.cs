using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TurnBasedGameDemo.ViewModels;
using TurnBasedGameDemo.Views;

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

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        private bool _isPlayer1Selected;
        public bool IsPlayer1Selected
        {
            get { return _isPlayer1Selected; }
            set
            {
                _isPlayer1Selected = value;

                if (_isPlayer1Selected)
                    GameField.SelectedPlayer = Player1;
                else
                    GameField.SelectedPlayer = Player2;
            }
        }

        public Game()
        {
            GameField = new GameField(0, 0, null);
            Player1 = new Player("pack://application:,,,/Resources/swordsman1.png",
                "pack://application:,,,/Resources/archer1.png",
                "pack://application:,,,/Resources/peasant1.png");
            Player2 = new Player("pack://application:,,,/Resources/swordsman2.png",
                "pack://application:,,,/Resources/archer2.png",
                "pack://application:,,,/Resources/peasant2.png");
            IsPlayer1Selected = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
