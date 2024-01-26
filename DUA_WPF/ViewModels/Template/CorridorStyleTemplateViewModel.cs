using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
using DUA_WPF.Services;
using System;
using System.Collections.Generic;



namespace DUA_WPF.ViewModels
{
    public class CorridorStyleTemplateViewModel:ViewModelBase
    {
        private readonly ICADService _cADService;
        #region Fields
        private string _name;
        private string _prefix;
        private string _suffix;

        private CorridorStyle _cr_Style;
        private Assembly _assembly;

        private int _selectedAssemblyIndex;

       
        public event Action<bool> ValidationChanged;
      
        private int _selectedCorridorStyleIndex;

      

        #endregion
        #region Props
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); OnPropertyChanged(nameof(FullName)); OnPropertyChanged(nameof(IsValid)); }
        }
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; OnPropertyChanged(nameof(Prefix)); OnPropertyChanged(nameof(FullName)); }
        }
        public string Suffix
        {
            get { return _suffix; }
            set { _suffix = value; OnPropertyChanged(nameof(Suffix)); OnPropertyChanged(nameof(FullName)); }
        }
        public string FullName => _prefix + _name + _suffix;
        public CorridorStyle SelectedCorridor_Style
        {
            get { return _cr_Style; }
            set { _cr_Style = value; OnPropertyChanged(nameof(SelectedCorridor_Style)); }
        }
        public Assembly SelectedAssembly
        {
            get { return _assembly; }
            set { _assembly = value;
                OnPropertyChanged(nameof(SelectedAssembly));
            
            }
        }
        public int SelectedCorridorStyleIndex
        {
            get { return _selectedCorridorStyleIndex; }
            set
            {
               
                    _selectedCorridorStyleIndex = value;

                    SelectedCorridor_Style = CorridorStyles[_selectedCorridorStyleIndex];
                    OnPropertyChanged(nameof(SelectedCorridorStyleIndex));
                    OnPropertyChanged(nameof(IsValid));
                    ValidationChanged?.Invoke(IsValid);
               
            }
        }
        public int SelectedAssemblyIndex
        {
            get { return _selectedAssemblyIndex; }
            set
            {
                _selectedAssemblyIndex = value;
               
                SelectedAssembly = Assemblies[_selectedAssemblyIndex];
                OnPropertyChanged(nameof(SelectedAssemblyIndex));
                OnPropertyChanged(nameof(IsValid));
                ValidationChanged?.Invoke(IsValid);
            }
        }
        public List<CorridorStyle> CorridorStyles => _cADService.CorridorStyles;
        public List<Assembly> Assemblies => _cADService.Assemblies;



        public bool IsValid
        {
            get
            {
                if (SelectedCorridorStyleIndex < 0)
                    return false;
                if (SelectedAssemblyIndex < 0)
                    return false;
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Name))
                    return false;
                if (SelectedCorridor_Style == null)
                    return false;
                if (SelectedAssembly == null)
                    return false;




                return true;
            }
        }

        #endregion
        public CorridorStyleTemplateViewModel(ICADService cADService)
        {
            _cADService = cADService;
            Name = "Corridor";


            if (CorridorStyles.Count > 0)
            {
                SelectedCorridorStyleIndex = 0;
            }
            if (Assemblies.Count > 0)
            {
                SelectedAssemblyIndex = 0;
            }
        }
    }
}
