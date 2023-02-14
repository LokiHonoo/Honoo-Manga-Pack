using SharpCompress.Archives;
using System.IO;

namespace Honoo.MangaPack
{
    internal class EntrySettings
    {
        public EntrySettings(string key, string? file, Stream? stream, IArchiveEntry? entry)
        {
            Key = key;
            File = file;
            Stream = stream;
            Entry = entry;
        }

        internal IArchiveEntry? Entry { get; }
        internal string? File { get; }
        internal string Key { get; set; }
        internal Stream? Stream { get; }
    }
}