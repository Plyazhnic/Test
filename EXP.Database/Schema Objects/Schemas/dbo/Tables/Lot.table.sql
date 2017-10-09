CREATE TABLE [dbo].[Lot] (
    [LotID]                INT        IDENTITY (1, 1) NOT NULL,
    [LotName]              NVARCHAR(50) NULL,
	[ParkingOperatorID]    INT        NULL,
	[ParkingManagerID]     INT        NULL,
    [LotNumber]            NVARCHAR(10) NULL,
    [LotLevel]             NVARCHAR(10) NULL,
	[AddressID]            INT        NULL,
	[IsBuilding]           BIT        NOT NULL,
	[MailingAddressID]     INT        NULL,
	[ReservedPrice]        INT        NULL, 
	[UnReservedPrice]      INT        NULL,
	[PhoneNumber]      VARCHAR(20)    NULL,
	[FaxNumber]        VARCHAR(20)    NULL,
	[MailingPhoneNumber] VARCHAR(20)    NULL,
	[MailingFaxNumber] VARCHAR(20)    NULL, 
	[IsActive]             BIT  NOT NULL,
	[Comments]         VARCHAR(500)    NULL, 
    [hasMultipleBuildings] NVARCHAR(10) NULL,
    [EffectiveFrom]        DATETIME   NULL,
    [EffectiveTo]          DATETIME   NULL
);

