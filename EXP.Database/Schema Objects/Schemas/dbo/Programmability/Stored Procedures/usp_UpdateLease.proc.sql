CREATE PROCEDURE [dbo].[usp_UpdateLease]
    @LeaseID int,
	@CompanyID int,
    @BuildingID int,
    @TermStart date,
    @TermEnd date,
    @LateFee decimal(6,2),
    @TaxExempt bit,
    @MarketRate bit,
    @Description nvarchar(1024),
    @DocumentName nvarchar(256)
AS
	UPDATE [dbo].[Lease]
	SET    [CompanyID]=@CompanyID
           ,[BuildingID]=@BuildingID
           ,[TermStart]=@TermStart
           ,[TermEnd]=@TermEnd
           ,[LateFee]=@LateFee
           ,[TaxExempt]=@TaxExempt
           ,[MarketRate]=@MarketRate
           ,[Description]=@Description
           ,[DocumentName]=@DocumentName
		   ,[UpdatedDate]=GETDATE()
  
   WHERE LeaseID = @LeaseID

RETURN 0
