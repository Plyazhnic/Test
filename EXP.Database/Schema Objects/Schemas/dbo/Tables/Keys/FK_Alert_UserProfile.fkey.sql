ALTER TABLE [dbo].[Alert]
    ADD CONSTRAINT [FK_ALert_UserProfile] FOREIGN KEY ([UserProfileID]) REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

