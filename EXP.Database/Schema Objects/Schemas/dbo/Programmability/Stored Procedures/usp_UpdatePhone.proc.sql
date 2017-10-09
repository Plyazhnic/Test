CREATE PROCEDURE [dbo].[usp_UpdatePhone]
	@PhoneID int,
	@PhoneTypeID int, 
	@AreaCode varchar(8), 
	@PhoneNumber varchar(15), 
	@PhoneDescription varchar(128) 
	
AS
	UPDATE [dbo].[Phone]
        SET PhoneTypeID=@PhoneTypeID,
		    AreaCode=@AreaCode,
            PhoneNumber=@PhoneNumber,
            PhoneDescription=@PhoneDescription,
            UpdatedDate=GETDATE()
			WHERE PhoneID=@PhoneID
RETURN 0