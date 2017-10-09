ALTER TABLE [dbo].[BuildingToCompanies]
    ADD CONSTRAINT [DF_BuildingToCompanies_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

