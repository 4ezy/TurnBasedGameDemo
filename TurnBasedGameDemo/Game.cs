using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

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

        public event Action<string> OnActionCompleted;
        public bool ActionCompleted { get; set; }
        public bool GameEnded
        {
            get { return _gameEnded; }
            set
            {
                _gameEnded = value;

                if (_gameEnded)
                    EndGame();
            }
        }
        public bool IsGameStopped { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public event Action OnGameEnded;

        private bool _isPlayer1Selected;
        private bool _gameEnded;

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

                NotifyPropertyChanged("IsPlayer1Selected");
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

        public void PrepareGame()
        {
            foreach (var c in GameField.Children)
            {
                var cell = c as GameFieldCell;
                cell.PreviewMouseRightButtonUp -= GameField.OnCellSelected;
                cell.PreviewMouseRightButtonUp += GameField.OnCellForAttackSelected;
            }

            CreateGameContextMenu();
        }

        public void EndGame()
        {
            foreach (var c in GameField.Children)
            {
                var cell = c as GameFieldCell;
                cell.PreviewMouseRightButtonUp -= GameField.OnCellForAttackSelected;
                cell.PreviewMouseRightButtonUp += GameField.OnCellSelected;
            }

            GameField.CreateContextMenuForGameField();
        }

        public void StartRound()
        {
            if (!GameEnded)
            {
                IsPlayer1Selected = true;

                for (int i = 0; i < Player1.UnitStacks.Count; i++)
                {
                    if (GameEnded)
                        break;

                    ActionCompleted = false;

                    GameField.SelectCell(Player1.UnitStacks[i].Cell);

                    while (!ActionCompleted) { }
                }
            }

            if (!GameEnded)
            {
                IsPlayer1Selected = false;

                for (int i = 0; i < Player2.UnitStacks.Count; i++)
                {
                    if (GameEnded)
                        break;

                    ActionCompleted = false;

                    GameField.SelectCell(Player2.UnitStacks[i].Cell);

                    while (!ActionCompleted) { }
                }
            }

            if (!GameEnded)
                StartRound();

            if (!IsGameStopped)
            {
                OnGameEnded?.Invoke();
                OnGameEnded = null;
            }
        }

        public void CreateGameContextMenu()
        {
            var contextMenu = new ContextMenu();
            var menuItemAttack = new MenuItem();
            var menuItemWait = new MenuItem();
            contextMenu.Items.Add(menuItemAttack);
            contextMenu.Items.Add(menuItemWait);
            menuItemAttack.Header = "Attack";
            menuItemWait.Header = "Wait";
            menuItemAttack.Click += AttackClick;
            menuItemWait.Click += WaitClick;
            GameField.ContextMenu = contextMenu;
        }

        public void WaitClick(object sender, System.Windows.RoutedEventArgs e)
        {
            string actionText = "Unitstack wait...";
            ActionCompleted = true;
            OnActionCompleted?.Invoke(actionText);
        }

        public void AttackClick(object sender, System.Windows.RoutedEventArgs e)
        {
            string actionText = GameField.SelectedCell.UnitStack.Attack(GameField.CellToAttack.UnitStack);

            if (GameField.CellToAttack.UnitStack != null &&
                GameField.CellToAttack.UnitStack != GameField.SelectedCell.UnitStack &&
                GameField.CellToAttack.UnitStack.Units.Count < GameField.CellToAttack.MaxUnitNumber)
            {
                if (GameField.CellToAttack.UnitStack.Units.Count > 1)
                {
                    GameField.CellToAttack.CurrentUnitNumber =
                        (GameField.CellToAttack.UnitStack.Units.Count - 1) *
                        GameField.CellToAttack.UnitStack.Units.FirstOrDefault().HitPoints +
                        GameField.CellToAttack.UnitStack.Units.Peek().CurrentHitPoints;
                }
                else if (GameField.CellToAttack.UnitStack.Units.Count == 1)
                {
                    GameField.CellToAttack.CurrentUnitNumber =
                        GameField.CellToAttack.UnitStack.Units.Peek().CurrentHitPoints;
                }
                else
                {
                    GameField.CellToAttack.CurrentUnitNumber = 0;
                    Player2.UnitStacks.Remove(GameField.CellToAttack.UnitStack);
                    Player1.UnitStacks.Remove(GameField.CellToAttack.UnitStack);

                    if (IsPlayer1Selected)
                        GameEnded = Player2.UnitStacks.Count > 0 ? false : true;
                    else
                        GameEnded = Player1.UnitStacks.Count > 0 ? false : true;

                    GameField.CellToAttack.ClearCell();
                }
            }

            ActionCompleted = true;
            OnActionCompleted?.Invoke(actionText);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
