﻿ALTER TABLE [dbo].[BuildingToCompanies]
    ADD CONSTRAINT [PK_BuildingToCompany] PRIMARY KEY CLUSTERED ([BuildingToCompanyID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);