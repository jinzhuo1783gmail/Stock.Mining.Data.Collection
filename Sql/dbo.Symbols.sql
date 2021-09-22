

--drop table dbo.Symbols

CREATE TABLE dbo.Symbols
   (
      Id bigint IDENTITY (1,1) NOT NULL,
	  Ticker varchar(10) NOT NULL,
	  CompanyName varchar(max) NOT NULL,
	  TotalIssuedShares int null,
	  ScanEnable bit NOT NULL,
	  Initialized bit NOT NUll
   )

ALTER TABLE dbo.Symbols
   ADD CONSTRAINT PK_Symbols_ID PRIMARY KEY CLUSTERED (Id);

CREATE NONCLUSTERED INDEX [IX_Symbols_Ticker] ON dbo.Symbols (Ticker)


INSERT INTO [dbo].[Symbols]
           
     VALUES
           ('PLL' , 'Piedmont Lithium', 1 ,1, 0),
		   ('LAC' , 'Lithium American Corp', 1, 1, 0)
GO