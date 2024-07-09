using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Honoo.MangaPack.Models;
using HonooUI.WPF;
using System.Globalization;
using System.Windows.Input;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class PasswordEditUserControlViewModel : ObservableObject
    {
        private readonly PasswordSettings _settings = ModelLocator.PasswordSettings;
        private string _password = string.Empty;

        public PasswordEditUserControlViewModel()
        {
            this.AddPasswordCommand = new RelayCommand(AddPasswordExecute, () => { return !string.IsNullOrWhiteSpace(this.Password); });
            this.RemovePasswordCommand = new RelayCommand<object?>(RemovePasswordExecute);
        }

        public ICommand AddPasswordCommand { get; set; }

        public string Password { get => _password; set => SetProperty(ref _password, value); }

        public ICommand RemovePasswordCommand { get; set; }

        public PasswordSettings Settings => _settings;

        private void AddPasswordExecute()
        {
            int weights = 0;
            for (int i = this.Settings.Passwords.Count - 1; i >= 0; i--)
            {
                if (this.Password == this.Settings.Passwords[i][0])
                {
                    if (int.TryParse(this.Settings.Passwords[i][1], out int w))
                    {
                        weights += w;
                    }
                    this.Settings.Passwords.RemoveAt(i);
                }
            }
            this.Settings.Passwords.Insert(0, [this.Password, weights.ToString(CultureInfo.InvariantCulture)]);
            this.Password = string.Empty;
        }

        private void RemovePasswordExecute(object? obj)
        {
            if (obj is string password)
            {
                DialogOptions dialogOptions = new DialogOptions() { Buttons = DialogButtons.YesNo, Image = DialogImage.Information };
                DialogManager.GetDialogAgent("PassordDialogHost").Show($"删除 \"{password}\"？", string.Empty, dialogOptions, (e) =>
                {
                    if (e.DialogResult == DialogResult.Yes)
                    {
                        for (int i = this.Settings.Passwords.Count - 1; i >= 0; i--)
                        {
                            if (password == this.Settings.Passwords[i][0])
                            {
                                this.Settings.Passwords.RemoveAt(i);
                            }
                        }
                    }
                }, null);
            }
        }
    }
}