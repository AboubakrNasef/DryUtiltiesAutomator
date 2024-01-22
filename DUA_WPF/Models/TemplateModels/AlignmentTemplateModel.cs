using Autodesk.Civil.DatabaseServices.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.Models
{
    public class AlignmentTemplateModel
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
            set { _name = value; }
        } 
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }
        public string Suffix
        {
            get { return _suffix; }
            set { _suffix = value; }
        }
        public  string FullName => _prefix+_name+_suffix;
        public AlignmentStyle Alignment_Style
        {
            get { return _al_Style; }
            set { _al_Style = value; }
        }

        #endregion

     
    }
}
