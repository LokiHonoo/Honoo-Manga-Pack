using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Honoo.MangaPack.Models
{
    public sealed class PasswordSettings : ObservableObject
    {
        private readonly ObservableCollection<string[]> _passwords = [];

        public ObservableCollection<string[]> Passwords => _passwords;
    }
}