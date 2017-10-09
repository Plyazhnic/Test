ALTER TABLE [UserProfile].[VehicleModel]
    ADD CONSTRAINT [DF_VehicleModel_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

