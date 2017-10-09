CREATE TABLE [UserProfile].[Vehicle] (
    [VehicleID]          INT          IDENTITY (1, 1) NOT NULL,
    [UserProfileID]      INT          NULL,
    [Year]               INT          NULL,
    [VehicleMakeID]      INT          NULL,
    [VehicleModelID]     INT          NULL,
    [Color]              VARCHAR (32) NULL,
    [LicensePlateNumber] VARCHAR (32) NULL,
	[PermitNumber]       VARCHAR (32) NULL,
    [isActive]           BIT          NULL,
    [CreatedDate]        DATETIME     NULL,
    [UpdatedDate]        DATETIME     NULL
);

