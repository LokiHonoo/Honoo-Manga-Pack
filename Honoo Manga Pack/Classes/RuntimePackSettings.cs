using System.IO;

namespace Honoo.MangaPack.Models
{
    public sealed class RuntimePackSettings
    {
        internal RuntimePackSettings(ObservableSettings settings)
        {
            this.WorkDirectly = Path.Combine(settings.WorkDirectly, "Packs");
            this.ResetName = settings.ResetName;
            this.MoveToRecycleBin = settings.MoveToRecycleBin;
            this.RemoveAD = settings.RemoveAD;
            this.ADHashs = [.. settings.ADHashs];
            this.AddTopTitle = settings.AddTopTitle;
            this.AddTag = settings.AddTag;
            this.SelectedTag = settings.SelectedTag;
        }

        public bool AddTag { get; }
        public bool AddTopTitle { get; }
        public byte[][] ADHashs { get; }
        public bool ExecuteAtDrop { get; }
        public bool MoveToRecycleBin { get; }
        public bool RemoveAD { get; }
        public bool ResetName { get; }
        public string SelectedTag { get; }

        public string WorkDirectly { get; }
    }
}