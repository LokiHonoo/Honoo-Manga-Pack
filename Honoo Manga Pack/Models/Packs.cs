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

        internal static bool Do(string dir, IList<KeyValuePair<bool, string>> log)
        {
            if (Directory.Exists(dir))
            {
                string parent = Path.GetDirectoryName(dir)!;
                if (!string.IsNullOrEmpty(parent))
                {
                    string title = Path.GetFileName(dir);
                    if (Common.Settings.SuffixOne && !string.IsNullOrWhiteSpace(Common.Settings.SuffixOneValue))
                    {
                        if (!title.EndsWith(']'))
                        {
                            title = $"{title} {Common.Settings.SuffixOneValue}";
                        }
                    }
                    if (Common.Settings.SuffixDiff && !string.IsNullOrWhiteSpace(Common.Settings.SuffixDiffValue))
                    {
                        if (title.IndexOf(Common.Settings.SuffixDiffValue) < 0)
                        {
                            title = $"{title} {Common.Settings.SuffixDiffValue}";
                        }
                    }
                    string root = Common.Settings.SaveTargetOption == 1 ? Path.Combine(parent, "~Manga Pack") : parent;
                    string zip = Path.Combine(root, $"{title}.zip");
                    int remove = dir.Length;
                    if (Common.Settings.StructureOption)
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
                                if (Common.Settings.ZipRootOption)
                                {
                                    key = $"{title}{key}";
                                }
                                archive.AddEntry(key, file);
                            }
                        }
                        if (archive.Entries.Count > 0)
                        {
                            if (Common.Settings.CollisionOption == 1)
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
                            log.Add(new KeyValuePair<bool, string>(true, dir));
                            return true;
                        }
                    }
                }
            }
            log.Add(new KeyValuePair<bool, string>(false, dir));
            return false;
        }
    }
}