using System;
using System.Collections.Generic;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface ICompanyRepository
    {
        /// <summary>
        /// Get company by id
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        Company GetCompany(int companyID);

         /// <summary>
        /// Get company for user
        /// </summary>
        /// <param name="userProfileID"></param>
        /// <returns></returns>
        Company GetCompanyForUser(int userProfileID);

        /// <summary>
        /// Create company
        /// </summary>
        /// <param name="newCompany"></param>
        /// <returns></returns>
        int CreateCompany(Company newCompany);

        /// <summary>
        /// Update company
        /// </summary>
        /// <param name="company"></param>
        void UpdateCompany(Company company);

        /// <summary>
        /// Update company manager
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="managerId"></param>
        void UpdateCompanyManager(int companyId, int managerId);

        /// <summary>
        /// Update company employee
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        void UpdateCompanyEmployee(int companyId, int employeeId);

        /// <summary>
        /// get list of tenants
        /// </summary>
        /// <returns></returns>
        List<Company> ListCompanies();

        List<Company> ListBuildingCompanies(int buildingId, DateTime fromDate);

        /// <summary>
        /// List tenants tor page
        /// </summary>
        /// <returns></returns>
        List<Company> ListTenantsForPage(TableList listParam);
        
        /// <summary>
        /// Get tenants count for list
        /// </summary>
        /// <returns></returns>       
        int GetTenantsCount(TableList listParam);

        /// <summary>
        /// List Employees For Page
        /// </summary>
        /// <returns></returns>       
        List<UserProfile> ListEmployeesForPage(TableList listParam);

        /// <summary>
        /// Get Employees Count
        /// </summary>
        /// <returns></returns>
        int GetEmployeesCount(TableList listParam);

        /// <summary>
        /// Create relationship user to company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userProfileId"></param>
        void AddUserToCompany(int companyId, int userProfileId);
    }
}
