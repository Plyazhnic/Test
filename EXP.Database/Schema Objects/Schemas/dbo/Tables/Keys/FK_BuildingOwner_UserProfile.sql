ALTER TABLE [dbo].[Building]
	ADD CONSTRAINT [FK_BuildingOwner_UserProfile]
	FOREIGN KEY ([OwnerID]) REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
