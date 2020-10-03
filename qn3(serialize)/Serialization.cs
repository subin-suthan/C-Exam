using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace Exam1
{
    [Serializable]
    public class Book
    {
        public string Author { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public override string ToString()
        {
            return string.Format($"Author: {Author}\nBook Id: {BookId}\nBook Title: {BookTitle}");
        }
    }
    class BinarySerializationDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter what to do(binary or xml)");
            string ch = Console.ReadLine();
            if(ch=="binary")
                 BinarySerialization();
            else
                  XmlSerialization();
        }
        private static void BinarySerialization()
        {
            Console.Write("Enter choice(read or write) ");
            string choice = Console.ReadLine();
            if (choice.ToLower() == "write")
                serialize();
            else
                deserialize();
        }

        private static void serialize()
        {
            Book b = new Book { Author = "Dan Brown", BookId =100 , BookTitle = "Origin" };
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("C:\\Users\\User\\source\\repos\\Exam1\\Exam1\\binary.txt", FileMode.OpenOrCreate, FileAccess.Write);
            bf.Serialize(fs, b);
            fs.Close();
        }

        private static void deserialize()
        {
            FileStream fs = new FileStream("C:\\Users\\User\\source\\repos\\Exam1\\Exam1\\binary.txt", FileMode.Open, FileAccess.Read);
            BinaryFormatter bf = new BinaryFormatter();
            Book b = bf.Deserialize(fs) as Book;
            Console.WriteLine(b.Author);
            Console.WriteLine(b.BookId);
            Console.WriteLine(b.BookTitle);
            fs.Close();
        }

        private static void XmlSerialization()
        {
            Console.WriteLine("What do U want to do today: Read or Write");
            string choice = Console.ReadLine();
            if (choice.ToLower() == "read")
                deserializingXml();
            else
                serializingXml();
        }

        private static void deserializingXml()
        {
            try
            {
                XmlSerializer sl = new XmlSerializer(typeof(Book));
                FileStream fs = new FileStream("C:\\Users\\User\\source\\repos\\Exam1\\Exam1\\data.xml", FileMode.Open, FileAccess.Read);
                Book s = (Book)sl.Deserialize(fs);
             
                Console.WriteLine(s);
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void serializingXml()
        {
            Book b = new Book();
            Console.WriteLine("Enter Author:");
            b.Author = Console.ReadLine();
            Console.WriteLine("Enter Book Id:");

            b.BookId= int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Title:");

            b.BookTitle = Console.ReadLine();
            FileStream fs = new FileStream("C:\\Users\\User\\source\\repos\\Exam1\\Exam1\\data.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            XmlSerializer sl = new XmlSerializer(typeof(Book));
            sl.Serialize(fs, b);
            fs.Flush();
            fs.Close();

        }
    }
}
