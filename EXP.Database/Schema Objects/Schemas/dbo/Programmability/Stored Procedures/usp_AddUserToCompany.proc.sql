CREATE PROCEDURE [dbo].[usp_AddUserToCompany]
	@CompanyID int,
	@UserProfileID int
AS
	INSERT INTO [dbo].[CompanyToProfiles]
           ([CompanyID]
           ,[UserProfileID])
     VALUES
           (@CompanyID
           ,@UserProfileID)
RETURN 0
