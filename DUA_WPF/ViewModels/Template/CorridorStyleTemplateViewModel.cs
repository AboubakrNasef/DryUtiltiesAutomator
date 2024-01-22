using Autodesk.Civil.DatabaseServices.Styles;

using System.Reflection;



namespace DUA_WPF.ViewModels.Templates
{
    public class CorridorStyleTemplateViewModel:ViewModelBase
    {
        #region Fields
        private string _name;
        private string _prefix;
        private string _suffix;

        private CorridorStyle _cr_Style;
        private Assembly _assembly;

        public Assembly Assembly
        {
            get { return _assembly; }
            set { _assembly = value; }
        }



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
        public CorridorStyle Corridor_Style
        {
            get { return _cr_Style; }
            set { _cr_Style = value; OnPropertyChanged(nameof(Corridor_Style)); }
        }

        #endregion

    }
}
