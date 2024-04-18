using CommonServiceLocator;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CNC_Program_Control_System
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            BootStrapperManager.Initialize(new SimpleInjectorBootStrapper());

            // Set the locator provider from BootStrapperManager
            ServiceLocator.SetLocatorProvider(() => BootStrapperManager.BootStrapper.ServiceLocator);
        }
    }

}
