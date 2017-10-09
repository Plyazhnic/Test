CREATE TABLE [SecurityGroup].[SecurityRolePermission] (
    [SecurityRolePermissionID] INT          IDENTITY (1, 1) NOT NULL,
    [SecurityRoleID]           INT          NOT NULL,
    [RolePrivelage]            VARCHAR (32) NOT NULL,
    [CreatedDate]              DATETIME     NULL
);

