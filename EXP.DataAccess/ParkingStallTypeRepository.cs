using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AutoMapper;
using EXP.Core;
using EXP.Core.BaseClasses;
using EXP.Core.Interface;
using EXP.Core.Util;
using EXP.Entity;
using EXP.Core.Exceptions;

namespace EXP.DataAccess
{
    public class ParkingStallTypeRepository : BaseRepository, IParkingStallTypeRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;
        /// <summary>
        /// Get list all Parking Stall Types
        /// </summary>
        /// <returns></returns>
        public List<ParkingStallType> ListParkingStallTypes()
        {
            List<ParkingStallType> list = new List<ParkingStallType>();                       
            try
            {
                Logger.DebugFormat("ListParkingStallTypes."); 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListParkingStallTypes]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Mapper.CreateMap<IDataReader, ParkingStallType>()
                                .ForMember(m => m.ParkingStallType1, opt => opt.MapFrom(r => r["ParkingStallType"]));
                        ParkingStallType parkingStallType = Mapper.DynamicMap<IDataReader, ParkingStallType>(reader);
                        list.Add(parkingStallType);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListParkingStallTypes failed.", exc);
            }
            return list;
        }
    }

}
