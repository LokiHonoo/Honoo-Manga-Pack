using System.Windows;

namespace Honoo.MangaPack.Models
{
    public sealed class Settings
    {
        public int CollisionOption { get; set; }

        public int PositionLeft { get; set; }
        public int PositionTop { get; set; }
        public int SaveTargetOption { get; set; }
        public Visibility ShowSettings { get; set; }
        public bool StructureOption { get; set; }
        public bool SuffixDiff { get; set; }
        public string SuffixDiffValue { get; set; } = string.Empty;
        public bool SuffixOne { get; set; }
        public string SuffixOneValue { get; set; } = string.Empty;
        public bool Topmost { get; set; }
        public bool ZipRootOption { get; set; }
    }
}