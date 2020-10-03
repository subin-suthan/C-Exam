

using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace Exam1

{
    interface ICustomerManager
    {
        bool AddNewCustomer(Cust ck);
        bool DeleteCustomer(int id);
        bool UpdateCustomer(Cust ck);
        Cust[] GetAllCustomers(string title);
    }

    class CustomerManager : ICustomerManager
    {

        private Cust[] allcust = null;

        private bool hasCust(int id)
        {
            foreach (Cust ck in allcust)
            {
                if ((ck != null) && (ck.CustID == id))
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
            bool available = hasCust(ck.CustID);
            if (available)
                throw new Exception("Customer by this ID already exists");
            for (int i = 0; i < allcust.Length; i++)
            {
             
                if (allcust[i] == null)
                {
                    allcust[i] = new Cust();
                    allcust[i].CustID = ck.CustID;
                    allcust[i].CustName = ck.CustName;
                    allcust[i].CustAddress = ck.CustAddress;
                    allcust[i].CustSalary = ck.CustSalary;
                    return true;
                }
            }
            return false;
        }

        public bool DeleteCustomer(int id)
        {
            for (int i = 0; i < allcust.Length; i++)
            {
           
                if ((allcust[i] != null) && (allcust[i].CustID == id))
                {
                    allcust[i] = null;
                    return true;
                }
            }
            return false;
        }

        public Cust[] GetAllCustomers(string name)
        {
            Cust[] copy = new Cust[allcust.Length];
            
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
              
                if ((allcust[i] != null) && (allcust[i].CustID == ck.CustID))
                {
                    allcust[i].CustName = ck.CustName;
                    allcust[i].CustAddress = ck.CustAddress;
                    allcust[i].CustSalary = ck.CustSalary;
                    return true;
                }
            }
            return false;
        }
    }

    class Cust
    {
        public int CustID
        {
            get; set;
        }
        public string CustName { get; set; }
        public double CustSalary { get; set; }
        public string CustAddress { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Cust)
            {
                Cust temp = obj as Cust;
                if ((this.CustID == temp.CustID) && (this.CustName == temp.CustName) && (this.CustSalary == temp.CustSalary))
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.CustID.GetHashCode();
        }
    }

    class BookFactoryComponent
    {
        public static ICustomerManager GetComponent(int size = 100)
        {
            string classname = ConfigurationManager.AppSettings["ClassName"];
            Debug.WriteLine(classname);
            Type selected = Assembly.GetExecutingAssembly().GetType(classname);
            Object type = Activator.CreateInstance(selected);
            return (ICustomerManager)type;
        }
    }
    class UIClient
    {
        static string menu = string.Empty;
        static ICustomerManager mgr = null;
        static void InitializeComponent()
        {
            menu = string.Format($"Customer MANAGEMENT SOFTWARE~\nTO ADD A NEW CUSTOMER-->PRESS 1\nTO UPDATE A CUSTOMER--->PRESS 2\nTO DELETE A CUSTOMER------------PRESS 3\nTO FIND A CUSTOMER------------->PRESS 4\nPS:ANY OTHER KEY IS CONSIDERED AS EXIT THE APP\n");
        
            mgr = BookFactoryComponent.GetComponent();
            mgr.AddNewCustomer(new Cust { CustID = 123, CustName = "subin", CustAddress = "tvm", CustSalary = 1200 });
            mgr.AddNewCustomer(new Cust { CustID = 124, CustName = "tom", CustAddress = "mum", CustSalary = 5000 });
            mgr.AddNewCustomer(new Cust { CustID = 125, CustName = "jack", CustAddress = "kollam", CustSalary = 3509 });
            mgr.AddNewCustomer(new Cust { CustID = 126, CustName = "arun", CustAddress = "delhi", CustSalary = 8000 });

        }

        static void Main(string[] args)
        {
            InitializeComponent();
            bool @continue = true;
            do
            {
                Console.WriteLine("enter choice:");
                string choice = Console.ReadLine();
                @continue = processing(choice);
            } while (@continue);
        }

        private static bool processing(string choice)
        {
            switch (choice)
            {
                case "1":
                    addingCustomerFeature();
                    break;
                case "2":
                    updatingCustomerFeature();
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
            string name = MyConsole.getString("Enter the name or part of the name to search");
            Cust[] cust = mgr.GetAllCustomers(name);
            foreach (var ck in cust)
            {
                if (ck != null)
                    Console.WriteLine(ck.CustName);
            }
        }

        private static void deletingFeature()
        {
            int id = MyConsole.getNumber("Enter the ID of the book to remove");
            if (mgr.DeleteCustomer(id))
                Console.WriteLine("Customer Deleted successfully");
            else
                Console.WriteLine("Could not find the book to delete");
        }

        private static void updatingCustomerFeature()
        {
            Cust ck = new Cust();
            Console.WriteLine("enter Book id:");
            ck.CustID = int.Parse(Console.ReadLine());

            Console.WriteLine("enter Book name:");

            ck.CustName = Console.ReadLine();
            Console.WriteLine("enter cust address:");

            ck.CustAddress = Console.ReadLine();
            Console.WriteLine("enter cust salary:");

            ck.CustSalary = int.Parse(Console.ReadLine());
            bool result = mgr.UpdateCustomer(ck);
            if (!result)
                Console.WriteLine($"No Customer by this id {ck.CustID} found to update");
            else
                Console.WriteLine($"Customer by ID {ck.CustID} is updated successfully to the database");
        }

        private static void addingCustomerFeature()
        {
            Cust ck = new Cust();
            Console.WriteLine("enter cust id:");
            ck.CustID =int.Parse(Console.ReadLine());

            Console.WriteLine("enter cust name:");

            ck.CustName = Console.ReadLine();
            Console.WriteLine("enter cust address:"); 

             ck.CustAddress = Console.ReadLine();
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