using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Honoo.MangaPack.Models
{
    public sealed class ADSettings : ObservableObject
    {
        private readonly ObservableCollection<string[]> _ads = [];

        public ObservableCollection<string[]> ADs => _ads;
    }
}