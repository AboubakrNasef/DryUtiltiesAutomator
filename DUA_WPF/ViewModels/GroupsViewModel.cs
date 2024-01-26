using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
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
        private int _selectedSurfaceIndex;
        private int _selectedGroupIndex;
        private bool _isEnabled;

        public bool CanDeleteEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;


                OnPropertyChanged(nameof(CanDeleteEnabled));
            }
        }


        public bool CanAddEnabled
        {
            get { return _templateService.Templates.Any(); }

        }

        public int SelectedSurfaceIndex
        {
            get { return _selectedSurfaceIndex; }
            set
            {
                _selectedSurfaceIndex = value;
                OnPropertyChanged(nameof(SelectedSurfaceIndex));
            }
        }
        public int SelectedGroupIndex
        {
            get { return _selectedGroupIndex; }
            set
            {
                _selectedGroupIndex = value;
                CanDeleteEnabled = _candelete();
                OnPropertyChanged(nameof(SelectedGroupIndex));
            }
        }

        public RelayCommand AddGroupCommand { get; }
        public RelayCommand DeleteGroupCommand { get; }
        public RelayCommand ApplyCommand { get; }
        public List<TinSurface> Surfaces => _CADservice.TinSurfaces;
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
            _modalService = modalService;
            _templateService.Templates.CollectionChanged += Templates_CollectionChanged;
            AddGroupCommand = new RelayCommand(_addGroup);
            DeleteGroupCommand = new RelayCommand(_deleteGroup);
            ApplyCommand = new RelayCommand(_applyAllGroups);
            CanDeleteEnabled = _candelete();
        }

        private void _applyAllGroups()
        {
            var point = _CADservice.GetPoint();
            foreach (var group in _groups)
            {
                _applyForGroup(group,ref point);
                Groups.Remove(group);
            }
        }

        private void _applyForGroup(GroupViewModel group,ref Point3d point)
        {
            int count = 1;
            foreach (var polyline in group.Polylines)
            {
                var align = CreateAlignment(group, count, polyline);

                var surfaceProfile = CreateSurfaceProfile(align);
                var OffsetProfile = CreateOffsetProfile(group, align, surfaceProfile);
                var profileView = CreateProfileView(group, point, align);
              
                point = new Point3d(point.X + profileView.StationEnd+50, point.Y, point.Z);
              var corridor =   CreateCorridor(group, align, OffsetProfile);

                count++;
            }
        }

        private Corridor CreateCorridor(GroupViewModel group, Alignment align, Profile OffsetProfile)
        {
            var assembly = group.SelectedTemplate.CorridorStyleTemplate.SelectedAssembly;
          return  _CADservice.CreateCorridor(align, OffsetProfile, assembly);
        }

        private ProfileView CreateProfileView(GroupViewModel group, Point3d point, Alignment align)
        {
            var profileViewBandSetStye = group.SelectedTemplate.ProfileViewTemplate.SelectedProfileViewBands;
            var profileViewStyle = group.SelectedTemplate.ProfileViewTemplate.SelectedProfileViewStyle;
           return _CADservice.CreateProfileView(align, point, profileViewBandSetStye, profileViewStyle);
        }

        private Profile CreateOffsetProfile(GroupViewModel group, Alignment align, Profile surfaceProfile)
        {
            var offset = group.SelectedTemplate.ProfileTemplate.Offset;
            var profileName = group.SelectedTemplate.ProfileTemplate.FullName;
            var profileStyle = group.SelectedTemplate.ProfileTemplate.SelectedProfileStyle;
            var profileLabeset = _CADservice.ProfileLabelSetStyles.FirstOrDefault();
            return _CADservice.CreateOffsetProfile(align, surfaceProfile, profileName, offset, profileStyle, profileLabeset);
        }

        private Profile CreateSurfaceProfile(Alignment align)
        {
            var surfaces = Surfaces[SelectedSurfaceIndex];
            var profileStyle = _CADservice.ProfileStyles.FirstOrDefault(s => s.Name == "Existing Ground Profile");
            var profileLabeset = _CADservice.ProfileLabelSetStyles.FirstOrDefault();
            return _CADservice.CreateSurfaceProfile(align, surfaces, profileStyle, profileLabeset);
        }

        private Alignment CreateAlignment(GroupViewModel group, int count, Polyline polyline)
        {
            var alignName = group.SelectedTemplate.AlignmentTemplate.FullName;
            var alignStyle = group.SelectedTemplate.AlignmentTemplate.SelectedAlignmentStyle;
            var alignLabels = group.SelectedTemplate.AlignmentTemplate.SelectedAlignmentLabelSetStyle;
            if (alignName == null)
            {
                throw new NullReferenceException("Alignment Name cannot be null");
            }
            if (alignStyle == null)
            {
                throw new NullReferenceException("Alignment Style cannot be null");
            }
            if (alignLabels == null)
            {
                throw new NullReferenceException("Alignment label set cannot be null");
            }
            return _CADservice.CreateAlignmentFromPolyLine(polyline, alignName + "-" + count.ToString(), alignStyle, alignLabels);
        }

        private void Templates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CanAddEnabled));
        }

        private bool _candelete()
        {

            if (Groups == null || Groups.Count < 1 || Groups.Count < SelectedGroupIndex)
            {


                return false;
            }

            return true;
        }
        private void _deleteGroup()
        {
            var group = Groups[SelectedGroupIndex];

            if (group != null)
            {
                Groups.Remove(group);
            }
        }

        private void _addGroup()
        {
            var groupVM = new GroupViewModel(_CADservice, _templateService, _modalService, _groups);

            _modalService.OpenModal(groupVM);
        }
    }
}
