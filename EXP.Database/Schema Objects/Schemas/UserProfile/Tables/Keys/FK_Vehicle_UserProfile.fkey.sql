ALTER TABLE [UserProfile].[Vehicle]
    ADD CONSTRAINT [FK_Vehicle_UserProfile] FOREIGN KEY ([UserProfileID]) REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

