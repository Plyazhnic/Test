CREATE TABLE [dbo].[ParkingStallType] (
    [ParkingStallTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [ParkingStallType]   VARCHAR (256) NULL,
    [isActive]           BIT           NULL,
    [UpdatedDate]         DATETIME      NULL,
    [CreatedDate]         DATETIME      NULL
);

