using CommunityToolkit.Mvvm.Input;
using DUA_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.Navigator
{
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }

        RelayCommand<CurrentState> NavigateTo { get; set; }
        event Action StateChanged;

    }
}
