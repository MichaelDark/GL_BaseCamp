using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Task17_1
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoAsync("file.txt");

            Console.ReadKey();
        }

        static async void DemoAsync(string filepath)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            try
            {
                var read = await ReadAllTextAsync(filepath, source.Token);
                Console.Write(read);
            }
            catch (AggregateException ex)
            {
                foreach (Exception e in ex.InnerExceptions)
                    Console.WriteLine($"Error: {e.Message} ");
            }
        }
        
        static Task<string> ReadAllTextAsync(string filepath, CancellationToken token)
        {
            return Task.Run(() => File.ReadAllText(filepath));
        }
    }   
}
