using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodStore.Model
{
    class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public Product()
        {
            Id = 0;
            Name = "Empty";
            Price = 0;
        }

        public Product(long id, string name, double price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public static bool operator !=(Product p1, Product p2)
        {
            return !(p1 == p2);
        }

        public static bool operator ==(Product p1, Product p2)
        {
            return p1.Id == p2.Id &&
                p1.Name.Equals(p2.Name) &&
                p1.Price == p2.Price;
        }

        public override string ToString()
        {
            return $"{Id}: {Name} - {Math.Round(Price, 2)} hrn";
        }
    }
}
