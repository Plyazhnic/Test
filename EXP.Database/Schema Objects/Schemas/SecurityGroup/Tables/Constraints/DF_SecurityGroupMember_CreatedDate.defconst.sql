ALTER TABLE [SecurityGroup].[SecurityGroupMember]
    ADD CONSTRAINT [DF_SecurityGroupMember_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

