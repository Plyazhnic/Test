CREATE PROCEDURE [UserProfile].[usp_GetEmployeesCount]
	@Search varchar(50),
	@CompanyID int
AS

SELECT @Search = '%' + RTRIM(@Search) + '%';

SELECT  COUNT(*) 
FROM [dbo].[CompanyToProfiles]
     INNER JOIN [UserProfile].[UserProfile] ON  dbo.CompanyToProfiles.UserProfileID = [UserProfile].[UserProfile].UserProfileID
WHERE dbo.CompanyToProfiles.CompanyID = @CompanyID    AND [UserProfile].[UserProfile].UserProfileTypeID = 3 AND [UserProfile].[UserProfile].FirstName LIKE @Search 

RETURN 0