CREATE PROCEDURE [dbo].[usp_CreateAlert]
    @AlertTypeID int,
    @UserProfileID int,
    @EntityTypeID int,
    @EntityID int

AS

    INSERT INTO [dbo].[Alert]
            ([AlertTypeID]
            ,[UserProfileID]
            ,[EntityTypeID]
            ,[EntityID]
            ,[isActive]
            ,[CreatedDate]
            ,[UpdatedDate])
     VALUES
            (@AlertTypeID,
            @UserProfileID,
            @EntityTypeID,
            @EntityID,
            1,
            GETDATE(),
            GETDATE());
RETURN 0