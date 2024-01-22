using Autodesk.Civil.DatabaseServices.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels.Templates
{
    public class AlignmentTemplateViewModel:ViewModelBase
    {
        #region Fields
        private string _name;
        private string _prefix;
        private string _suffix;

        private AlignmentStyle _al_Style;



        #endregion
        #region Props
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); OnPropertyChanged(nameof(FullName)); }
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
        public AlignmentStyle Alignment_Style
        {
            get { return _al_Style; }
            set { _al_Style = value; OnPropertyChanged(nameof(Alignment_Style)); }
        }

        #endregion

    }
}
