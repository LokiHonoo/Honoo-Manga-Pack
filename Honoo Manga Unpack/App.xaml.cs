using System.Windows;

namespace Honoo.MangaUnpack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool _repetition = false;

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (!_repetition)
            {
                Honoo.MangaUnpack.Properties.Settings.Default.PositionTop = Common.Settings.PositionTop;
                Honoo.MangaUnpack.Properties.Settings.Default.PositionLeft = Common.Settings.PositionLeft;
                Honoo.MangaUnpack.Properties.Settings.Default.Topmost = Common.Settings.Topmost;
                Honoo.MangaUnpack.Properties.Settings.Default.ShowSettings = Common.Settings.ShowSettings;
                Honoo.MangaUnpack.Properties.Settings.Default.StructureOption = Common.Settings.StructureOption;
                Honoo.MangaUnpack.Properties.Settings.Default.DelOriginalOption = Common.Settings.DelOriginalOption;
                Honoo.MangaUnpack.Properties.Settings.Default.Save();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Honoo.Threading.App.PrevInstance("cg4rb8tcJqWcDbQK"))
            {
                _repetition = true;
                Current.Shutdown();
            }
            else
            {
                Common.Settings.PositionTop = Honoo.MangaUnpack.Properties.Settings.Default.PositionTop;
                Common.Settings.PositionLeft = Honoo.MangaUnpack.Properties.Settings.Default.PositionLeft;
                Common.Settings.Topmost = Honoo.MangaUnpack.Properties.Settings.Default.Topmost;
                Common.Settings.ShowSettings = Honoo.MangaUnpack.Properties.Settings.Default.ShowSettings;
                Common.Settings.StructureOption = Honoo.MangaUnpack.Properties.Settings.Default.StructureOption;
                Common.Settings.DelOriginalOption = Honoo.MangaUnpack.Properties.Settings.Default.DelOriginalOption;

                if (Common.Settings.PositionTop + 100 > SystemParameters.PrimaryScreenHeight)
                {
                    Common.Settings.PositionTop = 300;
                }
                if (Common.Settings.PositionLeft + 100 > SystemParameters.PrimaryScreenWidth)
                {
                    Common.Settings.PositionLeft = 300;
                }
            }
        }
    }
}