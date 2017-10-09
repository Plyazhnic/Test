CREATE TABLE [dbo].[Tenant] (
    [TenantID]        INT            IDENTITY (1, 1) NOT NULL,
    [CompanyName]     VARCHAR (1024) NULL,
    [StreetAddress]   VARCHAR (1024) NULL,
    [Suite]           VARCHAR (16)   NULL,
    [City]            VARCHAR (256)  NULL,
    [StateCode]       CHAR (2)       NULL,
    [PostalCode]      VARCHAR (16)   NULL,
    [hasPermitEmail]  BIT            NULL,
    [CreatedDate]     DATETIME       NULL,
    [UpdatedDateTime] DATETIME       NULL
);

