CREATE TABLE [dbo].[BuildingToLots] (
	[BuildingToLotID]   INT IDENTITY (1, 1) NOT NULL,
    [BuildingID]        INT NOT NULL,
    [LotID]             INT NOT NULL,
	[IsActive]          BIT     NOT NULL,
    [UpdatedDate]       DATETIME  NULL,
	[CreatedDate]       DATETIME  NOT NULL
);

