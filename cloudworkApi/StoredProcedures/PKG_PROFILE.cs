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
    public class PKG_PROFILE : DataManager
    {
        public bool ChangeProfile(int userID, string fullName, string phone, string tin = "")
        {
            var connString = _connectionString;
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("changeProfile", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@fullName", SqlDbType.NVarChar, 100).Value = fullName;
            cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 50).Value = phone;
            if (tin == null)
                cmd.Parameters.Add("@tin", SqlDbType.NVarChar, 50).Value = "";
            else
                cmd.Parameters.Add("@tin", SqlDbType.NVarChar, 50).Value = tin;

            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            return Convert.ToBoolean(cmd.Parameters["@returnValue"].Value);
        }
        public bool ChangePassword(int userID, string password, string newPassword, string confirmNewPassword)
        {
            var connString = _connectionString;
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("changePassword", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@password", SqlDbType.NVarChar, 100).Value = password;
            cmd.Parameters.Add("@newPassword", SqlDbType.NVarChar, 100).Value = newPassword;
            cmd.Parameters.Add("@confirmNewPassword", SqlDbType.NVarChar, 100).Value = confirmNewPassword;
            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            return Convert.ToBoolean(cmd.Parameters["@returnValue"].Value);
        }
        public bool RecoverPassword(string email, string newRandomPassword)
        {
            var connString = _connectionString;
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("recoverPassword", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
            cmd.Parameters.Add("@newRandomPassword", SqlDbType.NVarChar).Value = newRandomPassword;
            cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            return Convert.ToBoolean(cmd.Parameters["@returnValue"].Value);
        }

    }
}
