﻿ALTER TABLE [UserProfile].[Address]
	ADD CONSTRAINT [FK_Address_zState] FOREIGN KEY ([StateID]) 
	REFERENCES [dbo].[zState] ([StateID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
