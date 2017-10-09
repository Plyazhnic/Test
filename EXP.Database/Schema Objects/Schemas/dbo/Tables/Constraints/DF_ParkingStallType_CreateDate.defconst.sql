ALTER TABLE [dbo].[ParkingStallType]
    ADD CONSTRAINT [DF_ParkingStallType_CreateDate] DEFAULT (getdate()) FOR [CreatedDate];

