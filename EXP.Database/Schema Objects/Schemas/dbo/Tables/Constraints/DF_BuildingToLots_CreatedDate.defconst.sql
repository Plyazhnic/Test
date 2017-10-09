ALTER TABLE [dbo].[BuildingToLots]
    ADD CONSTRAINT [DF_BuildingToLots_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

