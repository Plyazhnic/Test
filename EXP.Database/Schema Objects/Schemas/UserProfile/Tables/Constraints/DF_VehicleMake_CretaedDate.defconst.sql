ALTER TABLE [UserProfile].[VehicleMake]
    ADD CONSTRAINT [DF_VehicleMake_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

