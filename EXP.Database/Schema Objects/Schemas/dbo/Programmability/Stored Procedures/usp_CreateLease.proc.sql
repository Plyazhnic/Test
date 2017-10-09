CREATE PROCEDURE [dbo].[usp_CreateLease]
    @CompanyID int,
    @BuildingID int,
    @TermStart date,
    @TermEnd date,
    @LateFee decimal(6,2),
    @TaxExempt bit,
    @MarketRate bit,
    @Description nvarchar(1024),
    @DocumentName nvarchar(256),
	@NewLeaseID int out
AS
	INSERT INTO [dbo].[Lease]
           ([CompanyID]
           ,[BuildingID]
           ,[TermStart]
           ,[TermEnd]
           ,[LateFee]
           ,[TaxExempt]
           ,[MarketRate]
           ,[Description]
           ,[DocumentName]
           ,[CreatedDate])
     VALUES
           (@CompanyID
           ,@BuildingID
           ,@TermStart
           ,@TermEnd
           ,@LateFee
           ,@TaxExempt
           ,@MarketRate
           ,@Description
           ,@DocumentName
           ,GETDATE())

	SET @NewLeaseID = Scope_Identity();

RETURN 0
