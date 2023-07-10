using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using HandyControl.Controls;
using HandyControl.Data;
using Honoo.MangaPack.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class MainWindowViewModel : ObservableObject
    {
        private bool _packAbort = false;
        private ObservableCollection<string> _packEntries = new();
        private bool _packing;
        private int _packingProgress;
        private ObservableCollection<KeyValuePair<string, bool>> _packLog = new();
        private ObservableSettings _settings = new(Common.Settings);
        private bool _unpackAbort = false;
        private ObservableCollection<string> _unpackEntries = new();
        private bool _unpacking;
        private int _unpackingProgress;
        private ObservableCollection<KeyValuePair<string, bool>> _unpackLog = new();

        public MainWindowViewModel()
        {
            this.PackDropCommand = new RelayCommand<DragEventArgs>(PackDrop);
            this.PackClearCommand = new RelayCommand(PackClear, () => { return !_packing; });
            this.PackCommand = new AsyncRelayCommand(Pack);
            this.UnpackDropCommand = new RelayCommand<DragEventArgs>(UnpackDrop);
            this.UnpackClearCommand = new RelayCommand(UnpackClear, () => { return !_unpacking; });
            this.UnpackCommand = new AsyncRelayCommand(Unpack);

            //WeakReferenceMessenger.Default.Register<ValueChangedMessage<int>, string>(this, "PackProgress", (recipient, messenger) =>
            //{
            //});
            //WeakReferenceMessenger.Default.Register<ValueChangedMessage<int>, string>(this, "UnpackProgress", (recipient, messenger) =>
            //{
            //});
        }

        public IRelayCommand PackClearCommand { get; set; }
        public IRelayCommand PackCommand { get; set; }
        public IRelayCommand PackDropCommand { get; set; }
        public ObservableCollection<string> PackEntries { get => _packEntries; set => SetProperty(ref _packEntries, value); }
        public bool Packing { get => _packing; set => SetProperty(ref _packing, value); }
        public int PackingProgress { get => _packingProgress; set => SetProperty(ref _packingProgress, value); }
        public ObservableCollection<KeyValuePair<string, bool>> PackLog { get => _packLog; set => SetProperty(ref _packLog, value); }
        public ObservableSettings Settings { get => _settings; set => SetProperty(ref _settings, value); }
        public IRelayCommand UnpackClearCommand { get; set; }
        public IRelayCommand UnpackCommand { get; set; }
        public IRelayCommand UnpackDropCommand { get; set; }
        public ObservableCollection<string> UnpackEntries { get => _unpackEntries; set => SetProperty(ref _unpackEntries, value); }
        public bool Unpacking { get => _unpacking; set => SetProperty(ref _unpacking, value); }
        public int UnpackingProgress { get => _unpackingProgress; set => SetProperty(ref _unpackingProgress, value); }
        public ObservableCollection<KeyValuePair<string, bool>> UnpackLog { get => _unpackLog; set => SetProperty(ref _unpackLog, value); }

        private async Task Pack()
        {
            if (this.Packing)
            {
                _packAbort = false;
            }
            else
            {
                if (this.PackEntries.Count > 0)
                {
                    _packAbort = false;
                    this.Packing = true;
                    this.PackLog.Clear();
                    await Task.Run(() =>
                    {
                        int total = this.PackEntries.Count;
                        for (int i = this.PackEntries.Count - 1; i >= 0; i--)
                        {
                            if (!_packAbort)
                            {
                                int p = (int)Math.Floor((i + 1d) / total * 100);
                                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int>(p), "PackProgress");
                                if (!Packs.Do(this.PackEntries[i], Common.Settings.Clone(), out KeyValuePair<string, bool> log))
                                {
                                    Growl.WarningGlobal(new GrowlInfo
                                    {
                                        ShowCloseButton = false,
                                        ShowDateTime = false,
                                        Message = log.Key
                                    });
                                }
                                this.PackEntries.RemoveAt(i);
                                this.PackLog.Add(log);
                            }
                        }
                    });
                    this.Packing = false;
                }
            }
        }

        private void PackClear()
        {
            this.PackEntries.Clear();
        }

        private void PackDrop(DragEventArgs? e)
        {
            if (!this.Packing)
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
                                foreach (var entry2 in this.PackEntries)
                                {
                                    if (entry2 == entry)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }
                                if (!exists)
                                {
                                    this.PackEntries.Add(entry);
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task Unpack()
        {
            if (this.Unpacking)
            {
                _unpackAbort = false;
            }
            else
            {
                if (this.UnpackEntries.Count > 0)
                {
                    _unpackAbort = false;
                    this.Unpacking = true;
                    this.UnpackLog.Clear();
                    await Task.Run(() =>
                    {
                        int total = this.UnpackEntries.Count;
                        for (int i = this.UnpackEntries.Count - 1; i >= 0; i--)
                        {
                            if (!_unpackAbort)
                            {
                                int p = (int)Math.Floor((i + 1d) / total * 100);
                                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int>(p), "UnpackProgress");
                                if (!Packs.Do(this.UnpackEntries[i], Common.Settings.Clone(), out KeyValuePair<string, bool> log))
                                {
                                    Growl.WarningGlobal(new GrowlInfo
                                    {
                                        ShowCloseButton = false,
                                        ShowDateTime = false,
                                        Message = log.Key
                                    });
                                }
                                this.UnpackEntries.RemoveAt(i);
                                this.UnpackLog.Add(log);
                            }
                        }
                    });
                    this.Unpacking = false;
                }
            }
        }

        private void UnpackClear()
        {
            this.UnpackEntries.Clear();
        }

        private void UnpackDrop(DragEventArgs? e)
        {
            if (!this.Unpacking)
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
                                foreach (var entry2 in this.UnpackEntries)
                                {
                                    if (entry2 == entry)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }
                                if (!exists)
                                {
                                    this.UnpackEntries.Add(entry);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}