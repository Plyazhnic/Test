﻿ALTER TABLE [dbo].[Alert]
    ADD CONSTRAINT [PK_Alert] PRIMARY KEY CLUSTERED ([AlertID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

