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
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private GameField _gameField;

        public GameField GameField
        {
            get { return _gameField; }
            set
            {
                _gameField = value;
                OnPropertyChanged("GameField");
            }
        }

        public RelayCommand OpenGameFieldSettingsCommand { get; set; }

        public MainWindowViewModel()
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

            GetGameFieldSettingsWindow();
            OpenGameFieldSettingsCommand = new RelayCommand();
            OpenGameFieldSettingsCommand.ExecutedCommand += (() =>
            {
                GetGameFieldSettingsWindow();
            });
        }

        private void GetGameFieldSettingsWindow()
        {
            GameFieldSettingsWindow gameFieldSettingsWindow =
                new GameFieldSettingsWindow();
            gameFieldSettingsWindow.ShowDialog();
            GameFieldSettingsViewModel gameFieldSettingsViewModel =
                gameFieldSettingsWindow.DataContext as GameFieldSettingsViewModel;
            GameField = new GameField(
                gameFieldSettingsViewModel.GameFieldWidth,
                gameFieldSettingsViewModel.GameFieldHeight);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
