ALTER TABLE [dbo].[AccountsToProfiles]
    ADD CONSTRAINT [FK_AccountsToProfiles_UserProfile] FOREIGN KEY ([UserProfileID]) REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

