ALTER TABLE [dbo].[BuildingToCompanies]
    ADD CONSTRAINT [FK_BuildingToCompanies_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([CompanyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

