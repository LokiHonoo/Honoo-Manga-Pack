﻿using Microsoft.VisualBasic.FileIO;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Writers;
using System.IO;
using System.Text;

namespace Honoo.MangaPack.Models
{
    internal static class Pack
    {
        private static readonly WriterOptions _writerOptions = new(CompressionType.None)
        {
            ArchiveEncoding = new ArchiveEncoding(Encoding.UTF8, Encoding.UTF8)
        };

        internal static bool Do(string path, RuntimePackSettings settings, out Tuple<string, bool, Exception?> log)
        {
            if (Directory.Exists(path))
            {
                string title = Path.GetFileName(path);
                int remove = path.Length;
                string dir = path;
                if (settings.ResetName)
                {
                    string[] d = Directory.GetDirectories(dir);
                    string[] f = Directory.GetFiles(dir);
                    while (d.Length == 1 && f.Length == 0)
                    {
                        dir = d[0];
                        remove = dir.Length;
                        string t = Path.GetFileName(dir);
                        if (t.Length > title.Length)
                        {
                            title = t;
                        }
                        d = Directory.GetDirectories(dir);
                        f = Directory.GetFiles(dir);
                    }
                }
                if (settings.AddTag && !string.IsNullOrWhiteSpace(settings.SelectedTag) && !title.Contains(settings.SelectedTag, StringComparison.CurrentCulture))
                {
                    title = $"{title} {settings.SelectedTag}";
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
                    string zip = Path.Combine(settings.WorkDirectly, $"{title}.zip");
                    int n = 1;
                    while (File.Exists(zip))
                    {
                        zip = Path.Combine(settings.WorkDirectly, $"{title} ({n}).zip");
                        n++;
                    }
                    try
                    {
                        using var archive = ZipArchive.Create();
                        foreach (var file in files)
                        {
                            string key = file[remove..];
                            if (settings.AddTopTitle)
                            {
                                key = $"{title}{key}";
                            }
                            archive.AddEntry(key, file);
                        }
                        archive.SaveTo(zip, _writerOptions);
                    }
                    catch (Exception ex)
                    {
                        log = new(path, false, ex);
                        return false;
                    }
                    if (settings.MoveToRecycleBin)
                    {
                        FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                    log = new Tuple<string, bool, Exception?>(path, true, null);
                    return true;
                }
                log = new Tuple<string, bool, Exception?>(path, false, new FileNotFoundException("Connnot find files."));
                return false;
            }
            log = new Tuple<string, bool, Exception?>(path, false, new DirectoryNotFoundException("Directory not exists."));
            return false;
        }
    }
}