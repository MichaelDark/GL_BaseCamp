using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task17
{
    class Program
    {
        static void Main()
        {
            Console.Write("before");
            Demo();
            Console.Write("after");

            Console.ReadKey();
        }

        static async void Demo()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            var t = await FactorialAsync(4, source.Token);
            Console.Write(t);
            Console.Write("hi");
        }

        static async Task<int> FactorialAsync(int n, CancellationToken token)
        {
            return await Task.Run(() => Factorial(n, token));
        }

        static int Factorial(int n, CancellationToken token)
        {
            int prod = 1;
            for (int i = 1; i <= n; i++)
            {
                prod *= i;
                Thread.Sleep(200);
                Console.WriteLine($"{i * 0.2} sec");
                if (token.IsCancellationRequested)
                {
                    throw new Exception("111");
                }
                //token.ThrowIfCancellationRequested();    // it is better
            }
            return prod;
        }
    }
}
