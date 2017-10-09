

CREATE PROCEDURE dbo.GetMajorVersion
    @@ver int OUTPUT
AS
BEGIN
	DECLARE @version        nchar(100)
	DECLARE @dot            int
	DECLARE @hyphen         int
	DECLARE @SqlToExec      nchar(4000)

	SELECT @@ver = 7
	SELECT @version = @@Version
	SELECT @hyphen  = CHARINDEX(N' - ', @version)
	IF (NOT(@hyphen IS NULL) AND @hyphen > 0)
	BEGIN
		SELECT @hyphen = @hyphen + 3
		SELECT @dot    = CHARINDEX(N'.', @version, @hyphen)
		IF (NOT(@dot IS NULL) AND @dot > @hyphen)
		BEGIN
			SELECT @version = SUBSTRING(@version, @hyphen, @dot - @hyphen)
			SELECT @@ver     = CONVERT(int, @version)
		END
	END
END
GO   

/*****************************************************************************/

CREATE TABLE dbo.ASPStateTempSessions (
        SessionId           nvarchar(88)    NOT NULL PRIMARY KEY,
        Created             datetime        NOT NULL DEFAULT GETUTCDATE(),
        Expires             datetime        NOT NULL,
        LockDate            datetime        NOT NULL,
        LockDateLocal       datetime        NOT NULL,
        LockCookie          int             NOT NULL,
        Timeout             int             NOT NULL,
        Locked              bit             NOT NULL,
        SessionItemShort    VARBINARY(7000) NULL,
        SessionItemLong     image           NULL,
        Flags               int             NOT NULL DEFAULT 0,
    ) 
GO
    CREATE NONCLUSTERED INDEX Index_Expires ON dbo.ASPStateTempSessions(Expires)
GO
    CREATE TABLE dbo.ASPStateTempApplications (
        AppId               int             NOT NULL PRIMARY KEY,
        AppName             char(280)       NOT NULL,
    ) 
GO
    CREATE NONCLUSTERED INDEX Index_AppName ON dbo.ASPStateTempApplications(AppName)
GO

CREATE PROCEDURE dbo.CreateTempTables
AS
    CREATE TABLE dbo.ASPStateTempSessions (
        SessionId           nvarchar(88)    NOT NULL PRIMARY KEY,
        Created             datetime        NOT NULL DEFAULT GETUTCDATE(),
        Expires             datetime        NOT NULL,
        LockDate            datetime        NOT NULL,
        LockDateLocal       datetime        NOT NULL,
        LockCookie          int             NOT NULL,
        Timeout             int             NOT NULL,
        Locked              bit             NOT NULL,
        SessionItemShort    VARBINARY(7000) NULL,
        SessionItemLong     image           NULL,
        Flags               int             NOT NULL DEFAULT 0,
    ) 

    CREATE NONCLUSTERED INDEX Index_Expires ON dbo.ASPStateTempSessions(Expires)

    CREATE TABLE dbo.ASPStateTempApplications (
        AppId               int             NOT NULL PRIMARY KEY,
        AppName             char(280)       NOT NULL,
    ) 

    CREATE NONCLUSTERED INDEX Index_AppName ON dbo.ASPStateTempApplications(AppName)

    RETURN 0

GO   

/*****************************************************************************/

CREATE PROCEDURE dbo.TempGetVersion
    @ver      char(10) OUTPUT
AS
    SELECT @ver = 2
    RETURN 0
GO

/*****************************************************************************/

CREATE PROCEDURE dbo.GetHashCode
    @input varchar(280),
    @hash int OUTPUT
