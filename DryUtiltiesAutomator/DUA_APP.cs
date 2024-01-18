using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using DUA_WPF;

using System.Windows;
using App = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(DryUtiltiesAutomator.DUA_APP))]
namespace DryUtiltiesAutomator
{
    internal class DUA_APP : IExtensionApplication
    {

         

        [CommandMethod("OpenWPFWindow")]
        public void CmdOpenWPFWindow()
        {
            Window expWindow = new DUA_Main();
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
