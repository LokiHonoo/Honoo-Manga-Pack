﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Honoo.IO.Hashing;
using Honoo.MangaPack.Models;
using HonooUI.WPF;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class ADDialogUserControlViewModel : ObservableObject
    {
        private readonly string _adDirectly = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ads");
        private readonly Crc32 _crc = new();
        private readonly ADSettings _settings = ModelLocator.ADSettings;
        private string _ad = string.Empty;
        private string _adFile = string.Empty;
        private ImageSource? _adImage;
        private string _checksum = string.Empty;
        private string[]? _selectValue;

        public ADDialogUserControlViewModel()
        {
            this.OpenFileDialogCommand = new RelayCommand(OpenFileDialogExecute);
            this.AddADCommand = new RelayCommand(AddADExecute, () => { return !string.IsNullOrWhiteSpace(this.AD); });
            this.RemoveADCommand = new RelayCommand<string?>(RemoveADExecute);
        }

        public string AD
        {
            get => _ad;
            set
            {
                SetProperty(ref _ad, value);
                ((IRelayCommand)this.AddADCommand).NotifyCanExecuteChanged();
            }
        }

        public ICommand AddADCommand { get; set; }
        public string ADFile { get => _adFile; set => SetProperty(ref _adFile, value); }
        public ImageSource? ADImage { get => _adImage; set => SetProperty(ref _adImage, value); }
        public string Checksum { get => _checksum; set => SetProperty(ref _checksum, value); }
        public ICommand OpenFileDialogCommand { get; set; }

        public ICommand RemoveADCommand { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:属性不应返回数组", Justification = "<挂起>")]
        public string[]? SelectValue
        {
            get => _selectValue;
            set
            {
                SetProperty(ref _selectValue, value);
                if (_selectValue != null)
                {
                    string file = Path.Combine(_adDirectly, _selectValue[0] + _selectValue[1]);
                    try
                    {
                        var img = new BitmapImage();
                        img.BeginInit();
                        img.StreamSource = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                        img.EndInit();
                        this.ADImage = img;
                    }
                    catch
                    {
                        this.ADImage = null;
                    }
                    finally
                    {
                        this.Checksum = _selectValue[0];
                    }
                }
            }
        }

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
            string ext = Path.GetExtension(this.ADFile);
            string dstFile = this.AD + ext;
            if (!Directory.Exists(_adDirectly))
            {
                Directory.CreateDirectory(_adDirectly);
            }
            File.Copy(this.ADFile, Path.Combine(_adDirectly, dstFile), true);
            this.Settings.ADs.Insert(0, [this.AD, ext]);
            this.ADFile = string.Empty;
            this.AD = string.Empty;
            this.ADImage = null;
            this.Checksum = string.Empty;
        }

        private void OpenFileDialogExecute()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.*";
            if (dialog.ShowDialog() == true)
            {
                this.ADFile = dialog.FileName;
                using (FileStream fs = new FileStream(ADFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    int b;
                    while (true)
                    {
                        b = fs.ReadByte();
                        if (b >= 0)
                        {
                            _crc.Update((byte)b);
                        }
                        else
                        {
                            break;
                        }
                    }
                    this.AD = _crc.ComputeFinal(StringFormat.Hex).ToUpperInvariant();
                }
                try
                {
                    var img = new BitmapImage();
                    img.BeginInit();
                    img.StreamSource = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    img.EndInit();
                    this.ADImage = img;
                }
                catch
                {
                    this.ADImage = null;
                }
                finally
                {
                    this.Checksum = this.AD;
                }
            }
        }

        private void RemoveADExecute(string? ad)
        {
            DialogOptions dialogOptions = new DialogOptions() { Buttons = DialogButtons.YesNo, Image = DialogImage.Information };
            DialogManager.GetDialogHost("SubDialogHost").Show($"删除 \"{ad}\"？", string.Empty, dialogOptions, (e) =>
            {
                if (e.DialogResult == DialogResult.Yes)
                {
                    for (int i = this.Settings.ADs.Count - 1; i >= 0; i--)
                    {
                        if (ad == this.Settings.ADs[i][0])
                        {
                            string file = this.Settings.ADs[i][1];
                            if (File.Exists(file))
                            {
                                File.Delete(file);
                            }
                            this.Settings.ADs.RemoveAt(i);
                        }
                    }
                }
            }, null);
        }
    }
}