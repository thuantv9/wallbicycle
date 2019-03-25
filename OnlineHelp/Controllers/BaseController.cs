using Fisbank.Cbs.Controller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineHelp.Common;
using System.Data;

namespace OnlineHelp.Controllers
{
    public class BaseController : Controller
    {        
        public bool Login(string username, string password)
        {
            return CommonFunction.GetLoginStatus(username, password);
            
        }           
    }
}
