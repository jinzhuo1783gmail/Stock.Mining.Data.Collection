
--drop table dbo.InstitutionHoldings

CREATE TABLE dbo.InstitutionHoldings
   (
      Id bigint IDENTITY (1,1) NOT NULL,
	  SymbolId bigint NOT NULL,
	  InstitutionName nvarchar(255) NOT NULL,
	  MatchWord nvarchar(100) NOT NULL,
	  ReportDate datetime NOT NULL,
	  Postion decimal not null,
	  PrevPostion decimal not null,
	  [Value] decimal not null,
	  [Percentage] decimal null

	  
   )






ALTER TABLE dbo.InstitutionHoldings
   ADD CONSTRAINT PK_InstitutionHoldings_ID PRIMARY KEY CLUSTERED (Id);


ALTER TABLE dbo.InstitutionHoldings
   ADD CONSTRAINT FK_InstitutionHoldings_SymbolId FOREIGN KEY (SymbolId)
      REFERENCES dbo.Symbols (Id)
      ON DELETE CASCADE
      ON UPDATE CASCADE

CREATE NONCLUSTERED INDEX [IX_InstitutionHoldings_SymbolId] ON dbo.InstitutionHoldings (SymbolId ASC)
CREATE NONCLUSTERED INDEX [IX_InstitutionHoldings_InstitutionName] ON dbo.InstitutionHoldings (InstitutionName)


INSERT INTO [dbo].[InstitutionHoldings]
           ([SymbolId]
           ,[InstitutionName]
           ,[MatchWord]
           ,[ReportDate]
           ,[Postion]
           ,[PrevPostion]
           ,[Value]
           ,[Percentage])
     VALUES

(1, 'VGTSX - Vanguard Total International Stock Index Fund Investor Shares', '', '2021-06-29T00:00:00','16773274',0,'11717000','0'),
(1, 'VFSNX - Vanguard FTSE All-World ex-US Small-Cap Index Fund Institutional Shares','', '2021-06-29T00:00:00','4435822',0,'3099000','0'),
(1, 'SCHC - Schwab International Small-Cap Equity ETF','', '2021-07-27T00:00:00','1939946',0,'1241000','0'),
(1, 'VPACX - Vanguard Pacific Stock Index Fund Investor Shares','', '2021-06-29T00:00:00','1354696',0,'946000','0'),
(1, 'VT - Vanguard Total World Stock Index Fund ETF Shares','', '2021-06-29T00:00:00','724058',0,'506000','0'),
(1, 'Alyeska Investment Group, L.P.','', '2021-02-16T00:00:00','600000',0,'15930000','0'),
(1, 'Invesco Ltd.','', '2021-05-17T00:00:00','543905',0,'37796000','0'),
(1, 'PBW - Invesco WilderHill Clean Energy ETF','', '2021-06-29T00:00:00','486388',0,'34388000','0'),
(1, 'Principal Financial Group Inc','', '2021-05-10T00:00:00','167475',0,'11638000','0'),
(1, 'Brighthouse Funds Trust II - VanEck Global Natural Resources Portfolio Class A','', '2021-05-26T00:00:00','127100',0,'8832000','0'),
(1, 'Graham Capital Management, L.P.','', '2021-05-17T00:00:00','120481',0,'8372000','0'),
(1, 'Morgan Stanley','', '2021-05-17T00:00:00','98031',0,'6812000','0'),
(1, 'DZ BANK AG Deutsche Zentral Genossenschafts Bank, Frankfurt am Main','', '2021-05-17T00:00:00','78600',0,'5462000','0'),
(1, 'GHAAX - Global Hard Assets Fund Class A','', '2021-05-28T00:00:00','73700',0,'5121000','0'),
(1, 'Citadel Advisors Llc','', '2021-05-21T00:00:00','50165',0,'3486000','0'),
(1, 'PBD - Invesco Global Clean Energy ETF','', '2021-06-29T00:00:00','46856',0,'3313000','0'),
(1, 'PSABX - SmallCap Fund fka SmallCap Blend Fund R-1','', '2021-06-25T00:00:00','46200',0,'3266000','0'),
(1, 'AVDE - Avantis International Equity ETF','', '2021-07-26T00:00:00','45091',0,'29000','0'),
(1, 'Pinz Capital Management, LP','', '2021-05-17T00:00:00','39913',0,'2774000','0'),
(1, 'VanEck VIP Trust - VanEck VIP Global Hard Assets Fund Initial Class','', '2021-05-28T00:00:00','38300',0,'2661000','0'),
(1, 'Millennium Management Llc','', '2021-05-17T00:00:00','32451',0,'2255000','0'),
(1, 'VTMGX - Vanguard Developed Markets Index Fund Admiral Shares','', '2021-06-01T00:00:00','26128',0,'1816000','0'),
(1, 'Williams Jones Wealth Management, LLC.','', '2021-05-17T00:00:00','23915',0,'1662000','0'),
(1, 'PSPFX - Global Resources Fund','', '2021-05-28T00:00:00','20000',0,'1390000','0'),
(1, 'Jane Street Group, Llc','', '2021-05-18T00:00:00','15756',0,'1095000','0'),
(1, 'Royal Bank Of Canada','', '2021-07-20T00:00:00','15445',0,'1073000','0'),
(1, 'BATT - Amplify Advanced Battery Metals and Materials ETF','2021-06-29T00:00:00','', '14431',0,'1020000','0'),
(1, 'Maven Securities LTD','', '2021-05-17T00:00:00','10000',0,'677000','0'),
(1, 'PRINCIPAL VARIABLE CONTRACTS FUNDS INC - SmallCap Account Class 1','', '2021-05-24T00:00:00','9100',0,'632000','0'),
(1, 'JOHN HANCOCK INVESTMENT TRUST - John Hancock Diversified Real Assets Fund Class NAV','', '2021-05-28T00:00:00','7000',0,'486000','0'),
(1, 'Fiduciary Trust Co','', '2021-05-19T00:00:00','6650',0,'462000','0'),
(1, 'Davidson Kempner Capital Management Lp','', '2021-05-17T00:00:00','5000',0,'348000','0'),
(1, 'Caas Capital Management Lp','', '2021-05-17T00:00:00','4764',0,'331000','0'),
(1, 'Ameriprise Financial Inc','', '2021-05-17T00:00:00','3107',0,'216000','0'),
(1, 'Wells Fargo &amp; Company/mn','', '2021-05-13T00:00:00','2760',0,'192000','0'),
(1, 'Tower Research Capital LLC (TRC)','', '2021-05-17T00:00:00','1392',0,'97000','0'),
(1, 'FNCMX - Fidelity Nasdaq Composite Index Fund','', '2021-04-23T00:00:00','1000',0,'67000','0'),
(1, 'Bank Of America Corp /de/','', '2021-05-14T00:00:00','560',0,'39000','0'),
(1, 'Captrust Financial Advisors','', '2021-05-17T00:00:00','418',0,'29000','0'),
(1, 'Ameritas Investment Corp','', '2021-05-18T00:00:00','305',0,'21000','0'),
(1, 'Wealthcare Advisory Partners LLC','', '2021-04-30T00:00:00','260',0,'18000','0'),
(1, 'BDO Wealth Advisors, LLC','', '2021-05-14T00:00:00','65',0,'5000','0'),
(1, 'FinTrust Capital Advisors, LLC','', '2021-05-17T00:00:00','33',0,'2000','0'),
(1, 'Marshall Wace, Llp', 'Marshall Wace', '2021-08-13T00:00:00','0','224000','0','0')

