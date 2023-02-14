using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.IO;

namespace Honoo.MangaPack
{
    internal class Pack
    {
        #region Main

        internal static void Work()
        {
            int mode;
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
            Console.WriteLine($"  批量打包文件夹，打包的 ZIP 文件保存在 \"源文件夹所在文件夹\\{Common.OutFolder}\" 中，同名文件将会被覆盖。");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("    1. 从拖放文件夹的次级内容开始打包");
            Console.WriteLine("                                                                                    Q. 忽略 Thumbs.db 文件 -- {0}", Common.IgnoreThumbs);
            Console.WriteLine("    2. 从拖放文件夹自身开始打包");
            Console.WriteLine();
            Console.WriteLine("    3. 移除同名外包围文件夹");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("      └─ [Comic][火之鳥]Vol_01            <- 拖放此文件夹转换为 ->    └─ [Comic][火之鳥]Vol_01.zip");
            Console.WriteLine("            └─ [Comic][火之鳥]Vol_01                                        └─ Vol_01");
            Console.WriteLine("                  └─ [Comic][火之鳥]Vol_01                                        ├─ 001.jpg");
            Console.WriteLine("                        └─ Vol_01                                                 ├─ 002.jpg");
            Console.WriteLine("                              ├─ 001.jpg                                          ├─ 003.jpg");
            Console.WriteLine("                              ├─ 002.jpg                                          ├─ 004.jpg");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("    4. 保留或添加一层同名外包围文件夹");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("      └─ [Comic][火之鳥]Vol_01            <- 拖放此文件夹转换为 ->    └─ [Comic][火之鳥]Vol_01.zip");
            Console.WriteLine("            └─ [Comic][火之鳥]Vol_01                                        └─ [Comic][火之鳥]Vol_01");
            Console.WriteLine("                  └─ [Comic][火之鳥]Vol_01                                        └─ Vol_01");
            Console.WriteLine("                        └─ Vol_01                                                       ├─ 001.jpg");
            Console.WriteLine("                              ├─ 001.jpg                                                ├─ 002.jpg");
            Console.WriteLine("                              ├─ 002.jpg                                                ├─ 003.jpg");
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
                    case '1': mode = 1; modeInfo = "1. 从拖放文件夹的次级内容开始打包"; break;
                    case '2': mode = 2; modeInfo = "2. 从拖放文件夹自身开始打包"; break;
                    case '3': mode = 3; modeInfo = "3. 移除同名外包围文件夹"; break;
                    case '4': mode = 4; modeInfo = "4. 保留或添加一层同名外包围文件夹"; break;
                    case 'q': case 'Q': Common.IgnoreThumbs = !Common.IgnoreThumbs; goto Start;
                    default: continue;
                }
                break;
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"选择了功能：{modeInfo}");
                Console.WriteLine("拖放一个或多个文件夹到此窗口，回车后执行：");
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
                        using (ProgressBar progressBar = new(paths.Count, $"打包 {paths.Count} 个文件夹", Common.ProgressBarOptions))
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
                            Console.Beep();
                        }
                        if (log.Count > 0)
                        {
                            File.AppendAllLines(Common.LogFile, log);
                        }
                        if (error > 20)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"合计 {paths.Count} 个文件夹，完成 {paths.Count - error} 个，{error} 个没有打包。打开 Log.txt 查看详情。");
                        }
                        else if (error > 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"合计 {paths.Count} 个文件夹，完成 {paths.Count - error} 个，{error} 个没有打包。");
                            Console.WriteLine();
                            foreach (string l in log)
                            {
                                Console.WriteLine(l);
                            }
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"合计 {paths.Count} 个文件夹，完成 {paths.Count} 个。");
                        }
                    }
                    else
                    {
                        Console.WriteLine("输入错误，应是文件夹列表。");
                    }
                }
                else
                {
                    Console.WriteLine("没有输入文件夹列表。");
                }
                Console.WriteLine();
                Console.WriteLine("按 R 键再次输入文件夹，按其他键返回主菜单...");
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

        private static void Do(string path, int mode, IList<string> log, ref int error)
        {
            if (Directory.Exists(path))
            {
                string parent = Path.GetDirectoryName(path)!;
                if (!string.IsNullOrEmpty(parent))
                {
                    string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    if (files.Length > 0)
                    {
                        IList<EntrySettings> entries = new List<EntrySettings>();
                        if (Common.IgnoreThumbs)
                        {
                            foreach (var file in files)
                            {
                                if (!file.EndsWith("Thumbs.db", StringComparison.OrdinalIgnoreCase))
                                {
                                    entries.Add(new EntrySettings(file.Replace('/', '\\'), file, null));
                                }
                            }
                        }
                        else
                        {
                            foreach (var file in files)
                            {
                                entries.Add(new EntrySettings(file.Replace('/', '\\'), file, null));
                            }
                        }
                        if (entries.Count > 0)
                        {
                            string mro = Path.Combine(parent, Common.OutFolder);
                            if (!Directory.Exists(mro))
                            {
                                Directory.CreateDirectory(mro);
                            }
                            string main = Path.GetFileName(path);
                            entries = SetNewKeys(entries, parent.Replace('/', '\\'), main, mode);
                            Common.SaveZip(Path.Combine(mro, main + ".zip"), entries);
                        }
                        else
                        {
                            log.Add($"文件夹 {path} 中没有文件。");
                            error++;
                        }
                    }
                    else
                    {
                        log.Add($"文件夹 {path} 中没有文件。");
                        error++;
                    }
                }
                else
                {
                    log.Add($"不能选择驱动器根目录 {path} 。");
                    error++;
                }
            }
            else
            {
                log.Add($"文件夹 {path} 不存在。");
                error++;
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
                            return SetNewKeys(entries, root + "\\" + main, main, mode);
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
                            return SetNewKeys(entries, root + "\\" + main, main, mode);
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
                    {
                        int remove = root.Length;
                        foreach (var entry in entries)
                        {
                            entry.Key = entry.Key.Remove(0, remove);
                        }
                        return entries;
                    }

                case 1:
                default:
                    {
                        int remove = root.Length + 1 + main.Length;
                        foreach (var entry in entries)
                        {
                            entry.Key = entry.Key.Remove(0, remove);
                        }
                        return entries;
                    }
            }
        }
    }
}