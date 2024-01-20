using Autodesk.AutoCAD.DatabaseServices;
using DUA_WPF.CAD_Commands;
using DUA_WPF.ViewModels;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DUA_WPF
{
    /// <summary>
    /// Interaction logic for DUA_Main.xaml
    /// </summary>
    public partial class DUA_Main : Window
    {
        List<LayerTableRecord> layers;
        public DUA_Main(ViewModelBase viewModel)
        {
            loadResources();
            this.SetResourceReference(BackgroundProperty, "MaterialDesignPaper");
            InitializeComponent();
            DataContext = viewModel;
        
        }

        private void InitApp()
        {

         
        }

        private void loadResources()
        {
            var datatemplates = new Uri("pack://application:,,,/DUA_WPF;component/Styles/DataTemplates.xaml");
            var MaterialDesign = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml");
            var bundledTheme = new BundledTheme();
            bundledTheme.BaseTheme = BaseTheme.Dark;
            bundledTheme.PrimaryColor = PrimaryColor.DeepPurple;
            bundledTheme.SecondaryColor = SecondaryColor.Lime;
           this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = datatemplates });
            this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = MaterialDesign });
            this.Resources.MergedDictionaries.Add(bundledTheme);

        }

        private void ComboBoxLayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        
        }
    }
}
