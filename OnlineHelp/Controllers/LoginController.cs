using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;
using OnlineHelp.Models;
using Fisbank.Cbs.ObjectInfo;

namespace OriginCoffeManagement.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserPlugin model, string ReturnUrl)
        {
            if (Membership.ValidateUser(model.Account, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Account, false);
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Admin", "Home");
            }
            return View(model);
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }       
    }
}