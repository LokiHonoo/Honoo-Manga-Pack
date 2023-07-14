using CommunityToolkit.Mvvm.ComponentModel;
using Honoo.Configuration;
using System;
using System.IO;

namespace Honoo.MangaPack.Models
{
    public sealed class ObservableSettings : ObservableObject
    {
        private static readonly string _configFlie = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml");

        private bool _executeAtDrop;
        private bool _packDelOrigin;
        private bool _packRemoveNested;
        private bool _packRoot;
        private bool _packSaveTo;
        private bool _packSuffixDiff;
        private string _packSuffixDiffValue = string.Empty;
        private bool _packSuffixEnd;
        private string _packSuffixEndValue = string.Empty;
        private bool _settingsExpanded;
        private bool _showSettings;
        private bool _topmost;
        private bool _unpackDelOrigin;
        private bool _unpackRemoveNested;
        private bool _unpackResetName;
        private bool _unpackSaveTo;
        private int _windowLeft;
        private int _windowTop;

        public bool ExecuteAtDrop { get => _executeAtDrop; set => SetProperty(ref _executeAtDrop, value); }
        public bool PackDelOrigin { get => _packDelOrigin; set => SetProperty(ref _packDelOrigin, value); }
        public bool PackRemoveNested { get => _packRemoveNested; set => SetProperty(ref _packRemoveNested, value); }
        public bool PackRoot { get => _packRoot; set => SetProperty(ref _packRoot, value); }
        public bool PackSaveTo { get => _packSaveTo; set => SetProperty(ref _packSaveTo, value); }
        public bool PackSuffixDiff { get => _packSuffixDiff; set => SetProperty(ref _packSuffixDiff, value); }
        public string PackSuffixDiffValue { get => _packSuffixDiffValue; set => SetProperty(ref _packSuffixDiffValue, value); }
        public bool PackSuffixEnd { get => _packSuffixEnd; set => SetProperty(ref _packSuffixEnd, value); }
        public string PackSuffixEndValue { get => _packSuffixEndValue; set => SetProperty(ref _packSuffixEndValue, value); }
        public bool SettingsExpanded { get => _settingsExpanded; set => SetProperty(ref _settingsExpanded, value); }
        public bool ShowSettings { get => _showSettings; set => SetProperty(ref _showSettings, value); }
        public bool Topmost { get => _topmost; set => SetProperty(ref _topmost, value); }
        public bool UnpackDelOrigin { get => _unpackDelOrigin; set => SetProperty(ref _unpackDelOrigin, value); }
        public bool UnpackRemoveNested { get => _unpackRemoveNested; set => SetProperty(ref _unpackRemoveNested, value); }
        public bool UnpackResetName { get => _unpackResetName; set => SetProperty(ref _unpackResetName, value); }
        public bool UnpackSaveTo { get => _unpackSaveTo; set => SetProperty(ref _unpackSaveTo, value); }
        public int WindowLeft { get => _windowLeft; set => SetProperty(ref _windowLeft, value); }
        public int WindowTop { get => _windowTop; set => SetProperty(ref _windowTop, value); }

        public ObservableSettings Clone()
        {
            ObservableSettings settings = new()
            {
                WindowTop = this.WindowTop,
                WindowLeft = this.WindowLeft,
                Topmost = this.Topmost,
                SettingsExpanded = this.SettingsExpanded,
                ExecuteAtDrop = this.ExecuteAtDrop,
                PackSaveTo = this.PackSaveTo,
                PackSuffixEnd = this.PackSuffixEnd,
                PackSuffixEndValue = this.PackSuffixEndValue,
                PackSuffixDiff = this.PackSuffixDiff,
                PackSuffixDiffValue = this.PackSuffixDiffValue,
                PackRoot = this.PackRoot,
                PackRemoveNested = this.PackRemoveNested,
                PackDelOrigin = this.PackDelOrigin,
                UnpackSaveTo = this.UnpackSaveTo,
                UnpackRemoveNested = this.UnpackRemoveNested,
                UnpackResetName = this.UnpackResetName,
                UnpackDelOrigin = this.UnpackDelOrigin
            };
            return settings;
        }

