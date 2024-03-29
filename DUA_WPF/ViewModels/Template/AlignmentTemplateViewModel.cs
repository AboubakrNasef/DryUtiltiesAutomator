﻿using Autodesk.Civil.DatabaseServices.Styles;
using DUA_WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.ViewModels
{
    public class AlignmentTemplateViewModel:ViewModelBase
    {
        #region Fields
        private string _name;
        private string _prefix;
        private string _suffix;

        private AlignmentStyle _al_Style;
        private AlignmentLabelSetStyle _al_labelSet_Style;
        private readonly ICADService _cADService;
        public event Action<bool> ValidationChanged;

        #endregion
        #region Props
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); OnPropertyChanged(nameof(FullName)); OnPropertyChanged(nameof(IsValid)); ValidationChanged?.Invoke(IsValid); }
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
        public AlignmentStyle SelectedAlignmentStyle
        {
            get { return _al_Style; }
            set { _al_Style = value; OnPropertyChanged(nameof(SelectedAlignmentStyle)); }
        }
      

        public AlignmentLabelSetStyle SelectedAlignmentLabelSetStyle
        {
            get { return _al_labelSet_Style; }
            
            set { _al_labelSet_Style = value; OnPropertyChanged(nameof(SelectedAlignmentLabelSetStyle)); }
        }
        private int _selectedAlignmentStyleIndex;
        private int _selectedAlignmentLabelSetIndex;
        public List<AlignmentStyle> AlignmentStyles =>_cADService.AlignmentStyles;
        public List<AlignmentLabelSetStyle> AlignmentLabelSets =>_cADService.AlignmentLabelSetItems;

        
        public int SelectedAlignmentLabelSetIndex
        {
            get { return _selectedAlignmentLabelSetIndex; }
            set
            {
                _selectedAlignmentLabelSetIndex = value;
                SelectedAlignmentLabelSetStyle = AlignmentLabelSets[_selectedAlignmentLabelSetIndex];
                OnPropertyChanged(nameof(SelectedAlignmentLabelSetIndex));
                OnPropertyChanged(nameof(IsValid));
                ValidationChanged?.Invoke(IsValid);
            }
        }

        public int SelectedAlignmentStyleIndex
        {
            get { return _selectedAlignmentStyleIndex; }
            set { _selectedAlignmentStyleIndex = value;
                SelectedAlignmentStyle = AlignmentStyles[_selectedAlignmentStyleIndex];
                OnPropertyChanged(nameof(SelectedAlignmentStyleIndex));
                OnPropertyChanged(nameof(IsValid));
                ValidationChanged?.Invoke(IsValid);
            }
        }
        public bool IsValid { get {
                if (_selectedAlignmentStyleIndex <0)
                    return false;
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Name))
                    return false;               
                if (SelectedAlignmentStyle == null)
                    return false;
                    



                return true;
            } }
        #endregion
        public AlignmentTemplateViewModel(ICADService cADService)
        {
            _cADService = cADService;
            _name = "Alignment";

            if (AlignmentStyles.Count>0)
            {
                SelectedAlignmentStyleIndex = 0;
            }
            if (AlignmentLabelSets.Count>1)
            {
                SelectedAlignmentLabelSetIndex = 0;
            }

        }
    }
}
