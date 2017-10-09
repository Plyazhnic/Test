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
using EXP.Entity;
using EXP.Core.Util;
using EXP.Core.Exceptions;

namespace EXP.DataAccess
{
    public class ParkingRepository : BaseRepository, IParkingRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;

        public List<ParkingStall> ListParkingStall()
        {
            List<ParkingStall> list = new List<ParkingStall>();
            try
            {
                Logger.DebugFormat("ListParkingStall.");                             
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListParkingStall]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ParkingStall parkingStall = Mapper.DynamicMap<IDataReader, ParkingStall>(reader);
                            list.Add(parkingStall);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListParkingStall failed.", exc);
            }
            return list;
        }
    }
}