        public void Load()
        {
            using ConfigurationManager manager = new(_configFlie);
            this.WindowTop = manager.AppSettings.Properties.GetValue("WindowTop", 300);
            this.WindowLeft = manager.AppSettings.Properties.GetValue("WindowLeft", 300);
            this.Topmost = manager.AppSettings.Properties.GetValue("Topmost", false);
            this.SettingsExpanded = manager.AppSettings.Properties.GetValue("SettingsExpanded", true);
            this.ExecuteAtDrop = manager.AppSettings.Properties.GetValue("ExecuteAtDrop", false);
            this.PackSaveTo = manager.AppSettings.Properties.GetValue("PackSaveTo", false);
            this.PackSuffixEnd = manager.AppSettings.Properties.GetValue("PackSuffixEnd", false);
            this.PackSuffixEndValue = manager.AppSettings.Properties.GetValue("PackSuffixEndValue", "[中国翻訳]");
            this.PackSuffixDiff = manager.AppSettings.Properties.GetValue("PackSuffixDiff", false);
            this.PackSuffixDiffValue = manager.AppSettings.Properties.GetValue("PackSuffixDiffValue", "[中国翻訳]");
            this.PackRoot = manager.AppSettings.Properties.GetValue("PackRoot", true);
            this.PackRemoveNested = manager.AppSettings.Properties.GetValue("PackRemoveNested", true);
            this.PackDelOrigin = manager.AppSettings.Properties.GetValue("PackDelOrigin", false);
            this.UnpackSaveTo = manager.AppSettings.Properties.GetValue("UnpackSaveTo", false);
            this.UnpackRemoveNested = manager.AppSettings.Properties.GetValue("UnpackRemoveNested", true);
            this.UnpackResetName = manager.AppSettings.Properties.GetValue("UnpackResetName", true);
            this.UnpackDelOrigin = manager.AppSettings.Properties.GetValue("UnpackDelOrigin", false);
        }

        public void Save()
        {
            using ConfigurationManager manager = new(_configFlie);
            manager.AppSettings.Properties["WindowTop"] = this.WindowTop.ToString();
            manager.AppSettings.Properties["WindowLeft"] = this.WindowLeft.ToString();
            manager.AppSettings.Properties["Topmost"] = this.Topmost.ToString();
            manager.AppSettings.Properties["ShowSettings"] = this.SettingsExpanded.ToString();
            manager.AppSettings.Properties["ExecuteAtDrop"] = this.ExecuteAtDrop.ToString();
            manager.AppSettings.Properties["PackSaveTo"] = this.PackSaveTo.ToString();
            manager.AppSettings.Properties["PackSuffixEnd"] = this.PackSuffixEnd.ToString();
            manager.AppSettings.Properties["PackSuffixEndValue"] = this.PackSuffixEndValue;
            manager.AppSettings.Properties["PackSuffixDiff"] = this.PackSuffixDiff.ToString();
            manager.AppSettings.Properties["PackSuffixDiffValue"] = this.PackSuffixDiffValue;
            manager.AppSettings.Properties["PackRoot"] = this.PackRoot.ToString();
            manager.AppSettings.Properties["PackRemoveNested"] = this.PackRemoveNested.ToString();
            manager.AppSettings.Properties["PackDelOrigin"] = this.PackDelOrigin.ToString();
            manager.AppSettings.Properties["UnpackSaveTo"] = this.UnpackSaveTo.ToString();
            manager.AppSettings.Properties["UnpackRemoveNested"] = this.UnpackRemoveNested.ToString();
            manager.AppSettings.Properties["UnpackResetName"] = this.UnpackResetName.ToString();
            manager.AppSettings.Properties["UnpackDelOrigin"] = this.UnpackDelOrigin.ToString();
            manager.Save(_configFlie);
        }
    }
}