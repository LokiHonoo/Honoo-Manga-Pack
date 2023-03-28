using System.Windows;

namespace Honoo.MangaPack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Honoo.MangaPack.Properties.Settings.Default.PositionTop = Common.Settings.PositionTop;
            Honoo.MangaPack.Properties.Settings.Default.PositionLeft = Common.Settings.PositionLeft;
            Honoo.MangaPack.Properties.Settings.Default.Topmost = Common.Settings.Topmost;
            Honoo.MangaPack.Properties.Settings.Default.ShowSettings = Common.Settings.ShowSettings == Visibility.Visible ? true : false;
            Honoo.MangaPack.Properties.Settings.Default.SuffixOne = Common.Settings.SuffixOne;
            Honoo.MangaPack.Properties.Settings.Default.SuffixOneValue = Common.Settings.SuffixOneValue;
            Honoo.MangaPack.Properties.Settings.Default.SuffixDiff = Common.Settings.SuffixDiff;
            Honoo.MangaPack.Properties.Settings.Default.SuffixDiffValue = Common.Settings.SuffixDiffValue;
            Honoo.MangaPack.Properties.Settings.Default.ZipRootOption = Common.Settings.ZipRootOption;
            Honoo.MangaPack.Properties.Settings.Default.StructureOption = Common.Settings.StructureOption;
            Honoo.MangaPack.Properties.Settings.Default.SaveTargetOption = Common.Settings.SaveTargetOption;
            Honoo.MangaPack.Properties.Settings.Default.CollisionOption = Common.Settings.CollisionOption;
            Honoo.MangaPack.Properties.Settings.Default.Save();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Common.Settings.PositionTop = Honoo.MangaPack.Properties.Settings.Default.PositionTop;
            Common.Settings.PositionLeft = Honoo.MangaPack.Properties.Settings.Default.PositionLeft;
            Common.Settings.Topmost = Honoo.MangaPack.Properties.Settings.Default.Topmost;
            Common.Settings.ShowSettings = Honoo.MangaPack.Properties.Settings.Default.ShowSettings ? Visibility.Visible : Visibility.Collapsed;
            Common.Settings.SuffixOne = Honoo.MangaPack.Properties.Settings.Default.SuffixOne;
            Common.Settings.SuffixOneValue = Honoo.MangaPack.Properties.Settings.Default.SuffixOneValue;
            Common.Settings.SuffixDiff = Honoo.MangaPack.Properties.Settings.Default.SuffixDiff;
            Common.Settings.SuffixDiffValue = Honoo.MangaPack.Properties.Settings.Default.SuffixDiffValue;
            Common.Settings.ZipRootOption = Honoo.MangaPack.Properties.Settings.Default.ZipRootOption;
            Common.Settings.StructureOption = Honoo.MangaPack.Properties.Settings.Default.StructureOption;
            Common.Settings.SaveTargetOption = Honoo.MangaPack.Properties.Settings.Default.SaveTargetOption;
            Common.Settings.CollisionOption = Honoo.MangaPack.Properties.Settings.Default.CollisionOption;

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