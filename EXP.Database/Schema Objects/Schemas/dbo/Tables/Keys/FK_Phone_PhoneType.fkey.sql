﻿ALTER TABLE [dbo].[Phone]
	ADD CONSTRAINT [FK_Phone_PhoneType] FOREIGN KEY ([PhoneTypeID]) REFERENCES [dbo].[PhoneType] ([PhoneTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
