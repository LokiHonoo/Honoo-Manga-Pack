using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Honoo.MangaPack.Models
{
    public sealed class TagSettings : ObservableObject
    {
        private readonly ObservableCollection<string> _tags = ["[中国翻訳]"];
        private bool _tagRemoveConfirm = true;
        public bool TagRemoveConfirm { get => _tagRemoveConfirm; set => SetProperty(ref _tagRemoveConfirm, value); }

        public ObservableCollection<string> Tags => _tags;
    }
}