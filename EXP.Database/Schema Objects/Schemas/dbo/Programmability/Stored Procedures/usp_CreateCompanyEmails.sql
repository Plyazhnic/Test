CREATE PROCEDURE [dbo].[usp_CreateCompanyEmails]
	@String_data varchar(5000),
	@CompanyID int,
	@len int,
	@count int

AS
	DECLARE @email varchar(50)
	DECLARE @i int
	DECLARE @j int
	DECLARE @number int
	SELECT @i = 0
	SELECT @number = 0
	WHILE(@i < @count)
		BEGIN
			SELECT @email = substring(@String_data,@len*@number+1,@len)
			SET @number = @number + 1
			INSERT INTO dbo.[CompanyEmails]([CompanyID],[Email],[Send],[Active],[CreatedDate]) 
			VALUES (@CompanyID,RTRIM(@email),1,1,GETDATE());
			SET @i = @i + 1
		END


