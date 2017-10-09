CREATE TABLE [dbo].[ParkingStall] (
    [ParkingStallID]   INT            IDENTITY (1, 1) NOT NULL,
	[ParkingStallTypeID] INT          NOT NULL,
	[LotID]			   INT			  NOT NULL,
	[StallLocation]    VARCHAR (256)  NOT NULL,
    [StallNumber]      INT            NOT NULL,
	[MontlyRate]	   DECIMAL(6, 2)  NULL,
	[OverSell]         DECIMAL(5, 2)  NULL,
	[Rate]	           DECIMAL(6, 2)  NULL,
	[MaxRate]	       DECIMAL(6, 2)  NULL,
	[FlatRate]	       DECIMAL(6, 2)  NULL,
	[TimeIncrement]    INT            NULL,
	[GracePeriod]      INT            NULL,
	[StallDescription] VARCHAR (256)  NULL,
	[isActive]         BIT   NOT NULL,
	[StallName]        VARCHAR (32)   NULL,
    [LotLevel]         TINYINT        NULL,
    [StallImagePath]   VARCHAR (1024) NULL,
    [CreatedDate]      DATETIME       NULL,
    [UpdatedDate]      DATETIME       NULL
);

