using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DUA_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.Navigator
{
    public class Navigator :ObservableObject, INavigator
    {

        #region Fields
        TemplatesViewModel _templatesViewModel;
        GroupsViewModel _groupsViewModel;


        #endregion
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

       public RelayCommand<CurrentState> NavigateTo { get; set ; }

        public event Action StateChanged;
      
      public Navigator(TemplatesViewModel templatesViewModel, GroupsViewModel groupsViewModel)
        {
            NavigateTo = new RelayCommand<CurrentState>(navigateTo);

            _templatesViewModel = templatesViewModel;
            _groupsViewModel = groupsViewModel;
            CurrentViewModel = groupsViewModel;
        }

        private void navigateTo(CurrentState state)
        {
            switch (state)
            {
                case CurrentState.Groups:
                    CurrentViewModel= _groupsViewModel;
                    break;
                case CurrentState.Templates:
                    CurrentViewModel= _templatesViewModel;
                    break;
                default:
                    break;
            }
            StateChanged();
        }
    }
}
