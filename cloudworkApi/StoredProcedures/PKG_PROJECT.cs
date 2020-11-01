using cloudworkApi.DataManagers;
using cloudworkApi.Models;
using cloudworkApi.Models.dsModels;
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
    public class PKG_PROJECT : DataManager
    {
        public void AddProject(int userID, Project project)
        {
            //var connString = _connectionString;
            //SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("AddProject");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@projectCategoryID", SqlDbType.Int).Value = project.projectCategory;
            cmd.Parameters.Add("@projectType", SqlDbType.Int).Value = project.projectType;
            cmd.Parameters.Add("@projectName", SqlDbType.NVarChar).Value = project.projectName;
            cmd.Parameters.Add("@projectDescription", SqlDbType.NVarChar).Value = project.projectDescription;
            cmd.Parameters.Add("@criteria", SqlDbType.NVarChar).Value = project.projectCriteria;
            cmd.Parameters.Add("@budget", SqlDbType.Int).Value = project.budget;
            cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = project.startDate;
            cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = project.endDate;
            cmd.Parameters.Add("@monthsLength", SqlDbType.Int).Value = project.monthsLength;

            //cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            //connection.Open();

            this.ExecuteNonQuery(cmd);
            //connection.Close();
        }
    }
}
