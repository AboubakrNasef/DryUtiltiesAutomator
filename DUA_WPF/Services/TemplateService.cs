using DUA_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUA_WPF.Services
{
    public class TemplateService : ITemplateService
    {
        ObservableCollection<TemplateViewModel> _templates;
        public ObservableCollection<TemplateViewModel> Templates { get => _templates; }

        public TemplateService()
        {
                _templates = new ObservableCollection<TemplateViewModel>();
        }
    }

    public interface ITemplateService
    {
         ObservableCollection<TemplateViewModel> Templates { get;  }
    }
}
