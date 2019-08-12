-- Một số câu lệnh đối với SEQUENCE 
-- select current_value from sys.sequences where name='EventsSeq'
-- ALTER SEQUENCE EventsSeq
-- RESTART WITH 1
-- Kết thúc 
-- Readme
-- MedicalAlert va MedicalHistory trong Table Patient moi phan tu cach nhau boi dau |

-- Payment không thiết lập thành bảng mà sẽ thiết lập thành class và luu xuống bệnh nhân
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
	[yearofbirth] int null,
	[address]  nvarchar(300) null,
	[image] varchar(300) null,
	[gender] bit,
	[telephone] varchar(30) null,
	[age] int,	
	[email] varchar(200) null,
	[metadata] ntext null, -- dữ liệu khác
	[medicalalert] nvarchar(500) null, -- dữ liệu json danh mục cảnh báo thuốc
	[medicalhistory] nvarchar(500) null, -- dữ liệu json danh mục tiền sử bệnh
	[examjson] ntext null, -- dữ liệu json về việc khám bệnh bao gồm: triệu chứng bệnh, danh sách chẩn đoán bệnh, ghi chú.
	[treatmentjson] ntext null, -- dữ liệu json về danh sách phác đồ điều trị 
	[paymentjson] ntext null, -- dữ liệu json về danh sách phiếu thanh toán
	[status] nvarchar(300) null, -- Mới, Đang điều trị, Đã điều trị xong, tương đương statustreatment
	[statuspayment] nvarchar(300), -- Tình trạng thanh toán
	[statusinaday] nvarchar(300) null,	-- Tình trạng trong ngày: Đang chờ khám, đang khám (dữ liệu set up tại bảng All data json 
	[active] bit 
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
	@yearofbirth int,
	@address  nvarchar(300) ,
	@image varchar(300) ,
	@gender bit,
	@telephone varchar(30),
	@age int,	
	@email varchar(200),
	@metadata ntext,
	@medicalalert nvarchar(500),
	@medicalhistory nvarchar(500),
	@examjson ntext,
	@treatmentjson ntext,
	@paymentjson ntext,
	@status nvarchar(300),
	@statusinaday nvarchar(300),
	@active bit
as
begin
	 insert into Patients(name,yearofbirth,address,image,gender, telephone, age, email, metadata,medicalalert,medicalhistory,examjson,treatmentjson,paymentjson,status,statusinaday,active) values (@name,@yearofbirth,@address,@image,@gender, @telephone, @age, @email, @metadata,@medicalalert,@medicalhistory,@examjson,@treatmentjson,@paymentjson,@status,@statusinaday,@active); 
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
	@yearofbirth int,
	@address  nvarchar(300) ,
	@image varchar(300) ,
	@gender bit,
	@telephone varchar(30),
	@age int,	
	@email varchar(200),
	@metadata ntext,
	@medicalalert nvarchar(500),
	@medicalhistory nvarchar(500),
	@examjson ntext,
	@treatmentjson ntext,
	@paymentjson ntext,
	@status nvarchar(300),
	@statusinaday nvarchar(300),
	@active bit
as
begin
	 update Patients 
		set 
			name=ISNULL(@name,name),
			yearofbirth=ISNULL(@yearofbirth,yearofbirth),
			address=ISNULL(@address,address),
			image=ISNULL(@image,image),
			gender=ISNULL(@gender,gender),
			telephone=ISNULL(@telephone,telephone),
			age=ISNULL(@age,age),
			email=ISNULL(@email,email),
			metadata=ISNULL(@metadata,metadata),
			medicalalert =ISNULL(@medicalalert,medicalalert),
			medicalhistory = ISNULL(@medicalhistory,medicalhistory),
			examjson = ISNULL(@examjson,examjson),
			treatmentjson =ISNULL(@treatmentjson,treatmentjson),
			paymentjson =ISNULL(@paymentjson,paymentjson),
			status= ISNULL(@status,status),
			statusinaday = ISNULL(@statusinaday,statusinaday),
			active = ISNULL(@active,active)
		where id=@id	
end
go
-- Update StatusInADay Patients
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'UpdateStatusInADayPatients' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.UpdateStatusInADayPatients
go
create procedure dbo.UpdateStatusInADayPatients
	@id varchar(50),	
	@statusinaday nvarchar(300)
as
begin
declare @count integer
set @count =0
if((select count(*) from Patients where statusinaday = N'Đang khám') <= @count)
	 update Patients set statusinaday = ISNULL(@statusinaday,statusinaday) where id=@id			
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
	SET active = 0
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
	[description] varchar(500) ,
	[amount]  bigint,
	[categorytreatment] varchar(20) null, -- loại thủ thuật: 1 lần hay nhiều lần (cái này thiết lập trong bảng AllDataJson
	[numberoftreatment] int, -- Số lần điều trị: thủ thuật 1 lần sẽ là 1, thủ thuật nhiều lần sẽ là nhiều
	[idcause] nvarchar(500) null, -- thủ thuật này sẽ dùng cho các kết quả khám nào, ví dụ Exam01|Exam02 với Exam01: sâu răng, Exam02: viêm lợi.
	[steptreatment] ntext null 
	-- mô tả trường :steptreatment
	-- mô tả chi tiết các bước điều trị: trường này sẽ lưu trữ json của các bước điều trị ví dụ 4 bước thì sẽ bao gồm
	-- idstep : mã bước
	-- namestep: tên bước
	-- distancestep: thời gian từ bước trước (vd: 15 ngày)
	-- descriptionstep: mô tả bước
	-- amountstep: số tiền phải trả khi thực hiện bước này.
)
go

--- Sequence MedicalProcedure
IF EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[MedicalProcedureSeq]') AND TYPE = 'SO')
BEGIN
	DROP SEQUENCE MedicalProcedureSeq
END
CREATE SEQUENCE MedicalProcedureSeq
AS INT
START WITH 1 
INCREMENT BY 1;
GO

--- Get Next sequence MedicalProcedure
IF EXISTS (SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetNextMedicalProcedureSeq' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetNextMedicalProcedureSeq
GO
CREATE PROCEDURE dbo.GetNextMedicalProcedureSeq	
AS
BEGIN
	select next value for MedicalProcedureSeq
END
GO

-- MedicalExam - Danh sách các kết quả khám bệnh
IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[MedicalExam]') AND type in (N'U'))
begin	
	drop table MedicalExam
end
go
Create table MedicalExam (
	[id] varchar(20) primary key,
	[name] nvarchar(500),
	[description] varchar(500) 
)
go

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
go

-- Danh sách tiền sử bệnh - Medical History
IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[MedicalHistory]') AND type in (N'U'))
begin	
	drop table MedicalHistory
end
go
Create table MedicalHistory (
	[id] varchar(20) primary key,
	[name] nvarchar(500),
	[description] varchar(50)
)
go
-- All Data Json
IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[AllDataJson]') AND type in (N'U'))
begin	
	drop table AllDataJson
end
go
Create table AllDataJson (
	[key] nvarchar(200) ,
	[value] ntext
	)
go

-- Table: Payment
-- Khoản thanh toán này có thể từ: tiền khám, chi phí điều trị, chi phí vật liệu, đơn thuốc ....
IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[Payment]') AND type in (N'U'))
begin	
	drop table Payment
end
go
Create table Payment (
	[id] varchar(200),
	[namepayment] nvarchar(500), -- Tên khoản thanh toán
	[datetimepayment] datetime,
	[idpatient] nvarchar(200),
	[idtreatment] nvarchar(200),
	[categorypayment] varchar(20), -- loại thanh toán: thanh toán nhiều lần hay thanh toán 1 lần (tương ứng điều trị nhiều lần hoặc điều trị 1 lần)
	[amount] bigint,
	[steppayment] ntext,
	[statuspayment] nvarchar(200)
	-- [steppayment] là dữ liệu dạng json với các trường như sau:
		-- [datetimesteppayment] datetime
		-- [amountsteppayment] 
	)
go

-- Insert data for AllDataJson
insert into AllDataJson([key],value) values(N'PatientStatus',N'[{"id":"0","value":"Mới"},{"id":"1","value":"Đang điều trị"},{"id":"2","value":"Đã điều trị xong"}]')
insert into AllDataJson([key],value) values(N'PatientStatusInADay',N'[{"id":"0","value":"Chờ khám"},{"id":"1","value":"Chờ điều trị"},{"id":"2","value":"Chờ thanh toán"},{"id":"3","value":"Không khám"},{"id":"4","value":"Đã hoàn thành"}]')

-- Insert data Patient

-- INSERT [dbo].[Patients]  VALUES (N'BN29072019', N'Tran Van B', 1990, N'HA NOI', NULL, 0, N'0915525326', 29, N'thuan@gmail.com', NULL,null,null, NULL, N'Chờ khám', 1)
