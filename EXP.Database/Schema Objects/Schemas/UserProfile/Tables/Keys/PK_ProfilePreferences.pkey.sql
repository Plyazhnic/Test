﻿ALTER TABLE [UserProfile].[UserProfilePreferences]
    ADD CONSTRAINT [PK_ProfilePreferences] PRIMARY KEY CLUSTERED ([ProfilePreferencesID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

