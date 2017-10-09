ALTER TABLE [dbo].[Building]
	ADD CONSTRAINT [FK_BuildingManager_UserProfile]
	FOREIGN KEY ([ManagerID]) REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
