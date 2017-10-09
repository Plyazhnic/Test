CREATE TABLE [dbo].[Company] (
    [CompanyID]        INT            IDENTITY (1, 1) NOT NULL,
    [CompanyName]     VARCHAR (1024) NULL,
    [AddressID]       INT            NULL,
	[asBuilding]      BIT            NOT NULL,
	[ManagerID]       INT            NULL,
	[EmployeeID]      INT            NULL,
	[WorkPhoneID]       INT   NULL,
	[FaxID]             INT   NULL,
    [Suite]           VARCHAR (16)   NULL,  
	[EmailAddress]	  VARCHAR (128)  NULL,
    [hasPermitEmail]  BIT            NULL,
    [CreatedDate]     DATETIME       NULL,
    [UpdatedDateTime] DATETIME       NULL
);

