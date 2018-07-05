using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TurnBasedGameDemo.ViewModels;
using TurnBasedGameDemo.Views;

namespace TurnBasedGameDemo
{
    public class GameField : Canvas
    {
        public int HorzCellsCount { get; set; }
        public int VertCellsCount { get; set; }
        public List<UnitStack> PlayerOneUnits { get; set; }
        public List<UnitStack> PlayerTwoUnits { get; set; }
        private readonly int _rectSize = 50;

        public GameField() { }

        public GameField(int horzCellsCount, int vertCellsCount)
        {
            HorzCellsCount = horzCellsCount;
            VertCellsCount = vertCellsCount;
            Width = _rectSize * horzCellsCount;
            Height = _rectSize * vertCellsCount;
            PlayerOneUnits = new List<UnitStack>();
            PlayerTwoUnits = new List<UnitStack>();
            GenerateField();
        }

        public void GenerateField()
        {
            if (HorzCellsCount > 0 && VertCellsCount > 0)
            {
                var contextMenu = new ContextMenu();
                var menuItemAdd = new MenuItem();
                var menuItemRemove = new MenuItem();
                contextMenu.Items.Add(menuItemAdd);
                contextMenu.Items.Add(menuItemRemove);
                menuItemAdd.Header = "Add";
                menuItemRemove.Header = "Remove";

                menuItemAdd.Click += ((s, e) =>
                {
                    AddUnitWindow addUnitWindow = new AddUnitWindow();

                    if (addUnitWindow.ShowDialog() == false)
                        return;

                    AddUnitWindowViewModel addUnitWindowViewModel = 
                        addUnitWindow.DataContext as AddUnitWindowViewModel;


                });

                int vertOffset, horzOffset = 0;

                for (int i = 0; i < HorzCellsCount; i++)
                {
                    vertOffset = 0;

                    for (int j = 0; j < VertCellsCount; j++)
                    {
                        var newCell = new GameFieldCell()
                        {
                            Width = _rectSize,
                            Height = _rectSize
                        };

                        //BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/swordsman.png"));
                        //cell.img.Source = bitmapImage;

                        newCell.PreviewMouseLeftButtonUp += ((s, e) =>
                        {
                            var curField = s as GameFieldCell;
                            bool cellMode = !curField.IsSelected;

                            foreach (var c in Children)
                            {
                                var cell = c as GameFieldCell;
                                cell.IsSelected = false;
                            }

                            curField.IsSelected = cellMode;
                        });

                        newCell.ContextMenu = contextMenu;
                        Children.Add(newCell);
                        SetTop(newCell, vertOffset);
                        SetLeft(newCell, horzOffset);
                        vertOffset += _rectSize;
                    }

                    horzOffset += _rectSize;
                }
            }
        }

        public void AddUnitsStack(int unitsCount, UnitType unitType)
        {

        }
    }
}
