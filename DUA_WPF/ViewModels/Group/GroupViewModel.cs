using Autodesk.AutoCAD.DatabaseServices;
using DUA_WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels.Groups
{
    public class GroupViewModel:ViewModelBase
    {
        #region Fields
        private string _name;
        private LayerTableRecord _layer;
        private ObservableCollection<Polyline> _lines;

        private TemplateModel _template;

   


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
            set { _layer = value; OnPropertyChanged(nameof(Layer)); }
        }
        public ObservableCollection<Polyline> Polylines
        {
            get { return _lines; }
            set { _lines = value; OnPropertyChanged(nameof(Polylines)); }
        }

        public TemplateModel Template
        {
            get { return _template; }
            set { _template = value;OnPropertyChanged(nameof(Template)); }
        }
        #endregion
    }
}
