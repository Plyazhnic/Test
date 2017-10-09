CREATE PROCEDURE [dbo].[usp_CreatePhoneForUser]
	@UserProfileID int,
	@PhoneTypeID int, 
	@AreaCode varchar(8), 
	@PhoneNumber varchar(15), 
	@PhoneDescription varchar(128),
	@NewPhoneID int out
	
AS
	INSERT INTO [dbo].[Phone]
           ([PhoneTypeID]
		   ,[AreaCode]
           ,[PhoneNumber]
           ,[PhoneDescription]
           ,[UpdatedDate]
           ,[CreatedDate])
     VALUES
           (@PhoneTypeID, 
			@AreaCode, 
			@PhoneNumber, 
			@PhoneDescription,
            GETDATE(),
            GETDATE());
			
			SET @NewPhoneID = Scope_Identity();
	
	INSERT INTO [UserProfile].[PhonesToProfiles]
           ([PhoneID]
           ,[UserProfileID]
           ,[isActive]
           ,[UpdatedDate]
           ,[CreatedDate])
     VALUES
           (@NewPhoneID
           ,@UserProfileID
           ,1
           ,GETDATE()
           ,GETDATE())

RETURN 0