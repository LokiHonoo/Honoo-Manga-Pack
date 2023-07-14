using Microsoft.VisualBasic.FileIO;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Honoo.MangaPack.Models
{
    internal static class Packs
    {
        private static readonly WriterOptions _writerOptions = new(CompressionType.None)
        {
            ArchiveEncoding = new ArchiveEncoding(Encoding.UTF8, Encoding.UTF8)
        };

        internal static bool Do(string path, ObservableSettings settings, out KeyValuePair<string, bool> log)
        {
            if (Directory.Exists(path))
            {
                string dir = path;
                string root = Path.GetDirectoryName(dir)!;
                if (settings.PackSaveTo)
                {
                    root = Path.Combine(root, "~Manga Pack");
                }
                string title = Path.GetFileName(dir);
                if (settings.PackSuffixEnd && !string.IsNullOrWhiteSpace(settings.PackSuffixEndValue) && !title.EndsWith(']'))
                {
                    title = $"{title} {settings.PackSuffixEndValue}";
                }
                if (settings.PackSuffixDiff && !string.IsNullOrWhiteSpace(settings.PackSuffixDiffValue) && title.IndexOf(settings.PackSuffixDiffValue) < 0)
                {
                    title = $"{title} {settings.PackSuffixDiffValue}";
                }
                int remove = dir.Length;
                if (settings.PackRemoveNested)
                {
                    string[] d = Directory.GetDirectories(dir);
                    string[] f = Directory.GetFiles(dir);
                    while (d.Length == 1 && f.Length == 0)
                    {
                        dir = d[0];
                        remove = dir.Length;
                        d = Directory.GetDirectories(dir);
                        f = Directory.GetFiles(dir);
                    }
                }
                var files = new List<string>(Directory.GetFiles(dir, "*.*", System.IO.SearchOption.AllDirectories));
                for (int i = files.Count - 1; i >= 0; i--)
                {
                    string file = files[i];
                    if (file.EndsWith("Thumbs.db", StringComparison.OrdinalIgnoreCase) || file.EndsWith("desktop.ini", StringComparison.OrdinalIgnoreCase))
                    {
                        files.RemoveAt(i);
                    }
                }
                if (files.Count > 0)
                {
                    try
                    {
                        string zip = Path.Combine(root, $"{title}.zip");
                        int n = 1;
                        while (File.Exists(zip))
                        {
                            zip = Path.Combine(root, $"{title} ({n}).zip");
                            n++;
                        }
                        using (var archive = ZipArchive.Create())
                        {
                            foreach (var file in files)
                            {
                                string key = file[remove..];
                                if (settings.PackRoot)
                                {
                                    key = $"{title}{key}";
                                }
                                archive.AddEntry(key, file);
                            }
                            archive.SaveTo(zip, _writerOptions);
                        }
                        if (settings.PackDelOrigin)
                        {
                            try
                            {
                                FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            }
                            catch
                            {
                            }
                        }
                        log = new KeyValuePair<string, bool>(path, true);
                        return true;
                    }
                    catch
                    {
                    }
                }
            }
            log = new KeyValuePair<string, bool>(path, false);
            return false;
        }
    }
}