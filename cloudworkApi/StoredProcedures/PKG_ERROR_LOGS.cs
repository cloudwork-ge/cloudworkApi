using cloudworkApi.DataManagers;
using cloudworkApi.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace cloudworkApi.StoredProcedures
{
    public class PKG_ERROR_LOGS : DataManager
    {
        public void LogException(int userID, string message, string stacktrace, string requestURL, string ip_address, string queryString)
        {
            var connString = _connectionString;
            SqlConnection connection = new SqlConnection(connString);
            //var query = "INSERT INTO error_logs (userID,message,stacktrace,requestURL,ip_address,queryString)";
            //query += " VALUES(@userID,@message,@stacktrace,@requestURL,@ip_address,@queryString)";
            SqlCommand cmd = new SqlCommand("logError", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@message", SqlDbType.NVarChar, -1).Value = message;
            cmd.Parameters.Add("@stacktrace", SqlDbType.NVarChar, -1).Value = stacktrace;
            cmd.Parameters.Add("@requestURL", SqlDbType.NVarChar, 2000).Value = requestURL;
            cmd.Parameters.Add("@queryString", SqlDbType.NVarChar, -1).Value = queryString;
            cmd.Parameters.Add("@ip_address", SqlDbType.VarChar, 1000).Value = ip_address;

            connection.Open();
            //cmd.ExecuteNonQuery();
            cmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
