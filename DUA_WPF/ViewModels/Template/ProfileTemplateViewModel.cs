using Autodesk.Civil.DatabaseServices.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels.Templates
{
    public class ProfileTemplateViewModel:ViewModelBase
    {

        #region Fields
        private string _name;
        private string _prefix;
        private string _suffix;

        private ProfileStyle _profile_Style;



        #endregion
        #region Props
        public string Name
        {
            get { return _name; }
            set { _name = value;OnPropertyChanged(nameof(Name)); OnPropertyChanged(nameof(FullName)); }
        }
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; OnPropertyChanged(nameof(Prefix)); OnPropertyChanged(nameof(FullName)); }
        }
        public string Suffix
        {
            get { return _suffix; }
            set { _suffix = value; OnPropertyChanged(nameof(Suffix)); OnPropertyChanged(nameof(FullName)); }
        }
        public string FullName => _prefix + _name + _suffix;
        public ProfileStyle Profile_Style
        {
            get { return _profile_Style; }
            set { _profile_Style = value; OnPropertyChanged(nameof(Profile_Style)); }
        }

        #endregion
    }
}
