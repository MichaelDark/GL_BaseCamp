using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCamp
{
    class Program
    {
        static void Main(string[] args)
        {
            var col = new MyCollection<int> { 1, 2, 3, 4, 6, 8, 0, 9, 2 };

            for (var e = col.GetEnumerator(); e.MoveNext();)
                Console.Write(e.Current);
            Console.WriteLine();

            for (int i = 0; i < col.Count; i++)
                Console.Write(col[i]);
            Console.WriteLine();

            foreach (int n in col)
                Console.Write(n);
            Console.WriteLine();

            foreach (int n in Iter123())
                Console.Write(n);
            Console.WriteLine();

            Console.ReadKey();
        }

        static IEnumerable<int> Iter123()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }
    }

    class MyCollection<T> : List<T>
    {
        //public new IEnumerator<T> GetEnumerator()
        //{
        //    return new MyEnumerator<T>(this);
        //}
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = Count - 1; i >= 0; i--)
                yield return this[i];
        }
    }

    class MyEnumerator<T> : IEnumerator<T>
    {
        MyCollection<T> col;  
        int idx;           

        public MyEnumerator(MyCollection<T> col)
        {
            this.col = col;
            idx = col.Count;
        }

        public T Current => col[idx];

        public bool MoveNext()
        {
            idx--;
            return (idx > -1);
        }

        public void Reset()
        {
            idx = col.Count;
        }


        object IEnumerator.Current => throw new NotImplementedException();
        public void Dispose() { }   
    }
}
