CREATE TABLE [dbo].[AlertEntityType]
(
    [AlertEntityTypeID]     INT           IDENTITY (1, 1) NOT NULL,
    [AlertID]               INT  		  NOT NULL,
	[EntityTypeID]          INT  		  NOT NULL,
	[EntityID]              INT  		  NOT NULL,
    [isActive]              BIT           NOT NULL,
    [UpdatedDate]           DATETIME      NULL,
    [CreatedDate]           DATETIME      NOT NULL
);
