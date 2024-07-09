using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Honoo.MangaPack.Models;
using System.Windows.Input;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class ADsUserControlViewModel : ObservableObject
    {
        private readonly ADSettings _settings = ModelLocator.ADSettings;
        private string _ad = string.Empty;

        public ADsUserControlViewModel()
        {
            this.AddADCommand = new RelayCommand(AddADExecute, () => { return !string.IsNullOrWhiteSpace(this.AD); });
            this.RemoveADCommand = new RelayCommand<string?>(RemoveADExecute);
        }

        public string AD { get => _ad; set => SetProperty(ref _ad, value); }
        public ICommand AddADCommand { get; set; }
        public ICommand RemoveADCommand { get; set; }

        public ADSettings Settings => _settings;

        private void AddADExecute()
        {
            for (int i = this.Settings.ADs.Count - 1; i >= 0; i--)
            {
                if (this.AD == this.Settings.ADs[i][0])
                {
                    this.Settings.ADs.RemoveAt(i);
                }
            }
            this.Settings.ADs.Insert(0, [this.AD, ""]);
            this.AD = string.Empty;
        }

        private void RemoveADExecute(string? ad)
        {
            for (int i = this.Settings.ADs.Count - 1; i >= 0; i--)
            {
                if (ad == this.Settings.ADs[i][0])
                {
                    this.Settings.ADs.RemoveAt(i);
                }
            }
        }
    }
}