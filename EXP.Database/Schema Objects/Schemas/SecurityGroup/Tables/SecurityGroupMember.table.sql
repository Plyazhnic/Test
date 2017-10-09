CREATE TABLE [SecurityGroup].[SecurityGroupMember] (
    [SecurityGroupID]    INT      NOT NULL,
    [SecurityRoleID]     INT      NOT NULL,
    [AccountToProfileID] INT      NOT NULL,
    [CreatedDate]        DATETIME NOT NULL
);

