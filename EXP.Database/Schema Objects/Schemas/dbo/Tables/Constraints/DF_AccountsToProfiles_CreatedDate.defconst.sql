ALTER TABLE [dbo].[AccountsToProfiles]
    ADD CONSTRAINT [DF_AccountsToProfiles_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

