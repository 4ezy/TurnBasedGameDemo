using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo.ViewModels
{
    public class StartWindowViewModel
    {
        public StartWindowAction StartWindowAction { get; set; }
        public RelayCommand StartNewGameCommand { get; set; }
        public RelayCommand LoadGameCommand { get; set; }
        public RelayCommand ExitGameCommand { get; set; }

        public StartWindowViewModel()
        {
            StartNewGameCommand = new RelayCommand();
            LoadGameCommand = new RelayCommand();
            ExitGameCommand = new RelayCommand();

            StartNewGameCommand.ExecutedCommand += (() => 
            {
                StartWindowAction = StartWindowAction.StartGame;
            });

            LoadGameCommand.ExecutedCommand += (() =>
            {
                StartWindowAction = StartWindowAction.LoadGame;
            });

            ExitGameCommand.ExecutedCommand += (() =>
            {
                StartWindowAction = StartWindowAction.ExitGame;
            });
        }
    }
}
