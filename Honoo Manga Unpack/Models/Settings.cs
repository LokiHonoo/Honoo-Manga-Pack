using System.Windows;

namespace Honoo.MangaUnpack.Models
{
    public sealed class Settings
    {
        public int PositionLeft { get; set; }
        public int PositionTop { get; set; }

        public Visibility ShowSettings { get; set; }
        public bool StructureOption { get; set; }

        public bool Topmost { get; set; }
    }
}