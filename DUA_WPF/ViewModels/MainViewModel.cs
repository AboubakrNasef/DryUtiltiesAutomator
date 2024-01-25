using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DUA_WPF.Navigator;
using DUA_WPF.Services;
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
        private IModalService _modal;
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

        public RelayCommand CloseModalCommand { get; set; }

        public bool IsModalOpen => _modal.IsModalOpen;



        public ViewModelBase ModalContent => _modal.ModalContent;

        public MainViewModel(INavigator navigator,IModalService modal)
        {
            DrawerListViewModel = new DrawerListViewModel(navigator);
            _navigator = navigator;
            _navigator.StateChanged += _navigator_StateChanged;

            _modal = modal;
            _modal.ModalStateChanged += _modal_ModalStateChanged;
            CloseModalCommand = new RelayCommand(CloseModal);
        }

        private void CloseModal()
        {
            _modal.CloseModal();
        }

        private void _modal_ModalStateChanged()
        {
            OnPropertyChanged(nameof(ModalContent));
            OnPropertyChanged(nameof(IsModalOpen));
        }

        private void _navigator_StateChanged()
        {
           OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