AS     
    DECLARE @hi_16bit   int
    DECLARE @lo_16bit   int
    DECLARE @hi_t       int
    DECLARE @lo_t       int
    DECLARE @len        int
    DECLARE @i          int
    DECLARE @c          int
    DECLARE @carry      int

    SET @hi_16bit = 0
    SET @lo_16bit = 5381
    
    SET @len = DATALENGTH(@input)
    SET @i = 1
    
    WHILE (@i <= @len)
    BEGIN
        SET @c = ASCII(SUBSTRING(@input, @i, 1))

        /* Formula:                        
           hash = ((hash << 5) + hash) ^ c */

        /* hash << 5 */
        SET @hi_t = @hi_16bit * 32 /* high 16bits << 5 */
        SET @hi_t = @hi_t & 0xFFFF /* zero out overflow */
        
        SET @lo_t = @lo_16bit * 32 /* low 16bits << 5 */
        
        SET @carry = @lo_16bit & 0x1F0000 /* move low 16bits carryover to hi 16bits */
        SET @carry = @carry / 0x10000 /* >> 16 */
        SET @hi_t = @hi_t + @carry
        SET @hi_t = @hi_t & 0xFFFF /* zero out overflow */

        /* + hash */
        SET @lo_16bit = @lo_16bit + @lo_t
        SET @hi_16bit = @hi_16bit + @hi_t + (@lo_16bit / 0x10000)
        /* delay clearing the overflow */

        /* ^c */
        SET @lo_16bit = @lo_16bit ^ @c

        /* Now clear the overflow bits */	
        SET @hi_16bit = @hi_16bit & 0xFFFF
        SET @lo_16bit = @lo_16bit & 0xFFFF

        SET @i = @i + 1
    END

    /* Do a sign extension of the hi-16bit if needed */
    IF (@hi_16bit & 0x8000 <> 0)
        SET @hi_16bit = 0xFFFF0000 | @hi_16bit

    /* Merge hi and lo 16bit back together */
    SET @hi_16bit = @hi_16bit * 0x10000 /* << 16 */
    SET @hash = @hi_16bit | @lo_16bit

    RETURN 0
GO

/*****************************************************************************/

CREATE PROCEDURE dbo.TempGetAppID
    @appName    varchar(280),
    @appId      int OUTPUT
    AS
    SET @appName = LOWER(@appName)
    SET @appId = NULL

    SELECT @appId = AppId
    FROM dbo.ASPStateTempApplications
    WHERE AppName = @appName

    IF @appId IS NULL BEGIN
        BEGIN TRAN        

        SELECT @appId = AppId
        FROM dbo.ASPStateTempApplications WITH (TABLOCKX)
        WHERE AppName = @appName
        
        IF @appId IS NULL
        BEGIN
            EXEC GetHashCode @appName, @appId OUTPUT
            
            INSERT dbo.ASPStateTempApplications
            VALUES
            (@appId, @appName)
            
            IF @@ERROR = 2627 
            BEGIN
                DECLARE @dupApp varchar(280)
            
                SELECT @dupApp = RTRIM(AppName)
                FROM dbo.ASPStateTempApplications 
                WHERE AppId = @appId
                
                
            END
        END

        COMMIT
    END

    RETURN 0 
GO

/*****************************************************************************/

/* Find out the version */
    CREATE PROCEDURE dbo.TempGetStateItem
        @id         nvarchar(88),
        @itemShort  varbinary(7000) OUTPUT,
        @locked     bit OUTPUT,
        @lockDate   datetime OUTPUT,
        @lockCookie int OUTPUT
    AS
        DECLARE @textptr AS varbinary(16)
        DECLARE @length AS int
        DECLARE @now AS datetime
        SET @now = GETUTCDATE()

        UPDATE dbo.ASPStateTempSessions
        SET Expires = DATEADD(n, Timeout, @now), 
            @locked = Locked,
            @lockDate = LockDateLocal,
            @lockCookie = LockCookie,
            @itemShort = CASE @locked
                WHEN 0 THEN SessionItemShort
                ELSE NULL
                END,
            @textptr = CASE @locked
                WHEN 0 THEN TEXTPTR(SessionItemLong)
                ELSE NULL
                END,
            @length = CASE @locked
                WHEN 0 THEN DATALENGTH(SessionItemLong)
                ELSE NULL
                END
        WHERE SessionId = @id
        IF @length IS NOT NULL BEGIN
            READTEXT dbo.ASPStateTempSessions.SessionItemLong @textptr 0 @length
        END

        RETURN 0
GO

/*****************************************************************************/

CREATE PROCEDURE dbo.TempGetStateItem2
    @id         nvarchar(88),
    @itemShort  varbinary(7000) OUTPUT,
    @locked     bit OUTPUT,
    @lockAge    int OUTPUT,
    @lockCookie int OUTPUT
AS
    DECLARE @textptr AS varbinary(16)
    DECLARE @length AS int
    DECLARE @now AS datetime
    SET @now = GETUTCDATE()

    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, Timeout, @now), 
        @locked = Locked,
        @lockAge = DATEDIFF(second, LockDate, @now),
        @lockCookie = LockCookie,
        @itemShort = CASE @locked
            WHEN 0 THEN SessionItemShort
            ELSE NULL
            END,
        @textptr = CASE @locked
            WHEN 0 THEN TEXTPTR(SessionItemLong)
            ELSE NULL
            END,
        @length = CASE @locked
            WHEN 0 THEN DATALENGTH(SessionItemLong)
            ELSE NULL
            END
    WHERE SessionId = @id
    IF @length IS NOT NULL BEGIN
        READTEXT dbo.ASPStateTempSessions.SessionItemLong @textptr 0 @length
    END

    RETURN 0 
