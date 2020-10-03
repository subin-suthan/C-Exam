
using SampleConApp.Day2;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace Exam1

{
    interface ICustomermanger
    {
        bool AddNewCustomer(cust ck);
        bool DeleteCustomer(int id);
        bool UpdateCustomer(cust ck);
        Cust[] GetAllCustomers(string title);
    }

    class CustomerManager : ICustomermanager
    {

        private Cust[] allcust = null;

        private bool hasCust(int id)
        {
            foreach (cust ck in allcust)
            {
                if ((ck != null) && (ck.BookID == id))
                    return true;
            }
            return false;

        }
      
        public CustomerManager(int size)
        {
            allcust = new Cust[size];
        }
        public bool AddNewCustomer(Cust ck)
        {
            bool available = hasCustomer(ck.CustId);
            if (available)
                throw new Exception("Customer by this ID already exists");
            for (int i = 0; i < allcust.Length; i++)
            {
                //find the first occurance of null in the array...
                if (allcust[i] == null)
                {
                    allcust[i] = new Book();
                    allcust[i].CustId = ck.CustId;
                    allcust[i].CustName = ck.CustName;
                    allcust[i].CustAddress = ck.Address;
                    allcust[i].CustSalryt = ck.CustSalary;
                    return true;
                }
            }
            return false;
        }

        public bool DeleteCustomer(int id)
        {
            for (int i = 0; i < allcust.Length; i++)
            {
           
                if ((allcust[i] != null) && (allcust[i].CustId == id))
                {
                    allcust[i] = null;
                    return true;
                }
            }
            return false;
        }

        public Cust[] GetAllCustomers(string name)
        {
            Book[] copy = new Book[allcust.Length];
            
            int index = 0;
            foreach (Cust ck in allcust)
            {
                
                if ((ck != null) && (ck.CustName.Contains(name)))
                {
                    copy[index] = ck;
                    index++;
                }
            }

            
            return copy;
        }

        public bool UpdateCustomer(Cust ck)
        {
            for (int i = 0; i < allcust.Length; i++)
            {
                //find the first occurance of null in the array...
                if ((allBooks[i] != null) && (allBooks[i].BookID == bk.BookID))
                {
                    allBooks[i].Title = bk.Title;
                    allBooks[i].Author = bk.Author;
                    allBooks[i].Price = bk.Price;
                    return true;
                }
            }
            return false;
        }
    }

    class Book
    {
        public int BookID
        {
            get; set;
        }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Author { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Book)
            {
                Book temp = obj as Book;
                if ((this.Author == temp.Author) && (this.Title == temp.Title) && (this.Price == temp.Price))
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.BookID.GetHashCode();
        }
    }

    //Clients should not instantiate the object. they will call our factory method to get the type of the object they want...
    class BookFactoryComponent
    {
        public static IBookManager GetComponent(int size = 100)
        {
            string classname = ConfigurationManager.AppSettings["ClassName"];
            Debug.WriteLine(classname);
            Type selected = Assembly.GetExecutingAssembly().GetType(classname);
            Object type = Activator.CreateInstance(selected);
            return (IBookManager)type;
        }
    }
    class UIClient
    {
        static string menu = string.Empty;
        static IBookManager mgr = null;
        static void InitializeComponent()
        {
            menu = string.Format($"~~~~~~~BOOK STORE MANAGEMENT SOFTWARE~~~~~~~~~~~~~~~~~~~\nTO ADD A NEW BOOK------------->PRESS 1\nTO UPDATE A BOOK------------>PRESS 2\nTO DELETE A BOOK------------PRESS 3\nTO FIND A BOOK------------->PRESS 4\nPS:ANY OTHER KEY IS CONSIDERED AS EXIT THE APP\n");
            //int size = MyConsole.getNumber("Enter the no of Books U wish to store in the BookStore");
            mgr = BookFactoryComponent.GetComponent();
            mgr.AddNewBook(new Book { BookID = 123, Title = "A Suitable Boy", Author = "Vikram Seth", Price = 1200 });
            mgr.AddNewBook(new Book { BookID = 124, Title = "Disclosure", Author = "Micheal Crichton", Price = 500 });
            mgr.AddNewBook(new Book { BookID = 125, Title = "The Mahabharatha", Author = "C Rajagoalachari", Price = 350 });
            mgr.AddNewBook(new Book { BookID = 126, Title = "The Discovery of India", Author = "J . Nehru", Price = 800 });

        }

        static void Main(string[] args)
        {
            InitializeComponent();
            bool @continue = true;
            do
            {
                string choice = MyConsole.getString(menu);
                @continue = processing(choice);
            } while (@continue);
        }

        private static bool processing(string choice)
        {
            switch (choice)
            {
                case "1":
                    addingBookFeature();
                    break;
                case "2":
                    updatingBookFeature();
                    break;
                case "3":
                    deletingFeature();
                    break;
                case "4":
                    readingFeature();
                    break;
                default:
                    return false;
            }
            return true;
        }

        private static void readingFeature()
        {
            string title = MyConsole.getString("Enter the title or part of the title to search");
            Book[] books = mgr.GetAllBooks(title);
            foreach (var bk in books)
            {
                if (bk != null)
                    Console.WriteLine(bk.Title);
            }
        }

        private static void deletingFeature()
        {
            int id = MyConsole.getNumber("Enter the ID of the book to remove");
            if (mgr.DeleteBook(id))
                Console.WriteLine("Book Deleted successfully");
            else
                Console.WriteLine("Could not find the book to delete");
        }

        private static void updatingBookFeature()
        {
            Book bk = new Book();
            bk.BookID = MyConsole.getNumber("Enter the ISBN no of the book U wish to update");
            bk.Title = MyConsole.getString("Enter the new title of this book");
            bk.Author = MyConsole.getString("Enter the new Author of this book");
            bk.Price = MyConsole.getDouble("Enter the new Price of this book");
            bool result = mgr.UpdateBook(bk);
            if (!result)
                Console.WriteLine($"No book by this id {bk.BookID} found to update");
            else
                Console.WriteLine($"Book by ID {bk.BookID} is updated successfully to the database");
        }

        private static void addingCustomerFeature()
        {
            Cust ck = new Cust();
            Console.WriteLine("enter cust id:");
            ck.CustId =int.Parse(Console.ReadLine());

            Console.WriteLine("enter cust name:");

            ck.CustName = int.Parse(Console.ReadLine());
            Console.WriteLine("enter cust address:"); 

             ck.CustAddress = int.Parse(Console.ReadLine());
            Console.WriteLine("enter cust salary:");

             ck.CustSalary = int.Parse(Console.ReadLine());
            try
            {
                bool result = mgr.AddNewCustomer(ck);
                if (!result)
                    Console.WriteLine("No more customers could be added");
                else
                    Console.WriteLine($"Customer by Name {ck.CustName} is added successfully to the database");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}