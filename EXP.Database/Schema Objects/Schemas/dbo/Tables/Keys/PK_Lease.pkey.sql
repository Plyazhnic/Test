﻿ALTER TABLE [dbo].[Lease]
	ADD CONSTRAINT [PK_Lease] PRIMARY KEY CLUSTERED ([LeaseID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);