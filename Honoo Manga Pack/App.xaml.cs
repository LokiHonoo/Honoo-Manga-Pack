using Honoo.Configuration;
using Honoo.MangaPack.Models;
using System.IO;
using System.Windows;

namespace Honoo.MangaPack
{
    public partial class App : Application
    {
        private static readonly string _configFlie = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml");

        #region App

        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            SaveConfig();
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            if (Honoo.Threading.App.PrevInstance("4Bvmw9BMhj2BQFHb"))
            {
                Current.Shutdown();
            }
            else
            {
                LoadConfig();
                this.Exit += App_Exit;
            }
        }

        #endregion App

        public static void LoadConfig()
        {
            using HonooSettingsManager manager = File.Exists(_configFlie) ? new(_configFlie) : new();
            ModelLocator.MainSettings.WindowTop = manager.Default.Properties.GetValue("WindowTop", ModelLocator.MainSettings.WindowTop);
            ModelLocator.MainSettings.WindowLeft = manager.Default.Properties.GetValue("WindowLeft", ModelLocator.MainSettings.WindowLeft);
            ModelLocator.MainSettings.Topmost = manager.Default.Properties.GetValue("Topmost", ModelLocator.MainSettings.Topmost);
            ModelLocator.MainSettings.SettingExpanded = manager.Default.Properties.GetValue("SettingExpanded", ModelLocator.MainSettings.SettingExpanded);
            ModelLocator.MainSettings.WorkDirectly = manager.Default.Properties.GetValue("WorkDirectly", ModelLocator.MainSettings.WorkDirectly);
            ModelLocator.MainSettings.ExecuteAtDrop = manager.Default.Properties.GetValue("ExecuteAtDrop", ModelLocator.MainSettings.ExecuteAtDrop);
            ModelLocator.MainSettings.ResetName = manager.Default.Properties.GetValue("ResetName", ModelLocator.MainSettings.ResetName);
            ModelLocator.MainSettings.MoveToRecycleBin = manager.Default.Properties.GetValue("MoveToRecycleBin", ModelLocator.MainSettings.MoveToRecycleBin);
            if (manager.Default.Properties.TryGetArrayValue("Passwords", out string[][] passwords))
            {
                ModelLocator.PasswordSettings.Passwords.Clear();
                foreach (string[] password in passwords)
                {
                    ModelLocator.PasswordSettings.Passwords.Add(password);
                }
            }
            ModelLocator.MainSettings.UnpacksMoveToPacks = manager.Default.Properties.GetValue("UnpacksMoveToPacks", ModelLocator.MainSettings.UnpacksMoveToPacks);
            ModelLocator.MainSettings.PackUnpacks = manager.Default.Properties.GetValue("PackUnpacks", ModelLocator.MainSettings.PackUnpacks);
            ModelLocator.MainSettings.RemoveAD = manager.Default.Properties.GetValue("RemoveAD", ModelLocator.MainSettings.RemoveAD);
            ModelLocator.MainSettings.AddTopTitle = manager.Default.Properties.GetValue("AddTopTitle", ModelLocator.MainSettings.AddTopTitle);
            ModelLocator.MainSettings.AddTag = manager.Default.Properties.GetValue("AddTag", ModelLocator.MainSettings.AddTag);
            if (manager.Default.Properties.TryGetArrayValue("Tags", out string[] tags))
            {
                ModelLocator.TagSettings.Tags.Clear();
                foreach (string tag in tags)
                {
                    ModelLocator.TagSettings.Tags.Add(tag);
                }
            }
            ModelLocator.MainSettings.SelectedTag = manager.Default.Properties.GetValue("SelectedTag", string.Empty);
            if (manager.Default.Properties.TryGetArrayValue("ads", out string[][] ads))
            {
                ModelLocator.ADSettings.ADs.Clear();
                foreach (string[] ad in ads)
                {
                    ModelLocator.ADSettings.ADs.Add(ad);
                }
            }
        }

        public static void SaveConfig()
        {
            using HonooSettingsManager manager = new();
            manager.Default.Properties.AddOrUpdate("WindowTop", ModelLocator.MainSettings.WindowTop);
            manager.Default.Properties.AddOrUpdate("WindowLeft", ModelLocator.MainSettings.WindowLeft);
            manager.Default.Properties.AddOrUpdate("Topmost", ModelLocator.MainSettings.Topmost);
            manager.Default.Properties.AddOrUpdate("SettingExpanded", ModelLocator.MainSettings.SettingExpanded);
            manager.Default.Properties.AddOrUpdate("WorkDirectly", ModelLocator.MainSettings.WorkDirectly);
            manager.Default.Properties.AddOrUpdate("ExecuteAtDrop", ModelLocator.MainSettings.ExecuteAtDrop);
            manager.Default.Properties.AddOrUpdate("ResetName", ModelLocator.MainSettings.ResetName);
            manager.Default.Properties.AddOrUpdate("MoveToRecycleBin", ModelLocator.MainSettings.MoveToRecycleBin);
            manager.Default.Properties.AddOrUpdateArray("Passwords", ModelLocator.PasswordSettings.Passwords.ToArray());
            manager.Default.Properties.AddOrUpdate("UnpacksMoveToPacks", ModelLocator.MainSettings.UnpacksMoveToPacks);
            manager.Default.Properties.AddOrUpdate("PackUnpacks", ModelLocator.MainSettings.PackUnpacks);
            manager.Default.Properties.AddOrUpdate("RemoveAD", ModelLocator.MainSettings.RemoveAD);
            manager.Default.Properties.AddOrUpdateArray("ADs", ModelLocator.ADSettings.ADs.ToArray());
            manager.Default.Properties.AddOrUpdate("AddTopTitle", ModelLocator.MainSettings.AddTopTitle);
            manager.Default.Properties.AddOrUpdate("AddTag", ModelLocator.MainSettings.AddTag);
            manager.Default.Properties.AddOrUpdateArray("Tags", ModelLocator.TagSettings.Tags.ToArray());
            manager.Default.Properties.AddOrUpdate("SelectedTag", ModelLocator.MainSettings.SelectedTag);
            manager.Save(_configFlie);
        }
    }
}