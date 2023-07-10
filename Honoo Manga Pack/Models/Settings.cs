using Honoo.Configuration;
using System;
using System.IO;
using System.Windows;

namespace Honoo.MangaPack.Models
{
    public sealed class Settings
    {
        private static readonly string _configFlie = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml");
        private bool _packRemoveNested;
        private bool _packRoot;
        private bool _packSaveTo;
        private bool _packSuffixDiff;
        private string _packSuffixDiffValue = string.Empty;
        private bool _packSuffixEnd;
        private string _packSuffixEndValue = string.Empty;
        private int _packNamesake;
        private bool _showSettings;
        private bool _topmost;
        private bool _unpackDelOrigin;
        private bool _unpackRemoveNested;
        private bool _unpackSaveTo;
        private int _windowLeft;
        private int _windowTop;
        public bool PackRemoveNested { get => _packRemoveNested; set => _packRemoveNested = value; }
        public bool PackRoot { get => _packRoot; set => _packRoot = value; }
        public bool PackSaveTo { get => _packSaveTo; set => _packSaveTo = value; }
        public bool PackSuffixDiff { get => _packSuffixDiff; set => _packSuffixDiff = value; }
        public string PackSuffixDiffValue { get => _packSuffixDiffValue; set => _packSuffixDiffValue = value; }
        public bool PackSuffixEnd { get => _packSuffixEnd; set => _packSuffixEnd = value; }
        public string PackSuffixEndValue { get => _packSuffixEndValue; set => _packSuffixEndValue = value; }
        public int PackNamesake { get => _packNamesake; set => _packNamesake = value; }
        public bool ShowSettings { get => _showSettings; set => _showSettings = value; }
        public bool Topmost { get => _topmost; set => _topmost = value; }
        public bool UnpackDelOrigin { get => _unpackDelOrigin; set => _unpackDelOrigin = value; }
        public bool UnpackRemoveNested { get => _unpackRemoveNested; set => _unpackRemoveNested = value; }
        public bool UnpackSaveTo { get => _unpackSaveTo; set => _unpackSaveTo = value; }
        public int WindowLeft { get => _windowLeft; set => _windowLeft = value; }
        public int WindowTop { get => _windowTop; set => _windowTop = value; }

        public Settings Clone()
        {
            Settings settings = new()
            {
                WindowTop = _windowTop,
                WindowLeft = _windowLeft,
                Topmost = _topmost,
                ShowSettings = _showSettings,
                PackSuffixEnd = _packSuffixEnd,
                PackSuffixEndValue = _packSuffixEndValue,
                PackSuffixDiff = _packSuffixDiff,
                PackSuffixDiffValue = _packSuffixDiffValue,
                PackRoot = _packRoot,
                PackRemoveNested = _packRemoveNested,
                PackSaveTo = _packSaveTo,
                PackNamesake = _packNamesake,
                UnpackRemoveNested = _unpackRemoveNested,
                UnpackSaveTo = _unpackSaveTo,
                UnpackDelOrigin = _unpackDelOrigin
            };
            return settings;
        }

        public void Load()
        {
            using (ConfigurationManager manager = new(_configFlie))
            {
                var settings = (DictionarySection)manager.ConfigSections.Sections.GetOrAdd("settings", ConfigSectionKind.DictionarySection);
                _windowTop = settings.Properties.GetValue("WindowTop", 300);
                _windowLeft = settings.Properties.GetValue("WindowLeft", 300);
                _topmost = settings.Properties.GetValue("Topmost", false);
                _showSettings = settings.Properties.GetValue("ShowSettings", true);
                _packSuffixEnd = settings.Properties.GetValue("PackSuffixEnd", false);
                _packSuffixEndValue = settings.Properties.GetValue("PackSuffixEndValue", "[中国翻訳]");
                _packSuffixDiff = settings.Properties.GetValue("PackSuffixDiff", false);
                _packSuffixDiffValue = settings.Properties.GetValue("PackSuffixDiffValue", "[中国翻訳]");
                _packRoot = settings.Properties.GetValue("PackRoot", true);
                _packRemoveNested = settings.Properties.GetValue("PackRemoveNested", false);
                _packSaveTo = settings.Properties.GetValue("PackSaveTo", false);
                _packNamesake = settings.Properties.GetValue("PackNamesake", 0);
                _unpackRemoveNested = settings.Properties.GetValue("UnpackRemoveNested", false);
                _unpackSaveTo = settings.Properties.GetValue("UnpackSaveTo", false);
                _unpackDelOrigin = settings.Properties.GetValue("UnpackDelOrigin", false);
            }
            if (_windowTop > SystemParameters.PrimaryScreenHeight)
            {
                _windowTop = 300;
            }
            if (_windowLeft > SystemParameters.PrimaryScreenWidth)
            {
                _windowLeft = 300;
            }
        }

        public void Save()
        {
            using ConfigurationManager manager = new(_configFlie);
            var settings = (DictionarySection)manager.ConfigSections.Sections.GetOrAdd("settings", ConfigSectionKind.DictionarySection);
            settings.Properties["WindowTop"] = _windowTop;
            settings.Properties["WindowLeft"] = _windowLeft;
            settings.Properties["Topmost"] = _topmost;
            settings.Properties["ShowSettings"] = _showSettings;
            settings.Properties["PackSuffixEnd"] = _packSuffixEnd;
            settings.Properties["PackSuffixEndValue"] = _packSuffixEndValue;
            settings.Properties["PackSuffixDiff"] = _packSuffixDiff;
            settings.Properties["PackSuffixDiffValue"] = _packSuffixDiffValue;
            settings.Properties["PackRoot"] = _packRoot;
            settings.Properties["PackRemoveNested"] = _packRemoveNested;
            settings.Properties["PackSaveTo"] = _packSaveTo;
            settings.Properties["PackNamesake"] = _packNamesake;
            settings.Properties["UnpackRemoveNested"] = _unpackRemoveNested;
            settings.Properties["UnpackSaveTo"] = _unpackSaveTo;
            settings.Properties["UnpackDelOrigin"] = _unpackDelOrigin;
            manager.Save(_configFlie);
        }
    }
}