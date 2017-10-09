ALTER TABLE [dbo].[CompanyToProfiles]
	ADD CONSTRAINT [FK_CompanyToProfiles_Company]
	FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([CompanyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
