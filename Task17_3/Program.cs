using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Task17_3
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            int totalNumbers = 10_000_000;

            Stopwatch clock = Stopwatch.StartNew();
            Parallel.For(0, totalNumbers + 1, i => { if(IsPrime(i)) Interlocked.Increment(ref count); });
            clock.Stop();
            
            Console.WriteLine($"==Parallelism==");
            Console.WriteLine($"• Count(num): {count}");
            Console.WriteLine($"• Time (msc): {clock.ElapsedMilliseconds}");

            count = 0;

            clock = Stopwatch.StartNew();
            count = Enumerable.Range(0, totalNumbers + 1).AsParallel().Count(x => IsPrime(x));
            clock.Stop();

            Console.WriteLine($"==PLINQ==");
            Console.WriteLine($"• Count(num): {count}");
            Console.WriteLine($"• Time (msc): {clock.ElapsedMilliseconds}");

            Console.ReadKey();
        }

        static int CountPrime(int from, int range)
        {
            int res = 0;
            for (int i = from; i < from + range; i++)
                if (IsPrime(i))
                    res++;
            return res;
        }

        static bool IsPrime(int num)
        {
            if (num < 2)
                return false;
            int sqrt = (int)Math.Sqrt(num);
            for (int i = 2; i <= sqrt; i++)
                if (num % i == 0)
                    return false;
            return true;
        }
    }
}