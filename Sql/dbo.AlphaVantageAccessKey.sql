

--drop table dbo.AlphaVantageAccessKeys

CREATE TABLE dbo.AlphaVantageAccessKeys
   (
      
      Id int IDENTITY (1,1) NOT NULL,
	  AccessKey varchar(50) NOT NULL,
	  Account varchar(50) NOT NULL,
   )

ALTER TABLE dbo.AlphaVantageAccessKeys
   ADD CONSTRAINT PK_AlphaVantageAccessKeys_ID PRIMARY KEY CLUSTERED (Id);

INSERT INTO [dbo].[AlphaVantageAccessKeys]
           ([AccessKey]
		   ,[Account])
     VALUES
           ('J4K13749Z87THSTB', 'jinzhuo1783@gmail.com')
		   

go