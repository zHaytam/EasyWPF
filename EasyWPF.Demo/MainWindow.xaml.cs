using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;

namespace EasyWPF.Demo
{
    public partial class MainWindow
    {

        private readonly DemoViewModel _demoViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _demoViewModel = new DemoViewModel();
            DataContext = _demoViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_demoViewModel.Test == null)
            {
                _demoViewModel.Test = 200;
            }
            else
            {
                _demoViewModel.Test = null;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _demoViewModel.Items.Add("Added!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (_demoViewModel.Items.Count == 0)
                return;

            _demoViewModel.Items.RemoveAt(0);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            _demoViewModel.Items = new ObservableCollection<string>();
        }
    }
}
