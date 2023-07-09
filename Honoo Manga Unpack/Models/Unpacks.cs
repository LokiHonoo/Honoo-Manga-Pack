﻿using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Honoo.MangaUnpack.Models
{
    internal static class Unpacks
    {
        private static readonly ExtractionOptions _extractionOptions = new()
        {
            Overwrite = false,
            ExtractFullPath = true
        };

        private static readonly ReaderOptions _readerOptions = new()
        {
            ArchiveEncoding = new ArchiveEncoding(Encoding.GetEncoding("GB2312"), Encoding.GetEncoding("GB2312")),
            LookForHeader = true
        };

        internal static bool Do(string file, IList<KeyValuePair<bool, string>> log)
        {
            if (File.Exists(file))
            {
                string ext = Path.GetExtension(file).ToLowerInvariant()!;
                switch (ext)
                {
                    case ".zip": case ".rar": case ".7z": return DoZip(file, log);
                    case ".pdf": return DoPdf(file, log);
                    case ".mobi": return DoMobi(file, log);
                    default: break;
                }
            }
            log.Add(new KeyValuePair<bool, string>(false, file));
            return false;
        }

        private static bool DoMobi(string file, IList<KeyValuePair<bool, string>> log)
        {
            log.Add(new KeyValuePair<bool, string>(false, file));
            return false;
        }

        private static bool DoPdf(string file, IList<KeyValuePair<bool, string>> log)
        {
            string root = Path.GetDirectoryName(file)!;
            if (Common.Settings.SaveTargetOption == 1)
            {
                root = Path.Combine(root, "~Manga Unpack");
            }
            string title = Path.GetFileNameWithoutExtension(file);
            string dir = Path.Combine(root, title);
            int n = 1;
            while (Directory.Exists(dir))
            {
                dir = Path.Combine(root, $"{title} ({n})");
                n++;
            }
            try
            {
                Directory.CreateDirectory(dir);
                using FileStream stream = new(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                using PdfDocument document = new(new PdfReader(stream));
                int pages = document.GetNumberOfPages();
                ImageRenderListener strategy = new(pages, dir);
                PdfCanvasProcessor parser = new(strategy);
                for (var i = 1; i <= pages; i++)
                {
                    parser.ProcessPageContent(document.GetPage(i));
                }
                log.Add(new KeyValuePair<bool, string>(true, file));
                return true;
            }
            catch (System.Exception)
            {
            }
            log.Add(new KeyValuePair<bool, string>(false, file));
            return false;
        }

        private static bool DoZip(string file, IList<KeyValuePair<bool, string>> log)
        {
            string root = Path.GetDirectoryName(file)!;
            if (Common.Settings.SaveTargetOption == 1)
            {
                root = Path.Combine(root, "~Manga Unpack");
            }
            string title = Path.GetFileNameWithoutExtension(file);
            string dir = Path.Combine(root, title);
            int n = 1;
            while (Directory.Exists(dir))
            {
                dir = Path.Combine(root, $"{title} ({n})");
                n++;
            }
            try
            {
                Directory.CreateDirectory(dir);
                using (IArchive archive = ArchiveFactory.Open(file, _readerOptions))
                {
                    archive.WriteToDirectory(dir, _extractionOptions);
                }
                string dir2 = dir;
                string title2 = title;
                if (Common.Settings.StructureOption)
                {
                    string[] d = Directory.GetDirectories(dir2);
                    string[] f = Directory.GetFiles(dir2);
                    while (d.Length == 1 && f.Length == 0)
                    {
                        string t = Path.GetFileName(d[0]);
                        if (t.Length > title2.Length)
                        {
                            title2 = t;
                        }
                        dir2 = d[0];
                        d = Directory.GetDirectories(dir2);
                        f = Directory.GetFiles(dir2);
                    }
                    if (dir2 != dir)
                    {
                        if (title2 == title)
                        {
                            foreach (var item in d)
                            {
                                Directory.Move(item, dir);
                            }
                            foreach (string item in f)
                            {
                                string fileName = Path.GetFileName(item);
                                if (!fileName.Equals("Thumbs.db", System.StringComparison.OrdinalIgnoreCase))
                                {
                                    File.Move(item, Path.Combine(dir, fileName));
                                }
                            }
                            Directory.Delete(dir2, true);
                        }
                        else
                        {
                            string dir3 = Path.Combine(root, title2);
                            n = 1;
                            while (Directory.Exists(dir3))
                            {
                                dir3 = Path.Combine(root, $"{title2} ({n})");
                                n++;
                            }
                            Directory.CreateDirectory(dir3);
                            foreach (var item in d)
                            {
                                Directory.Move(item, dir3);
                            }
                            foreach (string item in f)
                            {
                                string fileName = Path.GetFileName(item);
                                if (!fileName.Equals("Thumbs.db", System.StringComparison.OrdinalIgnoreCase))
                                {
                                    File.Move(item, Path.Combine(dir3, fileName));
                                }
                            }
                            Directory.Delete(dir, true);
                        }
                    }
                }
                log.Add(new KeyValuePair<bool, string>(true, file));
                return true;
            }
            catch
            {
            }
            log.Add(new KeyValuePair<bool, string>(false, file));
            return false;
        }
    }
}