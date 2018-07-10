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
        public RelayCommand StopGameCommand { get; set; }

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
                    Game = new Game();
                    LoadGame();
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

            StopGameCommand = new RelayCommand();
            StopGameCommand.ExecutedCommand += StopGame;
        }

        private void StopGame()
        {
            Game.IsGameStopped = true;
            Game.GameEnded = true;
            Game.ActionCompleted = true;
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
            Game.PrepareGame();
            Game.GameEnded = false;
            Game.IsGameStopped = false;
            Game.ActionCompleted = false;
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
                SwordsmanImageUri = sourcePlayer.SwordsmanImageUri,
            };

            player.UnitStacksSerializableData = new List<UnitStackSerializableData>();

            foreach (var item in sourcePlayer.UnitStacks)
            {
                var unitStackSerializableData
                    = new UnitStackSerializableData
                    {
                        UnitsCapacity = item.UnitsCapacity,
                        Units = item.Units,
                        UnitType = item.UnitType,
                        CellIndex = item.CellIndex
                    };

                var gameFieldCellSerializableData =
                    new GameFieldCellSerializableData
                    {
                        CurrentUnitNumber = item.Cell.CurrentUnitNumber,
                        IsSelected = item.Cell.IsSelected,
                        MaxUnitNumber = item.Cell.MaxUnitNumber,
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
                GameFieldCellsSerializableData = new List<GameFieldCellSerializableData>(),
                HorizCellsCount = Game.GameField.HorizCellsCount,
                VertCellsCount = Game.GameField.VertCellsCount
            };

            foreach (var item in sourceField.GameFieldCells)
            {
                var gameFieldCellSerializableData =
                        new GameFieldCellSerializableData
                        {
                            CurrentUnitNumber = item.CurrentUnitNumber,
                            IsSelected = item.IsSelected,
                            MaxUnitNumber = item.MaxUnitNumber,
                        };

                gameFieldSerializableData.GameFieldCellsSerializableData.Add(gameFieldCellSerializableData);
            }

            return gameFieldSerializableData;
        }

        public void SaveGame()
        {
            var data = new GameSerializableData()
            {
                IsPlayer1Selected = Game.IsPlayer1Selected
            };

            data.Player1 = GetPlayerSerializableData(Game.Player1);
            data.Player2 = GetPlayerSerializableData(Game.Player2);
            data.GameFieldSerializableData = GetGameFieldSerializableData(Game.GameField);

            var binaryFormatter = new BinaryFormatter();
            using (var fs = new FileStream(gameSavedDataPath, FileMode.Create))
                binaryFormatter.Serialize(fs, data);
        }

        public void LoadPlayers(GameSerializableData data)
        {
            foreach (var item in data.Player1.UnitStacksSerializableData)
            {
                UnitStack unitStack = new UnitStack(item.UnitType, item.UnitsCapacity)
                {
                    Units = new Stack<Unit>(),
                    CellIndex = item.CellIndex
                };

                foreach (var unit in item.Units)
                {
                    Unit u = SimpleUnitFactory.CreateUnit(item.UnitType);
                    u.CurrentHitPoints = unit.CurrentHitPoints;
                    unitStack.Units.Push(u);
                }

                unitStack.Cell = Game.GameField.GameFieldCells[unitStack.CellIndex];
                Game.Player1.UnitStacks.Add(unitStack);
                Game.GameField.GameFieldCells[unitStack.CellIndex].UnitStack = Game.Player1.UnitStacks.Last();

                Game.GameField.GameFieldCells[unitStack.CellIndex].MaxUnitNumber =
                    data.GameFieldSerializableData.GameFieldCellsSerializableData[unitStack.CellIndex].MaxUnitNumber;
                Game.GameField.GameFieldCells[unitStack.CellIndex].CurrentUnitNumber =
                    data.GameFieldSerializableData.GameFieldCellsSerializableData[unitStack.CellIndex].CurrentUnitNumber;

                switch (Game.GameField.GameFieldCells[unitStack.CellIndex].UnitStack.UnitType)
                {
                    case UnitType.Swordsman:
                        Game.GameField.GameFieldCells[unitStack.CellIndex].UnitImage = Game.Player1.SwordsmanImage;
                        break;
                    case UnitType.Archer:
                        Game.GameField.GameFieldCells[unitStack.CellIndex].UnitImage = Game.Player1.ArcherImage;
                        break;
                    case UnitType.Peasant:
                        Game.GameField.GameFieldCells[unitStack.CellIndex].UnitImage = Game.Player1.PeasantImage;
                        break;
                    default:
                        break;
                }

            }

            foreach (var item in data.Player2.UnitStacksSerializableData)
            {
                UnitStack unitStack = new UnitStack(item.UnitType, item.UnitsCapacity)
                {
                    Units = new Stack<Unit>(),
                    CellIndex = item.CellIndex
                };

                foreach (var unit in item.Units)
                {
                    Unit u = SimpleUnitFactory.CreateUnit(item.UnitType);
                    u.CurrentHitPoints = unit.CurrentHitPoints;
                    unitStack.Units.Push(u);
                }

                unitStack.Cell = Game.GameField.GameFieldCells[unitStack.CellIndex];
                Game.Player2.UnitStacks.Add(unitStack);
                Game.GameField.GameFieldCells[unitStack.CellIndex].UnitStack = Game.Player2.UnitStacks.Last();

                Game.GameField.GameFieldCells[unitStack.CellIndex].MaxUnitNumber =
                    data.GameFieldSerializableData.GameFieldCellsSerializableData[unitStack.CellIndex].MaxUnitNumber;
                Game.GameField.GameFieldCells[unitStack.CellIndex].CurrentUnitNumber =
                    data.GameFieldSerializableData.GameFieldCellsSerializableData[unitStack.CellIndex].CurrentUnitNumber;

                switch (Game.GameField.GameFieldCells[unitStack.CellIndex].UnitStack.UnitType)
                {
                    case UnitType.Swordsman:
                        Game.GameField.GameFieldCells[unitStack.CellIndex].UnitImage = Game.Player2.SwordsmanImage;
                        break;
                    case UnitType.Archer:
                        Game.GameField.GameFieldCells[unitStack.CellIndex].UnitImage = Game.Player2.ArcherImage;
                        break;
                    case UnitType.Peasant:
                        Game.GameField.GameFieldCells[unitStack.CellIndex].UnitImage = Game.Player2.PeasantImage;
                        break;
                    default:
                        break;
                }

            }
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

            Game.GameField = new GameField(data.GameFieldSerializableData.HorizCellsCount,
                data.GameFieldSerializableData.VertCellsCount, null);
            Game.Player1 = new Player(data.Player1.SwordsmanImageUri,
                data.Player1.ArcherImageUri,
                data.Player1.PeasantImageUri);
            Game.Player2 = new Player(data.Player2.SwordsmanImageUri,
                data.Player2.ArcherImageUri,
                data.Player2.PeasantImageUri);
            Game.IsPlayer1Selected = data.IsPlayer1Selected;
            LoadPlayers(data);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
