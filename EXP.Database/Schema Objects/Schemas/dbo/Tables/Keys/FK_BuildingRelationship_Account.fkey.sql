ALTER TABLE [dbo].[BuildingRelationship]
    ADD CONSTRAINT [FK_BuildingRelationship_Account] FOREIGN KEY ([BuildingAccountID]) REFERENCES [dbo].[Account] ([AccountID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

