using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Honoo.MangaPack.Models
{
    public sealed class ObservableSettings : ObservableObject
    {
        private readonly Settings _settings;

        public ObservableSettings(Settings settings)
        {
            _settings = settings;
        }

        public int CollisionOption
        {
            get => _settings.CollisionOption;
            set => SetProperty(_settings.CollisionOption, value, _settings, (o, v) => o.CollisionOption = v);
        }

        public int PositionLeft
        {
            get => _settings.PositionLeft;
            set => SetProperty(_settings.PositionLeft, value, _settings, (o, v) => o.PositionLeft = v);
        }

        public int PositionTop
        {
            get => _settings.PositionTop;
            set => SetProperty(_settings.PositionTop, value, _settings, (o, v) => o.PositionTop = v);
        }

        public int SaveTargetOption
        {
            get => _settings.SaveTargetOption;
            set => SetProperty(_settings.SaveTargetOption, value, _settings, (o, v) => o.SaveTargetOption = v);
        }

        public bool ShowSettings
        {
            get => _settings.ShowSettings;
            set => SetProperty(_settings.ShowSettings, value, _settings, (o, v) => o.ShowSettings = v);
        }

        public bool StructureOption
        {
            get => _settings.StructureOption;
            set => SetProperty(_settings.StructureOption, value, _settings, (o, v) => o.StructureOption = v);
        }

        public bool SuffixDiff
        {
            get => _settings.SuffixDiff;
            set => SetProperty(_settings.SuffixDiff, value, _settings, (o, v) => o.SuffixDiff = v);
        }

        public string SuffixDiffValue
        {
            get => _settings.SuffixDiffValue;
            set => SetProperty(_settings.SuffixDiffValue, value, _settings, (o, v) => o.SuffixDiffValue = v);
        }

        public bool SuffixOne
        {
            get => _settings.SuffixOne;
            set => SetProperty(_settings.SuffixOne, value, _settings, (o, v) => o.SuffixOne = v);
        }

        public string SuffixOneValue
        {
            get => _settings.SuffixOneValue;
            set => SetProperty(_settings.SuffixOneValue, value, _settings, (o, v) => o.SuffixOneValue = v);
        }

        public bool Topmost
        {
            get => _settings.Topmost;
            set => SetProperty(_settings.Topmost, value, _settings, (o, v) => o.Topmost = v);
        }

        public bool ZipRootOption
        {
            get => _settings.ZipRootOption;
            set => SetProperty(_settings.ZipRootOption, value, _settings, (o, v) => o.ZipRootOption = v);
        }
    }
}