﻿ALTER TABLE [dbo].[Company]
    ADD CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([CompanyID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

