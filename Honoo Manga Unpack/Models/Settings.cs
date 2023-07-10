using System.Windows;

namespace Honoo.MangaUnpack.Models
{
    public sealed class Settings
    {
        public bool DelOriginalOption { get; set; }
        public int PositionLeft { get; set; }
        public int PositionTop { get; set; }
        public int SaveTargetOption { get; set; }
        public bool ShowSettings { get; set; }
        public bool StructureOption { get; set; }
        public bool Topmost { get; set; }
    }
}