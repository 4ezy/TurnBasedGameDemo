using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TurnBasedGameDemo.ViewModels;

namespace TurnBasedGameDemo.Views
{
    /// <summary>
    /// Логика взаимодействия для AddUnitWindow.xaml
    /// </summary>
    public partial class AddUnitWindow : Window
    {
        public AddUnitWindow()
        {
            InitializeComponent();
            DataContext = new AddUnitWindowViewModel();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
