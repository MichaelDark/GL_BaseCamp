using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task18
{
    class Program
    {
        static int count = 0;

        static void Main(string[] args)
        {
            Console.Write(Calc111(12, 100_000_000));

            Console.ReadKey();
        }

        static int Calc111(int a, int range)
        {
            Parallel.For(a, a + range + 1, x => CountNumbers(x));
            return count;
        }

        static void CountNumbers(int x)
        {
            if(HasThreeDigits(x))
            {
                Interlocked.Increment(ref count);
            }
        }

        static bool HasThreeDigits(int x, string pattern = "111")
        {
            return Convert.ToString(x, 2).Contains(pattern);
        }
    }
}
