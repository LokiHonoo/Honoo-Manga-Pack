using CommunityToolkit.Mvvm.ComponentModel;
using Honoo.MangaPack.Models;

namespace Honoo.MangaPack.ViewModels
{
    public sealed class ObservableSettings : ObservableObject
    {
        private readonly Settings _settings;

        public ObservableSettings(Settings settings)
        {
            _settings = settings;
        }

        public int PackNamesake
        {
            get => _settings.PackNamesake;
            set => SetProperty(_settings.PackNamesake, value, _settings, (model, val) => { model.PackNamesake = val; });
        }

        public bool PackRemoveNested
        {
            get => _settings.PackRemoveNested;
            set => SetProperty(_settings.PackRemoveNested, value, _settings, (model, val) => { model.PackRemoveNested = val; });
        }

        public bool PackRoot
        {
            get => _settings.PackRoot;
            set => SetProperty(_settings.PackRoot, value, _settings, (model, val) => { model.PackRoot = val; });
        }

        public bool PackSaveTo
        {
            get => _settings.PackSaveTo;
            set => SetProperty(_settings.PackSaveTo, value, _settings, (model, val) => { model.PackSaveTo = val; });
        }

        public bool PackSuffixDiff
        {
            get => _settings.PackSuffixDiff;
            set => SetProperty(_settings.PackSuffixDiff, value, _settings, (model, val) => { model.PackSuffixDiff = val; });
        }

        public string PackSuffixDiffValue
        {
            get => _settings.PackSuffixDiffValue;
            set => SetProperty(_settings.PackSuffixDiffValue, value, _settings, (model, val) => { model.PackSuffixDiffValue = val; });
        }

        public bool PackSuffixEnd
        {
            get => _settings.PackSuffixEnd;
            set => SetProperty(_settings.PackSuffixEnd, value, _settings, (model, val) => { model.PackSuffixEnd = val; });
        }

        public string PackSuffixEndValue
        {
            get => _settings.PackSuffixEndValue;
            set => SetProperty(_settings.PackSuffixEndValue, value, _settings, (model, val) => { model.PackSuffixEndValue = val; });
        }

        public bool ShowSettings
        {
            get => _settings.ShowSettings;
            set => SetProperty(_settings.ShowSettings, value, _settings, (model, val) => { model.ShowSettings = val; });
        }

        public bool Topmost
        {
            get => _settings.Topmost;
            set => SetProperty(_settings.Topmost, value, _settings, (model, val) => { model.Topmost = val; });
        }

        public bool UnpackDelOrigin
        {
            get => _settings.UnpackDelOrigin;
            set => SetProperty(_settings.UnpackDelOrigin, value, _settings, (model, val) => { model.UnpackDelOrigin = val; });
        }

        public bool UnpackRemoveNested
        {
            get => _settings.UnpackRemoveNested;
            set => SetProperty(_settings.UnpackRemoveNested, value, _settings, (model, val) => { model.UnpackRemoveNested = val; });
        }

        public bool UnpackSaveTo
        {
            get => _settings.UnpackSaveTo;
            set => SetProperty(_settings.UnpackSaveTo, value, _settings, (model, val) => { model.UnpackSaveTo = val; });
        }

        public int WindowLeft
        {
            get => _settings.WindowLeft;
            set => SetProperty(_settings.WindowLeft, value, _settings, (model, val) => { model.WindowLeft = val; });
        }

        public int WindowTop
        {
            get => _settings.WindowTop;
            set => SetProperty(_settings.WindowTop, value, _settings, (model, val) => { model.WindowTop = val; });
        }
    }
}