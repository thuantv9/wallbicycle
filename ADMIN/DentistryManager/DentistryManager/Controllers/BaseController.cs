using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentistryManager.Models;

namespace DentistryManager.Controllers
{
    public class BaseController : Controller
    {
        EventsRepository eventsrepository = new EventsRepository();
        // GET: /Base/
        public JsonResult GetAllEvents()
        {
            return Json(eventsrepository.List(), JsonRequestBehavior.AllowGet);
        }
	}
}