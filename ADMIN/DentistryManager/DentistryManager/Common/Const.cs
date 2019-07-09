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
    }
}