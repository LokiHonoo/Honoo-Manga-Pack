using System.Windows;

namespace Honoo.MangaUnpack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Honoo.MangaUnpack.Properties.Settings.Default.PositionTop = Common.Settings.PositionTop;
            Honoo.MangaUnpack.Properties.Settings.Default.PositionLeft = Common.Settings.PositionLeft;
            Honoo.MangaUnpack.Properties.Settings.Default.Topmost = Common.Settings.Topmost;
            Honoo.MangaUnpack.Properties.Settings.Default.ShowSettings = Common.Settings.ShowSettings == Visibility.Visible ? true : false;
            Honoo.MangaUnpack.Properties.Settings.Default.StructureOption = Common.Settings.StructureOption;
            Honoo.MangaUnpack.Properties.Settings.Default.Save();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Common.Settings.PositionTop = Honoo.MangaUnpack.Properties.Settings.Default.PositionTop;
            Common.Settings.PositionLeft = Honoo.MangaUnpack.Properties.Settings.Default.PositionLeft;
            Common.Settings.Topmost = Honoo.MangaUnpack.Properties.Settings.Default.Topmost;
            Common.Settings.ShowSettings = Honoo.MangaUnpack.Properties.Settings.Default.ShowSettings ? Visibility.Visible : Visibility.Collapsed;
            Common.Settings.StructureOption = Honoo.MangaUnpack.Properties.Settings.Default.StructureOption;

            if (Common.Settings.PositionTop + 100 > SystemParameters.PrimaryScreenHeight)
            {
                Common.Settings.PositionTop = 300;
            }
            if (Common.Settings.PositionLeft + 300 > SystemParameters.PrimaryScreenWidth)
            {
                Common.Settings.PositionLeft = 300;
            }
        }
    }
}