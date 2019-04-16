using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Common;
using Web.Models;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        DbContext dbcontext = new DbContext();
        #region Admin Product
        // lấy tất cả product
        public JsonResult GetAllProduct()
        {
            return Json(dbcontext.GetAllProduct(), JsonRequestBehavior.AllowGet);
        }
        // lấy by  id
        public JsonResult GetProductById(int id)
        {
            return Json(dbcontext.GetProductById(id), JsonRequestBehavior.AllowGet);
        }
        // lấy by category id
        public JsonResult GetProductByCategoryId(int CategoryId)
        {
            return Json(dbcontext.GetProductByCategoryId(CategoryId), JsonRequestBehavior.AllowGet);
        }
        // Thêm mới sản phẩm
        public JsonResult InsertProduct(Product product)
        {
            return Json(dbcontext.InsertProduct(product), JsonRequestBehavior.AllowGet);
        }
        // Cập nhật sản phẩm
        public JsonResult UpdateProduct(Product product)
        {
            return Json(dbcontext.UpdateProduct(product), JsonRequestBehavior.AllowGet);
        }
        // Xóa sản phẩm 
        public JsonResult DeleteProduct(int id)
        {
            return Json(dbcontext.DeleteProduct(id), JsonRequestBehavior.AllowGet);
        }

        // lấy số id tiếp theo của sản phẩm
        public JsonResult GetNextProductId()
        {
            return Json(dbcontext.GetNextProductId(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Admin Category
        // lấy tất cả category
        public JsonResult GetAllCategory()
        {
            return Json(dbcontext.GetAllCategory(), JsonRequestBehavior.AllowGet);
        }
        // lấy chủng loại theo Id
        public JsonResult GetCategoryById(int id)
        {
            return Json(dbcontext.GetCategoryById(id), JsonRequestBehavior.AllowGet);
        }
        // thêm mới category
        public JsonResult InsertCategory(Category category)
        {
            return Json(dbcontext.InsertCategory(category), JsonRequestBehavior.AllowGet);
        }
        // Cập nhật chủng loại
        public JsonResult UpdateCategory(Category category)
        {
            return Json(dbcontext.UpdateCategory(category), JsonRequestBehavior.AllowGet);
        }
        // lấy số id tiếp theo của chủng loại sản phẩm
        public JsonResult GetNextCategoryId()
        {
            return Json(dbcontext.GetNextCategoryId(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Admin Customer
        // lấy tất cả kh
        public JsonResult GetAllCustomer()
        {
            return Json(dbcontext.GetAllCustomer(), JsonRequestBehavior.AllowGet);
        }
        // lấy khách hàng teho id
        public JsonResult GetCustomerById(int id)
        {
            return Json(dbcontext.GetCustomerById(id), JsonRequestBehavior.AllowGet);
        }
        // thêm mới khách hàng
        public JsonResult InsertCustomer(Customer customer)
        {
            return Json(dbcontext.InsertCustomer(customer), JsonRequestBehavior.AllowGet);
        }
        // cập nhật khách hàng
        public JsonResult UpdateCustomer(Customer customer)
        {
            return Json(dbcontext.UpdateCustomer(customer), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNextCustomerId()
        {
            return Json(dbcontext.GetNextCustomerId(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCustomer(int id)
        {
            return Json(dbcontext.DeleteCustomer(id), JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Admin SlideImage
        // lấy tất cả slide
        public JsonResult GetAllSlideImage()
        {
            return Json(dbcontext.GetAllSlideImage(), JsonRequestBehavior.AllowGet);
        }
        // thêm mới slide
        public JsonResult InsertSlideImage(SlideImage slide)
        {
            return Json(dbcontext.InsertSlideImage(slide), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNextSlideId()
        {
            return Json(dbcontext.GetNextSlideId(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSlide(int id)
        {
            return Json(dbcontext.DeleteSlide(id), JsonRequestBehavior.AllowGet);
        }
        // lấy slide theo Id
        //public JsonResult GetSlideById(int id)
        //{ 
        //    return Json(dbcontext.)
        //}
        #endregion
        #region Admin Order
        // lấy tất cả đơn hàng
        public JsonResult GetAllOrder()
        {
            return Json(dbcontext.GetAllOrder(), JsonRequestBehavior.AllowGet);
        }
        // cập nhật đơn hàng
        public JsonResult InsertOrder(Order order)
        {
            return Json(dbcontext.InsertOrder(order), JsonRequestBehavior.AllowGet);
        }
        // update đơn hàng
        public JsonResult UpdateOrder(Order order)
        {
            return Json(dbcontext.UpdateOrder(order), JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Admin OrderItem
        // lấy tất cả chi tiết
        public JsonResult GetAllOrderItem()
        {
            return Json(dbcontext.GetAllOrderItem(), JsonRequestBehavior.AllowGet);
        }
        // lấy chi tiết đơn hàng theo mã đơn hàng
        public JsonResult GetOrderItemByOrderId(int OrderId)
        {
            return Json(dbcontext.GetOrderItemByOrderId(OrderId), JsonRequestBehavior.AllowGet);
        }
        // thêm mới chi tiết đơn hàng
        public JsonResult InsertOrderItem(OrderItem orderitem)
        {
            return Json(dbcontext.InsertOrderItem(orderitem), JsonRequestBehavior.AllowGet);
        }
        // Update chi tiết đơn hàng
        public JsonResult UpdateOrderItem(OrderItem orderitem)
        {
            return Json(dbcontext.UpdateOrderItem(orderitem), JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Admin News
        // lấy tất cả tin tức
        public JsonResult GetAllNews()
        {
            return Json(dbcontext.GetAllNews(), JsonRequestBehavior.AllowGet);
        }
        // lấy tin tức theo id
        public JsonResult GetNewsById(int id)
        {
            return Json(dbcontext.GetNewsById(id), JsonRequestBehavior.AllowGet);
        }
        // thêm mới tin tức
        public JsonResult InsertNews(News news)
        {
            return Json(dbcontext.InsertNews(news), JsonRequestBehavior.AllowGet);
        }
        // cập nhật tin tức
        public JsonResult UpdateNews(News news)
        {
            return Json(dbcontext.UpdateNews(news), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNextNewsId()
        {
            return Json(dbcontext.GetNextNewsId(), JsonRequestBehavior.AllowGet);
        }
        public bool Login(string username, string password)
        {
            return CommonFunction.GetLoginStatus(username, password);
        }
        // GetAllTags - Lay tat ca Tag
        public JsonResult GetAllTags()
        {
            return Json(dbcontext.GetAllTags(), JsonRequestBehavior.AllowGet);
        }
        // InsertTags - Them moi Tag
        public JsonResult InsertTags(Tags tags)
        {
            return Json(dbcontext.InsertTags(tags), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}