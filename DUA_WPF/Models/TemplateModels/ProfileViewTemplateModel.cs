using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.Models
{
    public class ProfileViewTemplateModel
    {
        #region Fields
        private string _name;
        private string _prefix;
        private string _suffix;
        private AlignmentTemplateModel _alignment;
        private ProfileViewStyle _profile_View_Style;
        private ProfileViewBandSet _profile_View_Bands;



        #endregion
        #region Props
        public string Name
        {
            get { return _alignment.FullName; }
            
        }
      

        public ProfileViewStyle Profile_View_Style
        {
            get { return _profile_View_Style; }
            set { _profile_View_Style = value; }
        }

        #endregion
    }
}
