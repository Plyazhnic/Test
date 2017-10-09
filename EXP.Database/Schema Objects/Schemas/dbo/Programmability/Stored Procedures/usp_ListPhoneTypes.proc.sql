CREATE PROCEDURE [dbo].[usp_ListPhoneTypes]
	AS
	SELECT PhoneTypeId, PhoneType FROM dbo.PhoneType WHERE isActive=1
RETURN 0