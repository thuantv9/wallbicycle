using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
namespace OnlineHelp.Common
{
    public static class Const
    {
        public static string Connectring=  ConfigurationManager.ConnectionStrings["ConString"].ConnectionString; 
     
    }
    public class CommonFunction
    {
        public static bool GetLoginStatus(string UserId, string Password)
        {
            string pass =ProtectData.EncryptData(Password.ToUpper());
            string sqlcommand = string.Format("select count(1) from UsersLogin where UPPER(UserId) = '{0}' and Convert(varchar,Password) = '{1}'", UserId.ToUpper(), pass);
            int scalar = DCC.ToInt(SqlHelper.ExecuteScalar(Const.Connectring, CommandType.Text, sqlcommand));
            return scalar > 0;
        }
    }
}