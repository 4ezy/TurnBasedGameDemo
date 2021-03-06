﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TurnBasedGameDemo.ViewModels;
using TurnBasedGameDemo.Views;

namespace TurnBasedGameDemo
{
    public class GameField : Canvas
    {
        public int HorizCellsCount { get; }
        public int VertCellsCount { get; }
        public Player SelectedPlayer { get; set; }
        public List<GameFieldCell> GameFieldCells { get; set; }
        public GameFieldCell SelectedCell { get; set; }
        public GameFieldCell CellToAttack { get; set; }

        private readonly int _rectSize = 50;

        public GameField() { }

        public GameField(int horzCellsCount, int vertCellsCount, Player selectedPlayer)
        {
            HorizCellsCount = horzCellsCount;
            VertCellsCount = vertCellsCount;
            Width = _rectSize * horzCellsCount;
            Height = _rectSize * vertCellsCount;
            SelectedPlayer = selectedPlayer;
            GameFieldCells = new List<GameFieldCell>();
            GenerateField(horzCellsCount, vertCellsCount);
            CreateContextMenuForGameField();
        }

        public void GenerateField(int horzCellsCount, int vertCellsCount)
        {
            if (horzCellsCount > 0 && vertCellsCount > 0)
            {
                int vertOffset, horzOffset = 0;

                for (int i = 0; i < horzCellsCount; i++)
                {
                    vertOffset = 0;

                    for (int j = 0; j < vertCellsCount; j++)
                    {
                        var newCell = new GameFieldCell()
                        {
                            Width = _rectSize,
                            Height = _rectSize
                        };

                        newCell.PreviewMouseRightButtonUp += OnCellSelected;

                        GameFieldCells.Add(newCell);
                        Children.Add(newCell);
                        SetTop(newCell, vertOffset);
                        SetLeft(newCell, horzOffset);
                        vertOffset += _rectSize;
                    }

                    horzOffset += _rectSize;
                }
            }
        }

        public void OnCellSelected(object sender, MouseButtonEventArgs e)
        {
            var selCell = sender as GameFieldCell;
            SelectCell(selCell);
        }

        public void OnCellSelectedLeftClick(object sender, MouseButtonEventArgs e)
        {
            var selCell = sender as GameFieldCell;
            SelectCell(selCell);
        }

        public void OnCellForAttackSelected(object sender, MouseButtonEventArgs e)
        {
            var cellToAttack = sender as GameFieldCell;
            SelectCellToAction(cellToAttack);
        }

        public void SelectCell(GameFieldCell selCell)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var c in Children)
                {
                    var cell = c as GameFieldCell;
                    cell.IsSelected = false;
                }

                selCell.IsSelected = true;
                SelectedCell = selCell;
            });
        }

        public void SelectCellToAction(GameFieldCell cellToAttack)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var c in Children)
                {
                    var cell = c as GameFieldCell;

                    if (cell != SelectedCell)
                        cell.IsSelected = false;
                }
            });

            cellToAttack.IsSelected = true;
            CellToAttack = cellToAttack;
        }

        public void CreateContextMenuForGameField()
        {
            var contextMenu = new ContextMenu();
            var menuItemAdd = new MenuItem();
            var menuItemRemove = new MenuItem();
            contextMenu.Items.Add(menuItemAdd);
            contextMenu.Items.Add(menuItemRemove);
            menuItemAdd.Header = "Add";
            menuItemRemove.Header = "Remove";

            menuItemAdd.Click += AddUnitClick;
            menuItemRemove.Click += RemoveUnitClick;

            ContextMenu = contextMenu;
        }

        private void RemoveUnitClick(object sender, RoutedEventArgs e)
        {
            if (SelectedPlayer.UnitStacks.Contains(SelectedCell.UnitStack))
            {
                SelectedPlayer.UnitStacks.Remove(SelectedCell.UnitStack);
                SelectedCell.UnitStack = null;
                SelectedCell.UnitImage = null;
            }
        }

        private void AddUnitClick(object sender, RoutedEventArgs e)
        {
            if (SelectedCell.UnitStack != null)
            {
                if (SelectedPlayer.UnitStacks.Contains(SelectedCell.UnitStack))
                {
                    var addUnitWindow = new AddUnitWindow();

                    if (addUnitWindow.ShowDialog() == false)
                        return;

                    AddUnitFromWidnowContext(addUnitWindow);

                    var remUnit = SelectedCell.UnitStack;
                    SelectedPlayer.UnitStacks.Remove(remUnit);
                }
            }
            else
            {
                var addUnitWindow = new AddUnitWindow();

                if (addUnitWindow.ShowDialog() == false)
                    return;

                AddUnitFromWidnowContext(addUnitWindow);
            }
        }

        private void AddUnitFromWidnowContext(AddUnitWindow addUnitWindow)
        {
            var addUnitWindowViewModel =
                addUnitWindow.DataContext as AddUnitWindowViewModel;

            UnitType unitType = addUnitWindowViewModel.SelectedUnitType;
            int numberOfUnits = addUnitWindowViewModel.NumberOfUnits;

            switch (unitType)
            {
                case UnitType.Swordsman:
                    SelectedCell.UnitImage = SelectedPlayer.SwordsmanImage;
                    break;
                case UnitType.Archer:
                    SelectedCell.UnitImage = SelectedPlayer.ArcherImage;
                    break;
                case UnitType.Peasant:
                    SelectedCell.UnitImage = SelectedPlayer.PeasantImage;
                    break;
                default:
                    break;
            }

            SelectedPlayer.UnitStacks.Add(new UnitStack(unitType, numberOfUnits, SelectedCell));
            SelectedPlayer.UnitStacks[SelectedPlayer.UnitStacks.Count - 1].CellIndex = GameFieldCells.IndexOf(SelectedCell);
            SelectedCell.UnitStack = SelectedPlayer.UnitStacks.Last();
        }
    }
}
