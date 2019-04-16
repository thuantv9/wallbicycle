using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
namespace Web.Controllers
{
    public class AdminController : Controller
    {

        DbContext dbcontext = new DbContext();
        public ActionResult Product()
        {
            return View();
        }
        public ActionResult Category()
        {
            return View();
        }
        public ActionResult Customer()
        {
            return View();
        }
        public ActionResult User()
        {
            return View();
        }
        public ActionResult Order()
        {
            return View();
        }
        public ActionResult OrderItem()
        {
            return View();
        }
        public ActionResult SlideImage()
        {
            return View();
        }
        public ActionResult News()
        {
            return View();
        }
        public ActionResult Tags()
        {
            return View(dbcontext.GetAllTags());
        }
    }
}