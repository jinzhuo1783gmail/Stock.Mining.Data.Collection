

--drop table dbo.SymbolUpdateHistories

CREATE TABLE dbo.SymbolUpdateHistories
   (
      Id bigint IDENTITY (1,1) NOT NULL,
	  SymbolId bigint NOT NULL,
	  Ticker varchar(10) NOT NULL,
	  UpdateTime datetime NOT NULL,
	  IsSuccess bit NOT NULL,
	  FailReason nvarchar(max) NULL,
	  NextUpdateTime datetime NOT NULL,
   )

ALTER TABLE dbo.SymbolUpdateHistories
   ADD CONSTRAINT PK_SymbolUpdateHistories_ID PRIMARY KEY CLUSTERED (Id);

CREATE NONCLUSTERED INDEX [IX_SymbolUpdateHistories_Ticker] ON dbo.SymbolUpdateHistories (Ticker)

ALTER TABLE dbo.SymbolUpdateHistories
   ADD CONSTRAINT FK_SymbolUpdateHistories_SymbolId FOREIGN KEY (SymbolId)
      REFERENCES dbo.Symbols (Id)
      ON DELETE CASCADE
      ON UPDATE CASCADE
