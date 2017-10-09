CREATE TABLE [dbo].[EntityType]
(
    [EntityTypeID] 		    INT           IDENTITY (1, 1) NOT NULL,
    [EntityType]            VARCHAR (50)  NOT NULL,
	[EntityTypeDescription] VARCHAR (256) NULL,
    [isActive]              BIT           NOT NULL,
    [UpdatedDate]           DATETIME      NULL,
    [CreatedDate]           DATETIME      NOT NULL
)
