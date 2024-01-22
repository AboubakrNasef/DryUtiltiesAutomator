using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.Models
{
    public class TemplateModel
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
            set { _name = value; }
        }

        public AlignmentTemplateModel AlignmentTemplate
        {
            get { return _alignmentTemplateModel; }
            set { _alignmentTemplateModel = value; }
        }

        public ProfileTemplateModel ProfileTemplate
        {
            get { return _profileTemplateModel; }
            set { _profileTemplateModel = value; }
        }

        public ProfileViewTemplateModel ProfileViewTemplate
        {
            get { return _profileViewTemplateModel; }
            set { _profileViewTemplateModel = value; }
        }

        public CorridorStyleTemplateModel corridorStyleTemplate
        {
            get { return _corridorStyleTemplateModel; }
            set { _corridorStyleTemplateModel = value; }
        }

        #endregion





    }
}
