using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentistryManager.Models;
using DentistryManager.Repository;

namespace DentistryManager.Controllers
{
    public class BaseController : Controller
    {
        EventsRepository eventsrepository = new EventsRepository();
        PatientsRepository patientsrepository = new PatientsRepository();

        //Events
        public JsonResult GetAllEvents()
        {
            return Json(eventsrepository.List(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertEvents(Events even)
        {
            return Json(eventsrepository.Add(even), JsonRequestBehavior.AllowGet);
        }

        // Patients
        public JsonResult GetAllPatients()
        {
            return Json(patientsrepository.List(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPatientByid(string id)
        {
            return Json(patientsrepository.GetById(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertPatients(Patients entity)
        {
            return Json(patientsrepository.Add(entity), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePatients(Patients entity)
        {
            return Json(patientsrepository.Edit(entity), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePatient(Patients entity)
        {
            return Json(patientsrepository.Delete(entity), JsonRequestBehavior.AllowGet);
        }
    }
}