GO
            

/*****************************************************************************/

CREATE PROCEDURE dbo.TempGetStateItem3
    @id         nvarchar(88),
    @itemShort  varbinary(7000) OUTPUT,
    @locked     bit OUTPUT,
    @lockAge    int OUTPUT,
    @lockCookie int OUTPUT,
    @actionFlags int OUTPUT
AS
    DECLARE @textptr AS varbinary(16)
    DECLARE @length AS int
    DECLARE @now AS datetime
    SET @now = GETUTCDATE()

    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, Timeout, @now), 
        @locked = Locked,
        @lockAge = DATEDIFF(second, LockDate, @now),
        @lockCookie = LockCookie,
        @itemShort = CASE @locked
            WHEN 0 THEN SessionItemShort
            ELSE NULL
            END,
        @textptr = CASE @locked
            WHEN 0 THEN TEXTPTR(SessionItemLong)
            ELSE NULL
            END,
        @length = CASE @locked
            WHEN 0 THEN DATALENGTH(SessionItemLong)
            ELSE NULL
            END,

        /* If the Uninitialized flag (0x1) if it is set,
           remove it and return InitializeItem (0x1) in actionFlags */
        Flags = CASE
            WHEN (Flags & 1) <> 0 THEN (Flags & ~1)
            ELSE Flags
            END,
        @actionFlags = CASE
            WHEN (Flags & 1) <> 0 THEN 1
            ELSE 0
            END
    WHERE SessionId = @id
    IF @length IS NOT NULL BEGIN
        READTEXT dbo.ASPStateTempSessions.SessionItemLong @textptr 0 @length
    END

    RETURN 0
GO

/*****************************************************************************/


CREATE PROCEDURE dbo.TempGetStateItemExclusive
    @id         nvarchar(88),
    @itemShort  varbinary(7000) OUTPUT,
    @locked     bit OUTPUT,
    @lockDate   datetime OUTPUT,
    @lockCookie int OUTPUT
AS
    DECLARE @textptr AS varbinary(16)
    DECLARE @length AS int
    DECLARE @now AS datetime
    DECLARE @nowLocal AS datetime

    SET @now = GETUTCDATE()
    SET @nowLocal = GETDATE()
    
    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, Timeout, @now), 
        LockDate = CASE Locked
            WHEN 0 THEN @now
            ELSE LockDate
            END,
        @lockDate = LockDateLocal = CASE Locked
            WHEN 0 THEN @nowLocal
            ELSE LockDateLocal
            END,
        @lockCookie = LockCookie = CASE Locked
            WHEN 0 THEN LockCookie + 1
            ELSE LockCookie
            END,
        @itemShort = CASE Locked
            WHEN 0 THEN SessionItemShort
            ELSE NULL
            END,
        @textptr = CASE Locked
            WHEN 0 THEN TEXTPTR(SessionItemLong)
            ELSE NULL
            END,
        @length = CASE Locked
            WHEN 0 THEN DATALENGTH(SessionItemLong)
            ELSE NULL
            END,
        @locked = Locked,
        Locked = 1
    WHERE SessionId = @id
    IF @length IS NOT NULL BEGIN
        READTEXT dbo.ASPStateTempSessions.SessionItemLong @textptr 0 @length
    END

    RETURN 0
GO


/*****************************************************************************/

CREATE PROCEDURE dbo.TempGetStateItemExclusive2
    @id         nvarchar(88),
    @itemShort  varbinary(7000) OUTPUT,
    @locked     bit OUTPUT,
    @lockAge    int OUTPUT,
    @lockCookie int OUTPUT
