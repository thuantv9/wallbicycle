using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        DbContext dbcontext = new DbContext();
        public ActionResult Index2()
        {
            ViewBag.Product = dbcontext.GetAllProduct();
            if (dbcontext.GetAllNews() != null && dbcontext.GetAllNews().Count > 3)
            {
                ViewBag.News = dbcontext.GetAllNews().Take(3);
            }
            else
            {
                ViewBag.News = dbcontext.GetAllNews();
            }
            if (dbcontext.GetAllSlideImage() != null && dbcontext.GetAllSlideImage().Any())
            {
                ViewBag.Slide = dbcontext.GetAllSlideImage().Count() > 3 ? dbcontext.GetAllSlideImage().Take(3) : dbcontext.GetAllSlideImage();
            }
            ViewBag.Tag = dbcontext.GetAllTags();
            ViewBag.Category = dbcontext.GetAllCategory();
            return View();
        }
        public ActionResult Category()
        {
            var id = Url.RequestContext.RouteData.Values["id"];
            // truyền categoryid
            if (id == null)
            {
                ViewBag.CategoryId = "";
            }
            else
            {
                ViewBag.CategoryId = id;
            }
            // lấy tất cả sản phẩm
            ViewBag.Product = dbcontext.GetAllProduct();
            // truyền model tất cả chủng loại sản phẩm
            return View(dbcontext.GetAllCategory());
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Category = dbcontext.GetAllCategory();
            return View();
        }

        public ActionResult Product(int id)
        {
            return View(dbcontext.GetProductById(id));
        }
        public ActionResult News(int id)
        {
            return View(dbcontext.GetNewsById(id));
        }
        public ActionResult NewsAll()
        {
            return View(dbcontext.GetAllNews());
        }

    }
}