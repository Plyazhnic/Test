ALTER TABLE [UserProfile].[UserProfileToCompany]
	ADD CONSTRAINT [FK_UserProfileToCompany_Company]
	FOREIGN KEY ([CompanyID]) 
	REFERENCES [dbo].[Company] ([CompanyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
