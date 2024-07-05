using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;

namespace Honoo.MangaPack.Models
{
    public sealed class MainSettings : ObservableObject
    {
        private bool _addTag;
        private bool _addTopTitle;
        private bool _executeAtDrop;
        private bool _moveToRecycleBin;
        private bool _packUnpacks;
        private bool _removeAD;
        private bool _resetName;
        private string _selectedTag = string.Empty;
        private bool _settingExpanded = true;
        private bool _topmost;
        private bool _unpacksMoveToPacks;
        private int _windowLeft = 300;
        private int _windowTop = 300;
        private string _workDirectly = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HonooMangaPack");
        public bool AddTag { get => _addTag; set => SetProperty(ref _addTag, value); }
        public bool AddTopTitle { get => _addTopTitle; set => SetProperty(ref _addTopTitle, value); }
        public bool ExecuteAtDrop { get => _executeAtDrop; set => SetProperty(ref _executeAtDrop, value); }
        public bool MoveToRecycleBin { get => _moveToRecycleBin; set => SetProperty(ref _moveToRecycleBin, value); }
        public bool PackUnpacks { get => _packUnpacks; set => SetProperty(ref _packUnpacks, value); }
        public bool RemoveAD { get => _removeAD; set => SetProperty(ref _removeAD, value); }
        public bool ResetName { get => _resetName; set => SetProperty(ref _resetName, value); }
        public string SelectedTag { get => _selectedTag; set => SetProperty(ref _selectedTag, value); }
        public bool SettingExpanded { get => _settingExpanded; set => SetProperty(ref _settingExpanded, value); }

        public bool Topmost { get => _topmost; set => SetProperty(ref _topmost, value); }
        public bool UnpacksMoveToPacks { get => _unpacksMoveToPacks; set => SetProperty(ref _unpacksMoveToPacks, value); }
        public int WindowLeft { get => _windowLeft; set => SetProperty(ref _windowLeft, value); }
        public int WindowTop { get => _windowTop; set => SetProperty(ref _windowTop, value); }
        public string WorkDirectly { get => _workDirectly; set => SetProperty(ref _workDirectly, value); }
    }
}