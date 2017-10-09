CREATE TABLE [UserProfile].[VehicleModel] (
    [VehicleModelID]          INT            IDENTITY (1, 1) NOT NULL,
    [VehicleMakeID]           INT            NOT NULL,
	[VehicleModel]            VARCHAR (256)  NULL,
	[Year1]                    INT            NULL,
    [VehicleModelDescription] VARCHAR (1024) NULL,
    [isActive]                BIT            NULL,
    [UpdatedDate]             DATETIME       NULL,
    [CreatedDate]             DATETIME       NULL
);

