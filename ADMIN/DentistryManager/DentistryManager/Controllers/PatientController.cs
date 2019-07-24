using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentistryManager.Repository;

namespace DentistryManager.Controllers
{
    public class PatientController : Controller
    {
        PatientsRepository patientrepository = new PatientsRepository();
        public ActionResult ManagePatient()
        {
            return View(patientrepository.List());
        }

        public ActionResult QueuePatient()
        {
            return View();
        }
    }
}