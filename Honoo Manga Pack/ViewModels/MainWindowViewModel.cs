using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Honoo.MangaPack.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class MainWindowViewModel : ObservableObject
    {
        private ObservableWorkStatus _packStatus = new();
        private ObservableSettings _settings = ModelLocator.ObservableSettings;
        private ObservableWorkStatus _unpackStatus = new();

        public MainWindowViewModel()
        {
            this.PackDropCommand = new RelayCommand<DragEventArgs>(PackDropExecute, (e) => { return !this.PackStatus.Running; });
            this.PackClearCommand = new RelayCommand(PackClearExecute, () => { return !this.PackStatus.Running; });
            this.PackCommand = new RelayCommand(PackExecute);
            this.UnpackDropCommand = new RelayCommand<DragEventArgs>(UnpackDropExecute, (e) => { return !this.UnpackStatus.Running; });
            this.UnpackClearCommand = new RelayCommand(UnpackClearExecute, () => { return !this.UnpackStatus.Running; });
            this.UnpackCommand = new RelayCommand(UnpackExecute);
        }

        public ICommand PackClearCommand { get; set; }
        public ICommand PackCommand { get; set; }
        public ICommand PackDropCommand { get; set; }
        public ObservableWorkStatus PackStatus { get => _packStatus; set => SetProperty(ref _packStatus, value); }
        public ObservableSettings Settings { get => _settings; set => SetProperty(ref _settings, value); }
        public ICommand UnpackClearCommand { get; set; }
        public ICommand UnpackCommand { get; set; }
        public ICommand UnpackDropCommand { get; set; }
        public ObservableWorkStatus UnpackStatus { get => _unpackStatus; set => SetProperty(ref _unpackStatus, value); }

        private void PackClearExecute()
        {
            this.PackStatus.Projects.Clear();
        }

        private void PackDropExecute(DragEventArgs? e)
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
                        if (this.Settings.ExecuteAtDrop && this.PackStatus.Projects.Count > 0)
                        {
                            this.PackCommand.Execute(null);
                        }
                    }
                }
            }
        }

        private void PackExecute()
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
                        ObservableSettings settings = this.Settings.Clone();
                        for (int i = this.PackStatus.Projects.Count - 1; i >= 0; i--)
                        {
                            if (!this.PackStatus.Abort)
                            {
                                num++;
                                this.PackStatus.Progress = Math.Floor(num / total * 100);
                                if (!Packs.Do(this.PackStatus.Projects[i], settings, out KeyValuePair<string, bool> log))
                                {
                                    this.PackStatus.HasError = true;
                                }
                                this.PackStatus.Projects.RemoveAt(i);
                                this.PackStatus.Log.Add(log);
                            }
                        }
                        this.PackStatus.Progress = 110;
                        Thread.Sleep(500);
                        this.PackStatus.Progress = 0;
                        this.PackStatus.Running = false;
                    });
                }
            }
        }

        private void UnpackClearExecute()
        {
            this.UnpackStatus.Projects.Clear();
        }

        private void UnpackDropExecute(DragEventArgs? e)
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
                        if (this.Settings.ExecuteAtDrop && this.UnpackStatus.Projects.Count > 0)
                        {
                            this.UnpackCommand.Execute(null);
                        }
                    }
                }
            }
        }

        private void UnpackExecute()
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
                        ObservableSettings settings = this.Settings.Clone();
                        for (int i = this.UnpackStatus.Projects.Count - 1; i >= 0; i--)
                        {
                            if (!this.UnpackStatus.Abort)
                            {
                                num++;
                                this.UnpackStatus.Progress = Math.Floor(num / total * 100);
                                if (!Unpacks.Do(this.UnpackStatus.Projects[i], settings, out KeyValuePair<string, bool> log))
                                {
                                    this.UnpackStatus.HasError = true;
                                }
                                this.UnpackStatus.Projects.RemoveAt(i);
                                this.UnpackStatus.Log.Add(log);
                            }
                        }
                        this.UnpackStatus.Progress = 110;
                        Thread.Sleep(500);
                        this.UnpackStatus.Progress = 0;
                        this.UnpackStatus.Running = false;
                    });
                }
            }
        }
    }
}