CREATE TABLE [dbo].[Lease]
(
	[LeaseID] INT identity(1,1) NOT NULL, 
    [CompanyID] INT NOT NULL, 
    [BuildingID] INT NOT NULL, 
    [TermStart] DATE NOT NULL, 
    [TermEnd] DATE NOT NULL, 
    [LateFee] DECIMAL(6, 2) NOT NULL, 
    [TaxExempt] BIT NOT NULL, 
    [MarketRate] BIT NOT NULL, 
    [Description] NVARCHAR(1024) NULL, 
    [DocumentName] NVARCHAR(256) NULL, 
    [UpdatedDate] DATETIME NULL, 
    [CreatedDate] DATETIME NOT NULL
)
