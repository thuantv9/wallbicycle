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

IF EXISTS(SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAllEvents' AND ROUTINE_SCHEMA = 'dbo')
		DROP PROCEDURE dbo.GetAllEvents
go
CREATE PROCEDURE dbo.GetAllEvents
as
begin
 select * from Events
end
