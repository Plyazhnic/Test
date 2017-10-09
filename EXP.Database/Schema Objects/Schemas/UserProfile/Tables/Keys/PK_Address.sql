﻿ALTER TABLE [UserProfile].[Address]
	ADD CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([AddressID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
