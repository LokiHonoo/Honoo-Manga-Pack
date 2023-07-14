using Honoo.MangaPack.Models;
using System.Windows;

namespace Honoo.MangaPack
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            ModelLocator.ObservableSettings.Save();
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            if (Honoo.Threading.App.PrevInstance("4Bvmw9BMhj2BQFHb"))
            {
                Current.Shutdown();
            }
            else
            {
                ModelLocator.ObservableSettings.Load();
                this.Exit += App_Exit;
            }
        }
    }
}