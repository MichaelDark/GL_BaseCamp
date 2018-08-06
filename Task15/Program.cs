using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task15
{
    class Program
    {
        static void Main(string[] args)
        {
            BlockchainDuration test = new BlockchainDuration(Block.MakeTestData());
            test.CalculateDuration(true);

            //Warehouse w = new Warehouse(10);
            //w.Start();
            //Thread.Sleep(10000);
            //w.Stop();

            //int sum = 0, mul = 0;
            //var flagSum = new ManualResetEvent(false);
            //var flagMul = new ManualResetEvent(false);
            //Thread sumThread = new Thread(() =>
            //{
            //    sum = 0;
            //    for (int i = 1; i <= 10; i++)
            //    {
            //        sum += i;
            //        Console.WriteLine("sum: " + sum);
            //        Thread.Sleep(100);
            //    }
            //    flagSum.Set();
            //});
            //Thread mulThread = new Thread(() =>
            //{
            //    mul = 1;
            //    for (int i = 1; i <= 10; i++)
            //    {
            //        mul *= i;
            //        Console.WriteLine("mul:" + mul);
            //        Thread.Sleep(100);
            //    }
            //    flagMul.Set();
            //});
            //sumThread.Start();
            //mulThread.Start();
            //WaitHandle.WaitAll(new [] { flagSum, flagSum });
            ////sumThread.Join();
            ////mulThread.Join();
            //Console.WriteLine("Result: " + (mul / sum));

            //Method1();

            Console.ReadKey();
        }

        public static void Method1()
        {
            Console.WriteLine("M1 Start");
            Thread t = new Thread( () => { Method2(); } );
            t.Start();
            t.Join(2000);
            if(t.IsAlive)
            {
                t.Abort();
                Console.WriteLine("M2 Aborted");
            }
            Console.WriteLine("M1 Stop");
        }
        public static void Method2()
        {
            Console.WriteLine("M2 Start");
            Thread.Sleep(1000);
            //Thread.Sleep(3000);
            Console.WriteLine("M2 Stop");
        }
    }

    public class BlockchainDuration
    {
        public Block ParentBlock { get; set; }
        int duration;

        public BlockchainDuration(Block block)
        {
            ParentBlock = block;
        }

        public void CalculateDuration(bool logging = false)
        {
            duration = 0;
            DoBlock(ParentBlock, logging);
        }

        void DoBlock(Block block, bool logging)
        {
            Thread t = new Thread(() =>
            {
                duration += block.Duration;
                if(logging)
                {
                    Console.WriteLine("Block name    : " + block.Name);
                    Console.WriteLine("Total duration: " + duration);
                    Console.WriteLine();
                }

                foreach (Block child in block)
                {
                    DoBlock(child, logging);
                }
            });
            t.Start();
        }
    }
    
    public class Block : IEnumerable
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public Delegate Actions { get; set; }
        public List<Block> ChildBlocks { get; set; }

        public Block(string name, int duration)
        {
            Name = name;
            Duration = duration;
            Actions = null;
            ChildBlocks = new List<Block>();
        }
        public Block(string name, int duration, Delegate action) : this(name, duration)
        {
            Actions = action;
        }

        public void AddChild(Block b)
        {
            ChildBlocks.Add(b);
        }

        public Block this[int n]
        {
            get { return ChildBlocks[n]; }
            set { ChildBlocks[n] = value; }
        }

        public static Block MakeTestData()
        {
            Block parent = new Block("A", 5);
            
            parent.AddChild(new Block("B", 4));
            parent.AddChild(new Block("C", 2));
            parent.AddChild(new Block("D", 3));

            parent[0].AddChild(new Block("E", 2));
            parent[1].AddChild(new Block("F", 3));

            parent[0][0].AddChild(new Block("G", 1));
            parent[0][0].AddChild(new Block("H", 5));
            parent[1][0].AddChild(new Block("I", 4));

            parent[1][0][0].AddChild(new Block("J", 6));
            parent[1][0][0].AddChild(new Block("K", 3));

            //       A5
            //     / | \
            //    B4 C2 D3
            //    |   | 
            //    E2 F3
            //   /|   |
            // G1 H5 I4
            //        | \
            //        J6 K3
            return parent;
        }

        public IEnumerator GetEnumerator()
        {
            return ChildBlocks.GetEnumerator();
        }
    }

    public class Warehouse
    {
        const int LIMIT = 100;
        int store;
        bool alive;
        object o = new object();

        public Warehouse(int n)
        {
            store = n;
        }

        public void Start()
        {
            alive = true;
            StartConsumer();
            StartProvider();
        }

        public void Stop()
        {
            alive = false;
        }

        public void StartConsumer()
        {
            new Thread(() =>
            {
                while (true)
                {
                    lock(o)
                        if (store > 0)
                            store--;
                    PrintStore();
                    Thread.Sleep(600);
                    if (!alive)
                        Thread.CurrentThread.Abort();
                }
            }).Start();
        }

        public void StartProvider()
        {
            new Thread(() =>
            {
                while (true)
                {
                    lock(o)
                        if (store < LIMIT)
                            store++;
                    PrintStore();
                    Thread.Sleep(100);
                    if (!alive)
                        Thread.CurrentThread.Abort();
                }
            }).Start();
        }

        public void PrintStore()
        {
            Console.WriteLine("Store: " + store);
        }
    }
}
