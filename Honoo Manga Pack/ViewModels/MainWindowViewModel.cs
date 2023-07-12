using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using HandyControl.Controls;
using HandyControl.Data;
using Honoo.MangaPack.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class MainWindowViewModel : ObservableObject
    {
        private ObservableWorkStatus _packStatus = new();
        private ObservableSettings _settings = new(Common.Settings);
        private ObservableWorkStatus _unpackStatus = new();

        public MainWindowViewModel()
        {
            this.PackDropCommand = new RelayCommand<DragEventArgs>(PackDrop, (e) => { return !this.PackStatus.Running; });
            this.PackClearCommand = new RelayCommand(PackClear, () => { return !this.PackStatus.Running; });
            this.PackCommand = new RelayCommand(Pack);
            this.UnpackDropCommand = new RelayCommand<DragEventArgs>(UnpackDrop, (e) => { return !this.UnpackStatus.Running; });
            this.UnpackClearCommand = new RelayCommand(UnpackClear, () => { return !this.UnpackStatus.Running; });
            this.UnpackCommand = new RelayCommand(Unpack);

            WeakReferenceMessenger.Default.Register<ValueChangedMessage<int>, string>(this, "PackProgress", (recipient, messenger) =>
            {
                this.PackStatus.Progress = messenger.Value;
            });
            WeakReferenceMessenger.Default.Register<ValueChangedMessage<int>, string>(this, "UnpackProgress", (recipient, messenger) =>
            {
                this.UnpackStatus.Progress = messenger.Value;
            });
        }

        public IRelayCommand PackClearCommand { get; set; }
        public IRelayCommand PackCommand { get; set; }
        public IRelayCommand PackDropCommand { get; set; }
        public ObservableWorkStatus PackStatus { get => _packStatus; set => SetProperty(ref _packStatus, value); }
        public ObservableSettings Settings { get => _settings; set => SetProperty(ref _settings, value); }
        public IRelayCommand UnpackClearCommand { get; set; }
        public IRelayCommand UnpackCommand { get; set; }
        public IRelayCommand UnpackDropCommand { get; set; }
        public ObservableWorkStatus UnpackStatus { get => _unpackStatus; set => SetProperty(ref _unpackStatus, value); }

        private void Pack()
        {
            if (this.PackStatus.Running)
            {
                this.PackStatus.Abort = true;
            }
            else
            {
                if (this.PackStatus.Projects.Count > 0)
                {
                    this.PackStatus.Abort = false;
                    this.PackStatus.Running = true;
                    this.PackStatus.Log.Clear();
                    this.PackStatus.HasError = false;
                    Task.Run(() =>
                   {
                       int total = this.PackStatus.Projects.Count;
                       double num = 0;
                       Settings settings = Common.Settings.Clone();
                       for (int i = this.PackStatus.Projects.Count - 1; i >= 0; i--)
                       {
                           if (!this.PackStatus.Abort)
                           {
                               num++;
                               int p = (int)Math.Floor(num / total * 100);
                               WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int>(p), "PackProgress");
                               if (!Packs.Do(this.PackStatus.Projects[i], settings, out KeyValuePair<string, bool> log))
                               {
                                   this.PackStatus.HasError = true;
                                   Growl.WarningGlobal(new GrowlInfo
                                   {
                                       ShowCloseButton = false,
                                       ShowDateTime = false,
                                       Message = log.Key
                                   });
                               }
                               this.PackStatus.Projects.RemoveAt(i);
                               this.PackStatus.Log.Add(log);
                           }
                       }
                       WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int>(0), "PackProgress");
                       this.PackStatus.Running = false;
                   });
                }
            }
        }

        private void PackClear()
        {
            this.PackStatus.Projects.Clear();
        }

        private void PackDrop(DragEventArgs? e)
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
                            foreach (var project in this.PackStatus.Projects)
                            {
                                if (project == entry)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            if (!exists)
                            {
                                this.PackStatus.Projects.Add(entry);
                            }
                        }
                    }
                }
            }
        }

        private void Unpack()
        {
            if (this.UnpackStatus.Running)
            {
                this.UnpackStatus.Abort = true; ;
            }
            else
            {
                if (this.UnpackStatus.Projects.Count > 0)
                {
                    this.UnpackStatus.Abort = false;
                    this.UnpackStatus.Running = true;
                    this.UnpackStatus.Log.Clear();
                    this.UnpackStatus.HasError = false;
                    Task.Run(() =>
                    {
                        int total = this.UnpackStatus.Projects.Count;
                        double num = 0;
                        Settings settings = Common.Settings.Clone();
                        for (int i = this.UnpackStatus.Projects.Count - 1; i >= 0; i--)
                        {
                            if (!this.UnpackStatus.Abort)
                            {
                                num++;
                                int p = (int)Math.Floor(num / total * 100);
                                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int>(p), "UnpackProgress");
                                if (!Unpacks.Do(this.UnpackStatus.Projects[i], settings, out KeyValuePair<string, bool> log))
                                {
                                    this.UnpackStatus.HasError = true;
                                    Growl.WarningGlobal(new GrowlInfo
                                    {
                                        ShowCloseButton = false,
                                        ShowDateTime = false,
                                        Message = log.Key
                                    });
                                }
                                this.UnpackStatus.Projects.RemoveAt(i);
                                this.UnpackStatus.Log.Add(log);
                            }
                        }
                        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int>(0), "UnpackProgress");
                        this.UnpackStatus.Running = false;
                    });
                }
            }
        }

        private void UnpackClear()
        {
            this.UnpackStatus.Projects.Clear();
        }

        private void UnpackDrop(DragEventArgs? e)
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
                            foreach (var entry2 in this.UnpackStatus.Projects)
                            {
                                if (entry2 == entry)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            if (!exists)
                            {
                                this.UnpackStatus.Projects.Add(entry);
                            }
                        }
                    }
                }
            }
        }
    }
}