using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task13
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Book> books = new List<Book>();
            books.Add(new Book() { Title = "1", Year = 1992 });
            books.Add(new Book() { Title = "2", Year = 1934 });
            books.Add(new Book() { Title = "3", Year = 2018 });
            books.Add(new Book() { Title = "4", Year = 1934 });
            books.Add(new Book() { Title = "5", Year = 1995 });
            books.Add(new Book() { Title = "6", Year = 1997 });

            List<User> users = new List<User>();
            users.Add(new User { Name = "U1", Age = 13 });
            users.Add(new User { Name = "U2", Age = 15 });
            users.Add(new User { Name = "U3", Age = 18 });
            users.Add(new User { Name = "U4", Age = 16 });

            List<Mark> marks = new List<Mark>();
            marks.Add(new Mark() { User = users[0], Book = books[5], Value = 7 });
            marks.Add(new Mark() { User = users[2], Book = books[3], Value = 6 });
            marks.Add(new Mark() { User = users[2], Book = books[4], Value = 3 });
            marks.Add(new Mark() { User = users[1], Book = books[2], Value = 8 });
            marks.Add(new Mark() { User = users[1], Book = books[1], Value = 2 });
            marks.Add(new Mark() { User = users[0], Book = books[0], Value = 3 });
            marks.Add(new Mark() { User = users[0], Book = books[4], Value = 3 });

            var task1 = books.Count(x => x.Year > 1900 && x.Year < 2000);
            var task2 = users.Where(x => x.Age > 10);
            var task3 = users.Where(y => y.Age == users.Min(x => x.Age));
            var task4 = books
                            .Where(x => x.Year > 1900 && x.Year < 2000)
                            .GroupBy(book => (book.Year % 100) / 10 + 1, (decade, count) => new { D = decade, C = count.Count() });
            var task5 = marks.GroupBy(user => user.User, (user, titles) => new { Name = user.Name, Titles = titles.Select(o => o.Book.Title) }).OrderBy(x => x.Name);
            //var task5 = users
            //                //.Where(x => marks.Count(y => y.User.Name == x.Name) > 0)
            //                .GroupJoin(marks, user => user.Name, book => book.User.Name, (user, book) => new { Name = user.Name, Titles = book.Select(x => x.Book.Title).OrderBy(x => x) }).OrderBy(x => x.Name);
            var task6 = users.Where(x => marks.Where(y => y.User.Name == x.Name).Average(z => z.Value) < marks.Average(z => z.Value)).OrderByDescending(x => x.Name);
            var task7 = books.Where(x => marks.Count(y => y.Book.Title == x.Title) == 0).OrderBy(x => x.Title);

            var task8 = books.Where(book => marks.Where(mark => mark.Book.Title == book.Title).Count() > 0).OrderByDescending(x => marks.Where(mark => mark.Book.Title == x.Title).Average(y => y.Value));

            foreach (var a in task5)
                Console.WriteLine(a.Name + " " + String.Join(" ", a.Titles));
           
            Console.ReadKey();
        }

        static int Fact(int n)
        {
            return Enumerable.Range(1, n).Aggregate(1, (a, x) => a * x);
        }
    }

    class Book
    {
        public string Title { set; get; }
        public int Year { set; get; }
    }
    class User
    {
        public string Name { set; get; }
        public int Age { set; get; }
    }
    class Mark
    {
        public Book Book { set; get; }
        public User User { set; get; }
        public int Value { set; get; }
    }
}
