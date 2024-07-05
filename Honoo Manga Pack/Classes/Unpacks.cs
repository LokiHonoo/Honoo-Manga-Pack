using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.VisualBasic.FileIO;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using System.Globalization;
using System.IO;
using System.Text;

namespace Honoo.MangaPack.Classes
{
    internal static class Unpack
    {
        private static readonly ExtractionOptions _extractionOptions = new()
        {
            Overwrite = true,
            ExtractFullPath = true
        };

        private static readonly ReaderOptions _readerOptions = new()
        {
            ArchiveEncoding = new ArchiveEncoding(Encoding.UTF32, Encoding.UTF32),
            LookForHeader = true
        };

        internal static bool Do(string path, RuntimeUnpackSettings settings, out Tuple<string, string, bool, Exception?> log)
        {
            if (File.Exists(path))
            {
                string ext = Path.GetExtension(path).ToUpperInvariant()!;
                switch (ext)
                {
                    case ".ZIP": case ".RAR": case ".7Z": return TryDoZip(path, settings, out log);
                    case ".PDF": return TryDoPdf(path, settings, out log);
                    default:
                        log = new Tuple<string, string, bool, Exception?>(path, string.Empty, false, new IOException($"Unsupported file extension - \"{ext}\"."));
                        return false;
                }
            }
            log = new Tuple<string, string, bool, Exception?>(path, string.Empty, false, new FileNotFoundException("File not exists."));
            return false;
        }

        private static bool TryDoPdf(string path, RuntimeUnpackSettings settings, out Tuple<string, string, bool, Exception?> log)
        {
            string title = Path.GetFileNameWithoutExtension(path);
            string dir = Path.Combine(settings.WorkDirectly, title);
            int n = 1;
            while (Directory.Exists(dir))
            {
                dir = Path.Combine(settings.WorkDirectly, $"{title} ({n})");
                n++;
            }
            try
            {
                Directory.CreateDirectory(dir);
                using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var reader = new PdfReader(stream);
                using PdfDocument document = new(reader);
                int pages = document.GetNumberOfPages();
                ImageRenderListener strategy = new(pages, dir);
                PdfCanvasProcessor parser = new(strategy);
                for (var i = 1; i <= pages; i++)
                {
                    parser.ProcessPageContent(document.GetPage(i));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                log = new Tuple<string, string, bool, Exception?>(path, string.Empty, false, ex);
                return false;
            }
            if (settings.MoveToRecycleBin)
            {
                FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }
            log = new Tuple<string, string, bool, Exception?>(path, dir, true, null);
            return true;
        }

        private static bool TryDoZip(string path, RuntimeUnpackSettings settings, out Tuple<string, string, bool, Exception?> log)
        {
            string title = Path.GetFileNameWithoutExtension(path);
            string tmp = Path.Combine(settings.WorkDirectly, Path.GetRandomFileName());
            try
            {
                Directory.CreateDirectory(tmp);
            }
            catch (Exception ex)
            {
                log = new Tuple<string, string, bool, Exception?>(path, string.Empty, false, ex);
                return false;
            }
            _readerOptions.Password = null;
            if (!TryDoZip(path, tmp, out Exception? exception))
            {
                bool extracted = false;
                foreach (var password in settings.Passwords)
                {
                    _readerOptions.Password = password[0];
                    if (TryDoZip(path, tmp, out exception))
                    {
                        if (!int.TryParse(password[1], out int weights))
                        {
                            weights = 0;
                        }
                        password[1] = (weights + 1).ToString(CultureInfo.InvariantCulture);
                        extracted = true;
                        break;
                    }
                }
                if (!extracted)
                {
                    log = new Tuple<string, string, bool, Exception?>(path, string.Empty, false, exception);
                    return false;
                }
            }
            string deepDir = tmp;
            if (settings.ResetName)
            {
                string[] d = Directory.GetDirectories(deepDir);
                string[] f = Directory.GetFiles(deepDir);
                while (d.Length == 1 && f.Length == 0)
                {
                    deepDir = d[0];
                    string t = Path.GetFileName(deepDir);
                    if (t.Length > title.Length)
                    {
                        title = t;
                    }
                    d = Directory.GetDirectories(deepDir);
                    f = Directory.GetFiles(deepDir);
                }
            }
            string dir = Path.Combine(settings.WorkDirectly, title);
            int n = 1;
            while (Directory.Exists(dir))
            {
                dir = Path.Combine(settings.WorkDirectly, $"{title} ({n})");
                n++;
            }
            Directory.Move(deepDir, dir);
            if (Directory.Exists(tmp))
            {
                Directory.Delete(tmp, true);
            }
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
            if (settings.MoveToRecycleBin)
            {
                FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }
            log = new Tuple<string, string, bool, Exception?>(path, dir, true, null);
            return true;
        }

        private static bool TryDoZip(string path, string dir, out Exception? exception)
        {
            try
            {
                using (IArchive archive = ArchiveFactory.Open(path, _readerOptions))
                {
                    archive.WriteToDirectory(dir, _extractionOptions);
                }
                exception = null;
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }
    }
}