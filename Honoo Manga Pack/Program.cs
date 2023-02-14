using Honoo.Windows;
using System;
using System.Text;

namespace Honoo.MangaPack
{
    internal class Program
    {
        #region Main

        private static void Main()
        {
            ConsoleHelper.DisableQuickEditMode();
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
        //
        Start:
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================================================================================================");
                Console.WriteLine();
                Console.WriteLine("                                             漫画打包工具 Ver. " + Common.Version);
                Console.WriteLine();
                Console.WriteLine("                                    Copyright (C) Loki Honoo 2022. All rights reserved.");
                Console.WriteLine();
                Console.WriteLine("========================================================================================================================");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  1. 文件夹打包 ZIP （无压缩）");
                Console.WriteLine("                                                                                    Q. 忽略 Thumbs.db 文件 -- {0}", Common.IgnoreThumbs);
                Console.WriteLine("  2. 解压缩 ZIP/RAR/7ZIP/PDF");
                Console.WriteLine();
                Console.WriteLine("  3. ZIP/RAR/7ZIP/PDF 转换 ZIP （无压缩）");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("选择一个功能：（1，2）...");
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
                        case '1': Pack.Work(); break;
                        case '2': Unpack.Work(); break;
                        case '3': Repack.Work(); break;
                        case 'q': case 'Q': Common.IgnoreThumbs = !Common.IgnoreThumbs; goto Start;
                        default: continue;
                    }
                    break;
                }
            }
        }

        #endregion Main
    }
}