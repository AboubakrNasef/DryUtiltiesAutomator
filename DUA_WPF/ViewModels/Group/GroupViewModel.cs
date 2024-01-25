using Autodesk.AutoCAD.DatabaseServices;
using CommunityToolkit.Mvvm.Input;
using DUA_WPF.Models;
using DUA_WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels
{
    public class GroupViewModel : ViewModelBase
    {
        #region Fields
        private string _name;
        private LayerTableRecord _layer;
        private ObservableCollection<Polyline> _lines;

        private TemplateViewModel _template;

        private readonly ICADService _CADService;
        private readonly ITemplateService _templateService;
        private readonly IModalService _modalService;
        private int _selectedLayerIndex;
        private int _selectedTemplateIndex;
        private ObservableCollection<GroupViewModel> _groups;

        #endregion
        #region Props
        public string Name
        {
            get { return _name; }
            set { _name = value;OnPropertyChanged(nameof(Name)); }
        }
        public List<LayerTableRecord> Layers => _CADService.Layers;
        public LayerTableRecord SelectedLayer
        {
            get { return _layer; }
            set { _layer = value; OnPropertyChanged(nameof(SelectedLayer)); }
        }
        public ObservableCollection<Polyline> Polylines
        {
            get { return _lines; }
            set { _lines = value; OnPropertyChanged(nameof(Polylines)); }
        }
        public ObservableCollection<TemplateViewModel> Templates=> _templateService.Templates;
        public TemplateViewModel SelectedTemplate
        {
            get { return _template; }
            set { _template = value; OnPropertyChanged(nameof(SelectedTemplate)); }
        }
        public int SelectedLayerIndex
        {
            get { return _selectedLayerIndex; }
            set
            {
                _selectedLayerIndex = value;
                SelectedLayer = Layers[_selectedLayerIndex];
                OnPropertyChanged(nameof(SelectedLayerIndex));
                OnPropertyChanged(nameof(SelectedLayer));
            }
        }
        public int SelectedTemplateIndex
        {
            get { return _selectedTemplateIndex; }
            set
            {
                _selectedTemplateIndex = value;
                SelectedTemplate = Templates[_selectedTemplateIndex];
                OnPropertyChanged(nameof(SelectedTemplateIndex));
                OnPropertyChanged(nameof(SelectedTemplate));
            }
        }
        #endregion
        #region Commands
        public RelayCommand AddGroup { get;  }
        #endregion

        public GroupViewModel(ICADService cADService,ITemplateService templateService,IModalService modalService,ObservableCollection<GroupViewModel> groups)
        {
            Name = "Group";
            _templateService = templateService;
            _modalService = modalService;
            _CADService = cADService;
            _groups= groups;
            AddGroup = new RelayCommand(_addGroup);
        }

        private void _addGroup()
        {
            _groups.Add(this);
            _modalService.CloseModal();


        }
    }
}
