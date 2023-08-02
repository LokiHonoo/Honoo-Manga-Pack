using Honoo.MangaPack.Models;
using System.IO;

namespace Honoo.MangaPack.Classes
{
    internal sealed class RuntimeUnpackSettings
    {
        internal RuntimeUnpackSettings(ObservableSettings settings)
        {
            this.WorkDirectly = Path.Combine(settings.WorkDirectly, "Unpacks");
            this.ResetName = settings.ResetName;
            this.MoveToRecycleBin = settings.MoveToRecycleBin;
            this.Passwords = [.. settings.Passwords];
        }

        internal bool MoveToRecycleBin { get; }
        internal string[] Passwords { get; }
        internal bool ResetName { get;  }
        internal string WorkDirectly { get; }
    }
}