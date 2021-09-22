
--drop table dbo.InsiderTransactions

CREATE TABLE dbo.InsiderTransactions
   (
      Id bigint IDENTITY (1,1) NOT NULL,
	  SymbolId bigint NOT NULL,
	  TransactionDate dateTime NOT NULL,
	  HolderName nvarchar(max)  null,
	  Shares decimal(18, 4)   null,
	  [Value] decimal(18, 4)   null,
	  Side nvarchar(15)  not null,
	  [Description] nvarchar(max)  not null,
	  [Role] nvarchar(15)  not null,
  
   )




ALTER TABLE dbo.InsiderTransactions
   ADD CONSTRAINT PK_InsiderTransactions_ID PRIMARY KEY CLUSTERED (Id);


ALTER TABLE dbo.InsiderTransactions
   ADD CONSTRAINT FK_InsiderTransactions_SymbolId FOREIGN KEY (SymbolId)
      REFERENCES dbo.Symbols (Id)
      ON DELETE CASCADE
      ON UPDATE CASCADE

CREATE NONCLUSTERED INDEX [IX_InsiderTransactions_SymbolId] ON dbo.InsiderTransactions (SymbolId ASC)

