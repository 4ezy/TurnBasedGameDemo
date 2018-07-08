using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using TurnBasedGameDemo.Views;

namespace TurnBasedGameDemo.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public Game Game { get; set; }

        private bool _isGameStarted;
        public bool IsGameStarted
        {
            get { return _isGameStarted; }
            set
            {
                _isGameStarted = value;
                NotifyPropertyChanged("IsGameStarted");
            }
        }

        private string _actionText;
        public string ActionText
        {
            get { return _actionText; }
            set
            {
                _actionText = value;
                NotifyPropertyChanged("ActionText");
            }
        }

        public RelayCommand OpenGameFieldSettingsCommand { get; set; }
        public RelayCommand StartGameCommand { get; set; }

        public MainWindowViewModel()
        {
            var startWindow = new StartWindow();

            if (startWindow.ShowDialog() == false)
            {
                Application.Current.Shutdown();
                return;
            }

            StartWindowAction startWindowAction =
                (startWindow.DataContext as StartWindowViewModel).StartWindowAction;

            switch (startWindowAction)
            {
                case StartWindowAction.NewGame:
                    Game = new Game();
                    break;
                case StartWindowAction.LoadGame:
                    break;
                case StartWindowAction.ExitGame:
                    Application.Current.Shutdown();
                    break;
                default:
                    break;
            }

            Game.OnActionCompleted += ((s) =>
            {
                ActionText = s;
            });

            GetGameFieldSettingsWindow();
            OpenGameFieldSettingsCommand = new RelayCommand();
            OpenGameFieldSettingsCommand.ExecutedCommand += (() =>
            {
                GetGameFieldSettingsWindow();
            });

            StartGameCommand = new RelayCommand();
            StartGameCommand.ExecutedCommand += StartGame;
        }

        private void StartGame()
        {
            Game.Player1.UnitStacks.Sort();
            Game.Player1.UnitStacks.Reverse();
            Game.Player2.UnitStacks.Sort();
            Game.Player2.UnitStacks.Reverse();
            Game.PrepareRound();
            IsGameStarted = true;

            Game.OnGameEnded += (() =>
            {
                if (Game.IsPlayer1Selected)
                    MessageBox.Show("Player 1 win!");
                else
                    MessageBox.Show("Player 2 win!");
            });

            Task.Factory.StartNew(() =>
            {
                Game.StartRound();
                IsGameStarted = false;
            });
        }

        private void GetGameFieldSettingsWindow()
        {
            var gameFieldSettingsWindow =
                new GameFieldSettingsWindow();

            if (gameFieldSettingsWindow.ShowDialog() == false)
                return;

            var gameFieldSettingsViewModel =
                gameFieldSettingsWindow.DataContext as GameFieldSettingsViewModel;

            Player player;

            if (Game.IsPlayer1Selected)
                player = Game.Player1;
            else
                player = Game.Player2;

            Game.GameField = new GameField(
                gameFieldSettingsViewModel.GameFieldWidth,
                gameFieldSettingsViewModel.GameFieldHeight,
                player);
            Game.Player1.UnitStacks.Clear();
            Game.Player2.UnitStacks.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
