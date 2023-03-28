using iText.Kernel.Pdf;
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

        private static readonly ReaderOptions _readOptions = new()
        {
            ArchiveEncoding = new ArchiveEncoding(Encoding.UTF8, Encoding.UTF8),
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
            string dir = Path.GetFileNameWithoutExtension(file)!;
            try
            {
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
            string parent = Path.GetDirectoryName(file)!;
            string title = Path.GetFileNameWithoutExtension(file)!;
            string dir = Path.Combine(parent, title);
            int n = 1;
            while (Directory.Exists(dir))
            {
                dir = Path.Combine(parent, $"{title} ({n})");
                n++;
            }
            try
            {
                Directory.CreateDirectory(dir);
                using (IArchive archive = ArchiveFactory.Open(file, _readOptions))
                {
                    archive.WriteToDirectory(dir, _extractionOptions);
                }

                if (Common.Settings.StructureOption)
                {
                    string[] d = Directory.GetDirectories(dir);
                    string[] f = Directory.GetFiles(dir);
                    string dirl = dir;
                    while (d.Length == 1 && f.Length == 0)
                    {
                        dirl = d[0];
                        d = Directory.GetDirectories(dirl);
                        f = Directory.GetFiles(dirl);
                    }
                    if (dirl != dir)
                    {
                        string dirD = Directory.GetDirectories(dir)[0];
                        foreach (string e in d)
                        {
                            Directory.Move(e, dir);
                        }
                        foreach (string e in f)
                        {
                            File.Move(e, Path.Combine(dir, Path.GetFileName(e)));
                        }
                        Directory.Delete(dirD, true);
                    }
                }
                string[] t = Directory.GetFiles(dir, "Thumbs.db", SearchOption.AllDirectories);
                foreach (string e in t)
                {
                    File.Delete(e);
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