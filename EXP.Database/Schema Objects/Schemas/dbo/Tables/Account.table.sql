CREATE TABLE [dbo].[Account] (
    [AccountID]     INT        IDENTITY (1, 1) NOT NULL,
    [AccountTypeID] INT        NOT NULL,
    [AccountName]   NCHAR (10) NULL,
    [Status]        NCHAR (10) NULL,
    [UpdatedBy]     NCHAR (10) NULL,
    [UpdatedDate]   NCHAR (10) NULL,
    [CreatedDate]   NCHAR (10) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Account', @level2type = N'COLUMN', @level2name = N'AccountID';

