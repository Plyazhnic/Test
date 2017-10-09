CREATE TABLE [dbo].[PhoneType]
(
	[PhoneTypeID]          INT           IDENTITY (1, 1) NOT NULL,
    [PhoneType]            VARCHAR (32)  NOT NULL,
    [PhoneTypeDescription] VARCHAR (128) NULL,
    [isActive]             BIT           NOT NULL,
    [UpdatedDate]          DATETIME      NULL,
    [CreatedDate]          DATETIME      NOT NULL
)
