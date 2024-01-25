using CommunityToolkit.Mvvm.Input;
using DUA_WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DUA_WPF.ViewModels
{
    public class TemplatesViewModel:ViewModelBase
    {
        private readonly ITemplateService _templateService;
        private readonly IModalService _modalService;
        private readonly ICADService _cADService;
   
        public RelayCommand AddTemplateCommand { get; }
        public RelayCommand DeleteTemplateCommand { get; }

        private int _selectedTemplateIndex;

        public int SelectedTemplateIndex
        {
            get { return _selectedTemplateIndex; }
            set { _selectedTemplateIndex = value; OnPropertyChanged(nameof(SelectedTemplateIndex)); }
        }


        public ObservableCollection<TemplateViewModel> Templates=>_templateService.Templates;

        public TemplatesViewModel(ITemplateService templateService,ICADService cADService,IModalService modalService)
        {
            _templateService = templateService;
            _modalService= modalService;
            _cADService = cADService;

            AddTemplateCommand = new RelayCommand(_addTemplate);
            DeleteTemplateCommand = new RelayCommand(_deleteTemplate,_candelete);



        }

        private bool _candelete()
        {
            if (Templates==null || Templates.Count<0||Templates.Count< SelectedTemplateIndex)
            {
                return false;
            }

            return true;
        }

        private void _deleteTemplate()
        {
            

            var template = Templates[SelectedTemplateIndex];

            if (template != null)
            {
                Templates.Remove(template);
            }
        }

        private void _addTemplate()
        {
            var templateVM = new TemplateViewModel(_cADService,_templateService, _modalService);
            _modalService.OpenModal(templateVM);
           
        }
    }
}
