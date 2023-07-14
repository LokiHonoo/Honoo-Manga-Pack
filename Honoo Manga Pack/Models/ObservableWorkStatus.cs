using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class ObservableWorkStatus : ObservableObject
    {
        private bool _abort;
        private bool _hasError;
        private ObservableCollection<KeyValuePair<string, bool>> _Log = new();
        private double _progress;
        private ObservableCollection<string> _projects = new();
        private bool _running;

        public bool Abort { get => _abort; set => SetProperty(ref _abort, value); }
        public bool HasError { get => _hasError; set => SetProperty(ref _hasError, value); }
        public ObservableCollection<KeyValuePair<string, bool>> Log { get => _Log; set => SetProperty(ref _Log, value); }
        public double Progress { get => _progress; set => SetProperty(ref _progress, value); }
        public ObservableCollection<string> Projects { get => _projects; set => SetProperty(ref _projects, value); }
        public bool Running { get => _running; set => SetProperty(ref _running, value); }
    }
}