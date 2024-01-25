using Autodesk.Civil.DatabaseServices.Styles;
using DUA_WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels
{
    public class ProfileTemplateViewModel:ViewModelBase
    {

        #region Fields
        private string _name;
        private string _prefix;
        private string _suffix;
        private double _offset;

    
        private ProfileStyle _profile_Style;
        private readonly ICADService _cADService;

        private int _selectedProfileStyleId;

        public int SelectedProfileStyleIndex
        {
            get { return _selectedProfileStyleId; }
            set { _selectedProfileStyleId = value; OnPropertyChanged(nameof(_selectedProfileStyleId));
                SelectedProfileStyle = ProfileStyles[_selectedProfileStyleId];
            }
        }


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

        public double Offset
        {
            get { return _offset; }
            set { _offset = value; OnPropertyChanged(nameof(Offset)); }
        }

        public ProfileStyle SelectedProfileStyle
        {
            get { return _profile_Style; }
            set { _profile_Style = value; OnPropertyChanged(nameof(SelectedProfileStyle)); }
        }
        public List<ProfileStyle> ProfileStyles => _cADService.ProfileStyles;



        public bool IsValid
        {
            get
            {
                if (SelectedProfileStyleIndex < 0)
                    return false;
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Name))
                    return false;
                if (SelectedProfileStyle == null)
                    return false;




                return true;
            }
        }
        #endregion

        public ProfileTemplateViewModel(ICADService cADService)
        {
            Name = "Profile";
            _cADService = cADService;
            Offset = 0.6;
        }
    }
}
