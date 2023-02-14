using System.IO;

namespace Honoo.MangaPack
{
    internal class EntrySettings
    {
        public EntrySettings(string key, string? file, Stream? stream)
        {
            Key = key;
            File = file;
            Stream = stream;
        }

        internal string? File { get; }
        internal string Key { get; set; }
        internal Stream? Stream { get; }
    }
}