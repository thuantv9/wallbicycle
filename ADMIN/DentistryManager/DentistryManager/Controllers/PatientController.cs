using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentistryManager.Repository;
using DentistryManager.Models;

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
            IEnumerable<Patients> patients;
            Func<Patients, bool> func = it => it.status == string.Empty;
            patients = patientrepository.List(func);
            return View(patients);
        }
    }
}