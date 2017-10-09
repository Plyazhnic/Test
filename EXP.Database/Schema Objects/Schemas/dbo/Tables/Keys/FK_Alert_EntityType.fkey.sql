ALTER TABLE [dbo].[Alert]
    ADD CONSTRAINT [FK_Alert_EntityType] FOREIGN KEY ([EntityTypeID]) REFERENCES [dbo].[EntityType] ([EntityTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
