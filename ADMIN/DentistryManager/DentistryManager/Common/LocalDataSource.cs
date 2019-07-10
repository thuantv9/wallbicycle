using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.ComponentModel;
namespace DentistryManager.Common
{
    public class LocalDataSource
    {
        public static List<T> GetListDataFromProcedure<T>(string connectionString, string procName, params object[] paramsProcs) where T : class, new()
        {
            try
            {
                var ds = new DataSet();
                if (paramsProcs.IsNull())
                    ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, procName);
                else
                    ds = SqlHelper.ExecuteDataset(connectionString, procName, paramsProcs);
                var obj = DataTableToList<T>(ds.Tables[0]);
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<T> GetListDataFromCommand<T>(string connectionString, string sqlCommand) where T : class, new()
        {
            try
            {
                var ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sqlCommand);
                var obj = DataTableToList<T>(ds.Tables[0]);
                return obj;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static T GetDataFromProcedure<T>(string connectionString, string procName, params object[] paramsProcs) where T : class, new()
        {
            try
            {
                var ds = new DataSet();
                if (paramsProcs.IsNull())
                    ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, procName);
                else
                    ds = SqlHelper.ExecuteDataset(connectionString, procName, paramsProcs);
                var obj = DataTableToList<T>(ds.Tables[0]);
                return obj.FirstOrDefault();
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static T GetDataFromCommand<T>(string connectionString, string sqlCommand) where T : class, new()
        {
            try
            {
                var ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sqlCommand);
                var obj = DataTableToList<T>(ds.Tables[0]);
                return obj.FirstOrDefault();
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static DataSet GetDataSetFromProcedure(string connectionString, string procName, params object[] paramsProcs)
        {
            try
            {
                var ds = new DataSet();
                if (paramsProcs.IsNull())
                    ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, procName);
                else
                    ds = SqlHelper.ExecuteDataset(connectionString, procName, paramsProcs);
                return ds;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static DataSet GetDataSetFromCommand(string connectionString, string sqlCommand)
        {
            try
            {
                return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sqlCommand);
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static DataTable GetDataTableFromProceduce(string connectionString, string procName, params object[] paramsProcs)
        {
            try
            {
                var ds = new DataSet();
                if (paramsProcs.IsNull())
                    ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, procName);
                else
                    ds = SqlHelper.ExecuteDataset(connectionString, procName, paramsProcs);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static DataTable GetDataTableFromCommand(string connectionString, string sqlCommand)
        {
            try
            {
                var ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sqlCommand);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        public static List<T> DataTableToList<T>(DataTable data) where T : class, new()
        {
            try
            {
                var result = new List<T>();
                foreach (var row in data.AsEnumerable())
                {
                    T obj = new T();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    result.Add(obj);
                }
                return result;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            try
            {
                var obj = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(T));
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    obj.Columns.Add(prop.Name, prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    obj.Rows.Add(values);
                }
                return obj;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static object[] ConvertObjectToArrayParam<T>(T data) where T : class, new()
        {
            try
            {
                var props = TypeDescriptor.GetProperties(typeof(T));
                object[] values = new object[props.Count];
                for (int i = 0; i < props.Count; i++)
                {
                    values[i] = props[i].GetValue(data);
                }
                return values;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static int CallProcedure(string connectionString, string procName, params object[] paramsProcs)
        {
            try
            {
                int res;
                if (paramsProcs.IsNull())
                    res = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, procName);
                else
                    res = SqlHelper.ExecuteNonQuery(connectionString, procName, paramsProcs);
                return res;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }
        public static int CallCommandExecute(string connectionString, string sqlCommand)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sqlCommand);
            }
            catch (Exception ex)
            {

                return -1;
            }
        }
        public static List<string> GetListStringFromProcedure(string connectionString, string procName, params object[] paramsProcs)
        {
            try
            {
                var obj = new List<string>();
                var ds = SqlHelper.ExecuteDataset(connectionString, procName, paramsProcs);
                foreach (DataRow dr in ds.Tables[0].Rows) obj.Add(dr[0].ToString());
                return obj;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static List<string> GetListStringFromCommand(string connectionString, string sqlCommand)
        {
            try
            {
                var obj = new List<string>();
                var ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sqlCommand);
                foreach (DataRow dr in ds.Tables[0].Rows) obj.Add(dr[0].ToString());
                return obj;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static int GetReturnValueFromProcedure(string connectionString, string procName, params object[] paramsProcs)
        {
            try
            {
                int res;
                if (paramsProcs.IsNull())
                    res = (int)SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, procName);
                else
                    res = (int)SqlHelper.ExecuteScalar(connectionString, procName, paramsProcs);
                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static string GetReturnValueFromCommand(string connectionString, string sqlCommand)
        {
            try
            {
                return SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlCommand).ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static long GetNextSequenceValue(string connectionString, string sequenceName)
        {
            try
            {
                string command = "SELECT NEXT VALUE FOR " + sequenceName;
                var rs = SqlHelper.ExecuteScalar(connectionString, CommandType.Text, command);
                long nextVal = Convert.ToInt64(rs);
                return nextVal;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}

