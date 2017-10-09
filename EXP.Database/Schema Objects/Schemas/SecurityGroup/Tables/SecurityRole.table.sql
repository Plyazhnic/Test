CREATE TABLE [SecurityGroup].[SecurityRole] (
    [SecurityRoleID] INT          IDENTITY (1, 1) NOT NULL,
    [SecurityRole]   VARCHAR (32) NULL,
    [CreatedDate]    DATETIME     NULL
);

