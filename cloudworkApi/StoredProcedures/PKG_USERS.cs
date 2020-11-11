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
    public class PKG_USERS : DataManager
    {
        //public PKG_USERS(string connectionString)
        //{
        //    //_connectionString = connectionString;
        //}

        //public string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString.ToString();
        //smarterasp.net antsge : Ants.ge1!
        public Boolean Authenticate(string email, string password, out AuthUser authUser)
        {
            var connString = _connectionString;
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("Authenticate",connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@email",SqlDbType.VarChar,100).Value = email;
            cmd.Parameters.Add("@password",SqlDbType.VarChar,200).Value = password;
            cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            connection.Open();
            //cmd.ExecuteNonQuery();
            var user = new AuthUser();
            this.ExecuteReader(cmd, delegate(SqlDataReader reader) {
                user.ID = (int)reader["ID"];
                user.email = reader["email"].ToString();
                user.phone = reader["phone"].ToString();
                user.fullName = reader["fullName"].ToString();
                user.userType = reader["userType"].ToString();
            });
            authUser = user;
            return Convert.ToBoolean(cmd.Parameters["@returnValue"].Value);
        }
        public Boolean Register(string email, string password, string fullName, string phone, string tin, string samformaType = "", decimal userType = 0)
        {
            //var connString = _connectionString;
            //SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("Register");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 200).Value = password;
            cmd.Parameters.Add("@fullname", SqlDbType.NVarChar, 200).Value = fullName;
            cmd.Parameters.Add("@phone", SqlDbType.VarChar, 200).Value = phone;
            cmd.Parameters.Add("@tin", SqlDbType.VarChar, 200).Value = tin;
            cmd.Parameters.Add("@samforma", SqlDbType.NVarChar,1000).Value = samformaType;
            cmd.Parameters.Add("@userType", SqlDbType.Int).Value = userType;
            cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            //connection.Open();

            this.ExecuteNonQuery(cmd);
            //connection.Close();
            return Convert.ToBoolean(cmd.Parameters["@returnValue"].Value);
        }

        public List<Modules> GetUserModules(int userID)
        {
            var connString = _connectionString;
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("GetUserModules", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            connection.Open();
            //cmd.ExecuteNonQuery();
            var list = new List<Modules>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Modules() { ID = (int)reader["Id"], 
                    moduleName = reader["moduleName"].ToString(),
                    moduleNameEn = reader["moduleNameEn"].ToString(),
                    iconName = reader["iconName"].ToString(),
                    iconType = reader["iconType"].ToString(),
                    url = reader["url"].ToString()
                });
            }
            reader.Close();
            connection.Close();
            return list;
        }
    }
}
