CREATE TABLE [dbo].[AccountType] (
    [AccountTypeID]          INT            IDENTITY (1, 1) NOT NULL,
    [AccountType]            VARCHAR (256)  NOT NULL,
    [AccountTypeDescription] VARCHAR (1024) NOT NULL,
    [isActive]               BIT            NOT NULL,
    [UpdatedDate]            DATETIME       NULL,
    [CreatedDate]            DATETIME       NOT NULL
);