AS
    DECLARE @textptr AS varbinary(16)
    DECLARE @length AS int
    DECLARE @now AS datetime
    DECLARE @nowLocal AS datetime

    SET @now = GETUTCDATE()
    SET @nowLocal = GETDATE()
    
    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, Timeout, @now), 
        LockDate = CASE Locked
            WHEN 0 THEN @now
            ELSE LockDate
            END,
        LockDateLocal = CASE Locked
            WHEN 0 THEN @nowLocal
            ELSE LockDateLocal
            END,
        @lockAge = CASE Locked
            WHEN 0 THEN 0
            ELSE DATEDIFF(second, LockDate, @now)
            END,
        @lockCookie = LockCookie = CASE Locked
            WHEN 0 THEN LockCookie + 1
            ELSE LockCookie
            END,
        @itemShort = CASE Locked
            WHEN 0 THEN SessionItemShort
            ELSE NULL
            END,
        @textptr = CASE Locked
            WHEN 0 THEN TEXTPTR(SessionItemLong)
            ELSE NULL
            END,
        @length = CASE Locked
            WHEN 0 THEN DATALENGTH(SessionItemLong)
            ELSE NULL
            END,
        @locked = Locked,
        Locked = 1
    WHERE SessionId = @id
    IF @length IS NOT NULL BEGIN
        READTEXT dbo.ASPStateTempSessions.SessionItemLong @textptr 0 @length
    END

    RETURN 0  
GO


/*****************************************************************************/

CREATE PROCEDURE dbo.TempGetStateItemExclusive3
    @id         nvarchar(88),
    @itemShort  varbinary(7000) OUTPUT,
    @locked     bit OUTPUT,
    @lockAge    int OUTPUT,
    @lockCookie int OUTPUT,
    @actionFlags int OUTPUT
AS
    DECLARE @textptr AS varbinary(16)
    DECLARE @length AS int
    DECLARE @now AS datetime
    DECLARE @nowLocal AS datetime

    SET @now = GETUTCDATE()
    SET @nowLocal = GETDATE()
    
    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, Timeout, @now), 
        LockDate = CASE Locked
            WHEN 0 THEN @now
            ELSE LockDate
            END,
        LockDateLocal = CASE Locked
            WHEN 0 THEN @nowLocal
            ELSE LockDateLocal
            END,
        @lockAge = CASE Locked
            WHEN 0 THEN 0
            ELSE DATEDIFF(second, LockDate, @now)
            END,
        @lockCookie = LockCookie = CASE Locked
            WHEN 0 THEN LockCookie + 1
            ELSE LockCookie
            END,
        @itemShort = CASE Locked
            WHEN 0 THEN SessionItemShort
            ELSE NULL
            END,
        @textptr = CASE Locked
            WHEN 0 THEN TEXTPTR(SessionItemLong)
            ELSE NULL
            END,
        @length = CASE Locked
            WHEN 0 THEN DATALENGTH(SessionItemLong)
            ELSE NULL
            END,
        @locked = Locked,
        Locked = 1,

        /* If the Uninitialized flag (0x1) if it is set,
           remove it and return InitializeItem (0x1) in actionFlags */
        Flags = CASE
            WHEN (Flags & 1) <> 0 THEN (Flags & ~1)
            ELSE Flags
            END,
        @actionFlags = CASE
            WHEN (Flags & 1) <> 0 THEN 1
            ELSE 0
            END
    WHERE SessionId = @id
    IF @length IS NOT NULL BEGIN
        READTEXT dbo.ASPStateTempSessions.SessionItemLong @textptr 0 @length
    END

    RETURN 0
GO


/*****************************************************************************/


CREATE PROCEDURE dbo.TempReleaseStateItemExclusive
    @id         nvarchar(88),
    @lockCookie int
AS
    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, Timeout, GETUTCDATE()), 
        Locked = 0
    WHERE SessionId = @id AND LockCookie = @lockCookie

    RETURN 0
GO


/*****************************************************************************/

CREATE PROCEDURE dbo.TempInsertUninitializedItem
    @id         nvarchar(88),
    @itemShort  varbinary(7000),
    @timeout    int
AS    

    DECLARE @now AS datetime
    DECLARE @nowLocal AS datetime
    
    SET @now = GETUTCDATE()
    SET @nowLocal = GETDATE()

    INSERT dbo.ASPStateTempSessions 
        (SessionId, 
         SessionItemShort, 
         Timeout, 
         Expires, 
         Locked, 
         LockDate,
         LockDateLocal,
         LockCookie,
         Flags) 
    VALUES 
        (@id, 
         @itemShort, 
         @timeout, 
         DATEADD(n, @timeout, @now), 
         0, 
         @now,
         @nowLocal,
         1,
         1)

    RETURN 0
GO


/*****************************************************************************/

CREATE PROCEDURE dbo.TempInsertStateItemShort
    @id         nvarchar(88),
    @itemShort  varbinary(7000),
    @timeout    int
