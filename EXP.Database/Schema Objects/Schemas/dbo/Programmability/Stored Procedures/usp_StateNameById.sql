CREATE PROCEDURE [dbo].[usp_StateNameById]
	@StateID int
	
AS
	SELECT dbo.zState.StateName FROM dbo.zState
	
	WHERE dbo.zState.StateID = @StateID
RETURN 0
