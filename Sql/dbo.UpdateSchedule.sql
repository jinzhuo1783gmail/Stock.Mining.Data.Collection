

--drop table dbo.UpdateSchedule

CREATE TABLE dbo.UpdateSchedule
   (
      Id bigint IDENTITY (1,1) NOT NULL,
	  UpdateTime varchar(10) NOT NULL,
   )

ALTER TABLE dbo.UpdateSchedule
   ADD CONSTRAINT PK_UpdateSchedule_ID PRIMARY KEY CLUSTERED (Id);

INSERT INTO [dbo].[UpdateSchedule]
           
     VALUES
           ('7:45:00')