using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class ObservableWorkbench : ObservableObject
    {
        private bool _abort;
        private bool _hasError;
        private bool _isRunning;
        private ObservableCollection<Tuple<string, bool, Exception?>> _Log = [];
        private double _progress;
        private ObservableCollection<string> _projects = [];
        public bool Abort { get => _abort; set => SetProperty(ref _abort, value); }
        public bool HasError { get => _hasError; set => SetProperty(ref _hasError, value); }
        public bool IsRunning { get => _isRunning; set => SetProperty(ref _isRunning, value); }
        public ObservableCollection<Tuple<string, bool, Exception?>> Log { get => _Log; set => SetProperty(ref _Log, value); }
        public double Progress { get => _progress; set => SetProperty(ref _progress, value); }
        public ObservableCollection<string> Projects { get => _projects; set => SetProperty(ref _projects, value); }
    }
}