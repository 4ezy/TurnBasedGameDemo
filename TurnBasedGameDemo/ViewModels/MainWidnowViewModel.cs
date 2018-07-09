using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Xml.Serialization;
using TurnBasedGameDemo.Views;

namespace TurnBasedGameDemo.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public Game Game { get; set; }

        private readonly string gameSavedDataPath = "savedata.dat";

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
        public RelayCommand SaveGameCommand { get; set; }
        public RelayCommand LoadGameCommand { get; set; }

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

            SaveGameCommand = new RelayCommand();
            SaveGameCommand.ExecutedCommand += SaveGame;

            LoadGameCommand = new RelayCommand();
            LoadGameCommand.ExecutedCommand += LoadGame;
        }

        private void StartGame()
        {
            if (Game.Player1.UnitStacks.Count == 0 ||
                Game.Player2.UnitStacks.Count == 0)
            {
                MessageBox.Show("You need add unitstacks for both players to start the game!");
                return;
            }

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

        private PlayerSerializableData GetPlayerSerializableData(Player sourcePlayer)
        {
            PlayerSerializableData player = new PlayerSerializableData();

            player = new PlayerSerializableData()
            {
                ArcherImageUri = sourcePlayer.ArcherImageUri,
                PeasantImageUri = sourcePlayer.PeasantImageUri,
                SwordsmanImageUri = sourcePlayer.PeasantImageUri,
            };

            player.UnitStacksSerializableData = new List<UnitStackSerializableData>();

            foreach (var item in sourcePlayer.UnitStacks)
            {
                var unitStackSerializableData
                    = new UnitStackSerializableData
                    {
                        UnitsCapacity = item.UnitsCapacity,
                        Units = item.Units
                    };

                var gameFieldCellSerializableData =
                    new GameFieldCellSerializableData
                    {
                        CurrentUnitNumber = item.Cell.CurrentUnitNumber,
                        IsSelected = item.Cell.IsSelected,
                        MaxUnitNumber = item.Cell.MaxUnitNumber,
                        UnitStack = unitStackSerializableData
                    };

                unitStackSerializableData.Cell = gameFieldCellSerializableData;

                player.UnitStacksSerializableData.Add(unitStackSerializableData);
            }

            return player;
        }

        private GameFieldSerializableData GetGameFieldSerializableData(GameField sourceField)
        {
            var gameFieldSerializableData = new GameFieldSerializableData
            {
                GameFieldCellsSerializableData = new List<GameFieldCellSerializableData>()
            };

            foreach (var item in sourceField.GameFieldCells)
            {
                var unitStackSerializableData = new UnitStackSerializableData
                {
                    Units = item.UnitStack?.Units,
                    UnitsCapacity = item.UnitStack != null ? item.UnitStack.UnitsCapacity : 0
                };

                var gameFieldCellSerializableData =
                    new GameFieldCellSerializableData
                    {
                        CurrentUnitNumber = item.CurrentUnitNumber,
                        IsSelected = item.IsSelected,
                        MaxUnitNumber = item.MaxUnitNumber,
                        UnitStack = unitStackSerializableData
                    };

                unitStackSerializableData.Cell = gameFieldCellSerializableData;
                gameFieldSerializableData.GameFieldCellsSerializableData.Add(gameFieldCellSerializableData);
            }

            return gameFieldSerializableData;
        }

        public void SaveGame()
        {
            var data = new GameSerializableData()
            {
                ActionCompleted = Game.ActionCompleted,
                GameEnded = Game.GameEnded,
                IsPlayer1Selected = Game.IsPlayer1Selected
            };

            data.Player1 = GetPlayerSerializableData(Game.Player1);
            data.Player2 = GetPlayerSerializableData(Game.Player2);
            data.GameFieldSerializableData = GetGameFieldSerializableData(Game.GameField);

            var binaryFormatter = new BinaryFormatter();
            using (var fs = new FileStream(gameSavedDataPath, FileMode.Create))
                binaryFormatter.Serialize(fs, data);
        }

        public void LoadGame()
        {
            GameSerializableData data = null;
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            try
            {
                using (FileStream fs = new FileStream(gameSavedDataPath, FileMode.Open))
                    data = (GameSerializableData)binaryFormatter.Deserialize(fs);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File with saved game does not exist!");
                return;
            }
            catch (SerializationException)
            {
                MessageBox.Show("File with saved game is corrupted!");
                return;
            }

            Game.GameField = new GameField();
            Game.ActionCompleted = data.ActionCompleted;
            Game.GameEnded = data.GameEnded;
            Game.IsPlayer1Selected = data.IsPlayer1Selected;
            Game.Player1 = new Player(data.Player1.SwordsmanImageUri,
                data.Player1.ArcherImageUri,
                data.Player1.PeasantImageUri);
            Game.Player2 = new Player(data.Player2.SwordsmanImageUri,
                data.Player2.ArcherImageUri,
                data.Player2.PeasantImageUri);
            
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
