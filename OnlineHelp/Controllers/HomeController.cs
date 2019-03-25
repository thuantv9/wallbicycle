using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using OnlineHelp.Models;
using OnlineHelp.Common;
using Newtonsoft.Json;

namespace OnlineHelp.Controllers
{
    public class HomeController : BaseController
    {
        CategoryDB categoryDB = new CategoryDB();
        public HomeController()
        {
        }
        //  /Home/Index : lấy tất cả Category
        [AllowAnonymous]
        public ActionResult Index()
        {
            //var categoryid = Request["categoryid"];
            //if (categoryid == null)
            //{
            //    ViewBag.categoryid = 1;
            //}
            //else
            //{
            //    ViewBag.categoryid = categoryid;
            //}
            try
            {
                Category _category = null;
                var mappingscreen = Request["mappingscreen"];
                if (mappingscreen != null)
                {
                    _category = categoryDB.GetCategory_ByMappingScreen(mappingscreen);
                }
                if (_category == null)
                {
                    _category = categoryDB.ListAll().FirstOrDefault();
                }
                ViewBag.mappingscreen = _category;
            }
            catch (Exception)
            {
                ViewBag.mappingscreen = null;
            }
            return View(categoryDB.ListAll());
        }

        // /Home/Admin
        [Authorize]
        public ActionResult Admin()
        {
            return View();
        }

        // Truyền ajax lấy tất cả dữ liệu truyền vào Admin
        public JsonResult List()
        {
            return Json(categoryDB.ListAll(), JsonRequestBehavior.AllowGet);
        }

        // truyền Json để AJAX lấy dữ liệu hiển thị nội dung của mỗi Category khi được Click
        public JsonResult GetContentFromCategoryName(string categoryname)
        {
            Category c = new Category();
            string query = string.Format("select * from Categories where CategoryName=N'{0}'", categoryname);
            SqlConnection connection = new SqlConnection(Const.Connectring);
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        c.CategoryID = Int32.Parse(reader["CategoryID"].ToString());
                        c.Level = Int32.Parse(reader["Level"].ToString());
                        c.CategoryName = reader["CategoryName"].ToString();
                        c.ParentCategoryID = Int32.Parse(reader["ParentCategoryID"].ToString());
                        c.EditDate = Convert.ToDateTime(reader["EditDate"].ToString());
                        c.Editor = reader["Editor"].ToString();
                        c.Description = reader["Description"].ToString();
                        c.Remarks = reader["Remarks"].ToString();
                    }
                }
            }
            return Json(c, JsonRequestBehavior.AllowGet);
        }
        // Truyền Ajax để lấy Category theo ID
        public JsonResult GetByID(int id)
        {
            return Json(categoryDB.GetCategoryByID(id), JsonRequestBehavior.AllowGet);
        }

        // Truyền AJAX Add category
        public JsonResult Add(Category category)
        {
            return Json(categoryDB.Create(category), JsonRequestBehavior.AllowGet);
        }
        // Truyen Update Category
        public JsonResult Update(Category category)
        {
            return Json(categoryDB.Update(category), JsonRequestBehavior.AllowGet);
        }
        // Delete Category
        public JsonResult Delete(int ID)
        {
            return Json(categoryDB.Delete(ID), JsonRequestBehavior.AllowGet);
        }

        // lấy số Category tiếp theo
        public JsonResult GetNextCategoryID()
        {
            return Json(categoryDB.GetNextCategoryID(), JsonRequestBehavior.AllowGet);
        }
        // lấy level tiếp theo
        public JsonResult GetNextLevel()
        {
            return Json(categoryDB.GetNextLevel(), JsonRequestBehavior.AllowGet);
        }

        // lấy List level
        public JsonResult GetListLevel()
        {
            return Json(categoryDB.GetListLevel(), JsonRequestBehavior.AllowGet);
        }
        // Phương thức lấy Category cha truyền vào là level con.

        public JsonResult GetGetListCategoryByLevelParent(int ID)
        {

            return Json(categoryDB.GetListCategoryByLevelParent(ID), JsonRequestBehavior.AllowGet);
        }
        // 
        public JsonResult GetListCategoryByLevel(int ID)
        {
            return Json(categoryDB.GetListByLevel(ID), JsonRequestBehavior.AllowGet);
        }

        // phương thức lấy nhưng Category có level trước level truyền vào
        public JsonResult Getparentcategorybylevelofchild(int ID)
        {
            return Json(categoryDB.Getparentcategorybylevelofchild(ID), JsonRequestBehavior.AllowGet);
        }

        // lấy Mapping Screen
        public JsonResult GetMappingSCreen()
        {
            return Json(categoryDB.GetMappingScreen(), JsonRequestBehavior.AllowGet);
        }

        //Update Mapping Screen
        [HttpPost]
        public JsonResult UpdateMappingScreen(string screenmapping1)
        {
            return Json(categoryDB.UpdateMappingScreen(screenmapping1), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetCategory_ByMappingScreen(string screenid)
        {
            return Json(categoryDB.GetCategory_ByMappingScreen(screenid), JsonRequestBehavior.AllowGet);
        }
    }
}
