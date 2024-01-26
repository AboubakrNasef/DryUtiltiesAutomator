using CommunityToolkit.Mvvm.Input;
using DUA_WPF.Models;
using DUA_WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels
{
    public class TemplateViewModel : ViewModelBase
    {

        #region Fields
        private readonly ICADService _cADService;
        private readonly ITemplateService _templateService;
        private readonly IModalService _modalService;
        private string _name;
        private AlignmentTemplateViewModel _alignmentTemplateModel;

        private ProfileTemplateViewModel _profileTemplateModel;

        private ProfileViewStyleViewModel _profileViewTemplateModel;

        private CorridorStyleTemplateViewModel _corridorStyleTemplateModel;






        #endregion

        #region Commands
        private bool _isEnabled;

        public bool CanApplyEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value;

            OnPropertyChanged(nameof(CanApplyEnabled));
            
            }
        }

        public RelayCommand ButtonModeCommand { get; private set; }
        public RelayCommand EditCommand { get; }
        
        #endregion

        #region Props
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _buttonmodeContent;

        public string ButtonModeContent
        {
            get { return _buttonmodeContent; }
            set { _buttonmodeContent = value;OnPropertyChanged(nameof(ButtonModeContent)); }
        }

        public AlignmentTemplateViewModel AlignmentTemplate
        {
            get { return _alignmentTemplateModel; }
            set { _alignmentTemplateModel = value; OnPropertyChanged(nameof(AlignmentTemplate)); }
        }

        public ProfileTemplateViewModel ProfileTemplate
        {
            get { return _profileTemplateModel; }
            set { _profileTemplateModel = value; OnPropertyChanged(nameof(ProfileTemplate)); }
        }

        public ProfileViewStyleViewModel ProfileViewTemplate
        {
            get { return _profileViewTemplateModel; }
            set { _profileViewTemplateModel = value; OnPropertyChanged(nameof(ProfileViewTemplate)); }
        }

        public CorridorStyleTemplateViewModel CorridorStyleTemplate
        {
            get { return _corridorStyleTemplateModel; }
            set { _corridorStyleTemplateModel = value; OnPropertyChanged(nameof(CorridorStyleTemplate)); }
        }

        #endregion

        public TemplateViewModel(ICADService cADService, ITemplateService templateService, IModalService modalService)
        {
            _name = "Template";
            _alignmentTemplateModel = new AlignmentTemplateViewModel(cADService);
            _profileTemplateModel = new ProfileTemplateViewModel(cADService);

            _profileViewTemplateModel = new ProfileViewStyleViewModel(_alignmentTemplateModel, cADService);
            _corridorStyleTemplateModel = new CorridorStyleTemplateViewModel(cADService);
            _templateService = templateService;
            _cADService = cADService;
            _modalService = modalService;
            ButtonModeCommand = new RelayCommand(_addTemplate,_canApply);
            ButtonModeContent = "Add";
            EditCommand = new RelayCommand(_editTemplateCommand);


            _alignmentTemplateModel.ValidationChanged += ValidationChanged;
            _profileTemplateModel.ValidationChanged += ValidationChanged;
            _profileViewTemplateModel.ValidationChanged += ValidationChanged;
            _corridorStyleTemplateModel.ValidationChanged += ValidationChanged;

            CanApplyEnabled = _canApply();
        }

        private void ValidationChanged(bool obj)
        {
          
                CanApplyEnabled = _canApply();
            

        
           
        }

        private bool _canApply()
        {

           
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Name))
            {
                return false;
            }

            if (!AlignmentTemplate.IsValid || !ProfileTemplate.IsValid || !ProfileViewTemplate.IsValid || !CorridorStyleTemplate.IsValid)
            {
          
                return false;
            }

            return true;
        }

        private void _editTemplateCommand()
        {
            ButtonModeCommand = new RelayCommand(_applyEditTemplate, _canApply);
            ButtonModeContent = "Apply Edit";
            _modalService.OpenModal(this);

        }
        private void _applyEditTemplate()
        {
        
            _modalService.CloseModal();

        }
        private void _addTemplate()
        {
            _templateService.Templates.Add(this);
            _modalService.CloseModal();

        }

       
    }
}
