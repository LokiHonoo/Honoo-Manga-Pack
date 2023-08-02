using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Honoo.MangaPack.Classes;
using Honoo.MangaPack.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class MainWindowViewModel : ObservableObject
    {
        private ObservableWorkbench _packWorkbench = new();
        private ObservableSettings _settings = ModelLocator.ObservableSettings;
        private ObservableWorkbench _unpackWorkbench = new();

        public MainWindowViewModel()
        {
            this.BrowserWorkDirectlyCommand = new RelayCommand(BrowserWorkDirectlyCommandExecute);
            this.UnpackDropCommand = new RelayCommand<DragEventArgs>(UnpackDropExecute, (e) => { return !this.UnpackWorkbench.IsRunning; });
            this.UnpackCommand = new RelayCommand(UnpackExecute);
            this.UnpackClearCommand = new RelayCommand(UnpackClearExecute, () => { return !this.UnpackWorkbench.IsRunning; });
            this.PackDropCommand = new RelayCommand<DragEventArgs>(PackDropExecute, (e) => { return !this.PackWorkbench.IsRunning; });
            this.PackCommand = new RelayCommand(PackExecute);
            this.PackClearCommand = new RelayCommand(PackClearExecute, () => { return !this.PackWorkbench.IsRunning; });
        }

        public ICommand BrowserWorkDirectlyCommand { get; set; }

        public ICommand PackClearCommand { get; set; }

        public ICommand PackCommand { get; set; }

        public ICommand PackDropCommand { get; set; }

        public ObservableWorkbench PackWorkbench { get => _packWorkbench; set => SetProperty(ref _packWorkbench, value); }

        public ObservableSettings Settings { get => _settings; set => SetProperty(ref _settings, value); }

        public ICommand UnpackClearCommand { get; set; }

        public ICommand UnpackCommand { get; set; }

        public ICommand UnpackDropCommand { get; set; }

        public ObservableWorkbench UnpackWorkbench { get => _unpackWorkbench; set => SetProperty(ref _unpackWorkbench, value); }

        private void BrowserWorkDirectlyCommandExecute()
        {
            OpenFolderDialog dialog = new()
            {
                InitialDirectory = this.Settings.WorkDirectly
            };
            bool? result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                this.Settings.WorkDirectly = dialog.FolderName;
            }
        }

        private void PackClearExecute()
        {
            this.PackWorkbench.Projects.Clear();
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
                            foreach (var project in this.PackWorkbench.Projects)
                            {
                                if (project == entry)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            if (!exists)
                            {
                                this.PackWorkbench.Projects.Add(entry);
                            }
                        }
                        if (this.Settings.ExecuteAtDrop && this.PackWorkbench.Projects.Count > 0)
                        {
                            this.PackCommand.Execute(null);
                        }
                    }
                }
            }
        }

        private void PackExecute()
        {
            if (this.PackWorkbench.IsRunning)
            {
                this.PackWorkbench.Abort = true;
            }
            else
            {
                if (this.PackWorkbench.Projects.Count > 0)
                {
                    this.PackWorkbench.Abort = false;
                    this.PackWorkbench.IsRunning = true;
                    this.PackWorkbench.Log.Clear();
                    this.PackWorkbench.HasError = false;
                    Task.Run(() =>
                    {
                        bool dirOk = true;
                        RuntimePackSettings settings = new(_settings);
                        if (!Directory.Exists(settings.WorkDirectly))
                        {
                            try
                            {
                                Directory.CreateDirectory(settings.WorkDirectly);
                            }
                            catch (Exception ex)
                            {
                                Tuple<string, bool, Exception?> log = new(settings.WorkDirectly, false, ex);
                                this.PackWorkbench.Log.Add(log);
                                dirOk = false;
                            }
                        }
                        if (dirOk)
                        {
                            for (int i = this.PackWorkbench.Projects.Count - 1; i >= 0; i--)
                            {
                                if (!this.PackWorkbench.Abort)
                                {
                                    if (!Pack.Do(this.PackWorkbench.Projects[i], settings, out Tuple<string, bool, Exception?> log))
                                    {
                                        this.PackWorkbench.HasError = true;
                                    }
                                    this.PackWorkbench.Projects.RemoveAt(i);
                                    this.PackWorkbench.Log.Add(log);
                                }
                            }
                        }
                        this.PackWorkbench.IsRunning = false;
                    });
                }
            }
        }

        private void UnpackClearExecute()
        {
            this.UnpackWorkbench.Projects.Clear();
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
                            foreach (var entry2 in this.UnpackWorkbench.Projects)
                            {
                                if (entry2 == entry)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            if (!exists)
                            {
                                this.UnpackWorkbench.Projects.Add(entry);
                            }
                        }
                        if (this.Settings.ExecuteAtDrop && this.UnpackWorkbench.Projects.Count > 0)
                        {
                            this.UnpackCommand.Execute(null);
                        }
                    }
                }
            }
        }

        private void UnpackExecute()
        {
            if (this.UnpackWorkbench.IsRunning)
            {
                this.UnpackWorkbench.Abort = true;
            }
            else
            {
                if (this.UnpackWorkbench.Projects.Count > 0)
                {
                    this.UnpackWorkbench.Abort = false;
                    this.UnpackWorkbench.IsRunning = true;
                    this.UnpackWorkbench.Log.Clear();
                    this.UnpackWorkbench.HasError = false;
                    Task.Run(() =>
                    {
                        RuntimeUnpackSettings settings = new(_settings);
                        for (int i = this.UnpackWorkbench.Projects.Count - 1; i >= 0; i--)
                        {
                            if (!this.UnpackWorkbench.Abort)
                            {
                                if (!Unpack.Do(this.UnpackWorkbench.Projects[i], settings, out Tuple<string, bool, Exception?> log))
                                {
                                    this.UnpackWorkbench.HasError = true;
                                }
                                this.UnpackWorkbench.Projects.RemoveAt(i);
                                this.UnpackWorkbench.Log.Add(log);
                            }
                        }
                        this.UnpackWorkbench.IsRunning = false;
                    });
                }
            }
        }
    }
}