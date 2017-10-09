CREATE TABLE [dbo].[ParkingInventory] (
    [ParkingInventoryID] INT          IDENTITY (1, 1) NOT NULL,
    [BuildingID]         INT          NULL,
    [CompanyID]          INT          NULL,
    [LotID]              INT          NULL,
    [ParkingStallID]     INT          NULL,
	[NumberOfStalls]     INT          NULL,
	[MontlyRate]	     DECIMAL(6, 2)NOT NULL,
	[MarketRate]         BIT NOT NULL,
	[DiscountRate]	     DECIMAL(5,2) NOT NULL, 
	[OverSell]           DECIMAL(5, 2)NOT NULL,
    [UserProfileID]      INT          NULL,
    [UserVehicleID]      INT          NULL,
    [PermitNumber]       VARCHAR (32) NULL,
    [isActive]           BIT          NULL,
    [EffectiveFrom]      DATETIME     NULL,
    [EffectiveTo]        DATETIME     NULL,
	[UpdatedDate]        DATETIME     NULL,
	[CreatedDate]        DATETIME     NOT NULL
);

