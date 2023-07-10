using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.VisualBasic.FileIO;
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

        internal static bool Do(string path, out KeyValuePair<bool, string> log)
        {
            if (File.Exists(path))
            {
                string ext = Path.GetExtension(path).ToLowerInvariant()!;
                switch (ext)
                {
                    case ".zip": case ".rar": case ".7z": return DoZip(path, out log);
                    case ".pdf": return DoPdf(path, out log);
                    case ".mobi": return DoMobi(path, out log);
                    default: break;
                }
            }
            log = new KeyValuePair<bool, string>(false, path);
            return false;
        }

        private static bool DoMobi(string path, out KeyValuePair<bool, string> log)
        {
            log = new KeyValuePair<bool, string>(false, path);
            return false;
        }

        private static bool DoPdf(string path, out KeyValuePair<bool, string> log)
        {
            if (File.Exists(path))
            {
                string file = path;
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
                    if (Common.Settings.DelOriginalOption)
                    {
                        try
                        {
                            FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        }
                        catch
                        {
                        }
                    }
                    log = new KeyValuePair<bool, string>(true, path);
                    return true;
                }
                catch
                {
                }
            }
            log = new KeyValuePair<bool, string>(false, path);
            return false;
        }

        private static bool DoZip(string path, out KeyValuePair<bool, string> log)
        {
            if (File.Exists(path))
            {
                string file = path;
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
                                string dir3 = Path.Combine(root, Path.GetRandomFileName());
                                Directory.Move(dir2, dir3);
                                Directory.Delete(dir, true);
                                Directory.Move(dir3, dir);
                                string[] thumbs = Directory.GetFiles(dir, "Thumbs.db", System.IO.SearchOption.AllDirectories);
                                if (thumbs.Length > 0)
                                {
                                    foreach (string thumb in thumbs)
                                    {
                                        File.Delete(thumb);
                                    }
                                }
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
                                Directory.Move(dir2, dir3);
                                Directory.Delete(dir, true);
                                string[] thumbs = Directory.GetFiles(dir3, "Thumbs.db", System.IO.SearchOption.AllDirectories);
                                if (thumbs.Length > 0)
                                {
                                    foreach (string thumb in thumbs)
                                    {
                                        File.Delete(thumb);
                                    }
                                }
                            }
                        }
                    }
                    if (Common.Settings.DelOriginalOption)
                    {
                        try
                        {
                            FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        }
                        catch
                        {
                        }
                    }
                    log = new KeyValuePair<bool, string>(true, path);
                    return true;
                }
                catch
                {
                }
            }
            log = new KeyValuePair<bool, string>(false, path);
            return false;
        }
    }
}