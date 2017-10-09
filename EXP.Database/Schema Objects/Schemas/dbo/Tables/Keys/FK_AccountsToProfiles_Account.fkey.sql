ALTER TABLE [dbo].[AccountsToProfiles]
    ADD CONSTRAINT [FK_AccountsToProfiles_Account] FOREIGN KEY ([AccountID]) REFERENCES [dbo].[Account] ([AccountID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

