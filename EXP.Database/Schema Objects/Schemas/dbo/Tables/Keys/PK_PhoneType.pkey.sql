﻿ALTER TABLE [dbo].[PhoneType]
	ADD CONSTRAINT [PK_PhoneType] PRIMARY KEY CLUSTERED ([PhoneTypeID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);