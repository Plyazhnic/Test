ALTER TABLE [dbo].[BuildingToLots]
    ADD CONSTRAINT [FK_BuildingToLots_Building] FOREIGN KEY ([BuildingID]) REFERENCES [dbo].[Building] ([BuildingID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

