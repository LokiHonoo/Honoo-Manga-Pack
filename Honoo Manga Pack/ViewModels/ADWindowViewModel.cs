using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Honoo.MangaPack.Classes;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class ADWindowViewModel : ObservableObject
    {
        private readonly ObservableCollection<ADSearchedEntry> _entries = [];
        private ImageSource? _adImage;
        private string _checksum = string.Empty;
        private string _floder = string.Empty;
        private string[]? _selectValue;

        public ADWindowViewModel()
        {
            this.OpenFloderDialogCommand = new RelayCommand(OpenFloderDialogExecute);
            this.SearchCommand = new RelayCommand(SearchExecute);
            this.AddCommand = new RelayCommand(AddExecute);
        }

        public ICommand AddCommand { get; set; }

        public ImageSource? ADImage { get => _adImage; set => SetProperty(ref _adImage, value); }

        public string Checksum { get => _checksum; set => SetProperty(ref _checksum, value); }

        public ObservableCollection<ADSearchedEntry> Entries => _entries;
        public string Floder { get => _floder; set => SetProperty(ref _floder, value); }

        public ICommand OpenFloderDialogCommand { get; set; }

        public ICommand SearchCommand { get; set; }

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
            var openFolderDialog = new OpenFolderDialog();
            if (openFolderDialog.ShowDialog() == true)
            {
                this.Floder = openFolderDialog.FolderName;
            }
        }

        private void SearchExecute()
        {
            this.Entries.Clear();
            string[] files = Directory.GetFiles(this.Floder, "*.*", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
            }
        }
    }
}