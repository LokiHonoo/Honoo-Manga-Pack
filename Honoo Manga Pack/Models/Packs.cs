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

        internal static bool Do(string path, Settings settings, out KeyValuePair<string, bool> log)
        {
            if (Directory.Exists(path))
            {
                string dir = path;
                string root = Path.GetDirectoryName(dir)!;
                if (!string.IsNullOrEmpty(root))
                {
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
                    string[] files = Directory.GetFiles(dir, "*.*", System.IO.SearchOption.AllDirectories);
                    if (files.Length > 0)
                    {
                        using var archive = ZipArchive.Create();
                        for (int i = 0; i < files.Length; i++)
                        {
                            string file = files[i];
                            if (!file.EndsWith("Thumbs.db", StringComparison.OrdinalIgnoreCase))
                            {
                                string key = file[remove..];
                                if (settings.PackRoot)
                                {
                                    key = $"{title}{key}";
                                }
                                archive.AddEntry(key, file);
                            }
                        }
                        if (archive.Entries.Count > 0)
                        {
                            string zip = Path.Combine(root, $"{title}.zip");
                            if (settings.PackNamesake == 1)
                            {
                                int n = 1;
                                while (File.Exists(zip))
                                {
                                    zip = Path.Combine(root, $"{title} ({n}).zip");
                                    n++;
                                }
                            }
                            else
                            {
                                if (File.Exists(zip))
                                {
                                    FileSystem.DeleteFile(zip, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                                }
                            }
                            if (!Directory.Exists(root))
                            {
                                Directory.CreateDirectory(root);
                            }
                            archive.SaveTo(zip, _writerOptions);
                            log = new KeyValuePair<string, bool>(path, true);
                            return true;
                        }
                    }
                }
            }
            log = new KeyValuePair<string, bool>(path, false);
            return false;
        }
    }
}