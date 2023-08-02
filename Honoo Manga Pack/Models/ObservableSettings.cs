using CommunityToolkit.Mvvm.ComponentModel;
using Honoo.Configuration;
using Honoo.MangaPack.Classes;
using System.Collections.ObjectModel;
using System.IO;

namespace Honoo.MangaPack.Models
{
    public sealed class ObservableSettings : ObservableObject
    {
        private readonly List<byte[]> _adHashs = [];
        private bool _addTag = false;
        private bool _addTopTitle = false;
        private ObservableCollection<string> _ads = [];
        private bool _executeAtDrop = false;
        private bool _moveToRecycleBin = false;
        private ObservableCollection<string> _passwords = [];
        private bool _removeAD = false;
        private bool _resetName = false;
        private string _selectedTag = string.Empty;
        private ObservableCollection<string> _tags = ["[中国翻訳]"];
        private bool _topmost = false;
        private int _windowLeft = 300;
        private int _windowTop = 300;
        private string _workDirectly = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HonooMangaPack");
        public bool AddTag { get => _addTag; set => SetProperty(ref _addTag, value); }
        public bool AddTopTitle { get => _addTopTitle; set => SetProperty(ref _addTopTitle, value); }
        public List<byte[]> ADHashs => _adHashs;
        public ObservableCollection<string> ADs { get => _ads; set => SetProperty(ref _ads, value); }
        public bool ExecuteAtDrop { get => _executeAtDrop; set => SetProperty(ref _executeAtDrop, value); }
        public bool MoveToRecycleBin { get => _moveToRecycleBin; set => SetProperty(ref _moveToRecycleBin, value); }
        public ObservableCollection<string> Passwords { get => _passwords; set => SetProperty(ref _passwords, value); }
        public bool RemoveAD { get => _removeAD; set => SetProperty(ref _removeAD, value); }
        public bool ResetName { get => _resetName; set => SetProperty(ref _resetName, value); }
        public string SelectedTag { get => _selectedTag; set => SetProperty(ref _selectedTag, value); }
        public ObservableCollection<string> Tags { get => _tags; set => SetProperty(ref _tags, value); }
        public bool Topmost { get => _topmost; set => SetProperty(ref _topmost, value); }
        public int WindowLeft { get => _windowLeft; set => SetProperty(ref _windowLeft, value); }
        public int WindowTop { get => _windowTop; set => SetProperty(ref _windowTop, value); }
        public string WorkDirectly { get => _workDirectly; set => SetProperty(ref _workDirectly, value); }

        public void Load(string configFlie)
        {
            using HonooSettingsManager manager = File.Exists(configFlie) ? new(configFlie) : new();
            this.WindowTop = manager.Default.Properties.GetValue("WindowTop", 300);
            this.WindowLeft = manager.Default.Properties.GetValue("WindowLeft", 300);
            this.Topmost = manager.Default.Properties.GetValue("Topmost", false);
            this.WorkDirectly = manager.Default.Properties.GetValue("WorkDirectly", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HonooMangaPack"));
            this.ExecuteAtDrop = manager.Default.Properties.GetValue("ExecuteAtDrop", false);
            this.ResetName = manager.Default.Properties.GetValue("ResetName", false);
            this.MoveToRecycleBin = manager.Default.Properties.GetValue("MoveToRecycleBin", false);
            if (manager.Default.Properties.TryGetArrayValue("Passwords", out string[] passwords))
            {
                this.Passwords.Clear();
                foreach (var password in passwords)
                {
                    this.Passwords.Add(password);
                }
            }
            this.RemoveAD = manager.Default.Properties.GetValue("RemoveAD", false);
            if (manager.Default.Properties.TryGetArrayValue("ADs", out string[] ads))
            {
                this.ADs.Clear();
                _adHashs.Clear();
                foreach (var ad in ads)
                {
                    if (XValueHelper.TryParse(ad, out byte[]? hash))
                    {
                        this.ADs.Add(ad);
                        _adHashs.Add(hash!);
                    }
                }
            }
            this.AddTopTitle = manager.Default.Properties.GetValue("AddTopTitle", false);
            this.AddTag = manager.Default.Properties.GetValue("AddTag", false);
            if (manager.Default.Properties.TryGetArrayValue("Tags", out string[] tags))
            {
                this.Tags.Clear();
                foreach (var tag in tags)
                {
                    this.Tags.Add(tag);
                }
            }
            this.SelectedTag = manager.Default.Properties.GetValue("SelectedTag", string.Empty);
        }

        public void Save(string configFlie)
        {
            using HonooSettingsManager manager = new();
            manager.Default.Properties.AddOrUpdate("WindowTop", this.WindowTop);
            manager.Default.Properties.AddOrUpdate("WindowLeft", this.WindowLeft);
            manager.Default.Properties.AddOrUpdate("Topmost", this.Topmost);
            manager.Default.Properties.AddOrUpdate("WorkDirectly", this.WorkDirectly);
            manager.Default.Properties.AddOrUpdate("ExecuteAtDrop", this.ExecuteAtDrop);
            manager.Default.Properties.AddOrUpdate("ResetName", this.ResetName);
            manager.Default.Properties.AddOrUpdate("MoveToRecycleBin", this.MoveToRecycleBin);
            manager.Default.Properties.AddOrUpdateArray("Passwords", this.Passwords.ToArray());
            manager.Default.Properties.AddOrUpdate("RemoveAD", this.RemoveAD);
            manager.Default.Properties.AddOrUpdateArray("ADs", this.ADs.ToArray());
            manager.Default.Properties.AddOrUpdate("AddTopTitle", this.AddTopTitle);
            manager.Default.Properties.AddOrUpdate("AddTag", this.AddTag);
            manager.Default.Properties.AddOrUpdateArray("Tags", this.Tags.ToArray());
            manager.Default.Properties.AddOrUpdate("SelectedTag", this.SelectedTag);
            manager.Save(configFlie);
        }
    }
}