using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using SharpCompress.Archives;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.IO;

namespace Honoo.MangaPack
{
    internal class Unpack
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
                Console.WriteLine($"  批量解压缩 ZIP/RAR/7Z/PDF，解包的文件夹保存在 \"源文件所在文件夹\\{Common.OutFolder}\" 中，同名文件将会被覆盖。");
                Console.WriteLine("  不支持 ZIP/RAR/7Z 分卷压缩包，不支持图文混合 PDF。在程序文件夹下的 password.txt 保存可用密码列表。");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("    1. 移除同名外包围文件夹                                                         Q. 忽略 Thumbs.db 文件 -- {0}", Common.IgnoreThumbs);
                Console.ResetColor();
                Console.WriteLine("      └─ [Comic][火之鳥]Vol_01.rar        <- 拖放此文件解压为 ->    └─ [Comic][火之鳥]Vol_01");
                Console.WriteLine("            └─ [Comic][火之鳥]Vol_01                                      └─ Vol_01");
                Console.WriteLine("                  └─ [Comic][火之鳥]Vol_01                                      ├─ 001.jpg");
                Console.WriteLine("                        └─ Vol_01                                               ├─ 002.jpg");
                Console.WriteLine("                              ├─ 001.jpg                                        ├─ 003.jpg");
                Console.WriteLine("                              ├─ 002.jpg                                        ├─ 004.jpg");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("    2. 移除所有外包围文件夹");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("  （慎用：有些漫画压缩包内部文件夹是真实作品名称，文件名是精简或修改的）");
                Console.ResetColor();
                Console.WriteLine("      └─ [Comic][火之鳥]Vol_01.rar        <- 拖放此文件解压为 ->    └─ [Comic][火之鳥]Vol_01");
                Console.WriteLine("            └─ [Comic][火之鳥]Vol_01                                      ├─ 001.jpg");
                Console.WriteLine("                  └─ [Comic][火之鳥]Vol_01                                ├─ 002.jpg");
                Console.WriteLine("                        └─ Vol_01                                         ├─ 003.jpg");
                Console.WriteLine("                              ├─ 001.jpg                                  ├─ 004.jpg");
                Console.WriteLine("                              ├─ 002.jpg                                  ├─ 005.jpg");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("选择一个功能：（Z，1，2）...");
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
                        case '1': Do(1, "1. 移除同名外包围文件夹"); break;
                        case '2': Do(2, "2. 移除所有外包围文件夹"); break;
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
                        using (ProgressBar progressBar = new(paths.Count, $"解压缩 {paths.Count} 个文件", Common.ProgressBarOptions))
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
                            Console.WriteLine($"合计 {paths.Count} 个文件，完成 {paths.Count - error} 个，{error} 个没有解压缩。打开 Log.txt 查看详情。");
                        }
                        else if (error > 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"合计 {paths.Count} 个文件，完成 {paths.Count - error} 个，{error} 个没有解压缩。");
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
                string dir = Path.Combine(mro, main);
                UnpackStatus status = UnpackStatus.Unhandled;
                using (FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    Common.ReadOptions.Password = null;
                    status = GetZipEntries(stream, main, mode, out IList<EntrySettings> entries);
                    if (status == UnpackStatus.PasswordInvalid)
                    {
                        foreach (string password in Common.Passwords)
                        {
                            Common.ReadOptions.Password = password;
                            status = GetZipEntries(stream, main, mode, out entries);
                            if (status is UnpackStatus.Unpacked or UnpackStatus.IsEmpty)
                            {
                                break;
                            }
                        }
                    }
                    if (status == UnpackStatus.Unpacked)
                    {
                        foreach (EntrySettings entry in entries)
                        {
                            string file = Path.Combine(dir, entry.Key.Replace('\\', Path.DirectorySeparatorChar));
                            string c = Path.GetDirectoryName(file)!;
                            if (!Directory.Exists(c))
                            {
                                Directory.CreateDirectory(c);
                            }
                            entry.Entry!.WriteToFile(file, Common.ExtractionOptions);
                        }
                    }
                    else if (status == UnpackStatus.NotZip)
                    {
                        status = UnpackPdf(stream, dir);
                    }
                }
                if (status != UnpackStatus.Unpacked)
                {
                    try
                    {
                        Directory.Delete(dir);
                    }
                    catch { }
                    log.Add($"读取文件 {path} 失败。");
                    error++;
                }
            }
            else
            {
                log.Add($"文件 {path} 不存在。");
                error++;
            }
        }

        private static UnpackStatus GetZipEntries(Stream stream, string main, int mode, out IList<EntrySettings> entries)
        {
            entries = new List<EntrySettings>();
            stream.Seek(0, SeekOrigin.Begin);
            try
            {
                using IArchive archive = ArchiveFactory.Open(stream, Common.ReadOptions);
                try
                {
                    foreach (IArchiveEntry entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            if (entry.Key.EndsWith("Thumbs.db", StringComparison.OrdinalIgnoreCase) && Common.IgnoreThumbs)
                            {
                                continue;
                            }
                            string key = entry.Key.Replace('/', '\\');
                            entries.Add(new EntrySettings(key, null, null, entry));
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
                return UnpackStatus.NotZip;
            }
            if (entries.Count > 0)
            {
                if (mode == 2)
                {
                    entries = SetNewKeys2(entries);
                }
                else
                {
                    entries = SetNewKeys1(entries, main + '\\', main);
                }
                return UnpackStatus.Unpacked;
            }
            else
            {
                return UnpackStatus.IsEmpty;
            }
        }

        private static IList<EntrySettings> SetNewKeys1(IList<EntrySettings> entries, string root, string main)
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
                return SetNewKeys1(entries, root + main + '\\', main);
            }
            else
            {
                int remove = root.Length - 1 - main.Length;
                foreach (var entry in entries)
                {
                    entry.Key = entry.Key.Remove(0, remove).Trim('\\');
                }
                return entries;
            }
        }

        private static IList<EntrySettings> SetNewKeys2(IList<EntrySettings> entries)
        {
            List<string> roots = new();
            int remove = 0;
            string key = entries[0].Key.Trim('\\');
            int index = key.IndexOf('\\');
            while (index >= 0)
            {
                roots.Add(key[..(index + 1)]);
                index = key.IndexOf('\\', index + 1);
            }
            if (roots.Count > 1)
            {
                bool same = true;
                foreach (string root in roots)
                {
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
                        remove = root.Length;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (remove > 0)
            {
                foreach (var entry in entries)
                {
                    entry.Key = entry.Key.Remove(0, remove).Trim('\\');
                }
            }
            return entries;
        }

        private static UnpackStatus UnpackPdf(Stream stream, string dir)
        {
            stream.Seek(0, SeekOrigin.Begin);
            try
            {
                using PdfDocument document = new(new PdfReader(stream));
                ImageRenderListener strategy = new(false, dir);
                PdfCanvasProcessor parser = new(strategy);
                int pages = document.GetNumberOfPages();
                for (var i = 1; i <= pages; i++)
                {
                    parser.ProcessPageContent(document.GetPage(i));
                }
                return strategy.Error == 0 ? UnpackStatus.Unpacked : UnpackStatus.PdfUnsupported;
            }
            catch
            {
                return UnpackStatus.NotPdf;
            }
        }
    }
}