AS    

    DECLARE @now AS datetime
    DECLARE @nowLocal AS datetime
    
    SET @now = GETUTCDATE()
    SET @nowLocal = GETDATE()

    INSERT dbo.ASPStateTempSessions 
        (SessionId, 
         SessionItemShort, 
         Timeout, 
         Expires, 
         Locked, 
         LockDate,
         LockDateLocal,
         LockCookie) 
    VALUES 
        (@id, 
         @itemShort, 
         @timeout, 
         DATEADD(n, @timeout, @now), 
         0, 
         @now,
         @nowLocal,
         1)

    RETURN 0
GO


/*****************************************************************************/

CREATE PROCEDURE dbo.TempInsertStateItemLong
    @id         nvarchar(88),
    @itemLong   image,
    @timeout    int
AS    
    DECLARE @now AS datetime
    DECLARE @nowLocal AS datetime
    
    SET @now = GETUTCDATE()
    SET @nowLocal = GETDATE()

    INSERT dbo.ASPStateTempSessions 
        (SessionId, 
         SessionItemLong, 
         Timeout, 
         Expires, 
         Locked, 
         LockDate,
         LockDateLocal,
         LockCookie) 
    VALUES 
        (@id, 
         @itemLong, 
         @timeout, 
         DATEADD(n, @timeout, @now), 
         0, 
         @now,
         @nowLocal,
         1)

    RETURN 0
GO


/*****************************************************************************/


CREATE PROCEDURE dbo.TempUpdateStateItemShort
    @id         nvarchar(88),
    @itemShort  varbinary(7000),
    @timeout    int,
    @lockCookie int
AS    
    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, @timeout, GETUTCDATE()), 
        SessionItemShort = @itemShort, 
        Timeout = @timeout,
        Locked = 0
    WHERE SessionId = @id AND LockCookie = @lockCookie

    RETURN 0
GO


/*****************************************************************************/

CREATE PROCEDURE dbo.TempUpdateStateItemShortNullLong
    @id         nvarchar(88),
    @itemShort  varbinary(7000),
    @timeout    int,
    @lockCookie int
AS    
    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, @timeout, GETUTCDATE()), 
        SessionItemShort = @itemShort, 
        SessionItemLong = NULL, 
        Timeout = @timeout,
        Locked = 0
    WHERE SessionId = @id AND LockCookie = @lockCookie

    RETURN 0
GO


/*****************************************************************************/


CREATE PROCEDURE dbo.TempUpdateStateItemLong
    @id         nvarchar(88),
    @itemLong   image,
    @timeout    int,
    @lockCookie int
AS    
    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, @timeout, GETUTCDATE()), 
        SessionItemLong = @itemLong,
        Timeout = @timeout,
        Locked = 0
    WHERE SessionId = @id AND LockCookie = @lockCookie

    RETURN 0       
GO


/*****************************************************************************/

CREATE PROCEDURE dbo.TempUpdateStateItemLongNullShort
    @id         nvarchar(88),
    @itemLong   image,
    @timeout    int,
    @lockCookie int
AS    
    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, @timeout, GETUTCDATE()), 
        SessionItemLong = @itemLong, 
        SessionItemShort = NULL,
        Timeout = @timeout,
        Locked = 0
    WHERE SessionId = @id AND LockCookie = @lockCookie

    RETURN 0  
GO

/*****************************************************************************/


CREATE PROCEDURE dbo.TempRemoveStateItem
    @id     nvarchar(88),
    @lockCookie int
AS
    DELETE dbo.ASPStateTempSessions
    WHERE SessionId = @id AND LockCookie = @lockCookie
    RETURN 0   
GO
            
/*****************************************************************************/

CREATE PROCEDURE dbo.TempResetTimeout
    @id     nvarchar(88)
AS
    UPDATE dbo.ASPStateTempSessions
    SET Expires = DATEADD(n, Timeout, GETUTCDATE())
    WHERE SessionId = @id
    RETURN 0   
GO

            
/*****************************************************************************/

