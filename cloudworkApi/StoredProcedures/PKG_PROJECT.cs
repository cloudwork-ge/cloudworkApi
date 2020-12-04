using cloudworkApi.Common;
using cloudworkApi.DataManagers;
using cloudworkApi.Models;
using cloudworkApi.Models.dsModels;
using cloudworkApi.Models.tableModels;
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
        public void BidProject(tbProjectBids projectBid)
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
        public IQueryable<tbProjects> GetProjects()
        {
            using CloudWorkContext context = new CloudWorkContext();
            var projects = context.Projects.Select(x => x).Where(c => c.ID > 0);
            return projects;
        }
        public ProjectDetails GetProject(int projectId, int userId) {
            using CloudWorkContext context = new CloudWorkContext();
            var project = context.V_Projects.
                Where(c => c.ID == projectId && (c.userId == userId || c.workerUserId == userId)).
                Select(x => new ProjectDetails() { ID = x.ID,
                    projectName = x.projectName,
                    projectType = x.projectType,
                    projectDescription = x.projectDescription,
                    projectCategory = x.projectCategory,
                    budget = x.budget,
                    status = x.status,
                    statusText = x.statusText,
                    userId = x.userId,
                    workerUserId = x.workerUserId,
                    startDate = x.startDate,
                    workerFullName = x.workerFullName,
                    endDate = x.endDate,
                    doneRequested = x.doneRequested,
                    doneRequestDate = x.doneRequestDate}).FirstOrDefault();
                
            return project;
        }
        public void AcceptBid(int bidId, int userId)
        {
            using CloudWorkContext context = new CloudWorkContext();
            var bid = context.ProjectBids.FirstOrDefault(b => b.ID == bidId);
            var project = context.Projects.Where(a => a.ID == bid.projectID && a.userId == userId).FirstOrDefault();

            if (project == null || project.ID == 0)
                ResponseBuilder.throwError("თქვენ არ გაქვთ ამ პროექტზე ცვლილებების უფლება");

            if (project.status != 0)
                ResponseBuilder.throwError("იმისთვის რომ დაეთანხმოთ შეთავაზებას პროოექტის სტატუსი უნდა იყოს ღია");

            var bidsToRefuse = context.ProjectBids.Where(x => x.projectID == bid.projectID).ToList();


            // Changes here
            project.status = 1; // 0 ღია, 1 მიმდინარე, 2 დასრულებული
            project.workerUserId = bid.userID;
            bidsToRefuse.ForEach(x => x.status = 2); // refuse all

            bid.status = 1; // 0 - მოლოდინში, 1 - დადასტურებული, 2 - უარყოფილი

            //context.ProjectBids.Update(bid);
            //context.Projects.Update(project);
            context.SaveChanges();

        }
        public tbProjects ProjectDoneFreelancer(tbProjects project)
        {
            using CloudWorkContext context = new CloudWorkContext();
            //var bid = context.ProjectBids.FirstOrDefault(b => b.ID == bidId);
            var thisProject = context.Projects.Where(a => a.ID == project.ID && a.workerUserId == project.workerUserId).FirstOrDefault();

            if (thisProject == null || thisProject.ID == 0)
                ResponseBuilder.throwError("თქვენ არ გაქვთ ამ პროექტზე ცვლილებების უფლება");

            if (thisProject.status != 1)
                ResponseBuilder.throwError("იმისთვის რომ დაასრულოთ პროოექტი სტატუსი უნდა იყოს მიმდინარე");

            if (thisProject.doneRequested == 1)
                ResponseBuilder.throwError("ამ პროექტზე დასრულების მოთხოვნა უკვე გაგაზავნილია");


            thisProject.doneRequested = 1;
            thisProject.doneRequestDate = DateTime.Now;

            context.SaveChanges();
            return project;
        }
        public tbProjects ProjectDoneOwner(tbProjects project)
        {
            using CloudWorkContext context = new CloudWorkContext();
            //var bid = context.ProjectBids.FirstOrDefault(b => b.ID == bidId);
            var thisProject = context.Projects.Where(a => a.ID == project.ID && a.userId == project.userId).FirstOrDefault();

            if (thisProject == null || thisProject.ID == 0)
                ResponseBuilder.throwError("თქვენ არ გაქვთ ამ პროექტზე ცვლილებების უფლება");

            if (thisProject.status != 1)
                ResponseBuilder.throwError("იმისთვის რომ დაასრულოთ პროოექტი სტატუსი უნდა იყოს მიმდინარე");

            if (thisProject.doneRequested == 0)
                ResponseBuilder.throwError("ამ პროექტზე დასრულების მოთხოვნა ჯერ არ გაკეთებულა");

            thisProject.status = 2;
            thisProject.doneDate = DateTime.Now;

            context.SaveChanges();
            return thisProject;
        }
    }
}
