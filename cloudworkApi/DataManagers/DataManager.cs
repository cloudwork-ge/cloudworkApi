using cloudworkApi.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cloudworkApi.DataManagers
{
    public class DataManager
    {
        public static IConfiguration configuration;
        public static string _connectionString { get {
                return configuration.GetConnectionString("DefaultDB").ToString();
            }
        }
        public static DateTime timeStampDateUTC(int timestamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);// Universal time zone
            var timestampDate = dtDateTime.AddSeconds(timestamp);
            return timestampDate;
        }
        protected string connectionString { get {
                return DataManager._connectionString;
            }
        }

        public void ExecuteNonQuery(SqlCommand cmd) {
            var conn = new SqlConnection(_connectionString);
            cmd.Connection = conn;
            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Class >= 11 && ex.Class < 16) // Class between 11 and 16 are user errors RAISERROR Sql
                {
                    throw new UserExceptions(ex.Message);
                }
                else
                    throw new Exception(ex.Message);
            }
            conn.Close();
        }
        public delegate void DbEachRow(SqlDataReader reader);
        public void ExecuteReader(SqlCommand cmd, DbEachRow dbEachRow)
        {
            var conn = new SqlConnection(_connectionString);
            SqlDataReader reader = null;
            cmd.Connection = conn;
            conn.Open();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dbEachRow.Invoke(reader);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Class >= 11 && ex.Class < 16) // Class between 11 and 16 are user errors RAISERROR Sql
                {
                    throw new UserExceptions(ex.Message);
                }
                else
                    throw new Exception(ex.Message);
            }
            conn.Close();
        }
    }
}
