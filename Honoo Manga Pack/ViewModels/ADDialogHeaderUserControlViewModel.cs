using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class ADDialogHeaderUserControlViewModel : ObservableObject
    {
        public ADDialogHeaderUserControlViewModel()
        {
            this.OpenADWindowCommand = new RelayCommand<object?>(OpenADWindowExecute);
        }

        public ICommand OpenADWindowCommand { get; set; }

        private void OpenADWindowExecute(object? obj)
        {
            ADWindow window = new ADWindow() { Owner = (Window?)obj };
            window.ShowDialog();
        }
    }
}