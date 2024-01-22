using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.Models
{
    public class CorridorStyleTemplateModel
    {
        #region Fields
        private string _name;
        private string _prefix;
        private string _suffix;

        private CorridorStyle _cr_Style;
        private Assembly     _assembly;

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
        public string FullName => _prefix + _name + _suffix;
        public CorridorStyle Corridor_Style
        {
            get { return _cr_Style; }
            set { _cr_Style = value; }
        }

        #endregion

    }
}
