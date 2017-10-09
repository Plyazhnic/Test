using AutoMapper;
using EXP.Core;
using EXP.Core.BaseClasses;
using EXP.Core.Exceptions;
using EXP.Core.Interface;
using EXP.Core.Util;
using EXP.Entity;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace EXP.DataAccess
{
    public class LeaseRepository : BaseRepository, ILeaseRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;
        /// <summary>
        /// Create Lease
        /// </summary>
        /// <param name="newLease"></param>
        /// <returns></returns>
        public int CreateLease(Lease newLease)
        {
            try
            {
                Logger.DebugFormat("CreateLease. newLease: {0}", newLease.ToString());
                int leaseId = 0;
               
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateLease]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, newLease.CompanyID);
                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, newLease.BuildingID);
                    DatabaseUtils.AddInputParameter(cmd, "TermStart", SqlDbType.Date, newLease.TermStart);
                    DatabaseUtils.AddInputParameter(cmd, "TermEnd", SqlDbType.Date, newLease.TermEnd);
                    DatabaseUtils.AddInputParameter(cmd, "LateFee", SqlDbType.Decimal, newLease.LateFee);
                    DatabaseUtils.AddInputParameter(cmd, "TaxExempt", SqlDbType.Bit, newLease.TaxExempt);
                    DatabaseUtils.AddInputParameter(cmd, "MarketRate", SqlDbType.Bit, newLease.MarketRate);
                    DatabaseUtils.AddInputParameter(cmd, "Description", SqlDbType.NVarChar, 1024, newLease.Description);
                    DatabaseUtils.AddInputParameter(cmd, "DocumentName", SqlDbType.NVarChar, 256, newLease.DocumentName);
                    SqlParameter param = DatabaseUtils.AddOutputParameter(cmd, "NewLeaseID", SqlDbType.Int, 0);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    leaseId = (int)param.Value;
                }
                return leaseId;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateLease failed.", exc);
            }
        }

        /// <summary>
        /// Get lease by id
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        public Lease GetLease(int leaseId)
        {
            Lease lease = new Lease();
            try
            {
                Logger.DebugFormat("GetLease. leaseId: {0}", leaseId);            
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetLease]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "LeaseID", SqlDbType.Int, leaseId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lease = Lease(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetLease failed.", exc);
            }
            return lease;
        }

        /// <summary>
        /// Get lease for company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Lease GetLeaseForCompany(int companyId)
        {
            Lease lease = new Lease();
            try
            {
                Logger.DebugFormat("GetLeaseForCompany. companyId: {0}", companyId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetTenantLease]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, companyId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lease = Lease(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetLeaseForCompany failed.", exc);
            }
            return lease;
        }

        /// <summary>
        /// Update lease
        /// </summary>
        /// <param name="editLease"></param>
        public void UpdateLease(Lease editLease)
        {
            try
            {
                Logger.DebugFormat("UpdateLease. editLease: {0}", editLease.ToString());                               
                            
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateLease]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "LeaseID", SqlDbType.Int, editLease.LeaseID);
                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, editLease.CompanyID);
                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, editLease.BuildingID);
                    DatabaseUtils.AddInputParameter(cmd, "TermStart", SqlDbType.Date, editLease.TermStart);
                    DatabaseUtils.AddInputParameter(cmd, "TermEnd", SqlDbType.Date, editLease.TermEnd);
                    DatabaseUtils.AddInputParameter(cmd, "LateFee", SqlDbType.Decimal, editLease.LateFee);
                    DatabaseUtils.AddInputParameter(cmd, "TaxExempt", SqlDbType.Bit, editLease.TaxExempt);
                    DatabaseUtils.AddInputParameter(cmd, "MarketRate", SqlDbType.Bit, editLease.MarketRate);
                    DatabaseUtils.AddInputParameter(cmd, "Description", SqlDbType.NVarChar, 1024, editLease.Description);
                    DatabaseUtils.AddInputParameter(cmd, "DocumentName", SqlDbType.NVarChar, 256, editLease.DocumentName);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateLease failed.", exc);
            }
        }

        private Lease Lease(IDataReader reader)
        {
            Lease lease = Mapper.DynamicMap<IDataReader, Lease>(reader);

            Company company = Mapper.DynamicMap<IDataReader, Company>(reader);
            Building building = Mapper.DynamicMap<IDataReader, Building>(reader);
            Address address = Mapper.DynamicMap<IDataReader, Address>(reader);
            zState state = Mapper.DynamicMap<IDataReader, zState>(reader);
            address.State = state;
            building.BuildingAddress = address;
            lease.Company = company;
            lease.Building = building;
            return lease;
        }
    }
}
