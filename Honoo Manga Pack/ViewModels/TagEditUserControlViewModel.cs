using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Honoo.MangaPack.Models;
using HonooUI.WPF;
using System.Windows.Input;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class TagEditUserControlViewModel : ObservableObject
    {
        private readonly TagSettings _settings = ModelLocator.TagSettings;
        private string _tag = string.Empty;

        public TagEditUserControlViewModel()
        {
            this.AddTagCommand = new RelayCommand(AddTagExecute, () => { return !string.IsNullOrWhiteSpace(this.Tag); });
            this.MoveUpTagCommand = new RelayCommand<object?>(MoveUpTagExecute);
            this.MoveDownTagCommand = new RelayCommand<object?>(MoveDownExecute);
            this.RemoveTagCommand = new RelayCommand<object?>(RemoveTagExecute);
        }

        public ICommand AddTagCommand { get; set; }

        public ICommand MoveDownTagCommand { get; set; }

        public ICommand MoveUpTagCommand { get; set; }

        public ICommand RemoveTagCommand { get; set; }

        public TagSettings Settings => _settings;

        public string Tag { get => _tag; set => SetProperty(ref _tag, value); }

        private void AddTagExecute()
        {
            for (int i = this.Settings.Tags.Count - 1; i >= 0; i--)
            {
                if (this.Tag == this.Settings.Tags[i])
                {
                    this.Settings.Tags.RemoveAt(i);
                }
            }
            this.Settings.Tags.Insert(0, this.Tag);
            this.Tag = string.Empty;
        }

        private void MoveDownExecute(object? obj)
        {
            if (obj is string tag)
            {
                for (int i = 0; i < this.Settings.Tags.Count; i++)
                {
                    if (tag == this.Settings.Tags[i])
                    {
                        if (i != this.Settings.Tags.Count - 1)
                        {
                            this.Settings.Tags.Move(i, i + 1);
                            return;
                        }
                    }
                }
            }
        }

        private void MoveUpTagExecute(object? obj)
        {
            if (obj is string tag)
            {
                for (int i = 0; i < this.Settings.Tags.Count; i++)
                {
                    if (tag == this.Settings.Tags[i])
                    {
                        if (i != 0)
                        {
                            this.Settings.Tags.Move(i, i - 1);
                            return;
                        }
                    }
                }
            }
        }

        private void RemoveTagExecute(object? obj)
        {
            if (obj is string tag)
            {
                DialogOptions dialogOptions = new DialogOptions() { Buttons = DialogButtons.YesNo, Image = DialogImage.Information };
                DialogManager.GetDialogAgent("TagDialogHost").Show($"删除 \"{tag}\"？", string.Empty, dialogOptions, (e) =>
                {
                    if (e.DialogResult == DialogResult.Yes)
                    {
                        for (int i = this.Settings.Tags.Count - 1; i >= 0; i--)
                        {
                            if (tag == this.Settings.Tags[i])
                            {
                                this.Settings.Tags.RemoveAt(i);
                            }
                        }
                    }
                }, null);
            }
        }
    }
}