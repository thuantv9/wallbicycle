using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DentistryManager.Common
{
    public static class Const
    {
        public static string Connectring = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString; 

        /// <summary>
        ///  Store procedure
        /// </summary>

        // Patients
        public const string FSP_PATIENT_GETALL = "GetAllPatients";
        public const string FSP_PATIENT_INSERT = "InsertPatients";
        public const string FSP_PATIENT_DELETE = "PatientsDelete";
        public const string FSP_PATIENT_UPDATE = "UpdatePatients";

        // Events

    }
}