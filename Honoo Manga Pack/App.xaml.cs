using Honoo.MangaPack.Models;
using System.IO;
using System.Windows;

namespace Honoo.MangaPack
{
    public partial class App : Application
    {
        private static readonly string _configFlie = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml");

        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            ModelLocator.ObservableSettings.Save(_configFlie);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            if (Honoo.Threading.App.PrevInstance("4Bvmw9BMhj2BQFHb"))
            {
                Current.Shutdown();
            }
            else
            {
                ModelLocator.ObservableSettings.Load(_configFlie);
                this.Exit += App_Exit;
            }
        }
    }
}