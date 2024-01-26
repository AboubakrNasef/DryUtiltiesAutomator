using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;

    public abstract class ViewModelBase : ObservableObject
    {
 
    }

    
}
