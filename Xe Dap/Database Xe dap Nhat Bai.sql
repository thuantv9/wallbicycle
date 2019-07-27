create database [dbo].[XeDapNhatBai]
--1. tạo bảng
-- tạo bảng sản phẩm
IF OBJECT_ID('dbo.Product', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.Product
	DROP TABLE dbo.Product
END
go
create table Product
(
Id int not null primary key,
Name nvarchar(200),
MadeFrom nvarchar(100),
CategoryId int,
Quantity int,
Value decimal,
Tag nvarchar(max),
Image ntext,
Remark ntext,
Seo nvarchar(200),
Status bit
)
go
-- tạo bảng danh sách Tag
IF OBJECT_ID('dbo.Tags', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.Tags
	DROP TABLE dbo.Tags
END
go
create table Tags
(
Tag nvarchar(300)
)
go

-- tạo bảng danh mục sản phẩm
IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.Category
	DROP TABLE dbo.Category
END
go
create table Category
(
CategoryId int not null primary key,
CategoryName nvarchar(100),
CategorySeo nvarchar(150)
)
go
-- tạo bảng khách hàng (dùng cho phần hiển thị các dự án đã thực hiện)
IF OBJECT_ID('dbo.Customer', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.Customer
	DROP TABLE dbo.Customer
END
go
create table Customer
(
CustomerId	int not null primary key,
CustomerName nvarchar(300),
TelNumber varchar(100),
Address nvarchar(200),
CustomerDescription ntext,
CustomerRemark ntext
)
go
-- tạo bảng quản lý người dùng
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.Users
	DROP TABLE dbo.Users
END
go
create table Users
(
UserId int not null primary key,
UserName nvarchar(50),
Password ntext
)
go
-- tạo bảng quản lý hình ảnh chạy silde
IF OBJECT_ID('dbo.SlideImage', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.SlideImage
	DROP TABLE dbo.SlideImage
END
go
create table SlideImage
(
SlideId int not null primary key,
SlideImageName nvarchar(250)
)
go
-- tạo bảng đơn hàng
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.Orders
	DROP TABLE dbo.Orders
END
go
create table Orders
(
OrderId int not null primary key,
CustomerName nvarchar(300),
Creator nvarchar(300),
CreateDate datetime	
)
go
-- tạo bảng chi tiết đơn hàng
IF OBJECT_ID('dbo.OrderItem', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.OrderItem
	DROP TABLE dbo.OrderItem
END
go
create table OrderItem
(
OrderItemId int not null primary key,
OrderId int,
ProductId	int,
Quantity	int
)

-- tạo bảng tin tức
IF OBJECT_ID('dbo.News', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.News
	DROP TABLE dbo.News
END
go
create table News
(
NewsId int not null primary key,
NewsName nvarchar(300),
NewsImage nvarchar(300),
NewsDescription ntext,
NewsRemark ntext,
NewsMadeby nvarchar(300)
)
go

-- hết phần tạo bảng
--------------------------------------------------------------------------------------------------------

--------------------------------------------------------------------------------------------------------
--3. tạo procedure
-- đối với Product
-- lấy tất cả sản phẩm
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllProduct' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllProduct
go
create procedure dbo.GetAllProduct
as
begin
	select * from Product
end
go

-- lấy sản phẩm theo chủng loại sản phẩm (CategoryId)
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GeProductByCategoryId' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GeProductByCategoryId
go
create procedure dbo.GeProductByCategoryId
@CategoryId int
as
begin
	select * from Product where CategoryId=@CategoryId
end  
go

-- lấy sản phẩm theo Id sản phẩm
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetProductById' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetProductById
go
create procedure dbo.GetProductById
@Id int
as
begin
select a.Id,a.Name,a.MadeFrom,a.CategoryId,a.Quantity,a.Value,a.Tag,a.Image,a.Remark,a.Status,a.Seo,b.CategoryName
from Product a, Category b
where a.Id=@Id
and b.CategoryId in
(select CategoryId  from Product where Id=@Id)
end
go

-- lấy sản phẩm theo tên
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetProductByName' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetProductByName
go
create procedure GetProductByName
@Name nvarchar(200)
as
begin
	select * from Product where Name=@Name
end 
go

-- Thêm sản phẩm mới
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertProduct' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertProduct
go
create procedure dbo.InsertProduct
@Id int,
@Name nvarchar(200),
@MadeFrom nvarchar(100),
@CategoryId int,
@Quantity int,
@Value decimal,
@Tag nvarchar(max),
@Image ntext,
@Remark ntext,
@Status bit,
@Seo nvarchar(200)
as
begin
	insert into Product(Id,Name,MadeFrom,CategoryId,Quantity,Value,Tag,Image,Remark,Status,Seo) values (@Id,@Name,@MadeFrom,@CategoryId,@Quantity,@Value,@Tag,@Image,@Remark,@Status,@Seo); 
end
go

-- Update sản phẩm mới theo id
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'UpdateProduct' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.UpdateProduct
go
create procedure dbo.UpdateProduct
@Id int,
@Name nvarchar(200),
@MadeFrom nvarchar(100),
@CategoryId int,
@Quantity int,
@Value decimal,
@Tag nvarchar(max),
@Image ntext,
@Remark ntext,
@Status bit,
@Seo nvarchar(200)
as
begin
	update Product
	set	Name=@Name, MadeFrom=@MadeFrom, CategoryId=@CategoryId,Image=@Image,Remark=@Remark, Status=@Status, Seo=@Seo,Quantity=@Quantity,Value=@Value,Tag = @Tag
	where Id=@Id
end
go
-- Delete sản phẩm theo id
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'DeleteProduct' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.DeleteProduct
go
create procedure dbo.DeleteProduct
@Id int
as

begin
	delete from Product where Id=@Id
end
go 
-- procedure lấy số tiếp theo của ProdcutId
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetNextProductId' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetNextProductId
go
create procedure dbo.GetNextProductId
as
begin
	 select max(Id)+1 from Product
	
end
go

-- Bảng chủng loại sản phẩm
-- lấy tất cả chủng loại
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllCategory' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllCategory
go
create procedure dbo.GetAllCategory
as
begin 
	select * from Category
end
go
-- lấy chủng loại theo Id
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetCategoryById' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetCategoryById
go
create procedure dbo.GetCategoryById
@CategoryId int
as
begin
	select * from Category where CategoryId = @CategoryId
end
go

-- thêm chủng loại
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertCategory' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertCategory
go
create procedure dbo.InsertCategory
@CategoryId int,
@CategoryName nvarchar(100),
@CategorySeo nvarchar(150)
as
begin
	insert into Category values (@CategoryId,@CategoryName,@CategorySeo);
end
GO
-- Update chủng loại
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'UpdateCategory' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.UpdateCategory
GO

create procedure dbo.UpdateCategory
@CategoryId int,
@CategoryName nvarchar(100),
@CategorySeo nvarchar(150)
as
begin
	update  Category 
	set CategoryName=@CategoryName, CategorySeo=@CategorySeo
	where CategoryId=@CategoryId	
end
GO
-- Delete chủng loại theo id, nếu còn sản phẩm của chủng loại thì không cho xóa
-- Tham khảo từ Online Help
-- procedure lấy số tiếp theo của CategoryIdId
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetNextCategoryId' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetNextCategoryId
GO
create procedure dbo.GetNextCategoryId
as
begin
	 select max(CategoryId)+1 from Category	
end
go
 
-- Bảng khách hàng
--  lấy tất cả khách hàng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllCustomer' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllCustomer
GO
create procedure dbo.GetAllCustomer
as
begin
	select * from Customer
end
go
-- lấy khách hàng theo Customer id
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetCustomerById' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetCustomerById
go
create procedure dbo.GetCustomerById
@CustomerId int
as
begin
	select * from Customer where CustomerId=@CustomerId
end
go
-- thêm mới khách hàng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertCustomer' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertCustomer
go
create procedure dbo.InsertCustomer
@CustomerId int,
@CustomerName nvarchar(300),
@TelNumber varchar(100),
@Address nvarchar(100),
@CustomerDescription ntext,
@CustomerRemark ntext
as
begin
	insert into Customer(CustomerId,CustomerName,TelNumber,Address,CustomerRemark) values (@CustomerId,@CustomerName,@TelNumber,@Address,@CustomerRemark);
end
go

-- update khách hàng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'UpdateCustomer' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.UpdateCustomer
go
create procedure dbo.UpdateCustomer
@CustomerId int,
@CustomerName nvarchar(300),
@CustomerImage nvarchar(200),
@TelNumber varchar(100),
@Address nvarchar(100),
@CustomerRemark ntext
as
begin
	update Customer
	set CustomerName=@CustomerName, TelNumber=@TelNumber,Address=@Address,CustomerRemark=@CustomerRemark
	where CustomerId=@CustomerId
end
go
-- delete khách hàng theo id
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'DeleteCustomerById' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.DeleteCustomerById
go
create procedure dbo.DeleteCustomerById
@CustomerId int
as
begin
	delete from Customer where CustomerId=@CustomerId
end
go
-- procedure lấy số tiếp theo của CustomerId
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetNextCustomerId' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetNextCustomerId
go		
create procedure dbo.GetNextCustomerId
as
begin
	 select max(CustomerId)+1 from Customer
end
go
-- Bảng người dùng
-- lấy tất cả người dùng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllUser' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllUser
go
create procedure dbo.GetAllUser
as
begin
	select * from Users
end
go
-- lấy người dùng theo UserName
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetUserByUserName' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetUserByUserName
go
create procedure dbo.GetUserByUserName
@UserName nvarchar(50)
as
begin
	select * from Users where UserName=@UserName
end
go
-- thêm mới người dùng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertUser' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertUser
go
create procedure dbo.InsertUser
@UserId int,
@UserName nvarchar(50),
@Password ntext
as
begin
	insert into Users(UserId,UserName,Password) values (@UserId,@UserName,@Password); 
end
go
-- Update người dùng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'UpdateUser' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.UpdateUser
go
create procedure dbo.UpdateUser
@UserId int,
@UserName nvarchar(50),
@Password ntext
as
begin
	update Users
	set UserName=@UserName, Password=@Password
	where UserId=@UserId
end
go
-- Xóa người dùng theo ID
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'DeleteUser' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.DeleteUser
go
create procedure dbo.DeleteUser
@UserId int
as
begin
	delete from Users where UserId=@UserId
end
go


-- Bảng Slide Image
-- lấy tất cả slide
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllSlideImage' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllSlideImage
go
create procedure dbo.GetAllSlideImage
as
begin
	select * from SlideImage
end
go
-- thêm mới ảnh vào slide
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertSlideImage' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertSlideImage
go
create procedure dbo.InsertSlideImage
@SlideId int,
@SlideImageName nvarchar(50)
as
begin
	insert into SlideImage(SlideId,SlideImageName) values (@SlideId,@SlideImageName); 
end
go
-- xóa ảnh theo id
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'DeleteSlideImage' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.DeleteSlideImage
go
create procedure dbo.DeleteSlideImage
@SlideId int
as
begin
	delete from SlideImage where SlideId=@SlideId
end
go
-- lấy slide theo id
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetSlideById' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetSlideById
go
create procedure dbo.GetSlideById
@SlideId int
as
begin
	select * from SlideImage where SlideId=@SlideId
end
go
-- Update slide 
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'UpdateSlide' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.UpdateSlide
go
create procedure dbo.UpdateSlide 
@SlideId int,
@SlideImageName nvarchar(300)
as
begin
	update SlideImage set 	
	SlideImageName = @SlideImageName
	where 
	SlideId = @SlideId
end
go

-- lấy số tiếp theo của SlideId
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetNextSlideId' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetNextSlideId
go
create procedure dbo.GetNextSlideId
as
begin
	 select max(SlideId)+1 from SlideImage
	
end
go
-- Bảng đơn hàng
-- lấy tất cả đơn hàng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllOrder' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllOrder
go
create procedure dbo.GetAllOrder
as
begin
	select * from Orders 
end
go
-- thêm mới đơn hàng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertOrder' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertOrder
go
create procedure dbo.InsertOrder
(
@OrderId int,
@CustomerName nvarchar(300),
@Creator nvarchar(300),
@CreateDate datetime
)
as
begin
	insert into Orders values (@OrderId, @CustomerName, @Creator, @CreateDate);
end
go
--  Update đơn hàng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'UpdateOrder' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.UpdateOrder
go
create procedure dbo.UpdateOrder
@OrderId int,
@CustomerName nvarchar(300),
@Creator nvarchar(300),
@CreateDate datetime
as
begin
	update Orders 
	set CustomerName=@CustomerName, Creator=@Creator, CreateDate=@CreateDate
	where OrderId=@OrderId
end
go

-- bảng chi tiết đơn hàng
-- lấy tất cả chi tiết đơn hàng 
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllOrderItem' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllOrderItem
go
create procedure dbo.GetAllOrderItem
as
begin
	select * from OrderItem
end
go
-- lấy chi tiết đơn hàng theo mã đơn hàng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetOrderItemByOrderId' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetOrderItemByOrderId
go
create procedure dbo.GetOrderItemByOrderId
@OrderId int
as
begin
	select * from OrderItem where OrderId = @OrderId
end
go
-- Thêm mới chi tiết đơn hàng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertOrderItem' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertOrderItem
go
create procedure dbo.InsertOrderItem
@OrderItemId int,
@OrderId int,
@ProductId int,
@Quantity int
as
begin
	insert into OrderItem values (@OrderItemId,@OrderId,@ProductId,@Quantity);
end
go
-- Update chi tiết đơn hàng
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'UpdateOrderItem' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.UpdateOrderItem
go
create procedure dbo.UpdateOrderItem
@OrderItemId int,
@OrderId int,
@ProductId int,
@Quantity int
as
begin
	update  OrderItem 
	set OrderId=@OrderId,ProductId=@ProductId,Quantity=@Quantity
	where OrderItemId=@OrderItemId
end
go

-- Thử nghiệm split string trong SQL
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'splitstring' AND ROUTINE_SCHEMA = 'dbo')
		DROP FUNCTION dbo.splitstring
go
CREATE FUNCTION dbo.splitstring ( @stringToSplit VARCHAR(MAX) )
RETURNS
 @returnList TABLE ([Name] [nvarchar] (500))
AS
BEGIN

 DECLARE @name NVARCHAR(255)
 DECLARE @pos INT

 WHILE CHARINDEX(',', @stringToSplit) > 0
 BEGIN
  SELECT @pos  = CHARINDEX(',', @stringToSplit)  
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

  INSERT INTO @returnList 
  SELECT @name

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
 END

 INSERT INTO @returnList
 SELECT @stringToSplit

 RETURN
END
GO
-- Bảng tin tức News
-- lấy tất cả tin tức
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllNews' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllNews
go
create procedure dbo.GetAllNews
as
begin
	select * from News order by NewsId desc
end
go

-- lấy tin tức theo id
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetNewsById' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetNewsById
go
create procedure dbo.GetNewsById
@NewsId int
as
begin
	select * from News where NewsId=@NewsId
end
go

-- Thêm tin tức mới
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertNews' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertNews
go
create procedure dbo.InsertNews
@NewsId int,
@NewsName nvarchar(300),
@NewsImage nvarchar(300),
@NewsDescription ntext,
@NewsRemark ntext,
@NewsMadeby nvarchar(300)
as
begin
	insert into News(NewsId,NewsName,NewsImage,NewsDescription,NewsRemark,NewsMadeby) 
	values (@NewsId,@NewsName,@NewsImage,@NewsDescription,@NewsRemark,@NewsMadeby); 
end
go

-- Update tin tức mới
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'UpdateNews' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.UpdateNews
go
create procedure dbo.UpdateNews 
@NewsId int,
@NewsName nvarchar(300),
@NewsImage nvarchar(300),
@NewsDescription ntext,
@NewsRemark ntext,
@NewsMadeby nvarchar(300)
as
begin
	update News set 	
	NewsName = @NewsName,
	NewsImage = @NewsImage,
	NewsDescription = @NewsDescription,
	NewsRemark =@NewsRemark,
	NewsMadeby = @NewsMadeby	
	where 
	NewsId = @NewsId
	; 
end
go

-- lấy số tiếp theo của NewsId
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetNextNewsId' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetNextNewsId
go
create procedure dbo.GetNextNewsId
as
begin
	 select max(NewsId)+1 from News
	
end
go

-- procedure voi bang Tags
-- GetAllTags - Lay tat ca Tag
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllTags' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllTags
go
create procedure dbo.GetAllTags
as
begin
	 select * from Tags
	
end
go

-- InsertTags - Them moi Tag
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertTags' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertTags
go
create procedure dbo.InsertTags
@Tag nvarchar(300)
as
begin
	insert into Tags(Tag) values (@Tag); 
end
go

