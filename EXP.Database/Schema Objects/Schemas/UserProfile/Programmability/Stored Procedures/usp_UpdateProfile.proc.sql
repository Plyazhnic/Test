CREATE PROCEDURE [UserProfile].[usp_UpdateProfile]
    @UserName nchar(10),
    @ProfileTypeId int,
    @Email varchar(256),
    @FirstName varchar(256),
    @LastName varchar(256),
	@StatusID int,
	@AddressID int,
	@Comments varchar(1024),
	@tPrefix varchar(4),
	@SessionID varchar(32),
	@TenantID int,
	@isSupervisor bit,
	@HireDate date,
	@OperatorID int
AS
    IF @ProfileTypeId IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET UserProfileTypeID = @ProfileTypeId
        WHERE dbo.TRIM(LoginName) = @UserName
    END

    IF @Email IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET EmailAddress = @Email
        WHERE dbo.TRIM(LoginName) = @UserName
    END

    IF @FirstName IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET FirstName = @FirstName
        WHERE dbo.TRIM(LoginName) = @UserName
    END

    IF @LastName IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET LastName = @LastName
        WHERE dbo.TRIM(LoginName) = @UserName
    END
	
	IF @StatusID IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET UserProfileStatusID = @StatusID
        WHERE dbo.TRIM(LoginName) = @UserName
    END
	
	IF @AddressID IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET AddressID = @AddressID
        WHERE dbo.TRIM(LoginName) = @UserName
    END
	
	IF @Comments IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET Comments = @Comments
        WHERE dbo.TRIM(LoginName) = @UserName
    END
	
	IF @tPrefix IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET TitlePreffix = @tPrefix
        WHERE dbo.TRIM(LoginName) = @UserName
    END
	
	IF @SessionID IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET SessionID = @SessionID
        WHERE dbo.TRIM(LoginName) = @UserName
    END
	
	IF @TenantID IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET TenantID = @TenantID
        WHERE dbo.TRIM(LoginName) = @UserName
    END
	
	IF @isSupervisor IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET isSupervisor = @isSupervisor
        WHERE dbo.TRIM(LoginName) = @UserName
    END
	
	IF @HireDate IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET HireDate = @HireDate
        WHERE dbo.TRIM(LoginName) = @UserName
    END
	
	IF @OperatorID IS NOT NULL
    BEGIN
        UPDATE [UserProfile].[UserProfile] SET OperatorID = @OperatorID
        WHERE dbo.TRIM(LoginName) = @UserName
    END

RETURN 0