CREATE PROCEDURE [UserProfile].[usp_GetEmployeesForPage]
	@Start int ,
	@End int,
	@Search varchar(50),
	@CompanyID int
AS

SELECT @Search = '%' + RTRIM(@Search) + '%';

WITH EmployeePage AS
( 
SELECT   [UserProfile].[UserProfile].FirstName, [UserProfile].[UserProfile].LastName, [UserProfile].[UserProfile].UserProfileID,
ROW_NUMBER() OVER (ORDER BY [UserProfile].[UserProfile].UserProfileID ) AS RowNumber
FROM [dbo].[CompanyToProfiles]
     INNER JOIN [UserProfile].[UserProfile] ON  dbo.CompanyToProfiles.UserProfileID = [UserProfile].[UserProfile].UserProfileID
	 
WHERE dbo.CompanyToProfiles.CompanyID = @CompanyID    AND [UserProfile].[UserProfile].UserProfileTypeID = 3 AND [UserProfile].[UserProfile].FirstName LIKE @Search 
)
SELECT *
	FROM EmployeePage
	WHERE RowNumber > @Start AND RowNumber <= @End
	ORDER BY UserProfileID
RETURN 0