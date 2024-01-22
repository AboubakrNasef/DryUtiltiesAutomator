using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.Models
{
    public class GroupModel
    {
        #region Fields
        private string _name;
        private LayerTableRecord _layer;
        private List<Polyline> _lines;

        private TemplateModel _template;

        public TemplateModel Template
        {
            get { return _template; }
            set { _template = value; }
        }


        #endregion
        #region Props
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public LayerTableRecord Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }
        public List<Polyline> Polylines
        {
            get { return _lines; }
            set { _lines = value; }
        }
        #endregion




    }
}
