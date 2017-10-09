CREATE TABLE [dbo].[Alert] (
    [AlertID]              INT            IDENTITY (1, 1) NOT NULL,
    [AlertTypeID]          INT            NOT NULL,
    [UserProfileID]        INT            NOT NULL,
    [EntityTypeID]         INT            NOT NULL,
    [EntityID]             INT            NOT NULL,
    [isActive]             BIT            NOT NULL,
    [UpdatedDate]          DATETIME       NULL,
    [CreatedDate]          DATETIME       NOT NULL
);