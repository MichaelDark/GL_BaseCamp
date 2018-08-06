using System;
using System.Diagnostics;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            long sum = 0;
            for (int i = 0; i < 100; i++)
            {
                var proc = Process.Start(@"D:\projects\visual_studio\BaseCamp\Task19_1\bin\Debug\Task19_1.exe", @"D:\projects\visual_studio\BaseCamp\Task19_1\bin\Debug test" + i + ".txt");
                proc.WaitForExit();
                sum += proc.ExitCode;
            }

            Console.WriteLine("avg " + (sum / 100));

            Console.ReadKey();
        }
    }
}
