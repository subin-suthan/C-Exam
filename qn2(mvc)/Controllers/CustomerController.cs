using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using SystModels;
using Exam2.Models;

namespace Exam2.Controllers
{
    public class CustomerController : Controller
    {
        public ViewResult AllCustomers()
        {


            var context = new Mydb();
            var model = context.CustTables.ToList();
            return View(model);

        }

        public ViewResult Find(string id)
        {
            int cusId = int.Parse(id);
            var context = new Mydb();
            var model = context.CustTables.FirstOrDefault((e) => e.CustId == cusId);
            return View(model);
        }

        [HttpPost]

        public ActionResult Find(CustTable cus)
        {

            var context = new Mydb();
            var model = context.CustTables.FirstOrDefault((e) => e.CustId == cus.CustId);
            model.CustName = cus.CustName;
            model.CustAddress = cus.CustAddress;
            model.CustSalary = cus.CustSalary;
            context.SaveChanges();
            return RedirectToAction("AllCustomers");

        }


        public ViewResult NewCustomer()
        {
            var model = new CustTable();
            return View(model);
        }
        [HttpPost]
        public ActionResult NewCustomer(CustTable cus)
        {
            var context = new Mydb();
            context.CustTables.Add(cus);
            context.SaveChanges();
            return RedirectToAction("AllCustomers");
        }

        public ActionResult Delete(string id)
        {

            int cusId = int.Parse(id);
            var context = new Mydb();
            var model = context.CustTables.FirstOrDefault((e) => e.CustId == cusId);
            context.CustTables.Remove(model);
            context.SaveChanges();
            return RedirectToAction("AllCustomers");
        }
    }
}