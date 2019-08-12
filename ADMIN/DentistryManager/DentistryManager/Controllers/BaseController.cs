using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentistryManager.Models;
using DentistryManager.Repository;
using DentistryManager.Common;

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

        public JsonResult UpdateStatusInADayPatients(string id, string statusinaday)
        {
            return Json(patientsrepository.Edit_StatusInADay(id, statusinaday), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePatient(Patients entity)
        {
            return Json(patientsrepository.Delete(entity), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPatient_Queue()
        {
            Func<Patients, bool> func = it => it.statusinaday == Const.Patient_Waiting;
            return Json(patientsrepository.List(func), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPatient_Exammining()
        {
            Func<Patients, bool> func = it => it.statusinaday == Const.Patient_Examining;
            return Json(patientsrepository.List(func), JsonRequestBehavior.AllowGet);
        }
    }
}