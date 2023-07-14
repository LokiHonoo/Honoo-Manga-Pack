using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.VisualBasic.FileIO;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Honoo.MangaPack.Models
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

        internal static bool Do(string path, ObservableSettings settings, out KeyValuePair<string, bool> log)
        {
            if (File.Exists(path))
            {
                string ext = Path.GetExtension(path).ToLowerInvariant()!;
                switch (ext)
                {
                    case ".zip": case ".rar": case ".7z": return DoZip(path, settings, out log);
                    case ".pdf": return DoPdf(path, settings, out log);
                    case ".mobi": return DoMobi(path, settings, out log);
                    default: break;
                }
            }
            log = new KeyValuePair<string, bool>(path, false);
            return false;
        }

        private static bool DoMobi(string path, ObservableSettings settings, out KeyValuePair<string, bool> log)
        {
            log = new KeyValuePair<string, bool>(path, false);
            return false;
        }

        private static bool DoPdf(string path, ObservableSettings settings, out KeyValuePair<string, bool> log)
        {
            if (File.Exists(path))
            {
                string file = path;
                string root = Path.GetDirectoryName(file)!;
                if (settings.UnpackSaveTo)
                {
                    root = Path.Combine(root, "~Manga Pack");
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
                    if (settings.UnpackDelOrigin)
                    {
                        try
                        {
                            FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
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
            log = new KeyValuePair<string, bool>(path, false);
            return false;
        }

        private static bool DoZip(string path, ObservableSettings settings, out KeyValuePair<string, bool> log)
        {
            if (File.Exists(path))
            {
                string file = path;
                string root = Path.GetDirectoryName(file)!;
                if (settings.UnpackSaveTo)
                {
                    root = Path.Combine(root, "~Manga Pack");
                }
                try
                {
                    string title = Path.GetFileNameWithoutExtension(file);
                    string tmp = Path.Combine(root, Path.GetRandomFileName());
                    string tmpOri = tmp;
                    Directory.CreateDirectory(tmp);
                    using (IArchive archive = ArchiveFactory.Open(file, _readerOptions))
                    {
                        archive.WriteToDirectory(tmp, _extractionOptions);
                    }
                    if (settings.UnpackRemoveNested)
                    {
                        string[] d = Directory.GetDirectories(tmp);
                        string[] f = Directory.GetFiles(tmp);
                        while (d.Length == 1 && f.Length == 0)
                        {
                            tmp = d[0];
                            if (settings.UnpackResetName)
                            {
                                string title2 = Path.GetFileName(tmp);
                                if (title2.Length > title.Length)
                                {
                                    title = title2;
                                }
                            }
                            d = Directory.GetDirectories(tmp);
                            f = Directory.GetFiles(tmp);
                        }
                    }
                    string dir = Path.Combine(root, title);
                    int n = 1;
                    while (Directory.Exists(dir))
                    {
                        dir = Path.Combine(root, $"{title} ({n})");
                        n++;
                    }
                    Directory.Move(tmp, dir);
                    if (Directory.Exists(tmpOri))
                    {
                        Directory.Delete(tmpOri, true);
                    }
                    try
                    {
                        string[] dels = Directory.GetFiles(dir, "Thumbs.db", System.IO.SearchOption.AllDirectories);
                        if (dels.Length > 0)
                        {
                            foreach (string del in dels)
                            {
                                File.Delete(del);
                            }
                        }
                        dels = Directory.GetFiles(dir, "desktop.ini", System.IO.SearchOption.AllDirectories);
                        if (dels.Length > 0)
                        {
                            foreach (string del in dels)
                            {
                                File.Delete(del);
                            }
                        }
                        if (settings.UnpackDelOrigin)
                        {
                            FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        }
                    }
                    catch
                    {
                    }
                    log = new KeyValuePair<string, bool>(path, true);
                    return true;
                }
                catch
                {
                }
            }
            log = new KeyValuePair<string, bool>(path, false);
            return false;
        }
    }
}