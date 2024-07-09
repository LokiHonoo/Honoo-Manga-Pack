using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Honoo.MangaPack.Models
{
    public sealed class PasswordSettings : ObservableObject
    {
        private readonly ObservableCollection<string[]> _passwords = [];
        private bool _passwordRemoveConfirm = true;
        public bool PasswordRemoveConfirm { get => _passwordRemoveConfirm; set => SetProperty(ref _passwordRemoveConfirm, value); }
        public ObservableCollection<string[]> Passwords => _passwords;
    }
}