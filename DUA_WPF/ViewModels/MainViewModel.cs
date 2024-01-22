using CommunityToolkit.Mvvm.ComponentModel;
using DUA_WPF.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        public ViewModelBase CurrentViewModel { get { return _navigator.CurrentViewModel; } }

        private INavigator   _navigator;

        public INavigator Navigator
        {
            get { return _navigator; }
            set { _navigator = value; }
        }

        private DrawerListViewModel _DrawerListViewModel;

        public DrawerListViewModel DrawerListViewModel
        {
            get { return _DrawerListViewModel; }
            set { _DrawerListViewModel = value; OnPropertyChanged(nameof(DrawerListViewModel)); }
        }

        

        public MainViewModel(INavigator navigator)
        {
            DrawerListViewModel = new DrawerListViewModel(navigator);
            _navigator = navigator;
            _navigator.StateChanged += _navigator_StateChanged;
        }

        private void _navigator_StateChanged()
        {
           OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
