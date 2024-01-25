using CommunityToolkit.Mvvm.Input;
using DUA_WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels
{
    public class GroupsViewModel : ViewModelBase
    {
        private readonly ICADService _CADservice;
        private readonly ITemplateService _templateService;
        private readonly IModalService _modalService;
        private ObservableCollection<GroupViewModel> _groups;


        public RelayCommand AddGroupCommand { get; }
        public ObservableCollection<GroupViewModel> Groups
		{
			get { return _groups; }
			set { _groups = value; }
		}
        public GroupsViewModel(ICADService cADservice, ITemplateService templateService, IModalService modalService)
        {
            _groups = new ObservableCollection<GroupViewModel>();
            _CADservice = cADservice;
            _templateService = templateService;
            AddGroupCommand = new RelayCommand(_addGroup);
            _modalService = modalService;
        }

        private void _addGroup()
        {
            var groupVM = new GroupViewModel(_CADservice, _templateService, _modalService, _groups);

            _modalService.OpenModal(groupVM);
        }
    }
}
