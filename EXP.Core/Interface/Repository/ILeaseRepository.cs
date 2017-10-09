using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface ILeaseRepository
    {
        /// <summary>
        /// Create Lease
        /// </summary>
        /// <param name="newLease"></param>
        /// <returns></returns>
        int CreateLease(Lease newLease);

        /// <summary>
        /// Get lease by id
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        Lease GetLease(int leaseId);

        /// <summary>
        /// Get lease for company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Lease GetLeaseForCompany(int companyId);

        /// <summary>
        /// Update lease
        /// </summary>
        /// <param name="editLease"></param>
        void UpdateLease(Lease editLease);
    }
}
