using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class Book
    {
        public Book(int bookID, string title, string author, double price)
        {
            BookID = bookID;
            Title = title;
            Author = author;
            Price = price;
        }

        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
    }
    interface IBookManager
    {
        bool AddBook(Book bk);
        bool Deletebook(int id);
        Book[] FindBooks(string title);
        bool UpdateBook(int id);
        bool ViewAllBooks();
    }
    class BookManager : IBookManager
    {
        HashSet<Book> books = new HashSet<Book>();
        public bool AddBook(Book bk)
        {
            return books.Add(bk);
        }

        public bool Deletebook(int id)
        {
            foreach (Book bk in books)
            {
                if (bk.BookID == id)
                {
                    books.Remove(bk);
                    return true;
                }
            }
            return false;
        }

        public Book[] FindBooks(string title)
        {
            List<Book> findlist = new List<Book>();
            foreach (Book bk in books)
            {
                if (bk.Title.Contains(title))
                {
                    findlist.Add(bk);
                }
            }
            return findlist.ToArray();
        }

        public bool UpdateBook(int id)
        {
            foreach (Book bk in books)
            {
                if (bk.BookID == id)
                {
                    Console.Write("Enter the New Title: ");
                    string newtitle = Console.ReadLine();
                    Console.Write("Enter the New Author: ");
                    string newauthor = Console.ReadLine();
                    Console.Write("Enter the New Price: ");
                    double newprice = double.Parse(Console.ReadLine());
                    bk.Title = newtitle;
                    bk.Author = newauthor;
                    bk.Price = newprice;
                    return true;
                }
            }
            return false;
        }
        public bool ViewAllBooks()
        {
            foreach (var bk in books)
            {
                
                Console.WriteLine($"Book ID: {bk.BookID}");
                Console.WriteLine($"Title: {bk.Title}");
                Console.WriteLine($"Author: {bk.Author}");
                Console.WriteLine($"Price: {bk.Price}");
                
            }
            return true;
        }
    }

    class CollectionBookManager
    {
        static void Main(string[] args)
        {
            Book bk1 = new Book(100, "Hello", "Me", 2500);
            Book bk2 = new Book(110, "Hello World", "Someone", 800);
            Book bk3 = new Book(112, "C# Programming", "Raj", 5800);
            BookManager mgr = new BookManager();
            mgr.AddBook(bk1);
            mgr.AddBook(bk2);
            mgr.AddBook(bk3);
            mgr.ViewAllBooks();
            mgr.UpdateBook(100);
            mgr.ViewAllBooks();
            Book[] temp = mgr.FindBooks("He");
            foreach (var item in temp)
            {
                Console.WriteLine(item.BookID);
                Console.WriteLine(item.Title);
                Console.WriteLine(item.Author);
                Console.WriteLine(item.Price);
            }
        }

    }
}