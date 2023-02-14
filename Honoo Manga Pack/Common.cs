using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Honoo.MangaPack
{
    internal static class Common
    {
        private static readonly string[] _passwords;

        private static readonly WriterOptions _writerOptions = new(CompressionType.None)
        {
            ArchiveEncoding = new ArchiveEncoding(Encoding.UTF8, Encoding.UTF8)
        };

        static Common()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Password.txt");
            if (File.Exists(filePath))
            {
                List<string> list = new(File.ReadAllLines(filePath));
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(list[i]))
                    {
                        list.RemoveAt(i);
                    }
                    else
                    {
                        list[i] = list[i].Trim();
                    }
                }
                _passwords = list.Distinct().ToArray();
            }
            else
            {
                _passwords = Array.Empty<string>();
            }
            //
            //
            //
            // _passwords = new string[] { "123" };
        }

        internal static ExtractionOptions ExtractionOptions { get; } = new ExtractionOptions()
        {
            Overwrite = true,
            ExtractFullPath = true
        };

        internal static bool IgnoreThumbs { get; set; } = true;
        internal static string LogFile { get; } = Path.Combine(Environment.CurrentDirectory, "Log.txt");
        internal static string OutFolder { get; } = "~Manga Pack Output";
        internal static string[] Passwords => _passwords;

        internal static ProgressBarOptions ProgressBarOptions { get; } = new()
        {
            ProgressCharacter = '\u2593',
            ProgressBarOnBottom = true,
            ForegroundColor = ConsoleColor.Yellow,
            ForegroundColorDone = ConsoleColor.Green
        };

        internal static ReaderOptions ReadOptions { get; } = new()
        {
            ArchiveEncoding = new ArchiveEncoding(Encoding.UTF8, Encoding.UTF8),
            LookForHeader = true
        };

        internal static string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version!.ToString();

        internal static IList<string> GetPaths(string input)
        {
            List<string> paths = new();
            int index = 0;
            int end;
            while (index < input.Length)
            {
                var kc = input[index];
                if (kc == '"')
                {
                    index++;
                    end = input.IndexOf('"', index);
                    string path = input[index..end];
                    if (path.Length > 0)
                    {
                        paths.Add(path);
                    }

                    index = end + 1 + 1;
                }
                else if (kc == ' ')
                {
                    index++;
                    continue;
                }
                else
                {
                    end = input.IndexOf(' ', index);
                    if (end < 0)
                    {
                        end = input.Length;
                    }
                    string path = input[index..end];
                    if (path.Length > 0)
                    {
                        paths.Add(path);
                    }
                    index = end + 1;
                }
            }
            return paths;
        }

        internal static void SaveZip(string fileName, IList<EntrySettings> entries)
        {
            using var archive = ZipArchive.Create();
            foreach (var entry in entries)
            {
                if (entry.Stream != null)
                {
                    archive.AddEntry(entry.Key, entry.Stream, true);
                }
                else
                {
                    archive.AddEntry(entry.Key, entry.File!);
                }
            }
            archive.SaveTo(fileName, _writerOptions);
        }
    }
}