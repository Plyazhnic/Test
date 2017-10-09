CREATE TABLE [dbo].[BuildingToCompanies] (
	[BuildingToCompanyID]   INT IDENTITY (1, 1) NOT NULL,
    [BuildingID]            INT NOT NULL,
    [CompanyID]             INT NOT NULL,
	[IsActive]          BIT     NOT NULL,
    [UpdatedDate]       DATETIME  NULL,
	[CreatedDate]       DATETIME  NOT NULL
);

