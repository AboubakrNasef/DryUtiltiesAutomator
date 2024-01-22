using DUA_WPF.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels
{
    public class DrawerListViewModel : ViewModelBase
    {
        #region Fields
        private List<String> drawerItems;
        private int selectedIndex;

        private readonly INavigator _navigator; 

        #endregion
        #region Properties
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                Navigate(selectedIndex);
                OnPropertyChanged(nameof(SelectedIndex));

            }
        }

        private void Navigate(int value)
        {
            if (value == 0)
            {
                _navigator.NavigateTo.Execute(CurrentState.Groups);
            }
            else
            {
                _navigator.NavigateTo.Execute(CurrentState.Templates);
            }
        }

        public List<String> DrawerItems
        {
            get { return drawerItems; }
            set { drawerItems = value; }
        }

        #endregion

        public DrawerListViewModel(INavigator navigator)
        {
            _navigator = navigator;
            selectedIndex = 0;
            DrawerItems = new List<String>() { "Groups", "Templates" };
        }



    }
}
