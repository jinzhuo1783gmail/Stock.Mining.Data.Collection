

--drop table dbo.RestSharpAccessKeys

CREATE TABLE dbo.RestSharpAccessKeys
   (
      
      Id int IDENTITY (1,1) NOT NULL,
	  AccessKey varchar(50) NOT NULL,
	  Account varchar(50) NOT NULL,
	  MonthlyCount int NOT NULL,
	  NextRefreshMonth int NOT NULL
   )

ALTER TABLE dbo.RestSharpAccessKeys
   ADD CONSTRAINT PK_RestSharpAccessKeys_ID PRIMARY KEY CLUSTERED (Id);

INSERT INTO [dbo].[RestSharpAccessKeys]
           ([AccessKey]
		   ,[Account]
           ,[MonthlyCount]
		   ,[NextRefreshMonth])
     VALUES
           ('723f4c9fdamsh57710b7681839b0p19b820jsn5def7be0f91a', 'jinzhuo1783@gmail.com', 25, 9),
		   ('5a62c931aamsh98b980fbe229663p1f5b2djsneb8e4b7ca9ef', 'jinzhuo1784@gmail.com',3, 9),
		   ('fed9dc9434mshd2c7402fcc8829cp145d7fjsn991160ebdc92', 'jinzhuo1783@hotmail.com', 4, 9),
		   ('5ccdfec5acmshd2cc817c258c10cp1ddb74jsn09b688f4a0ce', 'jinzhuo1784@hotmail.com', 0, 9)

go