using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
namespace Web.Common
{
    public static class Const
    {
        public static string Connectring=  ConfigurationManager.ConnectionStrings["ConString"].ConnectionString; 
     
    }
    public class CommonFunction
    {
        public static bool GetLoginStatus(string UserName, string Password)
        {
            string sqlcommand = string.Format("select count(1) from Users where UserName = '{0}' and Password = '{1}'", UserName, Password);
            return (int)SqlHelper.ExecuteScalar(Const.Connectring, CommandType.Text, sqlcommand) > 0;
        }
    }
}