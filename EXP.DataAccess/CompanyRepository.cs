using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Core;
using EXP.Core.Interface;
using EXP.Core.BaseClasses;
using EXP.Entity;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using AutoMapper;
using EXP.Core.Util;
using EXP.Core.Exceptions;

namespace EXP.DataAccess
{
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;

        /// <summary>
        /// Get company by id
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public Company GetCompany(int companyID)
        {
            Company company = new Company();
            try
            {
                Logger.DebugFormat("GetCompany. companyID: {0}", companyID);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetCompany]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, companyID);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            company = Mapper.DynamicMap<IDataReader, Company>(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetCompany failed.", exc);
            }
            return company;
        }

        /// <summary>
        /// Get company for user
        /// </summary>
        /// <param name="userProfileID"></param>
        /// <returns></returns>
        public Company GetCompanyForUser(int userProfileID)
        {
            Company company = new Company();
            try
            {
                Logger.DebugFormat("GetCompanyForUser. userProfileID: {0}", userProfileID);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetCompanyForUser]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userProfileID);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            company = Company(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetCompanyForUser failed.", exc);
            }
            return company;
        }

        /// <summary>
        /// Create company
        /// </summary>
        /// <param name="newCompany"></param>
        /// <returns></returns>
        public int CreateCompany(Company newCompany)
        {
            try
            {
                Logger.DebugFormat("CreateCompany. newCompany: {0}", newCompany.ToString());
                int companyId = 0;
                 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateCompany]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "CompanyName", SqlDbType.VarChar, 1024, newCompany.CompanyName);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, newCompany.AddressID);
                    DatabaseUtils.AddInputParameter(cmd, "asBuilding", SqlDbType.Bit, newCompany.asBuilding);
                    DatabaseUtils.AddInputParameter(cmd, "WorkPhoneID", SqlDbType.Int, newCompany.WorkPhoneID);
                    DatabaseUtils.AddInputParameter(cmd, "FaxID", SqlDbType.Int, newCompany.FaxID);
                    DatabaseUtils.AddInputParameter(cmd, "Suite", SqlDbType.VarChar, 16, newCompany.Suite);
                    DatabaseUtils.AddInputParameter(cmd, "EmailAddress", SqlDbType.VarChar, 128, newCompany.EmailAddress);
                    SqlParameter param = DatabaseUtils.AddOutputParameter(cmd, "NewCompanyID", SqlDbType.Int, 0);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    companyId = (int)param.Value;
                }
                return companyId;

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateCompany failed.", exc);
            }
        }

        /// <summary>
        /// Update company
        /// </summary>
        /// <param name="company"></param>
        public void UpdateCompany(Company company)
        {
            try
            {
                Logger.DebugFormat("UpdateCompany. Company: {0}", company.ToString());
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateCompany]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;               

                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, company.CompanyID);
                    DatabaseUtils.AddInputParameter(cmd, "CompanyName", SqlDbType.VarChar, 1024, company.CompanyName);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, company.AddressID);
                    DatabaseUtils.AddInputParameter(cmd, "asBuilding", SqlDbType.Bit, company.asBuilding);
                    DatabaseUtils.AddInputParameter(cmd, "Suite", SqlDbType.VarChar, 16, company.Suite);
                    DatabaseUtils.AddInputParameter(cmd, "EmailAddress", SqlDbType.VarChar, 128, company.EmailAddress);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                Logger.Error("UpdateCompany failed.", exc);
                throw new ExpDatabaseException("UpdateCompany failed.", exc);
            }
        }
        
        /// <summary>
        /// Update company manager
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="managerId"></param>
        public void UpdateCompanyManager(int companyId, int managerId)
        {
            try
            {
                Logger.DebugFormat("UpdateCompanyManager. companyId: {0}, managerId: {1}", companyId,managerId);
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateCompanyManager]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;               

                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, companyId);
                    DatabaseUtils.AddInputParameter(cmd, "ManagerID", SqlDbType.Int, managerId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                Logger.Error("UpdateCompanyManager failed.", exc);
                throw new ExpDatabaseException("UpdateCompanyManager failed.", exc);
            }
        }

        /// <summary>
        /// Update company employee
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        public void UpdateCompanyEmployee(int companyId, int employeeId)
        {
            try
            {
                Logger.DebugFormat("UpdateCompanyEmployee. companyId: {0}, employeeId: {1}", companyId, employeeId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateCompanyEmployee]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;               

                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, companyId);
                    DatabaseUtils.AddInputParameter(cmd, "EmployeeID", SqlDbType.Int, employeeId);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                Logger.Error("UpdateCompanyEmployee failed.", exc);
                throw new ExpDatabaseException("UpdateCompanyEmployee failed.", exc);
            }
        }

        /// <summary>
        /// get list of tenants
        /// </summary>
        /// <returns></returns>
        public List<Company> ListCompanies()
        {
            List<Company> list = new List<Company>();
            try
            {
                Logger.DebugFormat("ListCompanies.");    
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListCompanies]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Company company = Mapper.DynamicMap<IDataReader, Company>(reader);
                            list.Add(company);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListCompanies failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// get list of building tenants
        /// </summary>
        /// <returns></returns>
        public List<Company> ListBuildingCompanies(int buildingId, DateTime fromDate)
        {
            List<Company> list = new List<Company>();
            try
            {
                Logger.DebugFormat("ListBuildingCompanies. buildingId: {0}, fromDate: {1}", buildingId, fromDate);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListBuildingCompanies]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;           

                    DatabaseUtils.AddInputParameter(cmd, "BuildingId", SqlDbType.Int, buildingId);
                    if (fromDate != DateTime.MinValue)
                    {
                        DatabaseUtils.AddInputParameter(cmd, "FromDate", SqlDbType.DateTime, fromDate);
                    }
                    else
                    {
                        DatabaseUtils.AddInputParameter(cmd, "FromDate", SqlDbType.DateTime, DBNull.Value);
                    }

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Company company = Mapper.DynamicMap<IDataReader, Company>(reader);
                            list.Add(company);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListBuildingCompanies failed.", exc);
            }
            return list;
        }


        public List<Company> ListTenantsForPage(TableList listParam)
        {
            List<Company> list = new List<Company>();
            try
            {
                Logger.DebugFormat("ListTenantsForPage. listParam: {0}", listParam.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListTenantsForPage]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "BuildingId", SqlDbType.Int, listParam.BuildingID);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Company CompanyTenant = Tenant(reader);
                        list.Add(CompanyTenant);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListTenantsForPage failed.", exc);
            }
            return list;
        }

        public int GetTenantsCount(TableList listParam)
        {
            object value;
            try
            {
                Logger.DebugFormat("GetTenantsCount. listParam: {0}", listParam.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetTenantsCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;           

                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "BuildingId", SqlDbType.Int, listParam.BuildingID);
                
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetTenantsCount failed.", exc);
            }
            return (int)value;
        
        }

        private Company Tenant(IDataReader reader)
        {
            Company company = Mapper.DynamicMap<IDataReader, Company>(reader);                       

            UserProfile userprofile = Mapper.DynamicMap<IDataReader, UserProfile>(reader);
            company.ManagerProfile = userprofile;

            Address address = Mapper.DynamicMap<IDataReader, Address>(reader);
            company.Address = address;

            return company;
        }

        private Company Company(IDataReader reader)
        {
            Company company = Mapper.DynamicMap<IDataReader, Company>(reader);

            Address address = Mapper.DynamicMap<IDataReader, Address>(reader);
            company.Address = address;
            Phone work = Mapper.DynamicMap<IDataReader, Phone>(reader);
            Mapper.CreateMap<IDataReader, Phone>()
                        .ForMember(m => m.PhoneID, opt => opt.MapFrom(r => r["FaxPhoneID"]));
            Mapper.CreateMap<IDataReader, Phone>()
                        .ForMember(m => m.PhoneNumber, opt => opt.MapFrom(r => r["FaxPhoneNumber"]));
            Mapper.CreateMap<IDataReader, Phone>()
                        .ForMember(m => m.PhoneTypeID, opt => opt.MapFrom(r => r["FaxPhoneTypeID"]));
            Phone fax = Mapper.DynamicMap<IDataReader, Phone>(reader);
            company.WorkPhone = work;
            company.Fax = fax;

            return company;
        }

        /// <summary>
        /// List Tenants For Page
        /// </summary>
        /// <returns></returns>
        public List<UserProfile> ListEmployeesForPage(TableList listParam)
        {
            List<UserProfile> list = new List<UserProfile>();
            try
            {
                Logger.DebugFormat("ListEmployeesForPage. listParam: {0}", listParam.ToString());            
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetEmployeesForPage]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;                       

                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, listParam.CompanyID);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UserProfile employee = new UserProfile();
                        employee = Mapper.DynamicMap<IDataReader, UserProfile>(reader);

                        list.Add(employee);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListEmployeesForPage failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// Get Employees Count
        /// </summary>
        /// <param name="Search"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public int GetEmployeesCount(TableList listParam)
        {
            object value;
            try
            {
                Logger.DebugFormat("GetEmployeesCount. listParam: {0}", listParam.ToString());   
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetEmployeesCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;           

                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, listParam.CompanyID);
                
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetEmployeesCount failed.", exc);
            }
            return (int)value;
        }

        /// <summary>
        /// Create relationship user to company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userProfileId"></param>
        public void AddUserToCompany(int companyId, int userProfileId)
        {
            try
            {
                Logger.DebugFormat("AddUserToCompany. companyId: {0}, userProfileId{1}", companyId, userProfileId);

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_AddUserToCompany]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, companyId);
                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userProfileId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("AddUserToCompany failed.", exc);
            }
        }
    }
}
