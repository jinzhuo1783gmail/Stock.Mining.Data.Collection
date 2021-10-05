
--drop table dbo.InsiderHistories

CREATE TABLE dbo.InsiderHistories
   (
      Id bigint IDENTITY (1,1) NOT NULL,
	  SymbolId bigint NOT NULL,
	  TransactionDate dateTime NOT NULL,
	  HolderName nvarchar(max)  null,
	  Shares decimal(18, 4)   null,
	  [Value] decimal(18, 4)   null,
	  Side nvarchar(15)  not null,
	  [Description] nvarchar(max)  not null,
	  [Role] nvarchar(150)  not null,
  
   )




ALTER TABLE dbo.InsiderHistories
   ADD CONSTRAINT PK_InsiderHistories_ID PRIMARY KEY CLUSTERED (Id);


ALTER TABLE dbo.InsiderHistories
   ADD CONSTRAINT FK_InsiderHistories_SymbolId FOREIGN KEY (SymbolId)
      REFERENCES dbo.Symbols (Id)
      ON DELETE CASCADE
      ON UPDATE CASCADE

CREATE NONCLUSTERED INDEX [IX_InsiderHistories_SymbolId] ON dbo.InsiderHistories (SymbolId ASC)

