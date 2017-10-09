CREATE TABLE [UserProfile].[Address]
(
	[AddressID]						INT IDENTITY (1, 1) NOT NULL,
	[City]						    VARCHAR (50)   NULL,
	[Address1]						VARCHAR (256)  NULL,
	[Address2]						VARCHAR (256)  NULL,
	[StateID]							INT            NULL, 
	[ZipCode]	    				VARCHAR (10)   NULL,
	[UpdatedDate]					DATETIME NULL,
	[CreatedDate]					DATETIME NOT NULL
)
