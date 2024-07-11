using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Honoo.MangaPack.Models;
using HonooUI.WPF;
using System.Windows.Input;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class TagDialogUserControlViewModel : ObservableObject
    {
        private readonly TagSettings _settings = ModelLocator.TagSettings;
        private string _tag = string.Empty;

        public TagDialogUserControlViewModel()
        {
            this.AddTagCommand = new RelayCommand(AddTagExecute, () => { return !string.IsNullOrWhiteSpace(this.Tag); });
            this.MoveUpTagCommand = new RelayCommand<string?>(MoveUpTagExecute);
            this.MoveDownTagCommand = new RelayCommand<string?>(MoveDownExecute);
            this.RemoveTagCommand = new RelayCommand<string?>(RemoveTagExecute);
        }

        public ICommand AddTagCommand { get; set; }

        public ICommand MoveDownTagCommand { get; set; }

        public ICommand MoveUpTagCommand { get; set; }

        public ICommand RemoveTagCommand { get; set; }

        public TagSettings Settings => _settings;

        public string Tag
        {
            get => _tag; set
            {
                SetProperty(ref _tag, value);
                ((IRelayCommand)this.AddTagCommand).NotifyCanExecuteChanged();
            }
        }

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

        private void MoveDownExecute(string? tag)
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

        private void MoveUpTagExecute(string? tag)
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

        private void RemoveTagExecute(string? tag)
        {
            DialogOptions dialogOptions = new DialogOptions() { Buttons = DialogButtons.YesNo, Image = DialogImage.Information };
            DialogManager.GetDialogHost("SubDialogHost").Show($"删除 \"{tag}\"？", string.Empty, dialogOptions, (e) =>
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