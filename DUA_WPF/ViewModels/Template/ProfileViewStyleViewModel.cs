using Autodesk.Civil.DatabaseServices.Styles;
using Autodesk.Civil.DatabaseServices;
using DUA_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUA_WPF.Services;

namespace DUA_WPF.ViewModels
{
    public class ProfileViewStyleViewModel:ViewModelBase
    {
        #region Fields
        private string _name;
        private string _prefix;
        private string _suffix;
        private readonly ICADService _cADService;
        private AlignmentTemplateViewModel _alignment;
        private ProfileViewStyle _profile_View_Style;
        private ProfileViewBandSetStyle _profile_View_Bands;

        private int _selectedProfileViewStyleIndex;

        public int SelectedProfileViewStyleIndex
        {
            get { return _selectedProfileViewStyleIndex; }
            set { _selectedProfileViewStyleIndex = value;
                SelectedProfileViewStyle = ProfileViewStyles[_selectedProfileViewStyleIndex];
                OnPropertyChanged(nameof(SelectedProfileViewStyleIndex));
            }
        }

        private int _selectedProfileViewBandSetsIndex;

        public int SelectedProfileViewBandSetsStyleIndex
        {
            get { return _selectedProfileViewBandSetsIndex; }
            set { _selectedProfileViewBandSetsIndex = value;
                SelectedProfileViewBands = ProfileViewBandSets[_selectedProfileViewBandSetsIndex];
                OnPropertyChanged(nameof(SelectedProfileViewBandSetsStyleIndex));
            }
        }


        #endregion
        #region Props
        public string Name
        {
            get { return _alignment.FullName; }

        }

        public List<ProfileViewStyle> ProfileViewStyles => _cADService.ProfileViewStyles;
        public List<ProfileViewBandSetStyle> ProfileViewBandSets => _cADService.ProfileViewBandSetStyles;
        public ProfileViewStyle SelectedProfileViewStyle
        {
            get { return _profile_View_Style; }
            set { _profile_View_Style = value; OnPropertyChanged(nameof(SelectedProfileViewStyle)); }
        }
        public ProfileViewBandSetStyle SelectedProfileViewBands
        {
            get { return _profile_View_Bands; }
            set { _profile_View_Bands = value; OnPropertyChanged(nameof(SelectedProfileViewBands)); }
        }




        public bool IsValid
        {
            get
            {
                if (SelectedProfileViewStyleIndex < 0)
                    return false;  
                if (SelectedProfileViewBandSetsStyleIndex < 0)
                    return false;
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Name))
                    return false;
                if (SelectedProfileViewBands == null)
                    return false;
                if (SelectedProfileViewBands == null)
                    return false;




                return true;
            }
        }
        #endregion

        public ProfileViewStyleViewModel(AlignmentTemplateViewModel alignment,ICADService cADService)
        {
                _cADService = cADService;
                _alignment = alignment;
        }
    }
}
