using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace EasyWPF.Demo
{
    public class DemoViewModel : ObservableObject
    {

        private object _test;
        private ObservableCollection<string> _items = new ObservableCollection<string>();


        public object Test
        {
            get => _test;
            set => Set(ref _test, value);
        }

        public ObservableCollection<string> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }
    
    }
}
