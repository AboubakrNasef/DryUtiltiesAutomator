using DUA_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels.Templates
{
    public class TemplateViewModel:ViewModelBase
    {

        #region Fields
        private string _name;
        private AlignmentTemplateModel _alignmentTemplateModel;

        private ProfileTemplateModel _profileTemplateModel;

        private ProfileViewTemplateModel _profileViewTemplateModel;

        private CorridorStyleTemplateModel _corridorStyleTemplateModel;





        #endregion
        #region Props
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public AlignmentTemplateModel AlignmentTemplate
        {
            get { return _alignmentTemplateModel; }
            set { _alignmentTemplateModel = value; OnPropertyChanged(nameof(AlignmentTemplate)); }
        }

        public ProfileTemplateModel ProfileTemplate
        {
            get { return _profileTemplateModel; }
            set { _profileTemplateModel = value; OnPropertyChanged(nameof(ProfileTemplate)); }
        }

        public ProfileViewTemplateModel ProfileViewTemplate
        {
            get { return _profileViewTemplateModel; }
            set { _profileViewTemplateModel = value; OnPropertyChanged(nameof(ProfileViewTemplate)); }
        }

        public CorridorStyleTemplateModel CorridorStyleTemplate
        {
            get { return _corridorStyleTemplateModel; }
            set { _corridorStyleTemplateModel = value; OnPropertyChanged(nameof(CorridorStyleTemplate)); }
        }

        #endregion

    }
}
