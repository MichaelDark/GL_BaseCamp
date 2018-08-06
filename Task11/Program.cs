using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task11
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var i = new RangeEnum(1, 10); i.MoveNext();)
                Console.Write(i.Current + ", ");
            Console.WriteLine();

            IEnumerable near = Nearest(21);
            for (var i = near.GetEnumerator(); i.MoveNext();)
                Console.Write(i.Current + ", ");
            Console.WriteLine();

            IEnumerable queen = Queen("a1");
            for (var i = queen.GetEnumerator(); i.MoveNext();)
                Console.Write(i.Current + ", ");
            Console.WriteLine();

            Console.ReadKey();
        }

        static bool InBoard(int x)
        {
            return x > -1 && x < 8;
        }

        static IEnumerable<string> Queen(string q)
        {
            KeyValuePair<int, int> point = GetCellCords(q);
            int x = point.Key;
            int y = point.Value;

            for (int i = 0; i < 8; i++)
            {
                if (i != x)
                    yield return FormatCellName(i, y);
                if (i != y)
                    yield return FormatCellName(x, i);
            }

            for (int i = -7; i < 8; i++)
            {
                int valX = x + i;
                int valY1 = y + i;
                int valY2 = y - i;

                if (x + i != x && InBoard(valX) && InBoard(valY1))
                    yield return FormatCellName(valX, valY1);
                if (x + i != x && InBoard(valX) && InBoard(valY2))
                    yield return FormatCellName(valX, valY2);
            }
        }

        static string FormatCellName(int x, int y)
        {
            return GetLetters()[x] + (y + 1);
        }

        static KeyValuePair<int, int> GetCellCords(string q)
        {
            int x = GetLetters().FindIndex(p => p.Equals(q.Substring(0, 1)));
            int y = Convert.ToInt32(q.Substring(1, 1)) - 1;
            return new KeyValuePair<int, int>(x, y);
        }

        static List<string> GetLetters()
        {
            return (new string[] { "a", "b", "c", "d", "e", "f", "g", "h" }).ToList<string>();
        }

        static IEnumerable<int> Backward(int[] m)
        {
            return m.Reverse();
        }

        static bool InRange(int x)
        {
            return x > -1 && x < 9;
        }

        static IEnumerable<int> Nearest(int n)
        {
            int[] biasX = new int[] { -1, 0, 1, 1, 1, 0, -1, -1 };
            int[] biasY = new int[] { -1, -1, -1, 0, 1, 1, 1, 0 };
            int x = n / 10;
            int y = n % 10;
            int valX;
            int valY;

            for(int i = 0; i < 8; i++)
            {
                valX = x + biasX[i];
                valY = y + biasY[i];
                if (InRange(valX) && InRange(valY))
                {
                    yield return valX * 10 + valY;
                }
            }
        }

    }

    public class RangeEnum : IEnumerator
    {
        int from;
        int to;
        int current;

        object IEnumerator.Current => throw new NotImplementedException();

        public RangeEnum(int min, int max)
        {
            from = min;
            to = max;
            current = min - 1;
        }

        public bool MoveNext()
        {
            current++;
            return current <= to;
        }

        public int Current => current;

        public void Reset() { current = from - 1; }
    }

    class MString : IEnumerable, ICloneable
    {
        char[] chars;

        public MString(string s)
        {
            chars = s.ToCharArray();
        }

        public IEnumerator GetEnumerator()
        {
            return chars.GetEnumerator();
        }

        public object Clone()
        {
            return new MString(chars.ToString());
        }
    }

}
