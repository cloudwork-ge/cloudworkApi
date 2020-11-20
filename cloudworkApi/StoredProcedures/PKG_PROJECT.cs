using cloudworkApi.Common;
using cloudworkApi.DataManagers;
using cloudworkApi.Models;
using cloudworkApi.Models.dsModels;
using cloudworkApi.SqlDataBaseEntity;
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
        public PKG_PROJECT()
        {
        }

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
        public void BidProject(ProjectBids projectBid)
        {
            using CloudWorkContext context = new CloudWorkContext();

            if (context.ProjectBids.Count(pb => pb.userID == projectBid.userID && pb.projectID == projectBid.projectID) > 0)
            {
                ResponseBuilder.throwError("თქვენ უკვე გაგზავნილი გაქვთ შეთავაზება");
            }
            if (context.Projects.Count(p => p.ID == projectBid.projectID && p.userId == projectBid.userID) > 0)
            {
                ResponseBuilder.throwError("თქვენივე დადებულ პროექტზე შეთავაზებას ვერ გააგზავნით");
            }
            context.ProjectBids.Add(projectBid);
            context.SaveChanges();
        }
        public IQueryable<Project> GetProjects()
        {
            using CloudWorkContext context = new CloudWorkContext();
            var projects = context.Projects.Select(x => x).Where(c => c.ID > 0);
            return projects;
        }
    }
}
