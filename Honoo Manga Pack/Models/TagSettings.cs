using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Honoo.MangaPack.Models
{
    public sealed class TagSettings : ObservableObject
    {
        private readonly ObservableCollection<string> _tags = ["[中国翻訳]"];

        public ObservableCollection<string> Tags => _tags;
    }
}