CREATE PROCEDURE dbo.DeleteExpiredSessions
AS
    SET NOCOUNT ON
    SET DEADLOCK_PRIORITY LOW 

    DECLARE @now datetime
    SET @now = GETUTCDATE() 

    CREATE TABLE #tblExpiredSessions 
    ( 
        SessionID nvarchar(88) NOT NULL PRIMARY KEY
    )

    INSERT #tblExpiredSessions (SessionID)
        SELECT SessionID
        FROM dbo.ASPStateTempSessions WITH (READUNCOMMITTED)
        WHERE Expires < @now

    IF @@ROWCOUNT <> 0 
    BEGIN 
        DECLARE ExpiredSessionCursor CURSOR LOCAL FORWARD_ONLY READ_ONLY
        FOR SELECT SessionID FROM #tblExpiredSessions 

        DECLARE @SessionID nvarchar(88)

        OPEN ExpiredSessionCursor

        FETCH NEXT FROM ExpiredSessionCursor INTO @SessionID

        WHILE @@FETCH_STATUS = 0 
            BEGIN
                DELETE FROM dbo.ASPStateTempSessions WHERE SessionID = @SessionID AND Expires < @now
                FETCH NEXT FROM ExpiredSessionCursor INTO @SessionID
            END

        CLOSE ExpiredSessionCursor

        DEALLOCATE ExpiredSessionCursor

    END 

    DROP TABLE #tblExpiredSessions

RETURN 0
GO
            
/*****************************************************************************/


CREATE PROCEDURE dbo.ASPState_Startup 
AS
    EXECUTE dbo.CreateTempTables

    RETURN 0
    

/*****************************************************************************/

/* Create the job to delete expired sessions */

-- Add job category
-- We expect an error if the category already exists.
--PRINT 'If the category already exists, an error from msdb.dbo.sp_add_category is expected.'
--EXECUTE msdb.dbo.sp_add_category @name = N'[Uncategorized (Local)]'
--GO

--BEGIN TRANSACTION            
--    DECLARE @JobID BINARY(16)  
--    DECLARE @ReturnCode int    
--    DECLARE @nameT nchar(200)
--    SELECT @ReturnCode = 0     

--    -- Add the job
--    SET @nameT = N'ASPState' + '_Job_DeleteExpiredSessions'
--    EXECUTE @ReturnCode = msdb.dbo.sp_add_job 
--            @job_id = @JobID OUTPUT, 
--            @job_name = @nameT, 
--            @owner_login_name = NULL, 
--            @description = N'Deletes expired sessions from the session state database.', 
--            @category_name = N'[Uncategorized (Local)]', 
--            @enabled = 1, 
--            @notify_level_email = 0, 
--            @notify_level_page = 0, 
--            @notify_level_netsend = 0, 
--            @notify_level_eventlog = 0, 
--            @delete_level= 0

--    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 
    
--    -- Add the job steps
--    SET @nameT = N'ASPState' + '_JobStep_DeleteExpiredSessions'
--    EXECUTE @ReturnCode = msdb.dbo.sp_add_jobstep 
--            @job_id = @JobID,
--            @step_id = 1, 
--            @step_name = @nameT, 
--            @command = N'EXECUTE DeleteExpiredSessions', 
--            @database_name = N'ASPState', 
--            @server = N'', 
--            @subsystem = N'TSQL', 
--            @cmdexec_success_code = 0, 
--            @flags = 0, 
--            @retry_attempts = 0, 
--            @retry_interval = 1, 
--            @output_file_name = N'', 
--            @on_success_step_id = 0, 
--            @on_success_action = 1, 
--            @on_fail_step_id = 0, 
--            @on_fail_action = 2

--    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 

--    EXECUTE @ReturnCode = msdb.dbo.sp_update_job @job_id = @JobID, @start_step_id = 1 
--    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 
    
--    -- Add the job schedules
--    SET @nameT = N'ASPState' + '_JobSchedule_DeleteExpiredSessions'
--    EXECUTE @ReturnCode = msdb.dbo.sp_add_jobschedule 
--            @job_id = @JobID, 
--            @name = @nameT, 
--            @enabled = 1, 
--            @freq_type = 4,     
--            @active_start_date = 20001016, 
--            @active_start_time = 0, 
--            @freq_interval = 1, 
--            @freq_subday_type = 4, 
--            @freq_subday_interval = 1, 
--            @freq_relative_interval = 0, 
--            @freq_recurrence_factor = 0, 
--            @active_end_date = 99991231, 
--            @active_end_time = 235959

--    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 
    
--    -- Add the Target Servers
--    EXECUTE @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @JobID, @server_name = N'(local)' 
--    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback 
    
--    COMMIT TRANSACTION          
--    GOTO   EndSave              
--QuitWithRollback:
--    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION 
--EndSave: 
--GO

--/*************************************************************/
--/*************************************************************/
--/*************************************************************/
--/*************************************************************/

--PRINT ''
--PRINT '------------------------------------------'
--PRINT 'Completed execution of InstallSqlState.SQL'
--PRINT '------------------------------------------'

