CREATE TABLE [dbo].[Phone]
(
	[PhoneID]          INT            IDENTITY (1, 1) NOT NULL,
	[PhoneTypeID]                 INT            NOT NULL,
	[AreaCode]                    VARCHAR (8)    NULL,
    [PhoneNumber]                 VARCHAR (15)   NOT NULL,
    [PhoneDescription]            VARCHAR (128)  NULL,
    [UpdatedDate]                  DATETIME       NULL,
	[CreatedDate]                 DATETIME       NULL
)
