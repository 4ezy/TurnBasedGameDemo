using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using TurnBasedGameDemo.Views;

namespace TurnBasedGameDemo.ViewModels
{
    public class MainWindowViewModel
    {
        public Game Game { get; set; }

        public RelayCommand OpenGameFieldSettingsCommand { get; set; }

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
                case StartWindowAction.StartGame:
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

            GetGameFieldSettingsWindow();
            OpenGameFieldSettingsCommand = new RelayCommand();
            OpenGameFieldSettingsCommand.ExecutedCommand += (() =>
            {
                GetGameFieldSettingsWindow();
            });
        }

        private void GetGameFieldSettingsWindow()
        {
            var gameFieldSettingsWindow =
                new GameFieldSettingsWindow();
            gameFieldSettingsWindow.ShowDialog();
            var gameFieldSettingsViewModel =
                gameFieldSettingsWindow.DataContext as GameFieldSettingsViewModel;
            Game.GameField = new GameField(
                gameFieldSettingsViewModel.GameFieldWidth,
                gameFieldSettingsViewModel.GameFieldHeight);
        }
    }
}
