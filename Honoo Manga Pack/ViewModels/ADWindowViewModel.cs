using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Honoo.MangaPack.Models;
using System.Globalization;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class ADWindowViewModel : ObservableObject
    {
        private ImageSource? _adImage;
        private string _checksum = string.Empty;
        private string _directory = string.Empty;
        private string[]? _selectValue;

        public ADWindowViewModel()
        {
            this.OpenFloderDialogCommand = new RelayCommand(OpenFloderDialogExecute);
            this.AddCommand = new RelayCommand(AddExecute);
        }

        public ICommand AddCommand { get; set; }
        public ImageSource? ADImage { get => _adImage; set => SetProperty(ref _adImage, value); }
        public string Checksum { get => _checksum; set => SetProperty(ref _checksum, value); }

        public string Directory { get => _directory; set => SetProperty(ref _directory, value); }
        public ICommand OpenFloderDialogCommand { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:属性不应返回数组", Justification = "<挂起>")]
        public string[]? SelectValue
        {
            get => _selectValue;
            set { SetProperty(ref _selectValue, value); }
        }

        private void AddExecute()
        {
        }

        private void OpenFloderDialogExecute()
        {
        }
    }
}