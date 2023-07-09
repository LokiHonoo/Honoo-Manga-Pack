using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Honoo.MangaUnpack.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Honoo.MangaUnpack.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly List<string> _dirs = new();
        private readonly List<KeyValuePair<bool, string>> _log = new();
        private int _countTitle = 0;
        private bool _hasFailed = false;
        private bool _idle = true;
        private int _progress = 0;
        private ObservableSettings _settings = new(Common.Settings);

        public MainWindowViewModel()
        {
            this.ToggleShowSettingsCommand = new RelayCommand(ToggleShowSettings);
            this.DropCommand = new RelayCommand<DragEventArgs>(Drop);
            this.ClearCommand = new RelayCommand(Clear);
            this.UnpackCommand = new AsyncRelayCommand(Unpack);

            WeakReferenceMessenger.Default.Register<ValueChangedMessage<int>, string>(this, "Progress", (recipient, messenger) =>
            {
                this.Progress = messenger.Value;
            });
        }

        public IRelayCommand ClearCommand { get; set; }
        public int CountTitle { get => _countTitle; set => SetProperty(ref _countTitle, value); }
        public IRelayCommand DropCommand { get; set; }
        public bool HasFailed { get => _hasFailed; set => SetProperty(ref _hasFailed, value); }
        public bool Idle { get => _idle; set => SetProperty(ref _idle, value); }
        public int Progress { get => _progress; set => SetProperty(ref _progress, value); }
        public ObservableSettings Settings { get => _settings; set => SetProperty(ref _settings, value); }
        public IRelayCommand ToggleShowSettingsCommand { get; set; }
        public IRelayCommand UnpackCommand { get; set; }

        private void Clear()
        {
            _dirs.Clear();
            this.CountTitle = 0;
        }

        private void Drop(DragEventArgs? e)
        {
            if (this.Idle)
            {
                if (e != null)
                {
                    if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    {
                        string[] entries = (string[])e.Data.GetData(DataFormats.FileDrop);
                        if (entries.Length > 0)
                        {
                            foreach (var entry in entries)
                            {
                                bool exists = false;
                                foreach (var dir in _dirs)
                                {
                                    if (entry == dir)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }
                                if (!exists)
                                {
                                    _dirs.Add(entry);
                                }
                            }
                            this.CountTitle = _dirs.Count;
                        }
                    }
                }
            }
        }

        private void ToggleShowSettings()
        {
            this.Settings.ShowSettings = this.Settings.ShowSettings == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private async Task Unpack()
        {
            if (_dirs.Count > 0)
            {
                this.Idle = false;
                this.Progress = 0;
                _log.Clear();
                bool hasFailed = false;
                await Task.Run(() =>
                {
                    for (int i = 0; i < _dirs.Count; i++)
                    {
                        int p = (int)Math.Floor((i + 1d) / _dirs.Count * 100);
                        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int>(p), "Progress");
                        if (!Unpacks.Do(_dirs[i], _log))
                        {
                            hasFailed = true;
                        }
                        this.CountTitle--;
                    }
                });
                _dirs.Clear();
                this.HasFailed = hasFailed;
                this.CountTitle = 0;
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int>(0), "Progress");
                this.Idle = true;
            }
        }
    }
}