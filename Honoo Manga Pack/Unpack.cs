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
            string modeInfo;
        Start:
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
            Console.WriteLine("   不支持 ZIP/RAR/7Z 分卷压缩包，不支持图文混合 PDF。在程序文件夹下的 password.txt 保存可用密码列表。");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("    1. 移除同名外包围文件夹");
            Console.WriteLine("                                                                                    Q. 忽略 Thumbs.db 文件 -- {0}", Common.IgnoreThumbs);
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("      └─ [Comic][火之鳥]Vol_01.rar        <- 拖放此文件转换为 ->    └─ [Comic][火之鳥]Vol_01");
            Console.WriteLine("            └─ [Comic][火之鳥]Vol_01                                      └─ Vol_01");
            Console.WriteLine("                  └─ [Comic][火之鳥]Vol_01                                      ├─ 001.jpg");
            Console.WriteLine("                        └─ Vol_01                                               ├─ 002.jpg");
            Console.WriteLine("                              ├─ 001.jpg                                        ├─ 003.jpg");
            Console.WriteLine("                              ├─ 002.jpg                                        ├─ 004.jpg");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("选择一个功能：（Z，1）...");
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
                    case '1': modeInfo = "1. 移除同名外包围文件夹"; break;
                    case 'q': case 'Q': Common.IgnoreThumbs = !Common.IgnoreThumbs; goto Start;
                    default: continue;
                }
                break;
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"选择了功能：{modeInfo}");
                Console.WriteLine("拖放一个或多个 ZIP/RAR/7Z/PDF 文件到此窗口，回车后执行：");
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
                                Do(path, log, ref error);
                                progressBar.Tick();
                                if (log.Count >= 2000)
                                {
                                    File.AppendAllLines(Common.LogFile, log);
                                    log.Clear();
                                }
                            }
                            Console.Beep();
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
                }
                else
                {
                    Console.WriteLine("没有输入文件列表。");
                }
                Console.WriteLine();
                Console.WriteLine("按 R 键再次输入文件，按其他键返回主菜单...");
                var kc = Console.ReadKey(true).KeyChar;
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    kc = '!';
                }
                switch (kc)
                {
                    case 'r': case 'R': continue;
                    default: break;
                }
                break;
            }
        }

        #endregion Main

        private static void Do(string path, IList<string> log, ref int error)
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
                    status = UnpackZip(stream, dir);
                    if (status == UnpackStatus.PasswordInvalid)
                    {
                        foreach (string password in Common.Passwords)
                        {
                            Common.ReadOptions.Password = password;
                            status = UnpackZip(stream, dir);
                            if (status == UnpackStatus.Unpacked)
                            {
                                break;
                            }
                        }
                    }
                    if (status == UnpackStatus.NotZip)
                    {
                        // PDF
                        status = UnpackPdf(stream, dir);
                    }
                }
                if (status == UnpackStatus.Unpacked)
                {
                    if (Common.IgnoreThumbs)
                    {
                        string[] ths = Directory.GetFiles(dir, "Thumbs.db", SearchOption.AllDirectories);
                        if (ths.Length > 0)
                        {
                            foreach (string th in ths)
                            {
                                File.Delete(th);
                            }
                        }
                    }
                    string move = GetMove(dir, main);
                    if (move != dir)
                    {
                        string[] dirs = Directory.GetDirectories(move, "*.*", SearchOption.TopDirectoryOnly);
                        string[] files = Directory.GetFiles(move, "*.*", SearchOption.TopDirectoryOnly);
                        if (dirs.Length > 0)
                        {
                            foreach (string d in dirs)
                            {
                                Directory.Move(d, Path.Combine(dir, Path.GetFileName(d)));
                            }
                        }
                        if (files.Length > 0)
                        {
                            foreach (string f in files)
                            {
                                File.Move(f, Path.Combine(dir, Path.GetFileName(f)));
                            }
                        }
                        Directory.Delete(Path.Combine(dir, main!), true);
                    }
                }
                else
                {
                    try
                    {
                        Directory.Delete(dir);
                    }
                    catch
                    {
                    }
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

        private static string GetMove(string root, string main)
        {
            string[] dirs = Directory.GetDirectories(root, main, SearchOption.TopDirectoryOnly);
            string[] files = Directory.GetFiles(root, "*.*", SearchOption.TopDirectoryOnly);
            if (dirs.Length == 1 && files.Length == 0)
            {
                return GetMove(Path.Combine(root, main), main);
            }
            else
            {
                return root;
            }
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

        private static UnpackStatus UnpackZip(Stream stream, string dir)
        {
            stream.Seek(0, SeekOrigin.Begin);
            try
            {
                using IArchive archive = ArchiveFactory.Open(stream, Common.ReadOptions);
                try
                {
                    archive.WriteToDirectory(dir, Common.ExtractionOptions);
                    return UnpackStatus.Unpacked;
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
        }
    }
}