using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using DUA_WPF;
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
            _serviceProvider=CreateServiceProvider();
        }

        private static IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainViewModel>();


            



            return services.BuildServiceProvider();
        }

        [CommandMethod("OpenWPFWindow")]
        public void CmdOpenWPFWindow()
        {
            ViewModelBase vm = _serviceProvider.GetRequiredService<MainViewModel>() ;
            Window expWindow = new DUA_Main(vm);
            var _expResult = App.ShowModalWindow(expWindow);

        }




        public void Initialize()
        {
          
        }

        public void Terminate()
        {
           
        }
    }
}
