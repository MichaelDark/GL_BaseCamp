using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task21
{
    class Program
    {
        const string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\projects\visual_studio\BaseCamp\BaseCampServer.mdf;Integrated Security = True; Connect Timeout = 30";
   
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                AddRecord(connection, "My message1", "Misha");
                AddRecord(connection, "My message2", "Misha");
                AddRecord(connection, "My message3", "Misha");

                AddRecord(connection, "Myn't message1", "Vlad");
                AddRecord(connection, "Myn't message2", "Vlad");
                AddRecord(connection, "Myn't message3", "Vlad");

                PrintRecords(GetAllRecords(connection), "All");
                PrintRecords(GetRecordsByAuthor(connection, "Misha"), "By Misha");

                connection.Close();
            }

            Console.ReadKey();
        }

        static void AddRecord(SqlConnection connection, string text, string author)
        {
            SqlCommand cmdInsert = connection.CreateCommand();
            cmdInsert.CommandText = "INSERT INTO Records (Text, Author, RecordDate) VALUES (@Text, @Author, @RecordDate)";
            cmdInsert.Parameters.AddWithValue("@Text", text);
            cmdInsert.Parameters.AddWithValue("@Author", author);
            cmdInsert.Parameters.AddWithValue("@RecordDate", DateTime.Now);
            cmdInsert.ExecuteNonQuery();
        }

        static List<Record> GetAllRecords(SqlConnection connection)
        {
            List<Record> records = new List<Record>();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Records";
            using (SqlDataReader dr = cmd.ExecuteReader())
                while (dr.Read())
                    records.Add(new Record((int)dr["Id"], (string)dr["Text"], (string)dr["Author"], (DateTime)dr["RecordDate"]));
            return records;
        }

        static List<Record> GetRecordsByAuthor(SqlConnection connection,  string author)
        {
            List<Record> records = new List<Record>();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Records WHERE Author=@Author";
            cmd.Parameters.AddWithValue("@Author", author);
            using (SqlDataReader dr = cmd.ExecuteReader())
                while (dr.Read())
                    records.Add(new Record((int)dr["Id"], (string)dr["Text"], (string)dr["Author"], (DateTime)dr["RecordDate"]));
            return records;
        }

        static void PrintRecords(List<Record> list, string title = null)
        {
            if (title != null) Console.WriteLine($"==={title}===");
            foreach (var item in list)
                Console.WriteLine(item);
        }
    }

    public class Record
    {
        public int Id { set; get; }
        public string Text { set; get; }
        public string Author { set; get; }
        public DateTime RecordDate { set; get; }

        public Record(int id, string text, string author, DateTime recordDate)
        {
            Id = id;
            Text = text;
            Author = author;
            RecordDate = recordDate;
        }

        public override string ToString()
        {
            return $"RECORD #{Id}, {Author}, {RecordDate}{Environment.NewLine}{Text}";
        }
    }
}
