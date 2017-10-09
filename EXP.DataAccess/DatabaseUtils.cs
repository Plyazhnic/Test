using System;
using System.Data;
using System.Data.SqlClient;

namespace EXP.DataAccess
{
    public class DatabaseUtils
    {
        public static void AddInputParameter(SqlCommand command, string name, SqlDbType type, object value)
        {
            SqlParameter param = command.Parameters.Add(name, type);
            param.Value = value ?? DBNull.Value;
            param.Direction = ParameterDirection.Input;
        }

        public static void AddInputParameter(SqlCommand command, string name, SqlDbType type, int size, object value)
        {
            SqlParameter param = command.Parameters.Add(name, type, size);

            param.Value = value ?? DBNull.Value;
            param.Direction = ParameterDirection.Input;
        }

        public static SqlParameter AddOutputParameter(SqlCommand command, string name, SqlDbType type, int size)
        {
            SqlParameter param = command.Parameters.Add(name, type, size);

            param.Direction = ParameterDirection.Output;

            return param;
        }

        public static void AddReturnParameter(SqlCommand command, string name, SqlDbType type)
        {
            SqlParameter param = command.Parameters.Add(name, type);
            param.Direction = ParameterDirection.ReturnValue;
        }

        public static object GetReturnParameter(IDbCommand command, string name)
        {
            return ((IDbDataParameter)command.Parameters[name]).Value;
        }

        public static T GetValue<T>(IDataReader reader, string column) where T : struct
        {
            return (T) reader[column];
        }

        public static T? GetNullableValue<T>(IDataReader reader, string column) where T : struct
        {
            if (reader[column] == DBNull.Value)
                return null;

            return (T) reader[column];
        }

        public static string GetString(IDataReader reader, string column)
        {
            if (reader[column] == DBNull.Value)
                return null;

            return (string) reader[column];
        }
    }
}