﻿ALTER TABLE [UserProfile].[OnlineCheck]
	ADD CONSTRAINT [PK_OnlineCheck] PRIMARY KEY CLUSTERED ([OnlineCheckID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);