using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Roler.Toolkit.File.Mobi;
using Roler.Toolkit.File.Mobi.Entity;
using SharpCompress.Archives;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.IO;

namespace Honoo.MangaPack
{
    internal class Repack
    {
        #region Main

        internal static void Work()
        {
            while (true)
            {
                Console.Clear();
                //Console.WriteLine("========================================================================================================================");
                //Console.WriteLine();
                //Console.WriteLine("                                             漫画打包工具 Ver. " + Common.Version);
                //Console.WriteLine();
                //Console.WriteLine("                                    Copyright (C) Loki Honoo 2022. All rights reserved.");
                //Console.WriteLine();
                //Console.WriteLine("========================================================================================================================");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  Z. 返回上级");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine($"  批量转换 ZIP/RAR/7Z/PDF/mobi，转换的 ZIP 文件保存在 \"源文件所在文件夹\\{Common.OutFolder}\" 中，同名文件将会被覆盖。");
                Console.WriteLine("  不支持 ZIP/RAR/7Z 分卷压缩包，PDF/mobi 仅提取图片。在程序文件夹下的 password.txt 保存压缩包可用密码列表。");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("    1. ZIP/RAR/7Z 保留原有文件夹结构，PDF/mobi 图片保存在根文件夹                   Q. 忽略 Thumbs.db 文件 -- {0}", Common.IgnoreThumbs);
                Console.WriteLine();
                Console.WriteLine("    2. 添加一层同名外包围文件夹");
                Console.WriteLine();
                Console.WriteLine("    3. 移除同名外包围文件夹");
                Console.ResetColor();
                Console.WriteLine("      └─ [Comic][火之鳥]Vol_01.rar        <- 拖放此文件转换为 ->    └─ [Comic][火之鳥]Vol_01.zip");
                Console.WriteLine("            └─ [Comic][火之鳥]Vol_01                                      └─ Vol_01");
                Console.WriteLine("                  └─ [Comic][火之鳥]Vol_01                                      ├─ 001.jpg");
                Console.WriteLine("                        └─ Vol_01                                               ├─ 002.jpg");
                Console.WriteLine("                              ├─ 001.jpg                                        ├─ 003.jpg");
                Console.WriteLine("                              ├─ 002.jpg                                        ├─ 004.jpg");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("    4. 保留或添加一层同名外包围文件夹");
                Console.ResetColor();
                Console.WriteLine("      └─ [Comic][火之鳥]Vol_01.rar        <- 拖放此文件换为 ->    └─ [Comic][火之鳥]Vol_01.zip");
                Console.WriteLine("            └─ [Comic][火之鳥]Vol_01                                      └─ [Comic][火之鳥]Vol_01");
                Console.WriteLine("                  └─ [Comic][火之鳥]Vol_01                                      └─ Vol_01");
                Console.WriteLine("                        └─ Vol_01                                                     ├─ 001.jpg");
                Console.WriteLine("                              ├─ 001.jpg                                              ├─ 002.jpg");
                Console.WriteLine("                              ├─ 002.jpg                                              ├─ 003.jpg");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("选择一个功能：（Z，1，2，3，4）...");
                while (true)
                {
                    var kc = Console.ReadKey(true).KeyChar;
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                        kc = '!';
                    }
                    switch (kc)
                    {
                        case 'z': case 'Z': return;
                        case '1': Do(1, "1. ZIP/RAR/7Z 保留原有文件夹结构，PDF 内容保存在根文件夹"); break;
                        case '2': Do(2, "2. 添加一层同名外包围文件夹"); break;
                        case '3': Do(3, "3. 移除同名外包围文件夹"); break;
                        case '4': Do(4, "4. 保留或添加一层同名外包围文件夹"); break;
                        case 'q': case 'Q': Common.IgnoreThumbs = !Common.IgnoreThumbs; break;
                        default: continue;
                    }
                    break;
                }
            }
        }

        #endregion Main

