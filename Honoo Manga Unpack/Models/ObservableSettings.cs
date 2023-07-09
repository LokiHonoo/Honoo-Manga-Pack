using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Honoo.MangaUnpack.Models
{
    public sealed class ObservableSettings : ObservableObject
    {
        private readonly Settings _settings;

        public ObservableSettings(Settings settings)
        {
            _settings = settings;
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

        public Visibility ShowSettings
        {
            get => _settings.ShowSettings;
            set => SetProperty(_settings.ShowSettings, value, _settings, (o, v) => o.ShowSettings = v);
        }

        public bool StructureOption
        {
            get => _settings.StructureOption;
            set => SetProperty(_settings.StructureOption, value, _settings, (o, v) => o.StructureOption = v);
        }

        public bool Topmost
        {
            get => _settings.Topmost;
            set => SetProperty(_settings.Topmost, value, _settings, (o, v) => o.Topmost = v);
        }
    }
}