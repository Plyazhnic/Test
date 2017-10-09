ALTER TABLE [dbo].[Company]
	ADD CONSTRAINT [FK_Company_Employee]
	FOREIGN KEY ([EmployeeID]) REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
