CREATE PROCEDURE [dbo].[usp_GetPhone]
	@PhoneID int
AS	
  SELECT TOP 1 Phone.*, PhoneType.PhoneType 
	FROM [dbo].[Phone] 
    INNER JOIN dbo.PhoneType ON dbo.Phone.PhoneTypeID = dbo.PhoneType.PhoneTypeID
    WHERE  PhoneID = @PhoneID
RETURN 0