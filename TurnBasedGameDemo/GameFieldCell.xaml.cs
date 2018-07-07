﻿using System;
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

        public UnitStack UnitStack { get; set; }
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
    }
}
