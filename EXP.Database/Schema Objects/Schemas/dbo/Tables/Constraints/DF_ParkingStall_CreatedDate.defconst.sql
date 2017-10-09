ALTER TABLE [dbo].[ParkingStall]
    ADD CONSTRAINT [DF_ParkingStall_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

