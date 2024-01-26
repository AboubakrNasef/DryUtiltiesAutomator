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

        #region Commands
        private bool _canApplyEnabled;

        public bool CanApplyEnabled
        {
            get { return _canApplyEnabled; }
            set
            {
                _canApplyEnabled = value;

                OnPropertyChanged(nameof(CanApplyEnabled));

            }
        }

        public RelayCommand ButtonModeCommand { get; private set; }
        public RelayCommand EditCommand { get; }
        public RelayCommand SelectLinesCommand { get; }
        public RelayCommand ClearLayerCommand { get; }
        #endregion
        #region Props
        private string _buttonmodeContent;
        private List<Polyline> polyLineFromLayer;
        private List<Polyline> polyLinesFromSelection;

        private List<string> _layers;
        public string ButtonModeContent
        {
            get { return _buttonmodeContent; }
            set { _buttonmodeContent = value; OnPropertyChanged(nameof(ButtonModeContent)); }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value;OnPropertyChanged(nameof(Name)); }
        }
        public List<string> Layers => _layers;
        private List<LayerTableRecord> _CadLayers => _CADService.Layers;
        public LayerTableRecord SelectedLayer
        {
            get { return _layer; }
            set { _layer = value;

                GetPolylinesFromLayer();
                CanApplyEnabled = _canApply();
                OnPropertyChanged(nameof(SelectedLayer)); }
        }

        private void GetPolylinesFromLayer()
        {
            if (SelectedLayer ==null)
            {
          polyLineFromLayer.Clear();

            }
            else
            {

            polyLineFromLayer =  _CADService.GetPolylinesFromLayer(SelectedLayer);
            }
            OnPropertyChanged(nameof(Polylines));
        }

        public List<Polyline> Polylines
        {
            get { return new List<Polyline>(polyLineFromLayer).Union(polyLinesFromSelection).ToList(); }
           
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
                if (_selectedLayerIndex ==0)
                {
                SelectedLayer = null;

                }
                else
                {

                SelectedLayer = _CadLayers[_selectedLayerIndex-1];
                }
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
       

        public GroupViewModel(ICADService cADService,ITemplateService templateService,IModalService modalService,ObservableCollection<GroupViewModel> groups)
        {
            Name = "Group";
            _templateService = templateService;
            _modalService = modalService;
            _CADService = cADService;
            _groups= groups;
          
            ButtonModeCommand = new RelayCommand(_addGroup);
            ButtonModeContent = "Add";
            EditCommand = new RelayCommand(_editGroupCommand);
            CanApplyEnabled = _canApply();
            SelectLinesCommand = new RelayCommand(_selectLines);
         
            polyLineFromLayer = new List<Polyline>();
            polyLinesFromSelection = new List<Polyline>();
            SelectedTemplateIndex = 0;
            SelectedLayerIndex = 0;
         _layers=   new List<string>().Append("--None--").Concat(_CadLayers.Select(layer => layer.Name)).ToList();
        }

     

        private void _selectLines()
        {
            polyLinesFromSelection = _CADService.GetSelectedPolylines();
            CanApplyEnabled = _canApply();
            OnPropertyChanged(nameof(Polylines));
        }

        private void _editGroupCommand()
        {
            ButtonModeCommand = new RelayCommand(_applyEditTemplate);
            
              ButtonModeContent = "Apply Edit";
            _modalService.OpenModal(this);
        }

        private void _addGroup()
        {
            _groups.Add(this);
            _modalService.CloseModal();


        }
        private void _applyEditTemplate()
        {

            _modalService.CloseModal();

        }
        private bool _canApply()
        {


            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Name))
            {
                return false;
            }

            if (SelectedTemplate ==null|| Polylines.Count<1)
            {

                return false;
            }

            return true;
        }

    }
}
