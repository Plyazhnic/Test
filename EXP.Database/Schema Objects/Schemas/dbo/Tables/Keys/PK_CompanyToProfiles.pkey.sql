﻿ALTER TABLE [dbo].[CompanyToProfiles]
    ADD CONSTRAINT [PK_CompanyToProfiles] PRIMARY KEY CLUSTERED ([CompanyToProfilesID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
