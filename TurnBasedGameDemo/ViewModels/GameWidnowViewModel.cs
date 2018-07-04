using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using TurnBasedGameDemo.ViewModels;

namespace TurnBasedGameDemo
{
    public class GameWidnowViewModel
    {
        public Canvas Canvas { get; set; }
        public RelayCommand OpenGameFieldSettingsCommand { get; set; }

        public GameWidnowViewModel()
        {
            StartWindow startWindow = new StartWindow();

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
                    break;
                case StartWindowAction.LoadGame:
                    break;
                case StartWindowAction.ExitGame:
                    Application.Current.Shutdown();
                    break;
                default:
                    break;
            }

            OpenGameFieldSettingsCommand = new RelayCommand();
            OpenGameFieldSettingsCommand.ExecutedCommand += (() =>
            {
                OpenGameAreaSettingWindow();
            });
        }

        private void OpenGameAreaSettingWindow()
        {
            GameFieldSettingsWindow gameFieldSettingsWindow =
                new GameFieldSettingsWindow();
            gameFieldSettingsWindow.ShowDialog();
            GameFieldSettingsViewModel gameFieldSettingsViewModel =
                gameFieldSettingsWindow.DataContext as GameFieldSettingsViewModel;
        }
    }
}
