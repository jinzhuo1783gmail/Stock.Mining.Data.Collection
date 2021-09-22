--drop table dbo.InstitutionHoldingsHistory

CREATE TABLE dbo.InstitutionHoldingsHistory
   (
      Id bigint IDENTITY (1,1) NOT NULL,
	  InstitutionId bigint NOT NULL,
	  FileDate datetime NOT NULL,
	  Postion decimal not null,
	  [Value] decimal not null,
	  [Action] varchar(20) not null
   )




ALTER TABLE dbo.InstitutionHoldingsHistory
   ADD CONSTRAINT PK_InstitutionHoldingsHistory_ID PRIMARY KEY CLUSTERED (Id);


ALTER TABLE dbo.InstitutionHoldingsHistory
   ADD CONSTRAINT FK_InstitutionHoldingsHistory_InstitutionId FOREIGN KEY (InstitutionId)
      REFERENCES dbo.InstitutionHoldings (Id)
      ON DELETE CASCADE
      ON UPDATE CASCADE

CREATE NONCLUSTERED INDEX [IX_InstitutionHoldingsHistory_InstitutionId] ON dbo.InstitutionHoldingsHistory (InstitutionId ASC)


INSERT INTO [dbo].[InstitutionHoldingsHistory]
           ([InstitutionId]
           ,[FileDate]
           ,[Postion]
           ,[Value]
           ,[Action])
     VALUES
           (44, '2021-08-13', 0, 0, 'Liquidated')

