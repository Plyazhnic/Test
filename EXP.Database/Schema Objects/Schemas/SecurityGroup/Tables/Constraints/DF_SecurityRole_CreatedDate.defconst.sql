ALTER TABLE [SecurityGroup].[SecurityRole]
    ADD CONSTRAINT [DF_SecurityRole_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

