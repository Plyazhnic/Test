CREATE TABLE [dbo].[AccountsToProfiles] (
    [AccountToProfileID] INT      IDENTITY (1, 1) NOT NULL,
    [AccountID]          INT      NOT NULL,
    [UserProfileID]      INT      NOT NULL,
    [isActive]           BIT      NOT NULL,
    [UpdatedDate]        DATETIME NULL,
    [CreatedDate]        DATETIME NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'should be part of PK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AccountsToProfiles', @level2type = N'COLUMN', @level2name = N'AccountID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'should be part of PK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AccountsToProfiles', @level2type = N'COLUMN', @level2name = N'UserProfileID';

