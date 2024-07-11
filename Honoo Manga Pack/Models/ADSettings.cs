using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Honoo.MangaPack.Models
{
    public sealed class ADSettings : ObservableObject
    {
        private readonly ObservableCollection<string[]> _ads = [];
        private bool _adRemoveConfirm = true;
        public bool ADRemoveConfirm { get => _adRemoveConfirm; set => SetProperty(ref _adRemoveConfirm, value); }
        public ObservableCollection<string[]> ADs => _ads;
    }
}