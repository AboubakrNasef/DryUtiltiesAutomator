using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using DUA_WPF;
using DUA_WPF.Navigator;
using DUA_WPF.Services;
using DUA_WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;
using App = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(DryUtiltiesAutomator.DUA_APP))]
namespace DryUtiltiesAutomator
{
    internal class DUA_APP : IExtensionApplication
    {

        public IServiceProvider _serviceProvider;
        public DUA_APP()
        {
            
        }

        private static IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<GroupsViewModel>();
            services.AddSingleton<TemplatesViewModel>();

            services.AddSingleton<INavigator,Navigator>();
            services.AddSingleton<ICADService,CADService>();
            services.AddSingleton<ITemplateService,TemplateService>();
            services.AddSingleton<IModalService,ModalService>();






            return services.BuildServiceProvider();
        }

        [CommandMethod("asd_OpenWPFWindow")]
        public void CmdOpenWPFWindow()
        {
            _serviceProvider = CreateServiceProvider();
            ViewModelBase vm = _serviceProvider.GetRequiredService<MainViewModel>() ;

            Window expWindow = new DUA_Main(vm);
            expWindow.Closed += ExpWindow_Closed;
            var _expResult = App.ShowModalWindow(expWindow);

        }

        private void ExpWindow_Closed(object sender, EventArgs e)
        {
          
            _serviceProvider = null;
        }

        public void Initialize()
        {
          
        }

        public void Terminate()
        {
           
        }
    }
}
