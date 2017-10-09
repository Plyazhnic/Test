ALTER TABLE [dbo].[Company]
	ADD CONSTRAINT [FK_Company_Manager]
	FOREIGN KEY ([ManagerID]) REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