        private static void Do(int mode, string info)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("========================================================================================================================");
                Console.WriteLine($"选择了功能：{info}");
                Console.WriteLine("拖放一个或多个 ZIP/RAR/7Z/PDF 文件到此窗口，回车后执行。直接回车返回上级菜单：");
                Console.Write("DO>");
                string input = Console.ReadLine()!;
                Console.Clear();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    input = input.Trim();
                    IList<string>? paths;
                    try
                    {
                        paths = Common.GetPaths(input);
                    }
                    catch
                    {
                        paths = null;
                    }
                    if (paths != null && paths.Count > 0)
                    {
                        File.WriteAllText(Common.LogFile, DateTime.Now.ToString() + "\r\n\r\n");
                        List<string> log = new();
                        int error = 0;
                        using (ProgressBar progressBar = new(paths.Count, $"转换 {paths.Count} 个文件", Common.ProgressBarOptions))
                        {
                            foreach (string path in paths)
                            {
                                Do(path, mode, log, ref error);
                                progressBar.Tick();
                                if (log.Count >= 2000)
                                {
                                    File.AppendAllLines(Common.LogFile, log);
                                    log.Clear();
                                }
                            }
                            if (error > 0)
                            {
                                progressBar.ObservedError = true;
                            }
                        }
                        if (log.Count > 0)
                        {
                            File.AppendAllLines(Common.LogFile, log);
                        }
                        if (error > 20)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"合计 {paths.Count} 个文件，完成 {paths.Count - error} 个，{error} 个没有转换。打开 Log.txt 查看详情。");
                        }
                        else if (error > 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"合计 {paths.Count} 个文件，完成 {paths.Count - error} 个，{error} 个没有转换。");
                            Console.WriteLine();
                            foreach (string l in log)
                            {
                                Console.WriteLine(l);
                            }
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"合计 {paths.Count} 个文件，完成 {paths.Count} 个。");
                        }
                    }
                    else
                    {
                        Console.WriteLine("输入错误，应是文件列表。");
                    }
                    Console.WriteLine();
                }
                else
                {
                    return;
                }
            }
        }

        private static void Do(string path, int mode, IList<string> log, ref int error)
        {
            if (File.Exists(path))
            {
                string mro = Path.GetDirectoryName(path)!;
                mro = Path.Combine(mro, Common.OutFolder);
                string main = Path.GetFileNameWithoutExtension(path);
                string ext = Path.GetExtension(path).ToLowerInvariant();
                UnpackStatus status = UnpackStatus.Unhandled;
                IList<EntrySettings> entries;
                string tempDir = Path.Combine(mro, main + "." + Path.GetRandomFileName());
                bool createTempDir = false;
                using (FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    switch (ext)
                    {
                        case ".mobi":
                            status = GetMobiEntries(stream, main, mode, tempDir, out createTempDir, out entries);
                            if (status == UnpackStatus.Failed)
                            {
                                Common.ReadOptions.Password = null;
                                status = GetZipEntries(stream, main, mode, tempDir, out createTempDir, out entries);
                            }
                            break;

                        case ".pdf":
                            status = GetPdfEntries(stream, main, mode, tempDir, out createTempDir, out entries);
                            if (status == UnpackStatus.Failed)
                            {
                                Common.ReadOptions.Password = null;
                                status = GetZipEntries(stream, main, mode, tempDir, out createTempDir, out entries);
                            }
                            break;

                        default:
                            Common.ReadOptions.Password = null;
                            status = GetZipEntries(stream, main, mode, tempDir, out createTempDir, out entries);
                            if (status == UnpackStatus.PasswordInvalid)
                            {
                                foreach (string password in Common.Passwords)
                                {
                                    Common.ReadOptions.Password = password;
                                    status = GetZipEntries(stream, main, mode, tempDir, out createTempDir, out entries);
                                    if (status is UnpackStatus.Unpacked or UnpackStatus.IsEmpty)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                }
                if (status == UnpackStatus.Unpacked)
                {
                    if (!Directory.Exists(mro))
                    {
                        Directory.CreateDirectory(mro);
                    }
                    Common.SaveZip(Path.Combine(mro, main + ".zip"), entries);
                }
                else
                {
                    log.Add($"读取文件 {path} 失败。");
                    error++;
                }
                if (createTempDir)
                {
                    Directory.Delete(tempDir, true);
                }
            }
            else
            {
                log.Add($"文件 {path} 不存在。");
                error++;
            }
        }

        private static UnpackStatus GetMobiEntries(Stream stream,
                                                   string main,
                                                   int mode,
                                                   string tempDir,
                                                   out bool createTempDir,
                                                   out IList<EntrySettings> entries)
        {
            createTempDir = false;
            stream.Seek(0, SeekOrigin.Begin);
            try
            {
                using var reader = new MobiReader(stream);
                Mobi mobi = reader.ReadWithoutText();
                int index = (int)mobi.Structure.MobiHeader.FirstImageIndex;
                int count = mobi.Structure.MobiHeader.LastContentRecordOffset - 1 - index;
                if (count > 0)
                {
                    int pad = count.ToString().Length;
                    IList<PalmDBRecordInfo> records = mobi.Structure.PalmDB.RecordInfoList;
                    stream.Seek(records[index].Offset, SeekOrigin.Begin);
                    if (stream.Length > 200 * 1024 * 1024)
                    {
                        if (!Directory.Exists(tempDir))
                        {
                            Directory.CreateDirectory(tempDir);
                        }
                        createTempDir = true;
                        entries = new List<EntrySettings>();
                        for (int i = 1; i < count + 1; i++)
                        {
                            string safe = i.ToString().PadLeft(pad, '0') + ".jpg";
                            string file = Path.Combine(tempDir, safe);
                            byte[] buffer = new byte[(int)records[index + 1].Offset - stream.Position];
                            stream.Read(buffer, 0, buffer.Length);
                            File.WriteAllBytes(file, buffer);
                            entries.Add(new EntrySettings(safe, file, null, null));
                            index++;
                        }
                    }
                    else
                    {
                        createTempDir = false;
                        entries = new List<EntrySettings>();
                        for (int i = 1; i < count + 1; i++)
                        {
                            string safe = i.ToString().PadLeft(pad, '0') + ".jpg";
                            byte[] buffer = new byte[(int)records[index + 1].Offset - stream.Position];
                            stream.Read(buffer, 0, buffer.Length);
                            MemoryStream temp = new(buffer);
                            entries.Add(new EntrySettings(safe, null, temp, null));
                            index++;
                        }
                    }
                }
                else
                {
                    entries = null!;
                    return UnpackStatus.IsEmpty;
                }
            }
            catch
            {
                entries = null!;
                return UnpackStatus.Failed;
            }
            if (entries.Count > 0)
            {
                entries = SetNewKeys(entries, main + '\\', main, mode);
                return UnpackStatus.Unpacked;
            }
            else
            {
                return UnpackStatus.IsEmpty;
            }
        }

        private static UnpackStatus GetPdfEntries(Stream stream,
                                                  string main,
                                                  int mode,
                                                  string tempDir,
                                                  out bool createTempDir,
                                                  out IList<EntrySettings> entries)
        {
            createTempDir = false;
            stream.Seek(0, SeekOrigin.Begin);
            try
            {
                using PdfDocument document = new(new PdfReader(stream));
                if (stream.Length > 200 * 1024 * 1024)
                {
                    if (!Directory.Exists(tempDir))
                    {
                        Directory.CreateDirectory(tempDir);
                    }
                    createTempDir = true;
                    ImageRenderListener strategy = new(false, tempDir);
                    PdfCanvasProcessor parser = new(strategy);
                    int pages = document.GetNumberOfPages();
                    for (var i = 1; i <= pages; i++)
                    {
                        parser.ProcessPageContent(document.GetPage(i));
                    }
                    entries = strategy.Entries;
                    if (strategy.Error > 0)
                    {
                        return UnpackStatus.Failed;
                    }
                }
                else
                {
                    createTempDir = false;
                    ImageRenderListener strategy = new(true, tempDir);
                    PdfCanvasProcessor parser = new(strategy);
                    int pages = document.GetNumberOfPages();
                    for (var i = 1; i <= pages; i++)
                    {
                        parser.ProcessPageContent(document.GetPage(i));
                    }
                    entries = strategy.Entries;
                    if (strategy.Error > 0)
                    {
                        return UnpackStatus.Failed;
                    }
                }
            }
            catch
            {
                entries = null!;
                return UnpackStatus.Failed;
            }
            if (entries.Count > 0)
            {
                entries = SetNewKeys(entries, main + '\\', main, mode);
                return UnpackStatus.Unpacked;
            }
            else
            {
                return UnpackStatus.IsEmpty;
            }
        }

        private static UnpackStatus GetZipEntries(Stream stream,
                                                  string main,
                                                  int mode,
                                                  string tempDir,
                                                  out bool createTempDir,
                                                  out IList<EntrySettings> entries)
        {
            createTempDir = false;
            entries = new List<EntrySettings>();
            stream.Seek(0, SeekOrigin.Begin);
            try
            {
                using IArchive archive = ArchiveFactory.Open(stream, Common.ReadOptions);
                try
                {
                    if (archive.TotalUncompressSize > 200 * 1024 * 1024)
                    {
                        if (!Directory.Exists(tempDir))
                        {
                            Directory.CreateDirectory(tempDir);
                        }
                        createTempDir = true;
                        int i = 0;
                        foreach (IArchiveEntry entry in archive.Entries)
                        {
                            if (!entry.IsDirectory)
                            {
                                if (entry.Key.EndsWith("Thumbs.db", StringComparison.OrdinalIgnoreCase) && Common.IgnoreThumbs)
                                {
                                    continue;
                                }
                                string key = entry.Key.Replace('/', '\\');
                                string temp = Path.Combine(tempDir, i.ToString());
                                entry.WriteToFile(temp);
                                entries.Add(new EntrySettings(key, temp, null, null));
                                i++;
                            }
                        }
                    }
                    else
                    {
                        createTempDir = false;
                        foreach (IArchiveEntry entry in archive.Entries)
                        {
                            if (!entry.IsDirectory)
                            {
                                if (entry.Key.EndsWith("Thumbs.db", StringComparison.OrdinalIgnoreCase) && Common.IgnoreThumbs)
                                {
                                    continue;
                                }
                                string key = entry.Key.Replace('/', '\\');
                                MemoryStream temp = new();
                                entry.OpenEntryStream().CopyTo(temp);
                                temp.Seek(0, SeekOrigin.Begin);
                                entries.Add(new EntrySettings(key, null, temp, null));
                            }
                        }
                    }
                }
                catch
                {
                    return UnpackStatus.PasswordInvalid;
                }
            }
            catch
            {
                return UnpackStatus.Failed;
            }
            if (entries.Count > 0)
            {
                entries = SetNewKeys(entries, main + '\\', main, mode);
                return UnpackStatus.Unpacked;
            }
            else
            {
                return UnpackStatus.IsEmpty;
            }
        }

        private static IList<EntrySettings> SetNewKeys(IList<EntrySettings> entries, string root, string main, int mode)
        {
            switch (mode)
            {
                case 4:
                    {
                        bool same = true;
                        foreach (var entry in entries)
                        {
                            if (!entry.Key.StartsWith(root))
                            {
                                same = false;
                                break;
                            }
                        }
                        if (same)
                        {
                            return SetNewKeys(entries, root + main + "\\", main, mode);
                        }
                        else
                        {
                            int remove = root.Length - 1 - main.Length;
                            foreach (var entry in entries)
                            {
                                string key = entry.Key.Remove(0, remove).Trim('\\');
                                key = main + "\\" + key;
                                entry.Key = key;
                            }
                            return entries;
                        }
                    }

                case 3:
                    {
                        bool same = true;
                        foreach (var entry in entries)
                        {
                            if (!entry.Key.StartsWith(root))
                            {
                                same = false;
                                break;
                            }
                        }
                        if (same)
                        {
                            return SetNewKeys(entries, root + main + "\\", main, mode);
                        }
                        else
                        {
                            int remove = root.Length - 1 - main.Length;
                            foreach (var entry in entries)
                            {
                                entry.Key = entry.Key.Remove(0, remove);
                            }
                            return entries;
                        }
                    }

                case 2:
                    foreach (var entry in entries)
                    {
                        entry.Key = main + "\\" + entry.Key;
                    }
                    return entries;

                case 1:
                default:
                    return entries;
            }
        }
    }
}