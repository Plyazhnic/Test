﻿ALTER TABLE [dbo].[Company]
	ADD CONSTRAINT [FK_Company_Fax]
	FOREIGN KEY ([FaxID]) REFERENCES [dbo].[Phone] ([PhoneID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
