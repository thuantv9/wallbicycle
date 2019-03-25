using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineHelp.Common;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;


namespace OnlineHelp.Models
{
    public class CategoryDB
    {

        // Phương thức List danh sách Category
        public List<Category> ListAll()
        {
            List<Category> categories = new List<Category>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ShowAllCategoryForAdmin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            CategoryID = DCC.ToInt(reader["CategoryID"]),
                            Level = DCC.ToInt(reader["Level"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            ParentCategoryID = Int32.Parse(reader["ParentCategoryID"].ToString()),
                            Description = reader["Description"].ToString()
                        });
                    }
                    return categories;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            //return LocalDataSource.GetListDataFromProcedure<Category>(Const.Connectring, "ShowAllCategoryForAdmin");
        }

        // lấy category by ID
        public Category GetCategoryByID(int categoryid)
        {
            //Category c = new Category();
            //try
            //{
            //    string query = string.Format("select * from Categories where CategoryID={0}", categoryid);
            //    SqlConnection connection = new SqlConnection(Const.Connectring);
            //    {
            //        using (SqlCommand cmd = new SqlCommand(query, connection))
            //        {
            //            connection.Open();
            //            SqlDataReader reader = cmd.ExecuteReader();
            //            while (reader.Read())
            //            {
            //                c.CategoryID = Int32.Parse(reader["CategoryID"].ToString());
            //                c.Level = Int32.Parse(reader["Level"].ToString());
            //                c.CategoryName = reader["CategoryName"].ToString();
            //                c.ParentCategoryID = Int32.Parse(reader["ParentCategoryID"].ToString());

            //                c.EditDate = Convert.ToDateTime(reader["EditDate"].ToString());
            //                c.Editor = reader["Editor"].ToString();
            //                c.Description = reader["Description"].ToString();
            //                c.Remarks = reader["Remarks"].ToString();
            //            }
            //            return c;
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            string query = string.Format("select * from Categories where CategoryID={0}", categoryid);
            return LocalDataSource.GetDataFromCommand<Category>(Const.Connectring, query);
        }
        public Category GetCategory_ByMappingScreen(string screenid)
        {
            //Category c = new Category();
            //try
            //{
            //    string query = string.Format("select * from Categories where CategoryID={0}", categoryid);
            //    SqlConnection connection = new SqlConnection(Const.Connectring);
            //    {
            //        using (SqlCommand cmd = new SqlCommand(query, connection))
            //        {
            //            connection.Open();
            //            SqlDataReader reader = cmd.ExecuteReader();
            //            while (reader.Read())
            //            {
            //                c.CategoryID = Int32.Parse(reader["CategoryID"].ToString());
            //                c.Level = Int32.Parse(reader["Level"].ToString());
            //                c.CategoryName = reader["CategoryName"].ToString();
            //                c.ParentCategoryID = Int32.Parse(reader["ParentCategoryID"].ToString());

            //                c.EditDate = Convert.ToDateTime(reader["EditDate"].ToString());
            //                c.Editor = reader["Editor"].ToString();
            //                c.Description = reader["Description"].ToString();
            //                c.Remarks = reader["Remarks"].ToString();
            //            }
            //            return c;
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            try
            {
                string query = string.Format("select * from Categories where MappingScreen='{0}'", screenid);
                List<Category> _lst = LocalDataSource.GetListDataFromCommand<Category>(Const.Connectring, query);
                return _lst.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }          
        }
        // 
        public List<Category> GetListByLevel(int level)
        {
            List<Category> categories = new List<Category>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetCategoryByLevel", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Level", level);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            CategoryID = DCC.ToInt(reader["CategoryID"]),
                            Level = DCC.ToInt(reader["Level"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            ParentCategoryID = Int32.Parse(reader["ParentCategoryID"].ToString()),
                            Description = reader["Description"].ToString()

                        });
                    }
                    return categories;
                }
            }
            catch (Exception)
            {
                throw;
            }
            //return LocalDataSource.GetListDataFromProcedure<Category>(Const.Connectring, "GetCategoryByLevel", level);
        }


        // Phương thức lấy list level
        public List<int> GetListLevel()
        {
            List<int> lstLevel = new List<int>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetListLevel", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lstLevel.Add(Int32.Parse(reader["Level"].ToString()));
                    }
                    con.Close();
                    return lstLevel;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Phương thức lấy Category cha truyền vào là level con.
        public DataTable GetListCategoryByLevelParent(int level)
        {
            return LocalDataSource.GetDataTableFromProceduce(Const.Connectring, "GetCategoryByLevelParent", level);
        }

        // Phương thức thêm mới Insert Category
        public int Create(Category category)
        {
            try
            {
                int i;
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("InsertUpdateCategory", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                    cmd.Parameters.AddWithValue("@Level", category.Level);
                    cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    cmd.Parameters.AddWithValue("@ParentCategoryID", category.ParentCategoryID);
                    cmd.Parameters.AddWithValue("@MappingScreen", category.MappingScreen);
                    cmd.Parameters.AddWithValue("@EditDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Editor", category.Editor);
                    cmd.Parameters.AddWithValue("@Description", category.Description);
                    // Check remarks là null thì truyền null
                    if (category.Remarks == null)
                    {
                        cmd.Parameters.AddWithValue("@Remarks", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Remarks", category.Remarks);
                    }
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                }
                return i;
            }
            catch (Exception)
            {
                return 0;
            }
            //object[] obj = LocalDataSource.ConvertObjectToArrayParam<Category>(category);
            //int length = obj.Length;
            //Array.Resize<object>(ref obj, length + 1);
            //obj[length] = "Insert";
            //return SqlHelper.ExecuteNonQuery(Const.Connectring, "InsertUpdateCategory", obj);
        }

        // Phương thức Update Category
        public int Update(Category category)
        {
            int i;
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("InsertUpdateCategory", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                    cmd.Parameters.AddWithValue("@Level", category.Level);
                    cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    cmd.Parameters.AddWithValue("@ParentCategoryID", category.ParentCategoryID);
                    cmd.Parameters.AddWithValue("@MappingScreen", category.MappingScreen);
                    cmd.Parameters.AddWithValue("@EditDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Editor", category.Editor);
                    cmd.Parameters.AddWithValue("@Description", category.Description);
                    cmd.Parameters.AddWithValue("@Remarks", category.Remarks);
                    cmd.Parameters.AddWithValue("@Action", "Update");
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                }
                return i;
            }
            catch (Exception)
            {
                return 0;
            }
            //object[] obj = LocalDataSource.ConvertObjectToArrayParam<Category>(category);
            //int length = obj.Length;
            //Array.Resize<object>(ref obj,length+1);
            //obj[length]="Update";            
            //return SqlHelper.ExecuteNonQuery(Const.Connectring, "InsertUpdateCategory", obj);
        }

        // Phương thức Delete
        public int Delete(int categoryid)
        {
            //int i;
            //using (SqlConnection con = new SqlConnection(Const.Connectring))
            //{
            //    con.Open();
            //    SqlCommand com = new SqlCommand("DeleteCategory", con);
            //    com.CommandType = CommandType.StoredProcedure;
            //    com.Parameters.AddWithValue("@CategoryID", categoryid);
            //    i = com.ExecuteNonQuery();
            //}
            //return i;
            return SqlHelper.ExecuteNonQuery(Const.Connectring, "DeleteCategory", categoryid);
        }

        // lấy số CategoryID tiếp theo 
        public int GetNextCategoryID()
        {
            //int i = 1;
            //using (SqlConnection con = new SqlConnection(Const.Connectring))
            //{
            //    con.Open();
            //    SqlCommand com = new SqlCommand("GetNextCategoryID", con);
            //    com.CommandType = CommandType.StoredProcedure;
            //    SqlDataReader reader = com.ExecuteReader();
            //    try
            //    {
            //        while (reader.Read())
            //        {
            //            i = Int32.Parse(reader[0].ToString());
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //return i;
            int i = DCC.ToInt(SqlHelper.ExecuteScalar(Const.Connectring, "GetNextCategoryID"));
            if (i != 0) return i;
            else return 1;
        }

        // lấy số level mới
        public int GetNextLevel()
        {

            //int i = 1;
            //using (SqlConnection con = new SqlConnection(Const.Connectring))
            //{
            //    con.Open();
            //    SqlCommand com = new SqlCommand("GetNextLevel", con);
            //    com.CommandType = CommandType.StoredProcedure;
            //    SqlDataReader reader = com.ExecuteReader();
            //    try
            //    {
            //        while (reader.Read())
            //        {
            //            i = Int32.Parse(reader[0].ToString());
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //return i;
            int i = DCC.ToInt(SqlHelper.ExecuteScalar(Const.Connectring, "GetNextLevel"));
            if (i != 0) return i;
            else return 1;
        }
        // lấy nhưng Category có level trước level truyền vào
        public List<Category> Getparentcategorybylevelofchild(int level)
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Getparentcategorybylevelofchild", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Level", level);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new Category()
                    {
                        CategoryID = Int32.Parse(reader["CategoryID"].ToString()),
                        CategoryName = reader["CategoryName"].ToString()
                    });
                }
                return categories;
            }

        }
        public int UpdateMappingScreen(string _lstmappingscreen)
        {
            return SqlHelper.ExecuteNonQuery(Const.Connectring, "UpdatePluginAllData", "OnlineHelp_MappingScreen", _lstmappingscreen);
        }
        public List<MappingScreen> GetMappingScreen()
        {
            try
            {
                DataTable dt = LocalDataSource.GetDataTableFromCommand(Const.Connectring,
                               "select * from PluginAllData where tablekey='OnlineHelp_MappingScreen'");
                DataRow dr = dt.AsEnumerable().FirstOrDefault();
                if (dr != null)
                    return JsonConvert.DeserializeObject<List<MappingScreen>>(dr.ItemArray[1].ToString());
                else return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}