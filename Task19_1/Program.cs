using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Task19_1
{
    class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG 
                string rootDirectory = @"D:\projects\visual_studio\BaseCamp\Task21\bin\Debug";
                string outputFile = "test.txt";
#else
                if (args.Length < 2)
                    return;
                string rootDirectory = args[0];
                string outputFile = args[1];
#endif

            long totalTime = 0;
            SHA256 ManagerSHA256 = SHA256Managed.Create();

            var files = CalculateDirectory(rootDirectory, ref totalTime, ManagerSHA256);

            Process p = Process.GetCurrentProcess();
            double userProcessorTm = p.UserProcessorTime.TotalMilliseconds;

            Console.WriteLine($"Time: {totalTime}");
            Console.WriteLine($"Time: {userProcessorTm}");
            Console.WriteLine();

            using (StreamWriter file = new StreamWriter(rootDirectory + "\\" + outputFile))
            {
                foreach (var fileHash in files)
                    file.WriteLine(fileHash.Hash + " - " + fileHash.Name);
                file.WriteLine($"Performance: { (double)files.Sum(x => x.Size) * 1000 / ((double)totalTime * 1024 * 1024) } MB / s (by CPU time)");
                file.WriteLine($"Performance: { (double)files.Sum(x => x.Size) * 1000 / (userProcessorTm * 1024 * 1024) } MB / s (by CPU time)");
            }
            
            //foreach (var file in files)
            //    Console.WriteLine($"{file.Name}  --  {file.Hash}");

            //Console.ReadKey();
        }

        static IEnumerable<FileInformation> CalculateDirectory(string path, ref long totalTime, HashAlgorithm hashAlgo)
        {
            Stopwatch clock = Stopwatch.StartNew();
            long time = 0;
            var files = Directory.GetFiles(path);
            var fileHashes = new ConcurrentBag<FileInformation>();
            Parallel.ForEach(files, fileName =>
            {
                Stopwatch subclock = Stopwatch.StartNew();
                fileHashes.Add(GetFileinfo(fileName, hashAlgo));
                subclock.Stop();
                Interlocked.Add(ref time, subclock.ElapsedMilliseconds);
            });
            var subDirectories = Directory.GetDirectories(path);
            clock.Stop();
            if (subDirectories.Length > 0)
                foreach (var dir in subDirectories)
                    if (Directory.GetFiles(dir).Length > 0)
                        fileHashes.Concat(CalculateDirectory(dir, ref totalTime, hashAlgo));
            Interlocked.Add(ref totalTime, clock.ElapsedMilliseconds);
            Interlocked.Add(ref totalTime, time);
            return fileHashes;
        }

        static FileInformation GetFileinfo(string path, HashAlgorithm hashAlgo)
        {
            FileInformation file = new FileInformation();
            file.Name = path;
            file.Size = new FileInfo(path).Length;
            try
            {
                FileStream filestream = new FileStream(path, FileMode.Open) { Position = 0 };
                file.Hash = GetHashSum(hashAlgo, filestream);
                filestream.Close();
            }
            catch (Exception e)
            {
                file.Hash = "file is used";
            }
            return file;
        }

        static string GetHashSum(HashAlgorithm hashAlgo, FileStream filestream)
        {
            byte[] hashValue = hashAlgo.ComputeHash(filestream);
            return BitConverter.ToString(hashValue).Replace("-", String.Empty);
        }
    }

    class FileInformation
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public long Size { get; set; }

        public FileInformation(string name, string hash, long size)
        {
            Name = name;
            Hash = hash;
            Size = size;
        }

        public FileInformation() { }
    }
}