using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Task17_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var network = Block.MakeTestData();

            DateTime before = DateTime.Now;
            foreach (var i in network)
                i.Job.Start();
            Task.WaitAll(network.Select(n => n.Job).ToArray());
            DateTime after = DateTime.Now;

            Console.WriteLine($"Working time {after - before}");
            Console.ReadLine();
        }
    }

    class Block
    {
        public string Name { get; set; }
        public List<Block> Previous { get; set; }
        public Task Job { get; set; }
        public int Duration { get; set; }

        public Block(string name, int duration, params Block[] previous)
        {
            Name = name;
            Duration = duration;
            Previous = previous.ToList();
            Job = new Task(Work);
        }

        public void Work()
        {
            if (Previous.Count != 0)
            {
                Task.WaitAll(Previous.Select(p => p.Job).ToArray());
            }
            Console.WriteLine($"Task {Name} started");
            Task.Delay(Duration * 1000).Wait();
            Console.WriteLine($"Task {Name} worked {Duration}");
        }

        public static List<Block> MakeTestData()
        {
            //    A
            //   / \
            //  •   •
            // B     C
            //  \   / \
            //   • •   •
            //    D     E
            // "A — • B" - path from A to B

            Block BlockA = new Block("A", 5);
            Block BlockB = new Block("B", 3, BlockA);
            Block BlockC = new Block("C", 2, BlockA);
            Block BlockD = new Block("D", 6, BlockB, BlockC);
            Block BlockE = new Block("E", 3, BlockC);
            return new List<Block>() { BlockA, BlockB, BlockC, BlockD, BlockE };
        }
    }
}