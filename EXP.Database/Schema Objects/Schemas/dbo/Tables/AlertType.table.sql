CREATE TABLE [dbo].[AlertType] (
    [AlertTypeID] 		   INT           IDENTITY (1, 1) NOT NULL,
    [AlertType]            VARCHAR (50)  NOT NULL,
	[AlertPeriodValue]     INT		     NULL,
	[AlertTypeDescription] VARCHAR (256) NOT NULL,
    [isActive]             BIT           NOT NULL,
    [UpdatedDate]           DATETIME      NULL,
    [CreatedDate]           DATETIME      NOT NULL
)