﻿ALTER TABLE [UserProfile].[CreditCard]
	ADD CONSTRAINT [PK_CreditCard] PRIMARY KEY CLUSTERED ([CreditCardID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);