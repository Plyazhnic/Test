ALTER TABLE [dbo].[Lot]
    ADD CONSTRAINT [DF_Lot_CreatedDate] DEFAULT (getdate()) FOR [EffectiveFrom];

