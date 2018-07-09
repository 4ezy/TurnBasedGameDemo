using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TurnBasedGameDemo
{
    /// <summary>
    /// Логика взаимодействия для GameFieldCell.xaml
    /// </summary>
    public partial class GameFieldCell : UserControl
    {
        private bool _isSelected;
        private BitmapImage _unitImage;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;

                if (_isSelected)
                    rect.Fill = new SolidColorBrush(SelectedColor);
                else
                    rect.Fill = new SolidColorBrush(DefaultColor);
            }
        }

        public Color SelectedColor { get; set; }
        public Color DefaultColor { get; set; }

        private int _currentUnitNumber;
        public int CurrentUnitNumber
        {
            get { return _currentUnitNumber; }
            set
            {
                _currentUnitNumber = value;
                pb.Value = _currentUnitNumber;
            }
        }

        private int _maxUnitNumber;
        public int MaxUnitNumber
        {
            get { return _maxUnitNumber; }
            set
            {
                _maxUnitNumber = value;
                pb.Maximum = _maxUnitNumber;
            }
        }

        private UnitStack _unitStack;
        public UnitStack UnitStack
        {
            get { return _unitStack; }
            set
            {
                _unitStack = value;

                if (_unitStack == null)
                {
                    pb.Value = 0;
                }
                else
                {
                    CurrentUnitNumber = _unitStack.UnitsCapacity * _unitStack.Units.FirstOrDefault().HitPoints;
                    MaxUnitNumber = _unitStack.UnitsCapacity * _unitStack.Units.FirstOrDefault().HitPoints;
                }
            }
        }
        public BitmapImage UnitImage {
            get { return _unitImage; }
            set
            {
                _unitImage = value;
                img.Source = _unitImage;
            }
        } 

        public GameFieldCell()
        {
            InitializeComponent();
            SelectedColor = Colors.Aqua;
            DefaultColor = Colors.LightBlue;
        }

        public void ClearCell()
        {
            UnitImage = null;
            UnitStack = null;
            CurrentUnitNumber = 0;
            MaxUnitNumber = 1;
        }
    }
}
