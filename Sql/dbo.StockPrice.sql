
--drop table dbo.SymbolPrice

CREATE TABLE dbo.SymbolPrice
   (
      Id bigint IDENTITY (1,1) NOT NULL,
	  SymbolId bigint NOT NULL,
	  PriceDate dateTime NOT NULL,
	  [Open] decimal(18, 4)  null,
	  [High] decimal(18, 4)   null,
	  [Low] decimal(18, 4)   null,
	  [Close] decimal(18, 4)  not null,
	  [AdjustClose] decimal(18, 4)  not null,
	  [Volume] decimal(18, 4)  not null,
	  [DividendAmount] decimal(18, 4)  null,
	  [SplitCoefficient] decimal(18, 4)  null

	  
   )






ALTER TABLE dbo.SymbolPrice
   ADD CONSTRAINT PK_SymbolPrice_ID PRIMARY KEY CLUSTERED (Id);


ALTER TABLE dbo.SymbolPrice
   ADD CONSTRAINT FK_SymbolPrice_SymbolId FOREIGN KEY (SymbolId)
      REFERENCES dbo.Symbols (Id)
      ON DELETE CASCADE
      ON UPDATE CASCADE

CREATE NONCLUSTERED INDEX [IX_SymbolPrice_SymbolId] ON dbo.SymbolPrice (SymbolId ASC)

