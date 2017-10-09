CREATE TABLE [UserProfile].[VehicleMake] (
    [VehicleMakeID]          INT            IDENTITY (1, 1) NOT NULL,
    [VehicleMake]            VARCHAR (256)  NULL,
    [VehicleMakeDescription] VARCHAR (1024) NULL,
    [isActive]               BIT            NULL,
    [UpdatedDate]            DATETIME       NULL,
    [CreatedDate]            DATETIME       NULL
);

