using Honoo.MangaPack.Models;
using System.IO;

namespace Honoo.MangaPack.Classes
{
    internal sealed class RuntimeUnpackSettings
    {
        internal RuntimeUnpackSettings()
        {
            this.WorkDirectly = Path.Combine(ModelLocator.MainSettings.WorkDirectly, "Unpacks");
            this.ResetName = ModelLocator.MainSettings.ResetName;
            this.MoveToRecycleBin = ModelLocator.MainSettings.MoveToRecycleBin;
            List<string[]> passwords = [];
            foreach (var password in ModelLocator.PasswordSettings.Passwords)
            {
                passwords.Add(password);
            }
            passwords.Sort((x, y) => { return string.CompareOrdinal(x[1], y[1]); });
            this.Passwords = passwords;
        }

        internal bool MoveToRecycleBin { get; }
        internal ICollection<string[]> Passwords { get; }
        internal bool ResetName { get; }
        internal string WorkDirectly { get; }
    }
}