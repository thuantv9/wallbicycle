-- Một số câu lệnh đối với SEQUENCE 
-- select current_value from sys.sequences where name='EventsSeq'
-- ALTER SEQUENCE EventsSeq
-- RESTART WITH 1
-- Kết thúc 
-- Create By Thuan Tran 12.7.2019
IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[Events]') AND type in (N'U'))
begin	
	drop table Events
end
go
Create table Events (
	[id] varchar(20) primary key,
	[title] nvarchar(500),
	[start] varchar(50) ,
	[end]  varchar(50),
	[url] varchar(200) null,
	[allDay] bit
)
-- Create SEQUENCE
IF EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[EventsSeq]') AND TYPE = 'SO')
BEGIN
	DROP SEQUENCE EventsSeq
END
CREATE SEQUENCE EventsSeq
AS INT
START WITH 1 
INCREMENT BY 1;
GO

-- GetAllEvents
IF EXISTS(SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllEvents' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllEvents
go
CREATE PROCEDURE dbo.GetAllEvents
as
begin
 select * from Events
end
go
-- Insert Events
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertEvents' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertEvents
go
create procedure dbo.InsertEvents
	@title nvarchar(500),
	@start varchar(50) ,
	@end  varchar(50),
	@url varchar(200) ,
	@allDay bit
as
begin
	 insert into Events(id,title,start,[end],url,allDay) values (Convert(varchar(20),next value for EventsSeq),@title,@start,@end,@url,@allDay); 
	end
go

--  Delete Events

-- Create table Patients
IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[Patients]') AND type in (N'U'))
begin	
	drop table Patients
end
go
Create table Patients (
	[id] varchar(50) primary key,
	[name] nvarchar(300),
	[birthday] datetime null,
	[address]  nvarchar(300) null,
	[image] varchar(300) null,
	[gender] bit,
	[telephone] varchar(30),
	[age] int,	
	[email] varchar(200),
	[metadata] ntext,
	[status] bit
)
go
-- Create sequence patient
IF EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[PatientsSeq]') AND TYPE = 'SO')
BEGIN
	DROP SEQUENCE PatientsSeq
END
CREATE SEQUENCE PatientsSeq
AS INT
START WITH 1 
INCREMENT BY 1;
GO

-- Get All Patients
IF EXISTS(SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllPatients' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllPatients
go
CREATE PROCEDURE dbo.GetAllPatients
as
begin
 select * from Patients
end
go

-- 
-- Insert Patients
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'InsertPatients' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.InsertPatients
go
create procedure dbo.InsertPatients	
	@name nvarchar(300),
	@birthday varchar(50),
	@address  nvarchar(300) ,
	@image varchar(300) ,
	@gender bit,
	@telephone varchar(30),
	@age int,	
	@email varchar(200),
	@metadata ntext,
	@status bit
as
begin
	 insert into Patients(name,birthday,address,image,gender, telephone, age, email, metadata,status) values (@name,@birthday,@address,@image,@gender, @telephone, @age, @email, @metadata,@status); 
	end
go
-- Update Patients
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'UpdatePatients' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.UpdatePatients
go
create procedure dbo.UpdatePatients
	@id varchar(50),
	@name nvarchar(300),
	@birthday varchar(50),
	@address  nvarchar(300) ,
	@image varchar(300) ,
	@gender bit,
	@telephone varchar(30),
	@age int,	
	@email varchar(200),
	@metadata ntext,
	@status bit
as
begin
	 update Patients 
		set 
			name=ISNULL(@name,name),
			birthday=ISNULL(@birthday,birthday),
			address=ISNULL(@address,address),
			image=ISNULL(@image,image),
			gender=ISNULL(@gender,gender),
			telephone=ISNULL(@telephone,telephone),
			age=ISNULL(@age,age),
			email=ISNULL(@email,email),
			metadata=ISNULL(@metadata,metadata),
			status= ISNULL(@status,status)
		where id=@id	
end
go
--  Delete Patients
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'PatientsDelete' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.PatientsDelete
GO
CREATE PROCEDURE dbo.PatientsDelete
	@id varchar(50)
AS
BEGIN
	UPDATE Patients
	SET status = 0
	WHERE id = @id;
END
GO

--  Get next Patients Sequence
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetNextPatientsSeq' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetNextPatientsSeq
GO
CREATE PROCEDURE dbo.GetNextPatientsSeq	
AS
BEGIN
	select next value for PatientsSeq
END
GO

-- Danh sách thủ thuật - Medical Procedures
IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[MedicalProcedure]') AND type in (N'U'))
begin	
	drop table MedicalProcedure
end
go
Create table MedicalProcedure (
	[id] varchar(20) primary key,
	[name] nvarchar(500),
	[description] varchar(50) ,
	[amount]  varchar(50),
	[category] varchar(200) null,
	[numberoftreatment] int
)


-- Danh sách cảnh báo sử dụng thuốc - Medical Alerts
IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[MedicalAlert]') AND type in (N'U'))
begin	
	drop table MedicalAlert
end
go
Create table MedicalAlert (
	[id] varchar(20) primary key,
	[name] nvarchar(500),
	[description] varchar(50)
)

-- Danh sách tiền sử bệnh - Medical History
IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[MedicalHistory') AND type in (N'U'))
begin	
	drop table MedicalHistory
end
go
Create table MedicalHistory (
	[id] varchar(20) primary key,
	[name] nvarchar(500),
	[description] varchar(50)
)

