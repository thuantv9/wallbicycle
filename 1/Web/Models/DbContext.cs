using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Web.Common;
namespace Web.Models
{
    public class DbContext
    {
        #region Table Product
        // lấy tất cả sản phẩm
        public List<Product> GetAllProduct()
        {
            List<Product> products = new List<Product>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllProduct", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product()
                        {
                            Id = Int32.Parse(reader["Id"].ToString()),
                            Name = reader["Name"].ToString(),
                            MadeFrom = reader["MadeFrom"].ToString(),
                            CategoryId = Int32.Parse(reader["CategoryId"].ToString()),
                            Quantity = Int32.Parse(reader["Quantity"].ToString()),
                            Value = Decimal.Parse(reader["Value"].ToString()),
                            Tag = reader["Tag"].ToString(),
                            Image = reader["Image"].ToString(),
                            Remark = reader["Remark"].ToString(),
                            Status = Boolean.Parse(reader["Status"].ToString()),
                            Seo = reader["Seo"].ToString(),
                        });
                    }
                    return products;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // lấy sản phẩm theo chủng loại sản phẩm
        public List<Product> GetProductByCategoryId(int CategoryId)
        {
            List<Product> products = new List<Product>();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GeProductByCategoryId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product()
                    {
                        Id = Int32.Parse(reader["ID"].ToString()),
                        Name = reader["Name"].ToString(),
                        MadeFrom = reader["MadeFrom"].ToString(),
                        CategoryId = Int32.Parse(reader["CategoryId"].ToString()),
                        Quantity = Int32.Parse(reader["Quantity"].ToString()),
                        Value = Decimal.Parse(reader["Value"].ToString()),
                        Tag = reader["Tag"].ToString(),
                        Image = reader["Image"].ToString(),
                        Remark = reader["Remark"].ToString(),
                        Status = Boolean.Parse(reader["Status"].ToString()),
                        Seo = reader["Seo"].ToString(),
                    });
                }
                return products;
            }
        }
        // lấy sản phẩm theo Id sản phẩm
        public Product GetProductById(int Id)
        {
            Product c = new Product();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetProductById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.Id = Int32.Parse(reader["Id"].ToString());
                    c.Name = reader["Name"].ToString();
                    c.MadeFrom = reader["MadeFrom"].ToString();
                    c.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                    c.Quantity = Int32.Parse(reader["Quantity"].ToString());
                    c.Value = Decimal.Parse(reader["Value"].ToString());
                    c.Tag = reader["Tag"].ToString();
                    c.Image = reader["Image"].ToString();
                    c.Remark = reader["Remark"].ToString();
                    c.Status = Boolean.Parse(reader["Status"].ToString());
                    c.Seo = reader["Seo"].ToString();
                }
                return c;
            }
        }
        // lấy sản phẩm theo tên
        public Product GetProductById(string Name)
        {
            Product c = new Product();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetProductByName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", Name);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.Id = Int32.Parse(reader["Id"].ToString());
                    c.Name = reader["Name"].ToString();
                    c.MadeFrom = reader["MadeFrom"].ToString();
                    c.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                    c.Quantity = Int32.Parse(reader["Quantity"].ToString());
                    c.Value = Decimal.Parse(reader["Value"].ToString());
                    c.Tag = reader["Tag"].ToString();
                    c.Image = reader["Image"].ToString();
                    c.Remark = reader["Remark"].ToString();
                    c.Status = Boolean.Parse(reader["Status"].ToString());
                    c.Seo = reader["Seo"].ToString();
                }
                return c;
            }
        }
        //  Thêm mới sản phẩm
        public int InsertProduct(Product product)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InsertProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Int32.Parse(product.Id.ToString()));
                cmd.Parameters.AddWithValue("@Name", product.Name.ToString());
                cmd.Parameters.AddWithValue("@MadeFrom", product.MadeFrom);
                cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Value", product.Value);
                //cmd.Parameters.AddWithValue("@Tag", product.Tag);
                cmd.Parameters.AddWithValue("@Image", product.Image);
                // Check remarks là null thì truyền null
                if (product.Tag == null)
                {
                    cmd.Parameters.AddWithValue("@Tag", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Tag", product.Tag);
                }
                if (product.Remark == null)
                {
                    cmd.Parameters.AddWithValue("@Remark", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Remark", product.Remark);
                }

                if (product.Seo == null)
                {
                    cmd.Parameters.AddWithValue("@Seo", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Seo", product.Seo);
                }
                cmd.Parameters.AddWithValue("@Status", Boolean.Parse(product.Status.ToString()));
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        // Cập nhật sản phẩm
        public int UpdateProduct(Product product)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Int32.Parse(product.Id.ToString()));
                cmd.Parameters.AddWithValue("@Name", product.Name.ToString());
                cmd.Parameters.AddWithValue("@MadeFrom", product.MadeFrom);
                cmd.Parameters.AddWithValue("@CategoryId", Int32.Parse(product.CategoryId.ToString()));
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Value", product.Value);
                cmd.Parameters.AddWithValue("@Image", product.Image);
                // Check remarks là null thì truyền null
                if (product.Tag == null)
                {
                    cmd.Parameters.AddWithValue("@Tag", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Tag", product.Tag);
                }
                if (product.Remark == null)
                {
                    cmd.Parameters.AddWithValue("@Remark", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Remark", product.Remark);
                }
                cmd.Parameters.AddWithValue("@Status", Boolean.Parse(product.Status.ToString()));
                // bo sung seo
                if (product.Seo == null)
                {
                    cmd.Parameters.AddWithValue("@Seo", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Seo", product.Seo);
                }
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        // Xóa sản phẩm 
        public int DeleteProduct(int Id)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand com = new SqlCommand("DeleteProduct", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", Id);
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        // lấy số Id product tiếp theo
        public int GetNextProductId()
        {
            int i = 1;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand com = new SqlCommand("GetNextProductId", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = com.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        i = Int32.Parse(reader[0].ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return i;
        }
        #endregion
        #region Table Category
        // lấy tất cả loại sản phẩm
        public List<Category> GetAllCategory()
        {
            List<Category> categories = new List<Category>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllCategory", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            CategoryId = Int32.Parse(reader["CategoryId"].ToString()),
                            CategoryName = reader["CategoryName"].ToString(),
                            CategorySeo = reader["CategorySeo"].ToString()
                        });
                    }
                    return categories;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // lấy chủng loại theo Id
        public Category GetCategoryById(int id)
        {
            Category c = new Category();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetCategoryById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                    c.CategoryName = reader["CategoryName"].ToString();
                    c.CategorySeo = reader["CategorySeo"].ToString();
                }
                return c;
            }
        }
        // Thêm chủng loại
        public int InsertCategory(Category category)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InsertCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", Int32.Parse(category.CategoryId.ToString()));
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                cmd.Parameters.AddWithValue("@CategorySeo", category.CategorySeo);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        // Cập nhật chủng loại
        public int UpdateCategory(Category category)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", Int32.Parse(category.CategoryId.ToString()));
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName.ToString());
                cmd.Parameters.AddWithValue("@CategorySeo", category.CategorySeo.ToString());
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        // lấy số Id category tiếp theo
        public int GetNextCategoryId()
        {
            int i = 1;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand com = new SqlCommand("GetNextCategoryId", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = com.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        i = Int32.Parse(reader[0].ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return i;
        }
        #endregion
        #region Table Customer
        // lấy tất cả khách hàng
        public List<Customer> GetAllCustomer()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllCustomer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        customers.Add(new Customer()
                        {
                            CustomerId = Int32.Parse(reader["CustomerId"].ToString()),
                            CustomerName = reader["CustomerName"].ToString(),
                            TelNumber = reader["TelNumber"].ToString(),
                            Address = reader["Address"].ToString(),
                            CustomerDescription = reader["CustomerDescription"].ToString(),
                            CustomerRemark = reader["CustomerRemark"].ToString()

                        });
                    }
                    return customers;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // láy customer theo id
        public Customer GetCustomerById(int id)
        {
            Customer c = new Customer();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetCustomerById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerId", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.CustomerId = Int32.Parse(reader["CustomerId"].ToString());
                    c.CustomerName = reader["CustomerName"].ToString();
                    c.TelNumber = reader["TelNumber"].ToString();
                    c.Address = reader["Address"].ToString();
                    c.CustomerDescription = reader["CustomerDescription"].ToString();
                    c.CustomerRemark = reader["CustomerRemark"].ToString();

                }
                return c;
            }
        }
        // thêm mới khách hàng
        public int InsertCustomer(Customer customer)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InsertCustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerId", Int32.Parse(customer.CustomerId.ToString()));
                cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName.ToString());
                // Check CustomerImage là null thì truyền null
                if (customer.TelNumber == null)
                {
                    cmd.Parameters.AddWithValue("@TelNumber", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TelNumber", customer.TelNumber);
                }
                if (customer.Address == null)
                {
                    cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                }
                // Check CustomerDescription là null thì truyền null
                if (customer.CustomerDescription == null)
                {
                    cmd.Parameters.AddWithValue("@CustomerDescription", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CustomerDescription", customer.CustomerDescription);
                }
                //  // Check CustomerRemark là null thì truyền null
                if (customer.CustomerRemark == null)
                {
                    cmd.Parameters.AddWithValue("@CustomerRemark", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CustomerRemark", customer.CustomerRemark);
                }
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        // Cập nhật khách hàng
        public int UpdateCustomer(Customer customer)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateCustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerId", Int32.Parse(customer.CustomerId.ToString()));
                cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName.ToString());
                // Check CustomerImage là null thì truyền null
                if (customer.TelNumber == null)
                {
                    cmd.Parameters.AddWithValue("@TelNumber", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TelNumber", customer.TelNumber);
                }
                if (customer.Address == null)
                {
                    cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                }
                // Check CustomerDescription là null thì truyền null
                if (customer.CustomerDescription == null)
                {
                    cmd.Parameters.AddWithValue("@CustomerDescription", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CustomerDescription", customer.CustomerDescription);
                }
                //  // Check CustomerRemark là null thì truyền null
                if (customer.CustomerRemark == null)
                {
                    cmd.Parameters.AddWithValue("@CustomerRemark", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CustomerRemark", customer.CustomerRemark);
                }
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        // Xóa khách hàng
        public int DeleteCustomerById(int CustomerId)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand com = new SqlCommand("DeleteCustomerById", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustomerId", CustomerId);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        // lấy số Id Customer tiếp theo
        public int GetNextCustomerId()
        {
            int i = 1;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand com = new SqlCommand("GetNextCustomerId", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = com.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        i = Int32.Parse(reader[0].ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return i;
        }
        public int DeleteCustomer(int Id)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand com = new SqlCommand("DeleteCustomerById", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustomerId", Id);
                i = com.ExecuteNonQuery();
            }
            return i;
        }


        #endregion
        #region Table User

        #endregion
        #region Table SlideImage
        // lấy tất cả slide anh
        public List<SlideImage> GetAllSlideImage()
        {
            List<SlideImage> slides = new List<SlideImage>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllSlideImage", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        slides.Add(new SlideImage()
                        {
                            SlideId = Int32.Parse(reader["SlideId"].ToString()),
                            SlideImageName = reader["SlideImageName"].ToString(),

                        });
                    }
                    return slides;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //  Thêm mới Slide
        public int InsertSlideImage(SlideImage slideimage)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InsertSlideImage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SlideId", Int32.Parse(slideimage.SlideId.ToString()));
                cmd.Parameters.AddWithValue("@SlideImageName", slideimage.SlideImageName.ToString());
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        // láy slide theo id
        public SlideImage GetSlideById(int id)
        {
            SlideImage c = new SlideImage();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetSlideById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SlideId", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.SlideId = Int32.Parse(reader["SlideId"].ToString());
                    c.SlideImageName = reader["SlideImageName"].ToString();
                }
                return c;
            }
        }
        // lấy số Id News tiếp theo
        public int GetNextSlideId()
        {
            int i = 1;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand com = new SqlCommand("GetNextSlideId", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = com.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        i = Int32.Parse(reader[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    return i;
                }
            }
            return i;
        }
        // Cập nhật slide
        public int UpdateSlide(SlideImage slide)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateSlide", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SlideId", Int32.Parse(slide.SlideId.ToString()));
                cmd.Parameters.AddWithValue("@SlideImageName", slide.SlideImageName.ToString());
                // Check NewsImage là null thì truyền null
                if (slide.SlideImageName == null)
                {
                    cmd.Parameters.AddWithValue("@SlideImageName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SlideImageName", slide.SlideImageName);
                }
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        // Xóa Slide
        public int DeleteSlide(int Id)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand com = new SqlCommand("DeleteSlideImage", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SlideId", Id);
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        #endregion
        #region Table Order
        // lấy tất cả đơn hàng
        public List<Order> GetAllOrder()
        {
            List<Order> orders = new List<Order>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllOrder", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new Order()
                        {
                            OrderId = Int32.Parse(reader["OrderId"].ToString()),
                            CustomerName = reader["CustomerName"].ToString(),
                            Creator = reader["Creator"].ToString(),
                            CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString())
                        });
                    }
                    return orders;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // thêm mới đơn hàng
        public int InsertOrder(Order order)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InsertOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", Int32.Parse(order.OrderId.ToString()));
                cmd.Parameters.AddWithValue("@CustomerName", order.CustomerName);
                cmd.Parameters.AddWithValue("@Creator", order.Creator);
                cmd.Parameters.AddWithValue("@CreateDate", order.CreateDate);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        // Update đơn hàng
        public int UpdateOrder(Order order)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", Int32.Parse(order.OrderId.ToString()));
                cmd.Parameters.AddWithValue("@CustomerName", order.CustomerName);
                cmd.Parameters.AddWithValue("@Creator", order.Creator);
                cmd.Parameters.AddWithValue("@CreateDate", order.CreateDate);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        #endregion
        #region Table OrderItem
        // lấy tất cả chi tiết hóa đơn
        public List<OrderItem> GetAllOrderItem()
        {
            List<OrderItem> orderitems = new List<OrderItem>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllOrderItem", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        orderitems.Add(new OrderItem()
                        {
                            OrderItemId = Int32.Parse(reader["OrderItemId"].ToString()),
                            OrderId = Int32.Parse(reader["OrderId"].ToString()),
                            ProductId = Int32.Parse(reader["ProductId"].ToString()),
                            Quantity = Int32.Parse(reader["Quantity"].ToString())
                        });
                    }
                    return orderitems;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // lấy chi tiết đơn hàng theo mã đơn hàng
        public List<OrderItem> GetOrderItemByOrderId(int OrderId)
        {
            List<OrderItem> orderitems = new List<OrderItem>();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetOrderItemByOrderId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", OrderId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orderitems.Add(new OrderItem()
                    {
                        OrderItemId = Int32.Parse(reader["OrderItemId"].ToString()),
                        OrderId = Int32.Parse(reader["OrderId"].ToString()),
                        ProductId = Int32.Parse(reader["ProductId"].ToString()),
                        Quantity = Int32.Parse(reader["Quantity"].ToString())
                    });
                }
                return orderitems;
            }
        }
        // Thêm mới chi tiết đơn hàng
        public int InsertOrderItem(OrderItem orderitem)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InsertOrderItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderItemId", Int32.Parse(orderitem.OrderItemId.ToString()));
                cmd.Parameters.AddWithValue("@OrderId", Int32.Parse(orderitem.OrderId.ToString()));
                cmd.Parameters.AddWithValue("@ProductId", Int32.Parse(orderitem.ProductId.ToString()));
                cmd.Parameters.AddWithValue("@Quantity", Int32.Parse(orderitem.Quantity.ToString()));
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        // Update chi tiết đơn hàng
        public int UpdateOrderItem(OrderItem orderitem)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateOrderItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderItemId", Int32.Parse(orderitem.OrderItemId.ToString()));
                cmd.Parameters.AddWithValue("@OrderId", Int32.Parse(orderitem.OrderId.ToString()));
                cmd.Parameters.AddWithValue("@ProductId", Int32.Parse(orderitem.ProductId.ToString()));
                cmd.Parameters.AddWithValue("@Quantity", Int32.Parse(orderitem.Quantity.ToString()));
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        #endregion
        #region Table News
        // lấy tất cả tin tức
        public List<News> GetAllNews()
        {
            List<News> newss = new List<News>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllNews", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        newss.Add(new News()
                        {
                            NewsId = Int32.Parse(reader["NewsId"].ToString()),
                            NewsName = reader["NewsName"].ToString(),
                            NewsImage = reader["NewsImage"].ToString(),
                            NewsDescription = reader["NewsDescription"].ToString(),
                            NewsRemark = reader["NewsRemark"].ToString(),
                            NewsMadeby = reader["NewsMadeby"].ToString()
                        });
                    }
                    return newss;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // láy tin tức theo id
        public News GetNewsById(int id)
        {
            News c = new News();
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetNewsById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NewsId", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.NewsId = Int32.Parse(reader["NewsId"].ToString());
                    c.NewsName = reader["NewsName"].ToString();
                    c.NewsImage = reader["NewsImage"].ToString();
                    c.NewsDescription = reader["NewsDescription"].ToString();
                    c.NewsRemark = reader["NewsRemark"].ToString();
                    c.NewsMadeby = reader["NewsMadeby"].ToString();
                }
                return c;
            }
        }
        // thêm mới tin tức
        public int InsertNews(News news)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InsertNews", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NewsId", Int32.Parse(news.NewsId.ToString()));
                cmd.Parameters.AddWithValue("@NewsName", news.NewsName.ToString());
                // Check NewsImage là null thì truyền null
                if (news.NewsImage == null)
                {
                    cmd.Parameters.AddWithValue("@NewsImage", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NewsImage", news.NewsImage);
                }
                // Check newsremark là null thì truyền null
                if (news.NewsRemark == null)
                {
                    cmd.Parameters.AddWithValue("@NewsRemark", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NewsRemark", news.NewsRemark);
                }
                // check NewsDescription la null 
                if (news.NewsDescription == null)
                {
                    cmd.Parameters.AddWithValue("@NewsDescription", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NewsDescription", news.NewsDescription);
                }
                // check NewsMadeby la null
                if (news.NewsMadeby == null)
                {
                    cmd.Parameters.AddWithValue("@NewsMadeby", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NewsMadeby", news.NewsMadeby);
                }

                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        // Cập nhật tin tức
        public int UpdateNews(News news)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateNews", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NewsId", Int32.Parse(news.NewsId.ToString()));
                cmd.Parameters.AddWithValue("@NewsName", news.NewsName.ToString());
                // Check NewsImage là null thì truyền null
                if (news.NewsImage == null)
                {
                    cmd.Parameters.AddWithValue("@NewsImage", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NewsImage", news.NewsImage);
                }
                // Check CustomerDescription là null thì truyền null
                if (news.NewsRemark == null)
                {
                    cmd.Parameters.AddWithValue("@NewsRemark", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NewsRemark", news.NewsRemark);
                }
                // check NewsDescription la null 
                if (news.NewsDescription == null)
                {
                    cmd.Parameters.AddWithValue("@NewsDescription", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NewsDescription", news.NewsDescription);
                }
                // check NewsMadeby la null
                if (news.NewsMadeby == null)
                {
                    cmd.Parameters.AddWithValue("@NewsMadeby", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@NewsMadeby", news.NewsMadeby);
                }
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        // lấy số Id News tiếp theo
        public int GetNextNewsId()
        {
            int i = 1;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand com = new SqlCommand("GetNextNewsId", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = com.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        i = Int32.Parse(reader[0].ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return i;
        }

        #endregion
        #region Tags
        // lấy tất cả tin tức
        public List<Tags> GetAllTags()
        {
            List<Tags> tags = new List<Tags>();
            try
            {
                using (SqlConnection con = new SqlConnection(Const.Connectring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllTags", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tags.Add(new Tags()
                        {
                            Tag = reader["Tag"].ToString()
                        });
                    }
                    return tags;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // Thêm mới Tag
        public int InsertTags(Tags tags)
        {
            int i;
            using (SqlConnection con = new SqlConnection(Const.Connectring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InsertTags", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tag", Int32.Parse(tags.Tag.ToString()));
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
        #endregion
    }
}