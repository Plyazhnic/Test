﻿ALTER TABLE [dbo].[Lease]
	ADD CONSTRAINT [FK_Lease_Company]
	FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([CompanyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
